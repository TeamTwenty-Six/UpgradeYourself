namespace UpgradeYourself.Windows.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json.Linq;

    using Windows.ViewModels;
    using Models.Models;
    public class QuestionService
    {
        private const string QuestionInSkillApiUrl = "http://localhost:53906/api/Question/Get?category={0}";
        private const string QuestionInSkillWithDifficultyApiUrl = "http://localhost:53906/api/Question/Get?category={0}&difficulty={1}";

        public IEnumerable<QuestionViewModel> GetQuestionsInSkill(Skill skill)
        {        
            var jsonResponse = this.GetQuestionsInSkillFromApi(skill);
            var parsedResponse = JArray.Parse(jsonResponse);

            var result = this.ParseJsonResponse(parsedResponse);
            return result;
        }

        public IEnumerable<QuestionViewModel> GetQuestionsInSkillWithDifficulty(Skill skill, int difficulty)
        {
            var jsonResponse = this.GetQuestionsInSkillWithDifficultyFromApi(skill, difficulty);
            var parsedResponse = JArray.Parse(jsonResponse);

            var result = this.ParseJsonResponse(parsedResponse);
            return result;
        }

        private string GetQuestionsInSkillFromApi(Skill skill)
        {
            HttpClient client = new HttpClient();
            var response = client.GetAsync(string.Format(QuestionInSkillApiUrl, skill.Name)).Result;
            Task<string> result = null;

            try
            {
                result = response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                // TODO: log
            }

            return result.Result;
        }

        private string GetQuestionsInSkillWithDifficultyFromApi(Skill skill, int difficulty)
        {
            HttpClient client = new HttpClient();
            var response = client.GetAsync(string.Format(QuestionInSkillWithDifficultyApiUrl, skill.Name, difficulty)).Result;
            Task<string> result = null;

            try
            {
                result = response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                // TODO: log
            }

            return result.Result;
        }

        private IEnumerable<QuestionViewModel> ParseJsonResponse(JArray parsedResponse)
        {
            return parsedResponse.Select(q => new QuestionViewModel
            {
                Skill = q["Category"].ToString(),
                Content = q["Content"].ToString(),
                Difficulty = q["Difficulty"].Value<int>(),
                Answers = q["Answers"].Select(a => new AnswerViewModel
                {
                    Content = a["Content"].ToString(),
                    IsCorrect = a["IsCorrect"].Value<bool>(),
                })
                .ToList()
            })
            .ToList();
        }
    }
}
