using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectLikeStackOverflow1.Models
{
    public class Questions
    {
        public int ID { get; set; }
        public int Vote { get; set; }
        public int Answer1 { get; set; }
        public int View { get; set; }
        public string Title { get; set; }
        public string Question { get; set; }
        public DateTime Date { get; set; }

        public string AcceptedAnswer { get; set; }

        public string UserName { get; set; }
    }
}