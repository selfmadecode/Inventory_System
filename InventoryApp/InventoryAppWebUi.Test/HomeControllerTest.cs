using System;
using System.Web.Mvc;
using inventoryAppDomain.Repository;
using inventoryAppDomain.Services;
using inventoryAppWebUi.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace InventoryAppWebUi.Test
{
    [TestClass]
    public class HomeControllerTest
    {
        private HomeController _acontroller;
        private Mock<IDrugCartService> _drugCartService;
        private Mock<IDrugService> _drugService;
        private Mock<ISupplierService> _suppService;
        private Mock<IOrderService> _orderService;
     
        public HomeControllerTest()
        {
            _drugCartService = new Mock<IDrugCartService>();
            _drugService = new Mock<IDrugService>();
            _orderService = new Mock<IOrderService>();
            _suppService = new Mock<ISupplierService>();

            _acontroller = new HomeController(_suppService.Object, _drugService.Object, _drugCartService.Object, _orderService.Object);
        }
        [TestMethod]
        public void TestMethod1()
        {
            var result = _acontroller.Index() as ViewResult;

            Assert.AreEqual("Index", result.ViewName);

        }
    }
}
