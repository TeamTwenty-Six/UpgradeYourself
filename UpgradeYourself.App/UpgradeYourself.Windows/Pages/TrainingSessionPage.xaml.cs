using Parse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UpgradeYourself.Models.Models;
using UpgradeYourself.Windows.DataModels;
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
    public sealed partial class TrainingSessionPage : Page
    {
        public TrainingSessionPage()
            : this(new TrainingSessionViewModel())
        {
        }

        public TrainingSessionPage(TrainingSessionViewModel viewModel)
        {
            this.InitializeComponent();
            this.ViewModel = viewModel;
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
            }

            this.ViewModel.CurrentIndex++;

            if (this.ViewModel.CurrentIndex == this.ViewModel.Questions.Count)
            {
                //// TODO: check
                //var skillSummaryService = new SkillSummaryService();

                //var userSkillSummary = skillSummaryService.GetUserSkillSummary(ParseUser.CurrentUser.Username, this.ViewModel.Skill);

                //if (userSkillSummary == null)
                //{
                //    userSkillSummary = new SkillSummary()
                //    {
                //        Username = ParseUser.CurrentUser.Username,
                //        Skill = this.ViewModel.Skill,
                //        Points = this.ViewModel.Points,
                //        Level = this.ViewModel.Level
                //    };

                //    skillSummaryService.InsertSkillSummary(userSkillSummary);
                //}

                //// update
                //userSkillSummary.Points += this.ViewModel.Points;
                //if (userSkillSummary.Level < this.ViewModel.Level)
                //{
                //    userSkillSummary.Level = this.ViewModel.Level;
                //    skillSummaryService.UpdateSkillSummary(userSkillSummary);
                //}

                // TODO: save points in user profile
                // navigate to user profile?
                // add skill summary page into database - update level and points
                this.Frame.Navigate(typeof(TrainingSessionSummaryPage), 
                    new TrainingSessionSummaryViewModel { Skill = this.ViewModel.Skill, Level = this.ViewModel.Level, Points = this.ViewModel.Points });
            }
        }

        private IList<QuestionViewModel> GetQuestions(string skillName, int level)
        {
            var questionService = new QuestionService();
            return questionService.GetQuestionsInSkill(skillName)
                .Where(q => q.Skill == skillName && q.Difficulty == level)
                .ToList();
        }
    }
}
