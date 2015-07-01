using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TODO.Models;

namespace TODO2
{
    // This test class is built to make our Unit Tests easier to test.
    // Specifically, this test class makes accessing, and editing our test
    // database context easier.
    public class TestClass
    {
        private TaskDBContext _context;

        public TestClass(TaskDBContext context)
        {
            _context = context;
        }

        // Adds task to our test database
        // task: TaskModel object to be saved to our test database
        public TaskModel AddTask(TaskModel task)
        {
            var taskAdd = _context.Tasks.Add(task);
            _context.SaveChanges();
            return taskAdd;
        }

        // Retrieves a list of all tasks from our test database
        // and returns the list in alphabetical order based on username
        public List<TaskModel> GetAllTasks()
        {
            var dbQuery = from t in _context.Tasks
                          orderby t.Username
                          select t;

            return dbQuery.ToList();
        }
    }
}
