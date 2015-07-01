using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace TODO.Models
{
    public interface ITaskDBContext
    {
        DbSet<TaskModel> Tasks { get; }
        int SaveChanges();
        void MarkAsModified(TaskModel task);
    }
}
