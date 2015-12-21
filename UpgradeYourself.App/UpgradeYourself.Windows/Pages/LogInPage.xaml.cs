using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UpgradeYourself.Windows.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UpgradeYourself.Windows.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LogInPage : Page
    {
        private const string InputErrorMessage = "All fields are required.";

        public LogInPage()
            : this(new LoginPageViewModel())
        {
        }

        public LogInPage(LoginPageViewModel viewModel)
        {
            this.InitializeComponent();
            this.ViewModel = viewModel;
        }

        public LoginPageViewModel ViewModel
        {
            get
            {
                return this.DataContext as LoginPageViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void OnLoginButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel == null)
            {
                // TODO: raise error
                return;
            }

            this.progressRing.Visibility = Visibility.Visible;
            bool isInputValid = this.ViewModel.ValidateInput();
            if (!isInputValid)
            {
                this.progressRing.Visibility = Visibility.Collapsed;
                this.ShowInfoMessage(InputErrorMessage);
                return;
            }
            else
            {
                this.progressRing.Visibility = Visibility.Collapsed;

                bool isLoggedIn = await ViewModel.Login();
                if (isLoggedIn)
                {
                    this.Frame.Navigate(typeof(SkillsPage));
                }
                else
                {
                    this.LoginFailed.Visibility = Visibility.Visible;
                }
            }
        }

        private async void ShowInfoMessage(string message)
        {
            var dialog = new MessageDialog(message);
            await dialog.ShowAsync();
        }
    }
}
