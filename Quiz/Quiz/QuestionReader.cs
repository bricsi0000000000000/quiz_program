using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz
{
    public static class QuestionReader
    {
        public static void ReadFile(string fileName)
        {
            QuestionManager.ClearQuestions();

            using (var reader = new StreamReader(fileName))
            {
                dynamic json = JsonConvert.DeserializeObject(reader.ReadToEnd());

                for (int i = 0; i < json.Count; i++)
                {
                    string question = json[i].Name;
                    var answers = new List<Answer>();

                    for (int j = 0; j < json[i].Answers.Count; j++)
                    {
                        string answer = json[i].Answers[j].Name;
                        bool right = json[i].Answers[j].Right;
                        answers.Add(new Answer(j, answer, right));
                    }

                    QuestionManager.AddQuestion(new Question(i, question, answers));
                }
            }

            foreach (var item in QuestionManager.Questions)
            {
                Console.WriteLine(item.ID + "\t" + item.Name);
            }

            ((QuestionsContent)TabManager.GetTab("Questions").Content).AddNewQuestion();
            ((SettingsContent)TabManager.GetTab("Settings").Content).UpdateQuestionItems(fileName);
        }
    }
}
