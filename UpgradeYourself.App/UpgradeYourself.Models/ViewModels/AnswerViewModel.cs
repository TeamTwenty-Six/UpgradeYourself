namespace UpgradeYourself.Models.ViewModels
{
    using SQLite;

    //[Table("Answer")]
    public class AnswerViewModel : BaseViewModel
    {
        //public int QuestionId { get; set; }

        //[MaxLength(250)]
        public string Content { get; set; }

        public bool IsCorrect { get; set; }
    }
}