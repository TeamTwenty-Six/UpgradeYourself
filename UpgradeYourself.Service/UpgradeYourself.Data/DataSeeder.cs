﻿namespace UpgradeYourself.Data
{
    using System.Collections.Generic;

    using System.Data.Entity.Migrations;
    using UpgradeYourself.Models;

    public class DataSeeder
    {
        public void SeedCategories(UpgradeYourselfDbContext context)
        {
            var categories = new List<Category>
            {
                new Category()
                {
                    Name = "JavaScript",
                    Description = "Improve your JavaScript knowledge."
                },
                new Category()
                {
                    Name = "C#",
                    Description = "Improve your C# knowledge."
                }
            };

            context.Categories.AddOrUpdate(categories.ToArray());
            context.SaveChanges();
        }

        public void SeedQuestions(UpgradeYourselfDbContext context)
        {
            var questions = new List<Question>
            {
                new Question()
                {
                    CategoryId = 1,
                    Content = "What is the correct syntax for referring to an external script called \"xxx.js\"?",
                    Answers = new List<Answer>
                    {
                            new Answer()
                            {
                                Content = "<script name=\"xxx.js\">",
                                IsCorrect = false,
                                QuestionId = 1
                            },
                            new Answer()
                            {
                                Content = "<script href=\"xxx.js\">",
                                IsCorrect = false,
                            },
                            new Answer()
                            {
                                Content = "<script src=\"xxx.js\">",
                                IsCorrect = true,
                            },
                    }
                },
                new Question()
                {
                    CategoryId = 1,
                    Content = "How do you write \"Hello World\" in an alert box?",
                    Answers = new List<Answer>
                    {
                        new Answer()
                        {
                            Content = "msg(\"Hello World\");",
                            IsCorrect = false,
                        },
                        new Answer()
                        {
                            Content = "alertBox(\"Hello World\");",
                            IsCorrect = false,
                        },
                        new Answer()
                        {
                            Content = "alert(\"Hello World\")",
                            IsCorrect = true,
                        }
                    }
                }
            };

            context.Questions.AddOrUpdate(questions.ToArray());
            context.SaveChanges();
        }
    }
}