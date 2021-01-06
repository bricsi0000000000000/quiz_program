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
        public string Name { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();

        public Question(int id, string name, List<Answer> answers)
        {
            ID = id;
            Name = name;
            Answers = answers;
        }

        public Question(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
