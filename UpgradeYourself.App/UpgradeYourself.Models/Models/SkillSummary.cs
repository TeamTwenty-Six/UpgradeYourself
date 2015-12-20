namespace UpgradeYourself.Models.Models
{
    using SQLite;

    [Table("SkillSummary")]
    public class SkillSummary : BaseModel
    {
        [MaxLength(50)]
        public string Username { get; set; }

        [MaxLength(50)]
        public string Skill { get; set; }

        public int Level { get; set; }

        public int Points { get; set; }
    }
}
