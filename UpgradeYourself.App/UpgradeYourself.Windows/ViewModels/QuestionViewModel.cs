namespace UpgradeYourself.Windows.ViewModels
{
    using System.Collections.Generic;

    using SQLite;

    using Models;
    using Base;
    //[Table("Question")]
    public class QuestionViewModel : BaseViewModel
    {
        // todo: answerviewmodel
        private ICollection<AnswerViewModel> answers;

        public QuestionViewModel()
        {
            this.answers = new List<AnswerViewModel>();
        }

        public string Skill { get; set; }

        //[MaxLength(250)]
        public string Content { get; set; }

        public ICollection<AnswerViewModel> Answers
        {
            get
            {
                return this.answers;
            }

            set
            {
                this.answers = value;
            }
        }
    }
}
