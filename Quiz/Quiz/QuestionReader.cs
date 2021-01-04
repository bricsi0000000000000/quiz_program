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
            using (var reader = new StreamReader(fileName))
            {
                dynamic json = JsonConvert.DeserializeObject(reader.ReadToEnd());

                for (int i = 0; i < json.questions.Count; i++)
                {
                    string question = json.questions[i].name;
                    var answers = new List<Answer>();

                    for (int j = 0; j < json.questions[i].answers.Count; j++)
                    {
                        string answer = json.questions[i].answers[j].name;
                        bool right = json.questions[i].answers[j].right;
                        answers.Add(new Answer(answer, right));
                    }

                    QuestionManager.AddQuestion(new Question(i, question, answers));
                }
            }
        }
    }
}
