using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feladatlap
{
    class Exam
    {
        private List<string> Questions;
        private List<string> Answers;
        private string name;
        public Exam(string name)
        {
            Questions = new List<string>();
            Answers = new List<string>();
            this.name = name;
        }

        public string getQuestion(int num)
        {
            return Questions[num];
        }

        public string getAnswer(int num)
        {
            return Answers[num];
        }

        public void add(string question, string answer)
        {
            Questions.Add(question);
            Answers.Add(answer);
        }

        public string getName()
        {
            return name;
        }

        public int Count()
        {
            return Questions.Count;
        }
    }
}
