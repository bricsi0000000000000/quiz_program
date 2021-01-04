using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz
{
  public  class Question
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public List<Answer> Answers { get; private set; } = new List<Answer>();

        public Question(int id, string name, List<Answer> answers)
        {
            ID = ++id;
            Name = name;
            Answers = answers;
        }
    }
}
