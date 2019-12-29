using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoSoft.Models
{
    public class Comment
    {
        private String text = "";
        public String Text {
            get { return text; }
            set { text = value; }
        }

        private DateTime date = DateTime.MinValue;
        public DateTime Date {
            get { return date; }
            set { date = value; }
        }
    }
}
