namespace UpgradeYourself.Windows.ViewModels
{
    using System.Collections.Generic;

    using SQLite;
    using GalaSoft.MvvmLight;    
    
    //[Table("TrainingSession")]
    public class TrainingSessionViewModel : ViewModelBase
    {
        private ICollection<QuestionViewModel> questions;

        public TrainingSessionViewModel()
        {
            this.questions = new HashSet<QuestionViewModel>();
        }

        public string Skill { get; set; }

        public int Points { get; set; }

        public ICollection<QuestionViewModel> Questions
        {
            get
            {
                return this.questions;
            }

            set
            {
                this.questions = value;
            }
        }
    }
}
