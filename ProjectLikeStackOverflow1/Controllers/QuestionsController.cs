using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ProjectLikeStackOverflow1.Models;
using ProjectLikeStackOverflow1.DAL;
using PagedList;
using System.Data.Entity;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.Script.Serialization;

namespace ProjectLikeStackOverflow1.Controllers
{
    public class QuestionsController : Controller
    {
        private QuestionsDBContext db = new QuestionsDBContext();
        public int IdOfQuestion;

        public int AnsNumber;
        // GET: Questions

        public ActionResult loaddata()
        {
            var data = db.QuestionDB.OrderBy(a => a.ID).ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {

            if (Response.Cookies["Username"] != null)
            //if (Session["UserID"] != null)
            {


                //paging code
                ViewBag.CurrentSort = sortOrder;
                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }
                //paging code


                ViewBag.CurrentFilter = searchString;

                ViewBag.DateSortParameter = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
                ViewBag.VoteSortParameter = sortOrder == "vote" ? "vote_desc" : "vote";
                ViewBag.AnswerNumberSortParameter = sortOrder == "answerNumber" ? "answerNumber_desc" : "answerNumber";
                ViewBag.ViewSortParameter = sortOrder == "View" ? "View_desc" : "View";

                var questionss = from s in db.QuestionDB
                                 select s;

                if (!String.IsNullOrEmpty(searchString))
                {
                    questionss = questionss.Where(s => s.Question.Contains(searchString)
                                           || s.Title.Contains(searchString));
                }

                switch (sortOrder)
                {
                    case "date_desc":
                        questionss = questionss.OrderByDescending(s => s.Date);
                        break;
                    case "vote":
                        questionss = questionss.OrderBy(s => s.Vote);
                        break;
                    case "vote_desc":
                        questionss = questionss.OrderByDescending(s => s.Vote);
                        break;
                    case "answerNumber":
                        questionss = questionss.OrderBy(s => s.Answer1);
                        break;
                    case "answerNumber_desc":
                        questionss = questionss.OrderByDescending(s => s.Answer1);
                        break;
                    case "View":
                        questionss = questionss.OrderBy(s => s.View);
                        break;
                    case "View_desc":
                        questionss = questionss.OrderByDescending(s => s.View);
                        break;
                    default:
                        questionss = questionss.OrderByDescending(s => s.Date);
                        break;
                }

                //paging code
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(questionss.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return RedirectToAction("Login");
            }
            //return View(questionss.ToList());
            //return View(db.QuestionDB.ToList());
            //return RedirectToAction("Create", "Questions");
        }

        public ActionResult GoForAnswer([Bind(Include = "ID,Vote,Answer1,View,Title,Question,Date,AcceptedAnswer,UserName")] Questions questions,string votingUp)
        {
            @ViewBag.CountNumber = int.Parse(Request.QueryString["vote"]);
            //Updating view count

            ViewBag.titleString = questions.Title = Request.QueryString["title"];


            questions.ID = int.Parse(Request.QueryString["idForAnswer"]);
            questions.View = int.Parse(Request.QueryString["View"]) + 1;
            questions.Date = DateTime.Parse(Request.QueryString["date"]);
            questions.Answer1 = int.Parse(Request.QueryString["answerNumber"]);
            questions.UserName = (Request.QueryString["UserName"]);
            //questions.Vote = @ViewBag.CountNumber = 10;
            IdOfQuestion = questions.ID;

            if (Request.QueryString["correctAnswer"] != null)
            {
                questions.AcceptedAnswer = Request.QueryString["correctAnswer"];
            }
            if (Request.QueryString["correctAnswer"] == null)
            {
                questions.AcceptedAnswer = null;
            }

            db.Entry(questions).State = EntityState.Modified;
            db.SaveChanges();
            //updating view count finish


            List<Answers> aList = new List<Answers>();
            aList = db.AnswerDB.ToList();
            ViewBag.AnswerList = aList.Where(a => a.IdOfQuestions == int.Parse(ViewBag.idForAnswer));
            ViewBag.idForAnswer = Request.QueryString["idForAnswer"];
            ViewBag.titles = Request.QueryString["title"];
            ViewBag.question = Request.QueryString["question"];

            ViewBag.AcceptedAnswer = Request.QueryString["correctAnswer"];

            if (ViewBag.AcceptedAnswer == null)
            {
                @ViewBag.stringToShow = "";
            }
            else
            {
                @ViewBag.stringToShow = "Accepted Answer";
            }
            return View();
        }

        [HttpPost]
        public string pp(int votingUp, int questionId, int viewNumber, int numberOfAnswer, string titleString, string dateOfQuestion, string questionOfQuestions, string UserName, string correctAnswer, [Bind(Include = "ID,Vote,Answer1,View,Title,Question,Date")] Questions questions)
        {
            questions.ID = questionId;
            questions.Vote = votingUp;
            questions.View = viewNumber;
            questions.Date = DateTime.Parse(dateOfQuestion);
            questions.Answer1 = numberOfAnswer;
            questions.Title = titleString.ToString();
            questions.Question = questionOfQuestions;
            questions.UserName = UserName;
            questions.AcceptedAnswer = correctAnswer;
            db.Entry(questions).State = EntityState.Modified;
            db.SaveChanges();
            return "";
            //return (votingUp.ToString());
        }

        [HttpPost]
        public ActionResult GoForAnswer([Bind(Include = "ID,Answer,IdOfQuestions,UserName")] Answers answers , [Bind(Include = "ID,Vote,Answer1,View,Title,Question,Date,AcceptedAnswer, UserName")] Questions questions, string votingUp)
        {

            Response.Write(@ViewBag.pp);
            
            answers.IdOfQuestions = int.Parse(Request.QueryString["idForAnswer"]);
            answers.UserName = Request.QueryString["answerUserName"].ToString();
            if (answers.Answer==null)
            {
                return View();
            }

            if (!ModelState.IsValid)
            {
                List<Answers> aList = new List<Answers>();
                aList = db.AnswerDB.ToList();
                ViewBag.AnswerList = aList.Where(a => a.IdOfQuestions == int.Parse(ViewBag.idForAnswer));
                ViewBag.idForAnswer = Request.QueryString["idForAnswer"];
                ViewBag.titles = Request.QueryString["title"];
                ViewBag.question = Request.QueryString["question"];
                //return View();
            }
            //if (ModelState.IsValid)
            {
                db.AnswerDB.Add(answers);
                db.SaveChanges();

                //Updating answers count
                questions.ID = int.Parse(Request.QueryString["idForAnswer"]);
                questions.View = int.Parse(Request.QueryString["View"]);
                questions.Date = DateTime.Parse(Request.QueryString["date"]);
                questions.Answer1 = int.Parse(Request.QueryString["answerNumber"]) + 1;
                questions.Vote = int.Parse(Request.QueryString["vote"]);
                questions.UserName = Request.QueryString["UserName"];
                

                ViewBag.nn = questions.Answer1;

                db.Entry(questions).State = EntityState.Modified;
                db.SaveChanges();
                //updating answers count finish

                return RedirectToAction("GoForAnswer",new { idForAnswer = Request.QueryString["idForAnswer"], title = Request.QueryString["title"], question = Request.QueryString["question"], view = Request.QueryString["View"], date = Request.QueryString["date"], correctAnswer = Request.QueryString["correctAnswer"], answerNumber = questions.Answer1, vote = Request.QueryString["vote"] , UserName = Request.QueryString["UserName"] , answerUserName = Session["Username"].ToString() });
            }

            //return View(answers);
        }
        

        // GET: Questions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Vote,Answer,View,Title,Question,Date,UserName")] Questions questions)
        {
            questions.Date = DateTime.Now;
            questions.UserName = Session["Username"].ToString();
            if (ModelState.IsValid)
            {
                db.QuestionDB.Add(questions);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(questions);
        }


        //code for login and registration

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserAccount account)
        {
            if (ModelState.IsValid)
            {
                using (QuestionsDBContext db = new QuestionsDBContext())
                {
                    db.userAccount.Add(account);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = account.FirstName + " " + account.LastName + " successfully registered";
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        //with microsoft Identity code start here



        //with microsoft identity code ends here

        [HttpPost]
        public ActionResult Login(UserAccount user, [Bind(Include = "ID,NameOfSession")] SessionSave Sessions)
        {
            using (QuestionsDBContext db = new QuestionsDBContext())
            {
                var usr = db.userAccount.Single(u => u.UserName == user.UserName && u.Password == user.Password);
                Session["UserID"] = usr.UserID.ToString();
                Session["Username"] = usr.UserName.ToString();
                if (user != null)
                {
                    //authorization code starts here
                    bool userAutherised = true;
                    if (userAutherised)
                    {
                        //create the authentication ticket
                        var serializer = new JavaScriptSerializer();
                        string userData = serializer.Serialize(usr.UserName.ToString());

                        var authTicket = new FormsAuthenticationTicket(
                          1,
                          usr.UserName.ToString(),  //user id
                          DateTime.Now,
                          DateTime.Now.AddMinutes(20),  // expiry
                          true,  //true to remember
                          userData, //roles 
                          FormsAuthentication.FormsCookiePath
                        );
                        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                        Response.Cookies.Add(cookie);

                        HttpCookie chkUsername = new HttpCookie("Username");
                        chkUsername.Expires = DateTime.Now.AddSeconds(3600);
                        chkUsername.Value = Session["Username"].ToString();
                        Request.Cookies.Add(chkUsername);

                    }

                    //authorization code ends here



                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Username or Password is wrong");
                }
            }
            return View();
        }

        public ActionResult LoggedIn()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }

            else
            {
                return RedirectToAction("Login");
            }
        }


    }
}
