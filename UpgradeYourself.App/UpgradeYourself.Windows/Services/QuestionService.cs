namespace UpgradeYourself.Windows.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json.Linq;

    using Models.Models;
    using Models.ViewModels;

    public class QuestionService
    {
        private const string QuestionApiUrl = "http://localhost:53906/api/Question/Get?category=";

        public IEnumerable<QuestionViewModel> GetQuestionsInSkill(Skill skill)
        {        
            var jsonResponse = this.GetQuestionsInSkillFromApi(skill);
            var parsedResponse = JArray.Parse(jsonResponse);

            var result = parsedResponse.Select(q => new QuestionViewModel
            {
                Skill = q["Category"].ToString(),
                Content = q["Content"].ToString(),
                Answers = q["Answers"].Select(a => new AnswerViewModel
                {
                    Content = a["Content"].ToString(),
                    IsCorrect = a["IsCorrect"].Value<bool>(),
                })
                .ToList()
            })
            .ToList();

            return result;
        }

        private string GetQuestionsInSkillFromApi(Skill skill) //Skill skill
        {
            HttpClient client = new HttpClient();
            var response = client.GetAsync(QuestionApiUrl + skill.Name).Result;
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
    }
}
