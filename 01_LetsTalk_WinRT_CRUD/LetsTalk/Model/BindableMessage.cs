using System;
using LetsTalk.Common;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace LetsTalk.Model
{
    public class BindableMessage : BindableBase
    {
        internal BindableMessage() { }
        public BindableMessage(Message message)
        {
            OriginalMessage = message;
            Id = message.Id;
            Body = message.Body;
            CreatedAt = message.CreatedAt;
        }

        public Message OriginalMessage { get; private set; }

        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _body = String.Empty;
        public string Body
        {
            get { return _body; }
            set { SetProperty(ref _body, value); }
        }
        
        private string _userName = "Dummy User";
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        private string _userImageUrl = "Assets/portrait.png";
        public string UserImageUrl
        {
            get { return _userImageUrl; }
            set
            {
                _userImage = null;
                SetProperty(ref _userImageUrl, value);
                OnPropertyChanged("UserImage");
            }
        }

        private ImageSource _userImage;
        public ImageSource UserImage
        {
            get
            {
                if (_userImage == null && _userImageUrl != null)
                {
                    _userImage = new BitmapImage(new Uri(App.BaseUri, _userImageUrl));
                }
                return _userImage;
            }
            set
            {
                _userImageUrl = null;
                SetProperty(ref _userImage, value);
            }
        }

        private DateTime _createdAt = DateTime.Now;
        public DateTime CreatedAt
        {
            get { return _createdAt; }
            set { SetProperty(ref _createdAt, value); }
        }
    }
}