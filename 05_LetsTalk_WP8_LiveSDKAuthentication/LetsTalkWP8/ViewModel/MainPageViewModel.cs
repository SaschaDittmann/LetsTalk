﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LetsTalkWP8.Common;
using LetsTalkWP8.Model;
using Microsoft.WindowsAzure.MobileServices;
using System.Windows;
using System.Windows.Threading;
using System.Threading.Tasks;
using Microsoft.Live;

namespace LetsTalkWP8.ViewModel
{
    public class MainPageViewModel : BindableBase, IMainPageViewModel
    {
        private const int MaxMessageCount = 50;

        private readonly MobileServiceClient _mobileServiceClient;
        private readonly LiveAuthClient _liveAuthClient;
        private readonly IMobileServiceTable<Message> _messagesTable;
        private readonly ObservableCollection<Message> _messages;
        private readonly DispatcherTimer _loadMessagesTimer;
        private bool _progressing = false;


        public MainPageViewModel()
        {
            _mobileServiceClient = new MobileServiceClient(
                App.MobileServiceUrl,
                App.MobileServiceKey);

            _liveAuthClient = new LiveAuthClient(App.ClientID);

            _messagesTable = _mobileServiceClient.GetTable<Message>();

            RefreshCommand = new DelegateCommand(LoadMessages,false);
            DeleteMessageCommand = new DelegateCommand(RemoveSelectedMessage, false);
            SendMessageCommand = new DelegateCommand(SendMessage,false);

            _messages = new ObservableCollection<Message>();
            _loadMessagesTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(30),
            };
            _loadMessagesTimer.Tick += LoadMessagesTimer_Tick;
            _loadMessagesTimer.Start();

            LoginCommand = new DelegateCommand(Login);
            LogoutCommand = new DelegateCommand(Logout,false);
        }

        private void EnableLogin(bool enable)
        {
            LoginCommand.IsEnabled = enable;
            LogoutCommand.IsEnabled = !enable;
            SendMessageCommand.IsEnabled = !enable;
            DeleteMessageCommand.IsEnabled = SelectedMessage != null && !enable;
            RefreshCommand.IsEnabled = !enable;
        }

        public bool Progressing
        {
            get { return _progressing; }
            set { SetProperty(ref _progressing, value); }
        }

        public ObservableCollection<Message> Messages
        {
            get { return _messages; }
        }

        private String _messageText;
        public String MessageText
        {
            get { return _messageText; }
            set { SetProperty(ref _messageText, value); }
        }

        private Message _selectedMessage;
        public Message SelectedMessage
        {
            get { return _selectedMessage; }
            set
            {
                DeleteMessageCommand.IsEnabled = value != null;
                SetProperty(ref _selectedMessage, value);
            }
        }

        public DelegateCommand SendMessageCommand { get; private set; }
        public DelegateCommand DeleteMessageCommand { get; private set; }
        public DelegateCommand RefreshCommand { get; private set; }

        public DelegateCommand LoginCommand { get; private set; }
        public DelegateCommand LogoutCommand { get; private set; }

