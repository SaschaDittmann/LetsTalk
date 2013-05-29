using LetsTalkWP8.Common;
using LetsTalkWP8.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsTalkWP8.ViewModel
{
    public class SampleMainPageViewModel : BindableBase, IMainPageViewModel
    {
        private readonly ObservableCollection<Message> _messages;

        public SampleMainPageViewModel()
        {
            _messages = new ObservableCollection<Message>();

            Messages.Add(new Message
            {
                Id = 1,
                Body =
                    "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed.",
                UserName = "Dummy User Name",
                UserImageUrl = "Assets/dummy01.png",
                CreatedAt = DateTime.Now.AddDays(-60),
            });
            Messages.Add(new Message
            {
                Id = 2,
                Body =
                    "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed.",
                UserName = "Dummy User Name",
                UserImageUrl = "Assets/dummy02.png",
                CreatedAt = DateTime.Now.AddDays(-30),
            });
            Messages.Add(new Message
            {
                Id = 3,
                Body =
                    "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed.",
                UserName = "Dummy User Name",
                UserImageUrl = "Assets/dummy03.png",
                CreatedAt = DateTime.Now.AddDays(-14),
            });
            Messages.Add(new Message
            {
                Id = 4,
                Body =
                    "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed.",
                UserName = "Dummy User Name",
                UserImageUrl = "Assets/dummy04.png",
                CreatedAt = DateTime.Now.AddDays(-7),
            });
            Messages.Add(new Message
            {
                Id = 5,
                Body =
                    "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed.",
                UserName = "Dummy User Name",
                UserImageUrl = "Assets/dummy05.png",
                CreatedAt = DateTime.Now.AddDays(-1),
            });
            Messages.Add(new Message
            {
                Id = 6,
                Body =
                    "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed.",
                UserName = "Dummy User Name",
                UserImageUrl = "Assets/dummy06.png",
                CreatedAt = DateTime.Now.AddHours(-12),
            });
            Messages.Add(new Message
            {
                Id = 7,
                Body =
                    "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed.",
                UserName = "Dummy User Name",
                UserImageUrl = "Assets/dummy07.png",
                CreatedAt = DateTime.Now.AddHours(-3),
            });
            Messages.Add(new Message
            {
                Id = 8,
                Body =
                    "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed.",
                UserName = "Dummy User Name",
                UserImageUrl = "Assets/dummy08.png",
                CreatedAt = DateTime.Now.AddHours(-1),
            });
            Messages.Add(new Message
            {
                Id = 9,
                Body =
                    "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed.",
                UserName = "Dummy User Name",
                UserImageUrl = "Assets/dummy09.png",
                CreatedAt = DateTime.Now.AddMinutes(-30),
            });
            Messages.Add(new Message
            {
                Id = 10,
                Body =
                    "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed.",
                UserName = "Dummy User Name",
                UserImageUrl = "Assets/dummy10.png",
                CreatedAt = DateTime.Now.AddSeconds(-10),
            });

            MessageText = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed.";
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
            set { SetProperty(ref _selectedMessage, value); }
        }

        public Boolean IsBusy
        {
            get { return false; }
        }

        public DelegateCommand SendMessageCommand { get; private set; }
        public DelegateCommand DeleteMessageCommand { get; private set; }
        public DelegateCommand RefreshCommand { get; private set; }

        public DelegateCommand LoginCommand { get; private set; }
        public DelegateCommand LogoutCommand { get; private set; }

        public Boolean IsAuthenticated
        {
            get { return true; }
        }
    }
}
