using System;
using System.Data;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;


namespace TODO.Models
{
    public class TaskModel
    {
        public int ID { get; set; }
        
        [Required]
        public string TaskHeader { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; }
        
        [MinLength(0)]
        public string Description { get; set; }

        [MinLength(1)]
        public string Username { get; set; }
    }


    public class TaskDBContext : DbContext, ITaskDBContext
    {
         public TaskDBContext() : base("name=TaskDBContext")
        {
        }

        public virtual DbSet<TaskModel> Tasks { get; set; }

        public void MarkAsModified(TaskModel task)
        {
            Entry(task).State = System.Data.Entity.EntityState.Modified;
        }
    }
}