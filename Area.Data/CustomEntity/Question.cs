using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area.Data.CustomEntity
{
    public class Question
    {
        public int ID { set; get; }
        public string QuestionText { set; get; }
        public List<Answer> Answers { set; get; }
        public int SelectedAnswer { set; get; }
        public Question()
        {
            Answers = new List<Answer>();
        }
    }
}
