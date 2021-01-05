using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz
{
  public  class Answer
    {
        public int ID { get; private set; }
        public string Name { get; set; }
        public bool Right { get; set; } = false;

        public Answer(int id, string name, bool right)
        {
            ID = id;
            Name = name;
            Right = right;
        }

        public Answer(int id)
        {
            ID = id;
        }
    }
}
