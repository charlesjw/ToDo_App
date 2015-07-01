using System;
using System.Linq;
using TODO.Models;

namespace TODO.Tests
{
    class TestTasksDbSet : TestDbSet<TaskModel>
    {
        public override TaskModel Find(params object[] keyValues)
        {
            return this.SingleOrDefault(task => task.ID == (int)keyValues.Single());
        }
    }
}
