using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TODO2;
using Moq;
using System.Net;
using TODO.Models;
using TODO.Controllers;
using TODO2.Tests;
using TODO2.Models;
using System.Data.Entity;

namespace TODO2.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        // Test to verify our Mock DB is working correctly, and TestClass basic 
        // functionality is working.
        [TestMethod]
        public void CreateTask_SaveTask()
        {
            var modelOne = Build_User_Model_For_Testing("TestUser0", "chasercize");
            var taskOne = Build_Task_For_Testing("", new DateTime(2015, 7, 5, 5, 5, 5), "", modelOne.UserName); 

            var mockSet = new Mock<DbSet<TaskModel>>();
            var mockDbContext = new Mock<TaskDBContext>();
            mockDbContext.Setup(m => m.Tasks).Returns(mockSet.Object);
            var testClass = new TestClass(mockDbContext.Object);
            
            testClass.AddTask(taskOne);

            mockSet.Verify(m => m.Add(It.IsAny<TaskModel>()), Times.Once());
            mockDbContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        // Test our task-builder, our Mock DB, and our TestClass getAllTasks() method.
        [TestMethod]
        public void GetAllTasks_Order_By_Username()
        {
            var taskData = new List<TaskModel>
            {
                Build_Task_For_Testing("B's Task", new DateTime(2015, 7, 3, 5, 5, 5), "B's Test Task", "B"),
                Build_Task_For_Testing("C's Task", new DateTime(2015, 7, 5, 5, 5, 5), "C's Test Task", "C"),
                Build_Task_For_Testing("D's Task", new DateTime(2015, 7, 6, 5, 5, 5), "D's Test Task", "D")
            }.AsQueryable();

            var taskData2 = new List<TaskModel> { }.AsQueryable();

            var mockSet = new Mock<DbSet<TaskModel>>();
            mockSet.As<IQueryable<TaskModel>>().Setup(m => m.Provider).Returns(taskData.Provider);
            mockSet.As<IQueryable<TaskModel>>().Setup(m => m.Expression).Returns(taskData.Expression);
            mockSet.As<IQueryable<TaskModel>>().Setup(m => m.ElementType).Returns(taskData.ElementType);
            mockSet.As<IQueryable<TaskModel>>().Setup(m => m.GetEnumerator()).Returns(taskData.GetEnumerator());

            var mockDbContext = new Mock<TaskDBContext>();
            mockDbContext.Setup(t => t.Tasks).Returns(mockSet.Object);

            var testClass = new TestClass(mockDbContext.Object);
            var allTasks = testClass.GetAllTasks();

            Assert.AreEqual(3, allTasks.Count);
            Assert.AreEqual("B", allTasks[0].Username);
            Assert.AreEqual("C", allTasks[1].Username);
            Assert.AreEqual("D", allTasks[2].Username);
        }

        [TestMethod]
        private void Test_Username_Generate_Method()
        {
            int listLength = 30;
            var namesList = new List<string>(listLength);
            string name = "a";
            namesList.Add(name);
            
            for (int i = 0; i < listLength - 1; i++)
            {
                string newName = Build_Username_For_Testing(name);
                namesList.Add(newName);
                name = newName;
            }

            Assert.AreEqual(listLength, namesList.Count());
            Assert.AreEqual("a", namesList[0]);
            Assert.AreEqual("b", namesList[1]);
            Assert.AreEqual("c", namesList[2]);
            Assert.AreEqual("aa", namesList[26]);
            Assert.AreEqual("aa", namesList[27]);
        }


        // Helper method to build fake user easily for testing by calling Build_User_Model_For_Testing
        private RegisterModel Build_User_Model_For_Testing(string _username, string _password)
        {
            RegisterModel buildRegModel = new RegisterModel();
            buildRegModel.UserName = _username;
            buildRegModel.Password = _password;
            return buildRegModel;
        }

        // Helper method to build tasks easily for testing by calling Build_Task_For_Testing
        private TaskModel Build_Task_For_Testing(string header, DateTime date, string desc, string username)
        {
            TaskModel buildTask = new TaskModel();
            buildTask.TaskHeader = header;
            buildTask.Deadline = date;
            buildTask.Username = username;
            return buildTask;
        }

        // Helper method to generate username when test cases require a large number
        // of unique users
        private string Build_Username_For_Testing(string prevName)
        {
            int prevCnt = prevName.Count();
            string newName = "";
            Char letter = (Char) prevName[prevCnt - 1];

            if (letter.Equals('z'))
            {
                for (int i = 0; i <= prevCnt; i++)
                {
                    newName += "a";
                }
            }
            else
            {
                int newChar = ++letter;
                Console.Write(newChar + "\r\n");
                for (int i = 0; i < prevCnt; i++)
                {
                    newName += (Char)newChar;
                    Console.Write(newName + "\r\n");
                }
            }

            return newName;
        }
    }
}
