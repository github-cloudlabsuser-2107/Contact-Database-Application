using NUnit.Framework;
using System.Web.Mvc;
using CRUD_application_2.Controllers;
using CRUD_application_2.Models;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Legacy;

namespace CRUD_application_2.Tests.Controllers
{
    [TestFixture]
    public class UserControllerTest
    {
        private UserController _controller;
        private List<User> _users;

        [SetUp]
        public void Setup()
        {
            // Initialize the controller and a list of users
            _controller = new UserController();
            _users = new List<User>
            {
                new User { Id = 1, Name = "Test User 1", Email = "test1@example.com" },
                new User { Id = 2, Name = "Test User 2", Email = "test2@example.com" }
            };

            // Set the static userlist field in UserController
            UserController.userlist = _users;
        }

        [Test]
        public void Index_ReturnsCorrectViewWithModel()
        {
            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.That(result.ViewName, Is.EqualTo("Index"));
            Assert.That(result.Model as List<User>, Is.EquivalentTo(_users));
        }

        [Test]
        public void Details_ReturnsCorrectViewWithModel()
        {
            // Act
            var result = _controller.Details(1) as ViewResult;

            // Assert
            Assert.That(result.ViewName, Is.EqualTo("Details"));
            Assert.That(result.Model as User, Is.EqualTo(_users[0]));
        }

        [Test]
        public void Create_Post_AddsUserToListAndRedirects()
        {
            // Arrange
            var newUser = new User { Id = 3, Name = "Test User 3", Email = "test3@example.com" };

            // Act
            var result = _controller.Create(newUser) as RedirectToRouteResult;

            // Assert
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
            Assert.That(UserController.userlist, Does.Contain(newUser));
        }

        [Test]
        public void Edit_Post_UpdatesUserAndRedirects()
        {
            // Arrange
            var updatedUser = new User { Id = 1, Name = "Updated User", Email = "updated@example.com" };

            // Act
            var result = _controller.Edit(1, updatedUser) as RedirectToRouteResult;

            // Assert
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
            Assert.That(UserController.userlist.First(u => u.Id == 1).Name, Is.EqualTo(updatedUser.Name));
            Assert.That(UserController.userlist.First(u => u.Id == 1).Email, Is.EqualTo(updatedUser.Email));
        }

        [Test]
        public void Delete_Post_RemovesUserAndRedirects()
        {
            // Act
            var result = _controller.Delete(1, new FormCollection()) as RedirectToRouteResult;

            // Assert
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
            Assert.That(UserController.userlist.FirstOrDefault(u => u.Id == 1), Is.Null);
        }
    }
}