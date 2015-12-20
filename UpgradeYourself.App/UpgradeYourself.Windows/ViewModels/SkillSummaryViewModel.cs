namespace UpgradeYourself.Windows.ViewModels
{
    using System;
    using System.Linq.Expressions;

    using Models.Models;

    public class SkillSummaryViewModel
    {
        public static Expression<Func<SkillSummary, SkillSummaryViewModel>> FromSkillSummary
        {
            get
            {
                return s => new SkillSummaryViewModel
                {
                    Username = s.Username,
                    Skill = s.Skill,
                    Level = s.Level,
                    Points = s.Points
                };
            }
        }

        public static Expression<Func<SkillSummaryViewModel, SkillSummary>> FromSkillSummaryViewModel
        {
            get
            {
                return s => new SkillSummary
                {
                    Username = s.Username,
                    Skill = s.Skill,
                    Level = s.Level,
                    Points = s.Points
                };
            }
        }

        public string Username { get; set; }

        public string Skill { get; set; }

        public int Level { get; set; }

        public int Points { get; set; }
    }
}
