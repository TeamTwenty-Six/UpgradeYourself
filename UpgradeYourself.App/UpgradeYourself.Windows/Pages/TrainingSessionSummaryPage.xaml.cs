using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UpgradeYourself.Windows.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Notifications;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UpgradeYourself.Windows.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TrainingSessionSummaryPage : Page
    {
        public TrainingSessionSummaryPage()
            : this(new TrainingSessionSummaryViewModel())
        {
        }

        public TrainingSessionSummaryPage(TrainingSessionSummaryViewModel viewModel)
        {
            this.InitializeComponent();
            this.ViewModel = viewModel;
        }

        public TrainingSessionSummaryViewModel ViewModel
        {
            get
            {
                return this.DataContext as TrainingSessionSummaryViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var parameter = e.Parameter as TrainingSessionSummaryViewModel;
            this.ViewModel.Skill = parameter.Skill;
            this.ViewModel.Level = parameter.Level;
            this.ViewModel.Points = parameter.Points;
            this.ViewModel.NumberOfQuestions = parameter.NumberOfQuestions;


            bool hasMaxPoints = CheckMaxPoints(this.ViewModel.NumberOfQuestions, this.ViewModel.Points);
            if (hasMaxPoints)
            {
                this.Congrats.Visibility = Visibility.Visible;
                this.TrainMore.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.TrainMore.Visibility = Visibility.Visible;
            }

            ActivateReminder();
        }

        private async void ActivateReminder()
        {
            var notifier = ToastNotificationManager.CreateToastNotifier();

            if (notifier.Setting != NotificationSetting.Enabled)
            {
                var dialog = new MessageDialog("Notifications are not enabled!");
                await dialog.ShowAsync();
                return;
            }

            var template = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
            var element = template.GetElementsByTagName("text")[0];
            element.AppendChild(template.CreateTextNode("Practice regularly to stay fit!"));

            var date = DateTimeOffset.Now.AddSeconds(5);
            var stn = new ScheduledToastNotification(template, date);
            notifier.AddToSchedule(stn);
        }

        private bool CheckMaxPoints(int numberOfQuestions, int points)
        {
            bool hasMax = false;
            if(numberOfQuestions * 10 == points)
            {
                hasMax = true;
                return hasMax;
            }

            return hasMax;
        }

        private void OnTrainMoreButtonClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SkillsPage));
        }

       
        private void Congrats_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            var canvas = (sender as Canvas);
            if (!(canvas.RenderTransform is CompositeTransform))
            {
                canvas.RenderTransform = new CompositeTransform();
            }

        }

        private void Congrats_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var canvas = (sender as Canvas);
            var delta = e.Delta;
            var scale = delta.Scale;
            var transform = canvas.RenderTransform as CompositeTransform;
            transform.ScaleX -= scale / 50;
            transform.ScaleY -= scale / 50;
            if(transform.ScaleX == 0)
            {
                canvas.Visibility = Visibility.Collapsed;
            }
        }

        private void Congrats_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            var canvas = (sender as Canvas);
            canvas.Visibility = Visibility.Collapsed;
            this.TrainMore.Visibility = Visibility.Visible;
        }
    }
}
