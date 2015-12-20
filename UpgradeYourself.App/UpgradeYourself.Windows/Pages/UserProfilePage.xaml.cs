using Parse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UpgradeYourself.Windows.Services;
using UpgradeYourself.Windows.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class UserProfilePage : Page
    {
        public UserProfilePage()
            : this(new UserProfileViewModel())
        {
        }

        public UserProfilePage(UserProfileViewModel viewModel)
        {
            this.InitializeComponent();
            this.ViewModel = viewModel;
        }

        public UserProfileViewModel ViewModel
        {
            get
            {
                return this.DataContext as UserProfileViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var userProfileService = new UserProfileService();
            var profiles = userProfileService.GetCurrentUserProfile(ParseUser.CurrentUser.Username);

            var skillSummaryService = new SkillSummaryService();
            var userSkillSummaries = skillSummaryService.GetAllUserSkillSummaries(ParseUser.CurrentUser.Username);

            this.ViewModel.Username = ParseUser.CurrentUser.Username;

            foreach (var skillSummary in userSkillSummaries)
            {
                this.ViewModel.AddLevelToSkill(skillSummary.Skill, skillSummary.Level);
                this.ViewModel.AddPointsToSkill(skillSummary.Skill, skillSummary.Points);
            }

            // TODO get user data for each skill summary page in db via username
        }

        private void OnLogOutButtonClick(object sender, RoutedEventArgs e)
        {
            ParseUser.LogOut();
            (Window.Current.Content as Frame).Navigate(typeof(LogInPage));
        }
    }
}
