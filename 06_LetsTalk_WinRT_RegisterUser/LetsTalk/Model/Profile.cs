using System;
using LetsTalk.Common;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace LetsTalk.Model
{
    [DataTable("profiles")]
    public class Profile : BindableBase
    {
        private int _id;
        [JsonProperty("id")]
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _name;
        [JsonProperty("name")]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _email;
        [JsonProperty("email")]
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        private string _userId;
        [JsonProperty("userId")]
        public string UserId
        {
            get { return _userId; }
            set { SetProperty(ref _userId, value); }
        }

        private string _imageUrl;
        [JsonProperty("ImageUrl")]
        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                _image = null;
                SetProperty(ref _imageUrl, value);
                OnPropertyChanged("Image");
            }
        }

        private ImageSource _image;
        [JsonIgnore]
        public ImageSource Image
        {
            get
            {
                if (_image == null && _imageUrl != null)
                {
                    _image = new BitmapImage(new Uri(App.BaseUri, _imageUrl));
                }
                return _image;
            }
            set 
            { 
                _imageUrl = null;
                SetProperty(ref _image, value);
            }
        }

        private DateTime _memberSince;
        [JsonProperty("memberSince")]
        public DateTime MemberSince
        {
            get { return _memberSince; }
            set { SetProperty(ref _memberSince, value); }
        }
    }
}