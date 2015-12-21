using Parse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UpgradeYourself.Data;
using UpgradeYourself.Models.Models;
using UpgradeYourself.Windows.DataModels;
using UpgradeYourself.Windows.Services;
using UpgradeYourself.Windows.ViewModels;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
    public sealed partial class TrainingSessionPage : Page
    {
        private SQLiteData sqliteData;
        private Accelerometer accelerometer;

        public TrainingSessionPage()
            : this(new TrainingSessionViewModel())
        {
        }

        public TrainingSessionPage(TrainingSessionViewModel viewModel)
        {
            this.InitializeComponent();
            this.ViewModel = viewModel;
            this.sqliteData = new SQLiteData();
            accelerometer = Accelerometer.GetDefault();
            accelerometer.Shaken += OnShake;

            //this.OnShake(null, null);
        }

        public TrainingSessionViewModel ViewModel
        {
            get
            {
                return this.DataContext as TrainingSessionViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var skillLevelDataModel = e.Parameter as SkillLevelDataModel;
            this.ViewModel.Skill = skillLevelDataModel.Skill;
            this.ViewModel.Level = skillLevelDataModel.Level;

            var questions = this.GetQuestions(this.ViewModel.Skill, this.ViewModel.Level);
            if (questions.Count == 0)
            {
                this.TextBlockNoAvailableTrainings.Visibility = Visibility.Visible;
                return;
            }

            this.ViewModel.Questions = questions;
            this.ViewModel.CurrentIndex = 0;
            //this.ViewModel.CurrentQuestion = this.ViewModel.Questions[0];
            // TODO: increment index on answered
        }

        private void OnGridViewAnswerItemClick(object sender, RoutedEventArgs e)
        {
            // TODO: fix crashes
            var textBlock = e.OriginalSource as TextBlock;
            if (textBlock == null)
            {
                var button = e.OriginalSource as Button;
                var parent = button.Parent as StackPanel;
                textBlock = parent.Children.Last() as TextBlock;
            }

            var answer = this.ViewModel.CurrentQuestion.Answers
                .FirstOrDefault(a => a.Content == textBlock.Text);

            if (answer.IsCorrect)
            {
                // TODO : display points
                this.ViewModel.Points += 10;
                this.AnswerStatusCorrect.Text = string.Format("Well done, you are correct! +{0} points", ViewModel.Points);
                this.AnswerStatusCorrect.Visibility = Visibility.Visible;
                Hide();
            }
            else
            {
                this.ViewModel.Points += 0;
                this.AnswerStatusWrong.Text = string.Format("Sorry, this is not the correct answer. {0} points", ViewModel.Points);
                this.AnswerStatusWrong.Visibility = Visibility.Visible;
                Hide();
            }

            this.ViewModel.CurrentIndex++;

            if (this.ViewModel.CurrentIndex == this.ViewModel.Questions.Count)
            {
                var skillSummaryService = new SkillSummaryService();

                var userSkillSummary = skillSummaryService.GetUserSkillSummary(ParseUser.CurrentUser.Username, this.ViewModel.Skill);

                if (userSkillSummary == null)
                {
                    userSkillSummary = new SkillSummary()
                    {
                        Username = ParseUser.CurrentUser.Username,
                        Skill = this.ViewModel.Skill,
                        Points = this.ViewModel.Points,
                        Level = this.ViewModel.Level
                    };

                    skillSummaryService.InsertSkillSummary(userSkillSummary);
                }

                // update
                if (userSkillSummary.Level < this.ViewModel.Level)
                {
                    userSkillSummary.Points += this.ViewModel.Points;
                    userSkillSummary.Level = this.ViewModel.Level;
                }

                skillSummaryService.UpdateSkillSummary(userSkillSummary);

                // TODO: save points in user profile
                // navigate to user profile?
                this.Frame.Navigate(typeof(TrainingSessionSummaryPage),
                    new TrainingSessionSummaryViewModel { Skill = this.ViewModel.Skill, Level = this.ViewModel.Level, Points = this.ViewModel.Points, NumberOfQuestions = this.ViewModel.Questions.Count });
            }
        }

        private IList<QuestionViewModel> GetQuestions(string skillName, int level)
        {
            var questionService = new QuestionService();
            return questionService.GetQuestionsInSkill(skillName)
                .Where(q => q.Skill == skillName && q.Difficulty == level)
                .ToList();
        }

        private void Question_Holding(object sender, HoldingRoutedEventArgs e)
        {
            var wrongAnswer = this.ViewModel.CurrentQuestion.Answers.FirstOrDefault(a => !a.IsCorrect);

            foreach (var item in gridViewAnswers.Items)
            {
                var button = item as AnswerViewModel;
                if (button == null)
                {
                    continue;
                }

                if (wrongAnswer != null && button.Content.ToString() == wrongAnswer.Content)
                {
                    button.Content = string.Empty;
                    this.ViewModel.Points -= 3;
                    break;
                }
            }

            this.Hint.Visibility = Visibility.Visible;
            //this.Hint.Text = "Hint";
            Hide();
        }

        private void Hint_Holding(object sender, HoldingRoutedEventArgs e)
        {
            this.Hint.Visibility = Visibility.Collapsed;
        }

        private void Hide()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 100);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            AnswerStatusWrong.Visibility = Visibility.Collapsed;
            AnswerStatusCorrect.Visibility = Visibility.Collapsed;
            Hint.Visibility = Visibility.Collapsed;
        }

        //private void TextBlock_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        //{
        //    var gridView = sender as TextBlock;
        //    var scale = e.Delta.Scale;
        //    gridView.Width *= 2;
        //    gridView.Height *= 2;
        //}

        private async void OnShake(object sender, AccelerometerShakenEventArgs args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                new MessageDialog("For hint long tap/hold on the question.").ShowAsync();
            });          
        }

        private void Shake(object sender, RoutedEventArgs args)
        {
            OnShake(null, null);
        }
    }
}
