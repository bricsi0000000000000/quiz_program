using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz
{
    public static class QuestionManager
    {
        public static List<Question> Questions { get; private set; } = new List<Question>();
        public static List<Question> WrongAnsweredQuestions = new List<Question>();

        public static void AddQuestion(Question question)
        {
            Questions.Add(question);
        }

        public static Question GetQuestion(int id)
        {
            return Questions.Find(x => x.ID == id);
        }

        public static Question GetWrongQuestion(int id)
        {
            return WrongAnsweredQuestions.Find(x => x.ID == id);
        }

        public static void ClearQuestions()
        {
            Questions.Clear();
        }

        public static void SetQuestion(Question question)
        {
            GetQuestion(question.ID).Name = question.Name;
            GetQuestion(question.ID).Answers = question.Answers;
        }

        public static void RemoveQuestion(int id)
        {
            Questions.RemoveAt(Questions.FindIndex(x => x.ID == id));
        }

        public static void RemoveWrongQuestion(int id)
        {
            WrongAnsweredQuestions.RemoveAt(WrongAnsweredQuestions.FindIndex(x => x.ID == id));
        }
    }
}
