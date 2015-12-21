namespace UpgradeYourself.Windows.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using SQLite;

    using UpgradeYourself.Common;
    using UpgradeYourself.Models.Models;
    using ViewModels;
    using Data;
    public class SkillSummaryService
    {
        private SQLiteAsyncConnection conn;
        private SQLiteData sqliteData;

        public SkillSummaryService()
            : this(new SQLiteAsyncConnection(GlobalConstants.DbName), new SQLiteData())
        {
        }

        public SkillSummaryService(SQLiteAsyncConnection conn, SQLiteData sqliteData)
        {
            this.conn = conn;
            this.sqliteData = sqliteData;
        }

        public ICollection<SkillSummary> GetAllUserSkillSummaries(string username)
        {
            var skillSummaries = this.sqliteData.AllAsync<SkillSummary>();
            var userSkillSummaries = skillSummaries.Where(u => u.Username == username).ToList();

            return userSkillSummaries;
        }

        public SkillSummary GetUserSkillSummary(string username, string skill)
        {
            var userSkillSummaries = this.GetAllUserSkillSummaries(username);
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
