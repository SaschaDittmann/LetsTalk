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
    public interface IMainPageViewModel
    {
        ObservableCollection<Message> Messages { get; }
        String MessageText { get; set; }
        Message SelectedMessage { get; set; }

        DelegateCommand SendMessageCommand { get; }
        DelegateCommand DeleteMessageCommand { get; }
        DelegateCommand RefreshCommand { get; }
    }
}
