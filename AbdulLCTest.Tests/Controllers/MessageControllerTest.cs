using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AbdulLCTest.Business;
using System.Web.Mvc;
using AbdulLCTest.Domain;
using AbdulLCTest.UI;
using AbdulLCTest.UI.ViewModels;
using AbdulLCTest.UI.Controllers;
using System.Collections.Generic;
using System.Web;
using System.Security.Principal;

namespace AbdulLCTest.Tests.Controllers
{
    [TestClass]
    public class MessageControllerTest
    {
        #region "Test Startup"
        private const string UserSessionKey = "test@test.com";
        private Mock<IPostServices> controllerpostserv = null;


        [TestInitialize]
        public void Setup()
        {
            controllerpostserv = new Mock<IPostServices>();
        }
        #endregion

        [TestMethod]
        public void ShouldGetIndexViewForAllPostList_Test()
        {
            // Arrange
            controllerpostserv = new Mock<IPostServices>();

            MessageController msgview = new MessageController(controllerpostserv.Object);
            // Act
            ViewResult result = msgview.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ViewName, "Index");
        }

        [TestMethod]
        public void ShouldGetAllPostMessageToController_Test()
        {
            //Arrange
            controllerpostserv = new Mock<IPostServices>();

            controllerpostserv.Setup(r => r.GetAllPosts()).Returns(GetDummyPostList());

            //Act
            MessageController controller = new MessageController(controllerpostserv.Object);
            var viewresult = controller.Index() as ViewResult;
            var result = viewresult.ViewData.Model as IList<PostBDO>;

            //Assert
            Assert.IsNotNull(viewresult);
            Assert.IsNotNull(result);

        }

        [TestMethod]
        public void ShouldGetPostMessageByIdToController_Test()
        {
            //Arrange
            int Id = 1;
            controllerpostserv = new Mock<IPostServices>();

            controllerpostserv.Setup(r => r.GetPostById(Id)).Returns(GetDummyPost());

            //Act
            MessageController controller = new MessageController(controllerpostserv.Object);
            var viewresult = controller.Details(Id) as ViewResult;
            var result = viewresult.ViewData.Model as PostBDO;

            //Assert
            Assert.IsNotNull(viewresult);
            Assert.IsNotNull(result);

        }

        [TestMethod]
        public void ShouldAddNewPostMessage_Test()
        {
            //Arrange
            int Id = 4;
            controllerpostserv = new Mock<IPostServices>();

            var controllerContext = new Mock<ControllerContext>();
            var principal = new Moq.Mock<IPrincipal>();
            principal.Setup(p => p.IsInRole("Admin")).Returns(true);
            principal.SetupGet(x => x.Identity.Name).Returns(UserSessionKey);
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);

            controllerpostserv.Setup(r => r.CreatePost(GetDummyPost())).Returns(Id);

            //Act
            MessageController controller = new MessageController(controllerpostserv.Object);
            controller.ControllerContext = controllerContext.Object;

            var result = controller.Create(AddNewDummyPost()) as RedirectToRouteResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteValues["Action"], "Index");
        }

        [TestMethod]
        public void ShouldEditPostMessage_Test()
        {
            //Arrange
            int Id = 1;
            controllerpostserv = new Mock<IPostServices>();

            var controllerContext = new Mock<ControllerContext>();
            var principal = new Moq.Mock<IPrincipal>();
            principal.Setup(p => p.IsInRole("Admin")).Returns(true);
            principal.SetupGet(x => x.Identity.Name).Returns(UserSessionKey);
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);

            controllerpostserv.Setup(r => r.UpdatePost(GetDummyPost())).Returns(true);

            //Act
            MessageController controller = new MessageController(controllerpostserv.Object);
            controller.ControllerContext = controllerContext.Object;

            var result = controller.Edit(Id, AddNewDummyPost()) as RedirectToRouteResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteValues["Action"], "Index");
        }

        [TestMethod]
        public void ShouldDeletePostMessage_Test()
        {
            //Arrange
            int Id = 1;
            controllerpostserv = new Mock<IPostServices>();


            var userMock = new Mock<IPrincipal>();
            userMock.Setup(p => p.IsInRole("Admin")).Returns(true);

            var contextMock = new Mock<HttpContextBase>();
            contextMock.SetupGet(ctx => ctx.User).Returns(userMock.Object);

            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.SetupGet(con => con.HttpContext)
                                 .Returns(contextMock.Object);

            controllerpostserv.Setup(r => r.DeletePost(It.IsAny<int>())).Returns(true);

            //Act
            MessageController controller = new MessageController(controllerpostserv.Object);
            controller.ControllerContext = controllerContextMock.Object;

            var result = controller.Delete(Id) as ViewResult;

            //Assert
            userMock.Verify(p => p.IsInRole("Admin"),Times.Never);
            Assert.IsNotNull(result);
        }

        #region "Setup Dummy Test Data"

        private PostBDO GetDummyPost()
        {
            //Arrange
            var postdummylist = new PostBDO
            {
                Id = 1,
                Subject = "Post01",
                Description = "test post 1",
                CreatedBy ="24cb48d7-bcd0-4dbe-9356-b9ab34e1d043",
                CreatedDate =DateTime.Now
            };

            return postdummylist;
        }

        private IList<PostBDO> GetDummyPostList()
        {
            //Arrange
            var postdummylist = new List<PostBDO>
            {
            new PostBDO {Id = 1, Subject = "Post01", Description = "test post 1", CreatedBy="24cb48d7-bcd0-4dbe-9356-b9ab34e1d043", CreatedDate=DateTime.Now},
            new PostBDO {Id = 2, Subject = "Post02", Description = "test post 2", CreatedBy="24cb48d7-bcd0-4dbe-9356-b9ab34e1d043", CreatedDate=DateTime.Now},
            new PostBDO {Id = 3, Subject = "Post03", Description = "test post 3", CreatedBy="24cb48d7-bcd0-4dbe-9356-b9ab34e1d043", CreatedDate=DateTime.Now},
            };

            return postdummylist;
        }

        private PostMessageViewModel AddNewDummyPost()
        {
            //Arrange
            var postdummylist = new PostMessageViewModel
            {
                Id = 4,
                Subject = "Post04",
                Description = "test post 4"
            };

            return postdummylist;
        }

        #endregion


        #region "Cleanup resources"
        [TestCleanup]
        public void CleanUp()
        {
            controllerpostserv = null;
        }
        #endregion
    }
}
