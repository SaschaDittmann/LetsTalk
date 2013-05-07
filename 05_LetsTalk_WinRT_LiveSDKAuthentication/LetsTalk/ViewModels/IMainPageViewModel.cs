using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using LetsTalk.Common;
using LetsTalk.Model;
using Windows.UI.Xaml.Controls;

namespace LetsTalk.ViewModels
{
    public interface IMainPageViewModel
    {
        ObservableCollection<Message> Messages { get; }
        String MessageText { get; set; }
        Message SelectedMessage { get; set; }

        Boolean IsBusy { get; }

        DelegateCommand SendMessageCommand { get; }
        DelegateCommand DeleteMessageCommand { get; }
        DelegateCommand RefreshCommand { get; }

        DelegateCommand LoginCommand { get; }
        DelegateCommand LogoutCommand { get; }
        Boolean IsAuthenticated { get; }
    }
}