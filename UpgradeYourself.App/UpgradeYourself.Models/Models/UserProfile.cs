namespace UpgradeYourself.Models.Models
{
    using System.Collections.Generic;

    using SQLite;

    [Table("UserProfile")]
    public class UserProfile : BaseModel
    {
        // private ICollection<int> trainingSessionIds;
           
        // public UserProfile()
        // {
        //     this.trainingSessionIds = new HashSet<int>();
        // }

        [MaxLength(50)]
        public string Name { get; set; }

        // public Image Image { get; set; }
           
        // public ICollection<int> TrainingSessionIds
        // {
        //     get
        //     {
        //         return this.trainingSessionIds;
        //     }
           
        //     set
        //     {
        //         this.trainingSessionIds = value;
        //     }
        // }
    }
}
