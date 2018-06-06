using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area.Data.CustomEntity
{
    public class Evaluation
    {
        public int visitplaceID { get; set; }
        public string PlacePositiveComment { get; set; }
        public string PlaceNegativeComment { get; set; }
        public List<Question> Questions { set; get; }
        public Evaluation()
        {
            Questions = new List<Question>();
        }
    }
}
