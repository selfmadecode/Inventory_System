using System;
using System.Text;
using System.Collections.Generic;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using inventoryAppDomain.Services;
using Moq;
using inventoryAppWebUi.Controllers;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using inventoryAppDomain.Repository;

namespace InventoryAppWebUi.Test
{
    /// <summary>
    /// Summary description for RoleControllerTest
    /// </summary>
    //[TestClass]
    public class RoleControllerTest
    {
        public RoleControllerTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [Test]
        public void IndexTest()
        {
            Mock<IRoleService> _mockRoles = new Mock<IRoleService>();


            var controller = new RoleController(_mockRoles.Object);

            var result = controller.Index() as ViewResult;

            Assert.That(result.ViewName, Is.EqualTo(""));
        }

        [Test]
        public void CreateTest()
        {
            var roleName = "Admin";
            //var newRole = new IdentityRole
            //{
            //    Id = "abcdef",
            //    Name = roleName,
                
            //};

            Mock<IRoleService> _mockRoles = new Mock<IRoleService>();
            var controller = new RoleController(_mockRoles.Object);

            controller.Create(roleName);
         
            var result = controller.Index();

            Assert.IsNotNull(result);
        }
    }
}
