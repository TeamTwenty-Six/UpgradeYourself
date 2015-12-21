namespace UpgradeYourself.Windows.ViewModels
{
    using GalaSoft.MvvmLight;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UpgradeYourself.Common;

    public class UserProfileViewModel : ViewModelBase
    {
        private string location;

        // TODO: map from user? - from summary skill
        public UserProfileViewModel()
        {
            this.LastLevelPassed = new Dictionary<string, int>();
            this.Points = new Dictionary<string, int>();
        }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Location
        {
            get
            {
                return this.location;
            }

            set
            {
                this.location = value;
                this.RaisePropertyChanged(() => this.Location);
            }
        }

        public IDictionary<string, int> LastLevelPassed { get; set; }

        public IDictionary<string, int> Points { get; set; }

        public void AddLevelToSkill(string skill, int level)
        {
            this.LastLevelPassed[skill] = level;
        }

        public void AddPointsToSkill(string skill, int points)
        {
            if (!this.Points.ContainsKey(skill))
            {
                this.Points[skill] = 0;
            }

            this.Points[skill] += points;
        }
    }
}
