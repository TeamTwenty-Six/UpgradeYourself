using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UpgradeYourself.Models.Models;
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
    public sealed partial class SelectLevelPage : Page
    {
        public SelectLevelPage()
            : this(new SelectLevelViewModel())
        {
        }

        public SelectLevelPage(SelectLevelViewModel viewModel)
        {
            this.InitializeComponent();
            this.ViewModel = viewModel;
        }

        public SelectLevelViewModel ViewModel
        {
            get
            {
                return this.DataContext as SelectLevelViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string skillName = e.Parameter.ToString();

            //TODO: make this show as title -> PageTitleText="{Binding SelectedSkill}" BeginTrainingViewModel
            this.ViewModel.SelectedSkill = string.Format("Skill: {0}", skillName);

            var questions = this.GetQuestons(skillName)
                .OrderBy(q => q.Difficulty)
                .GroupBy(q => q.Difficulty)
                .Select(gr => new { Skill = gr.Key, Items = gr.ToList() })
                .ToList();

            var levels = questions.Select(x => x.Skill)
                .ToList();

            this.ViewModel.Levels = levels.Select(l => "Level " + l)
                .ToList();

            if (!this.ViewModel.Levels.Any())
            {
                this.TextBlockNoAvailableTrainings.Visibility = Visibility.Visible;
            }

            // TODO: button levels - if in players dictionary <skill.name, hashset<int> levels> - grey color
        }

        private void OnGridViewLevelItemClick(object sender, RoutedEventArgs e)
        {
            var textBlock = e.OriginalSource as TextBlock;
            if (textBlock == null)
            {
                var button = e.OriginalSource as Button;
                var parent = button.Parent as StackPanel;
                textBlock = parent.Children.Last() as TextBlock;
            }

            var level = textBlock.Text.Split(' ')[1];

            this.Frame.Navigate(typeof(TrainingSessionPage), new { Skill = this.ViewModel.SelectedSkill, Level = level });
        }

        private ICollection<QuestionViewModel> GetQuestons(string skillName)
        {
            var questionService = new QuestionService();
            var skill = new Skill() { Name = skillName };
            return questionService.GetQuestionsInSkill(skill)
                .ToList();
            //  var questionsDifficulty = questionService.GetQuestionsInSkillWithDifficulty(skill, 0).ToList();

            //var session = new TrainingSessionViewModel()
            //{
            //    Skill = skill.Name,
            //    Questions = questions
            //};

            //this.DataContext = session;
        }
    }
}
