using System;
using System.Data.Entity;
using TODO.Models;

namespace TODO.Tests
{
    public class TestTaskDbContext : ITaskDBContext
    {
        public TestTaskDbContext()
        {
            this.Tasks = new TestTasksDbSet();
        }

        public DbSet<TaskModel> Tasks { get; set; }

        public int SaveChanges()
        {
            return 0;
        }

        public void MarkAsModified(TaskModel task) { }
        public void Dispose() { }
    }
}