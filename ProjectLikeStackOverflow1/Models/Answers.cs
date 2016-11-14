using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectLikeStackOverflow1.Models
{
    public class Answers
    {
        public int ID { get; set; }
        [Required]
        public int IdOfQuestions { get; set; }
        [Required]
        public string Answer { get; set; }

        public string UserName { get; set; }

    }
}