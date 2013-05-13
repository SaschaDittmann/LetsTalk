using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using LetsTalkWP8.Resources;
using LetsTalkWP8.ViewModel;

namespace LetsTalkWP8
{
    public partial class MainPage : PhoneApplicationPage
    {
        private readonly IMainPageViewModel _viewModel;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            _viewModel = new MainPageViewModel();
            DataContext = _viewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
               base.OnNavigatedTo(e);
               _viewModel.RefreshCommand.Execute();
        }
    }
}