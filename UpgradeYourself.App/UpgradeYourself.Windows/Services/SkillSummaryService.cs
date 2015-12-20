namespace UpgradeYourself.Windows.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SQLite;

    using UpgradeYourself.Common;
    using UpgradeYourself.Models.Models;
    using ViewModels;
    public class SkillSummaryService
    {
        private SQLiteAsyncConnection conn;

        public SkillSummaryService()
            : this(new SQLiteAsyncConnection(GlobalConstants.DbName))
        {
        }

        public SkillSummaryService(SQLiteAsyncConnection conn)
        {
            this.conn = conn;
        }

        public async Task<ICollection<SkillSummary>> GetAllUserSkillSummaries(string username)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(GlobalConstants.DbName);
            var queryProfiles = conn.Table<UserProfile>();
            var userProfiles = await queryProfiles.ToListAsync();

            var querySkills = conn.Table<SkillSummary>();
            var skills = await querySkills.ToListAsync();
            //var queryProfiles = conn.Table<UserProfile>();
            //var userProfiles = await queryProfiles.ToListAsync();

            //var querySkills = this.conn.Table<SkillSummary>();
            var skillSummaries = await querySkills.ToListAsync();
            var userSkillSummaries = skillSummaries.Where(u => u.Username == username).ToList();

            return userSkillSummaries;
        }

        public SkillSummary GetUserSkillSummary(string username, string skill)
        {
            var userSkillSummaries = this.GetAllUserSkillSummaries(username).Result;
            var skillSummary = userSkillSummaries.FirstOrDefault(s => s.Skill == skill);

            return skillSummary;
        }

        public async void InsertSkillSummary(SkillSummary model)
        {
            await this.conn.InsertAsync(model);
        }

        public async Task<int> UpdateSkillSummary(SkillSummary model)
        {
            return await conn.UpdateAsync(model);
        }
    }
}
