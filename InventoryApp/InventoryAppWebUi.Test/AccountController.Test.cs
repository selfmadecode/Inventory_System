using System;
using inventoryAppDomain.Services;
using inventoryAppWebUi.Controllers;
using Moq;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using NUnit.Framework;

namespace InventoryAppWebUi.Test
{
    //[TestClass]
    public class UnitTest1 : IDisposable
    {

        private readonly AccountController _controller;
        private readonly Mock<IRoleService> _roleService;
        private readonly Mock<IProfileService> _profileService;
        private readonly Mock<INotificationService> _notificationService;
       
        public UnitTest1()
        {
            _roleService = new Mock<IRoleService>();
            _profileService = new Mock<IProfileService>();
            _notificationService = new Mock<INotificationService>();
            _controller = new AccountController(_roleService.Object, _profileService.Object, _notificationService.Object);
        }
        [Test]
        public void TestMethod1()
        {
            int x = 4, y = 5;
            var z = 9;
            var target = x + y;
            Assert.AreEqual(target, z);

        }
        [Test]
        public void ManageUserTest()
        {
            var target = _controller.ManageUsers() as ViewResult;

            Assert.AreNotEqual("ManageUsers", target.Model);
        }
        [Test]
        public void SignupTest()
        {
         
            var target = _controller.SignUp() as ViewResult;

            Assert.IsNotNull(target.ViewName);
        }
        [Test]
        public void Login()
        {
            var returnUrl = Guid.NewGuid().ToString();

            var result = _controller.Login(returnUrl) as ViewResult;

            Assert.AreEqual("Login", result.Model);
        }
        [Test]
        public void ChangeRole()
        {
            var roleId = Guid.NewGuid().ToString();

            var result = _controller.ChangeRole(roleId);

            Assert.IsNotNull(result);
        }

        public void Dispose()
        {
            _controller.Dispose();
        }
    }
}
