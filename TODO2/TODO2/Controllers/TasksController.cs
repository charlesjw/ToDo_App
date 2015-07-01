using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TODO.Models;

namespace TODO.Controllers
{
    public class TasksController : Controller
    {
        // Enable other classes to pass in a different object (ITaskDBContext) for
        // the context class. This allows us to pass in a test context during our
        // unit tests.
        private ITaskDBContext db = new TaskDBContext();

        // Definition and Declaration for constructor
        public TasksController() { }

        public TasksController(ITaskDBContext context)
        {
            db = context;
        }

        // GET: /Tasks/
        // This method queries database for tasks that belong to currently signed in user.
        // date: Optional parameter that limits list to tasks which must be done by date 
        //          specified by this parameter
        public ActionResult Index(DateTime? date)
        {
            if(date.Equals(null)){
                date = DateTime.MaxValue;
            }

            IEnumerable<TaskModel> list = db.Tasks.SqlQuery("SELECT * FROM dbo.TaskModels WHERE Username='" + User.Identity.Name + "'").ToList();
            list = list.Where(d => d.Deadline.CompareTo(date) <= 0);
            
            return View(list);
        }

        // GET: /Tasks/Details/5
        // Shows task details when user selects one specific task's details
        // id: task database entry id number used to find chosen task in 
        //          database
        public ActionResult Details(int id = 0)
        {
            TaskModel taskmodel = db.Tasks.Find(id);
            if (taskmodel == null)
            {
                return HttpNotFound();
            }
            return View(taskmodel);
        }

        // GET: /Tasks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Tasks/Create
        // Adds task to database when user chooses to Create a new task.
        // taskmodel: object assembled with user's input to be added 
        //              and saved to database
        [HttpPost]
        public ActionResult Create(TaskModel taskmodel)
        {
            if (taskmodel.Username == null)
            {
                taskmodel.Username = User.Identity.Name;
            }
            if (ModelState.IsValid)
            {
                db.Tasks.Add(taskmodel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(taskmodel);
        }

        // GET: /Tasks/Edit/5
        // Edits specific task's details
        // id: task database entry id number used to find chosen task in 
        //          database
        public ActionResult Edit(int id = 0)
        {
            TaskModel taskmodel = db.Tasks.Find(id);
            taskmodel.Username = User.Identity.Name;
            if (taskmodel == null)
            {
                return HttpNotFound();
            }
            return View(taskmodel);
        }

        // POST: /Tasks/Edit/5
        // Edits specific task's details
        // taskmodel: task to be altered in database
        [HttpPost]
        public ActionResult Edit(TaskModel taskmodel)
        {
            if (ModelState.IsValid)
            {
                db.MarkAsModified(taskmodel);
                taskmodel.Username = User.Identity.Name;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(taskmodel);
        }

        // GET: /Tasks/Delete/5
        // Deletes task from database
        // id: task database entry id number used to find task to be  
        //          deleted in database
        public ActionResult Delete(int id = 0)
        {
            TaskModel taskmodel = db.Tasks.Find(id);
            if (taskmodel == null)
            {
                return HttpNotFound();
            }
            return View(taskmodel);
        }

        // POST: /Tasks/Delete/5
        // Confirms delete action.
        // id: task database entry id number used to find task to be  
        //          deleted in database
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            TaskModel taskmodel = db.Tasks.Find(id);
            db.Tasks.Remove(taskmodel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            //db.Dispose();
            base.Dispose(disposing);
        }
    }
}