using System;
using LetsTalk.Common;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace LetsTalk.Model
{
    /// <summary>
    /// A class used to store messages in Mobile Services
    /// </summary>
    [DataTable("messages")]
    public class Message : BindableBase
    {
        private int _id;
        [JsonProperty("id")]
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _body = String.Empty;
        [JsonProperty("body")]
        public string Body
        {
            get { return _body; }
            set { SetProperty(ref _body, value); }
        }

        private string _userName = "Dummy User";
        [JsonIgnore]
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        private string _userImageUrl = "Assets/portrait.png";
        [JsonIgnore]
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
        [JsonIgnore]
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
        [JsonProperty("createdAt")]
        public DateTime CreatedAt
        {
            get { return _createdAt; }
            set { SetProperty(ref _createdAt, value); }
        }
    }
}