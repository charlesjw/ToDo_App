using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TODO.Models;

namespace TODO2
{
    public class TestClass
    {
        private TaskDBContext _context;

        public TestClass(TaskDBContext context)
        {
            _context = context;
        }

        public TaskModel AddTask(TaskModel task)
        {
            var taskAdd = _context.Tasks.Add(task);
            _context.SaveChanges();
            return taskAdd;
        }

        public List<TaskModel> GetAllTasks()
        {
            var dbQuery = from t in _context.Tasks
                          orderby t.Username
                          select t;

            return dbQuery.ToList();
        }

        public async Task<List<TaskModel>> GetAllTasksAsync()
        {
            var dbQuery = from t in _context.Tasks
                          orderby t.Username
                          select t;

            return await dbQuery.ToListAsync();
        }
    }
}