        private async void LoadMessages()
        {
            Progressing = true;
            RefreshCommand.IsEnabled = false;

            var error = String.Empty;
            try
            {

                var newMessages = await _messagesTable
                                            .OrderByDescending(m => m.CreatedAt)
                                            .Take(MaxMessageCount)
                                            .ToListAsync();

                // Perform a simple sync of the existing list using the Ids.
                var matched = new Queue<Message>();
                var removals = new Queue<Message>();

                foreach (var item in Messages)
                {
                    var didMatch = false;

                    foreach (var newMessage in newMessages)
                    {
                        if (newMessage.Id == item.Id)
                        {
                            matched.Enqueue(newMessage);
                            didMatch = true;
                            break;
                        }
                    }

                    // If no match, we should remove from Items collection
                    if (!didMatch)
                    {
                        removals.Enqueue(item);
                    }

                    // remove new items as quickly as soon as they're matched 
                    while (matched.Count > 0)
                    {
                        newMessages.Remove(matched.Dequeue());
                    }
                }

                while (removals.Count > 0)
                {
                    Messages.Remove(removals.Dequeue());
                }

                // add any remaining newItems - the must be genuinely new items
                foreach (var newMessage in newMessages)
                {
                    var pos = 0;
                    foreach (var message in Messages)
                    {
                        if (newMessage.CreatedAt.CompareTo(message.CreatedAt) < 0)
                            pos++;
                        else
                            break;
                    }
                    Messages.Insert(pos, newMessage);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            if (!String.IsNullOrEmpty(error))
                MessageBox.Show(error,"Error while loading messages",MessageBoxButton.OK);
            RefreshCommand.IsEnabled = true;
            Progressing = false;
        }

        private async void SendMessage()
        {
            Progressing = true;
            var error = String.Empty;
            try
            {
                var message = new Message
                {
                    Body = MessageText,
                    CreatedAt = DateTime.Now,
                };

                MessageText = String.Empty;

                await _messagesTable.InsertAsync(message);
                Messages.Insert(0, message);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            if (!String.IsNullOrEmpty(error))
                MessageBox.Show(error, "Error while sending a message", MessageBoxButton.OK);
            Progressing = false;
        }

        private async void RemoveSelectedMessage()
        {
            Progressing = true;
            if (SelectedMessage == null) return;

            var error = String.Empty;
            try
            {
                var dialog = MessageBox.Show("Do you really want to delete the message:\n" + SelectedMessage.Body,
                    "Delete Message", MessageBoxButton.OKCancel);
                if (dialog == MessageBoxResult.OK)
                {
                    await _messagesTable.DeleteAsync(SelectedMessage);
                    Messages.Remove(SelectedMessage);
                    SelectedMessage = null;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            if (!String.IsNullOrEmpty(error))
                MessageBox.Show(error, "Error while deleting a message", MessageBoxButton.OK);
            Progressing = false;
        }

        private void LoadMessagesTimer_Tick(object sender, object e)
        {
            if (RefreshCommand.CanExecute(sender) && RefreshCommand.IsEnabled)
                RefreshCommand.Execute();
        }

        private async void Login()
        {
            Progressing = true;
            if (_mobileServiceClient.CurrentUser == null)
                await Authenticate();

            if (_mobileServiceClient.CurrentUser != null)
                EnableLogin(false);
            Progressing = false;
        }

        private void Logout()
        {
            Progressing = true;
            if (_mobileServiceClient.CurrentUser != null)
            {
                _mobileServiceClient.Logout();
                _liveAuthClient.Logout();
                Messages.Clear();
            }

            if (_mobileServiceClient.CurrentUser == null)
                EnableLogin(true);
            Progressing = false;
        }


        private async Task Authenticate()
        {
            if (_mobileServiceClient.CurrentUser != null) 
                return;

            var message = String.Empty;
            try
            {
                while (_liveAuthClient.Session == null)
                {
                    LiveLoginResult result = await _liveAuthClient.InitializeAsync(App.WLLoginScope);
                    if (result.Status!= LiveConnectSessionStatus.Connected)
                    {
                        result = await _liveAuthClient.LoginAsync(App.WLLoginScope);
                    }
                    if (result.Status == LiveConnectSessionStatus.Connected)
                    {
                        MobileServiceUser loginResult = await _mobileServiceClient
                            .LoginWithMicrosoftAccountAsync(result.Session.AuthenticationToken);
                    }
                    else
                    {
                        MessageBox.Show("Sie müssen sich anmelden!", "Login benötigt", MessageBoxButton.OK);
                        return;
                    }
                }

            }
            catch (InvalidOperationException)
            {
                message = "login unsuccessful";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            if (!String.IsNullOrEmpty(message))
                MessageBox.Show(message, "Authenticate User",MessageBoxButton.OK);
        }
    }
}
