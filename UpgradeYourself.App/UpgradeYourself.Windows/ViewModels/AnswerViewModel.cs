namespace UpgradeYourself.Windows.ViewModels
{
    using GalaSoft.MvvmLight;
    using SQLite;

    public class AnswerViewModel : ViewModelBase
    {
        private string content;

        public string Content
        {
            get
            {
                return this.content;
            }

            set
            {
                this.content = value;
                this.RaisePropertyChanged(() => this.Content);
            }
        }

        public bool IsCorrect { get; set; }
    }
}