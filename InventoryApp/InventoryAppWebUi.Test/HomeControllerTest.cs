using System;
using System.Text;
using System.Collections.Generic;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using inventoryAppDomain.Services;
using Moq;
using inventoryAppWebUi.Controllers;
using System.Web.Mvc;
using NUnit.Framework;

namespace InventoryAppWebUi.Test
{
    /// <summary>
    /// Summary description for HomeControllerTest
    /// </summary>
    //[TestClass]
    public class HomeControllerTest
    {
        public HomeControllerTest()
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
            Mock<IDrugCartService> _drugCartService = new Mock<IDrugCartService>();
            Mock<IDrugService> _drugService = new Mock<IDrugService>();
            Mock<ISupplierService> _suppService = new Mock<ISupplierService>();
            Mock<IOrderService> _orderService = new Mock<IOrderService>();

            var _acontroller = new HomeController(_suppService.Object, _drugService.Object, _drugCartService.Object, _orderService.Object);

            var result = _acontroller.Index() as ViewResult;

            Assert.IsNotNull(result.ViewName);
        }

        [Test]
        public void AboutTest()
        {
            Mock<IDrugCartService> _drugCartService = new Mock<IDrugCartService>();
            Mock<IDrugService> _drugService = new Mock<IDrugService>();
            Mock<ISupplierService> _suppService = new Mock<ISupplierService>();
            Mock<IOrderService> _orderService = new Mock<IOrderService>();

            var _acontroller = new HomeController(_suppService.Object, _drugService.Object, _drugCartService.Object, _orderService.Object);

            var result = _acontroller.About() as ViewResult;

            Assert.AreEqual(result.ViewName, "");
        }


    }
}
