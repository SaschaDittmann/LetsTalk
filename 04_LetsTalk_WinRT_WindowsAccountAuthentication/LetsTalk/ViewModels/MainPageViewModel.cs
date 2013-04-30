using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using LetsTalk.Common;
using LetsTalk.Model;
using Microsoft.WindowsAzure.MobileServices;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace LetsTalk.ViewModels
{
    public class MainPageViewModel : BindableBase, IMainPageViewModel
    {
        private const int MaxMessageCount = 50;

        private readonly MobileServiceClient _mobileServiceClient;
        private readonly IMobileServiceTable<Message> _messagesTable;
        private readonly ObservableCollection<Message> _messages;
        private readonly DispatcherTimer _loadMessagesTimer;

        public MainPageViewModel()
        {
            _mobileServiceClient = new MobileServiceClient(
                App.MobileServiceUrl,
                App.MobileServiceKey,
                new BusyHandler(busy => IsBusy = busy));

            _messagesTable = _mobileServiceClient.GetTable<Message>();

            RefreshCommand = new DelegateCommand(LoadMessages, false);
            DeleteMessageCommand = new DelegateCommand(RemoveSelectedMessage, false);
            SendMessageCommand = new DelegateCommand(SendMessage, false);

            _messages = new ObservableCollection<Message>();
            _loadMessagesTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(30),
                };
            _loadMessagesTimer.Tick += LoadMessagesTimer_Tick;
            _loadMessagesTimer.Start();

            LoginCommand = new DelegateCommand(Login);
            LogoutCommand = new DelegateCommand(Logout);
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
                DeleteMessageCommand.IsEnabled = value != null && IsAuthenticated;
                SetProperty(ref _selectedMessage, value);
            }
        }

        private Boolean _isBusy;
        public Boolean IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        public DelegateCommand SendMessageCommand { get; private set; }
        public DelegateCommand DeleteMessageCommand { get; private set; }
        public DelegateCommand RefreshCommand { get; private set; }

        public DelegateCommand LoginCommand { get; private set; }
        public DelegateCommand LogoutCommand { get; private set; }

        private Boolean _isAuthenticated;
        public Boolean IsAuthenticated
        {
            get { return _isAuthenticated; }
            set
            {
                SendMessageCommand.IsEnabled = value;
                DeleteMessageCommand.IsEnabled = SelectedMessage != null && value;
                SetProperty(ref _isAuthenticated, value);
            }
        }

        private async void LoadMessages()
        {
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
                await new MessageDialog(error, "Error while loading messages").ShowAsync();

            RefreshCommand.IsEnabled = true;
        }

        private async void SendMessage()
        {
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
                await new MessageDialog(error, "Error while sending a message").ShowAsync();
        }

        private async void RemoveSelectedMessage()
        {
            if (SelectedMessage == null) return;
            
            var error = String.Empty;
            try
            {
                var dialog = new MessageDialog(
                    "Do you really want to delete the message:\n" + SelectedMessage.Body,
                    "Delete Message");
                dialog.Commands.Add(new UICommand("Yes", null, "Yes"));
                dialog.Commands.Add(new UICommand("No", null, "No"));

                var result = await dialog.ShowAsync();
                if (result.Id.ToString() == "Yes")
                {
                    await _messagesTable.DeleteAsync(SelectedMessage);
                    Messages.Remove(SelectedMessage);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            if (!String.IsNullOrEmpty(error))
                await new MessageDialog(error, "Error while deleting a message").ShowAsync();
        }

        private void LoadMessagesTimer_Tick(object sender, object e)
        {
            if (RefreshCommand.CanExecute(sender))
                RefreshCommand.Execute();
        }

        private async void Login()
        {
            if (_mobileServiceClient.CurrentUser == null)
                await Authenticate(MobileServiceAuthenticationProvider.MicrosoftAccount);

            if (_mobileServiceClient.CurrentUser == null) return;

            IsAuthenticated = true;
        }

        private void Logout()
        {
            if (_mobileServiceClient.CurrentUser != null)
                _mobileServiceClient.Logout();

            IsAuthenticated = false;
        }

        private async Task Authenticate(MobileServiceAuthenticationProvider provider)
        {
            if (_mobileServiceClient.CurrentUser != null) return;

            var message = String.Empty;
            try
            {
                IsAuthenticated = false;
                await _mobileServiceClient.LoginAsync(provider);
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
                await new MessageDialog(message, "Authenticate User").ShowAsync();
        }
    }
}
