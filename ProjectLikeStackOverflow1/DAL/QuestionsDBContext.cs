using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ProjectLikeStackOverflow1.Models;

namespace ProjectLikeStackOverflow1.DAL
{
    public class QuestionsDBContext : DbContext
    {
        public DbSet<Questions> QuestionDB { get; set; }

        public DbSet<Answers> AnswerDB { get; set; }

        public DbSet<UserAccount> userAccount { get; set; }

        public DbSet<SessionSave> sessionDB { get; set; }
    }
}