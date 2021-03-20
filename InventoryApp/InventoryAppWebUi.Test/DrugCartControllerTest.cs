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
    public class DrugCartControllerTest
    {
      

     

        [TestMethod]
        public void IndexTest()
        {
           

          Mock<IDrugCartService> _mockDrug = new Mock<IDrugCartService>();
            //_mockDrug.Setup(z => z.RefreshCart(userId.ToString()));
           var controller = new DrugCartController(_mockDrug.Object);

            var result = controller.RemoveAllCart() as ViewResult;

            Assert.AreNotEqual("Index", result.ViewName);
        }
    }
}
