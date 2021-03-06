using AutoMapper;
using Classifieds.Domain.Model;
using Classifieds.Repository;
using Classifieds.Service;
using Classifieds.Web.Controllers;
using Classifieds.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using Xunit;

namespace Classifieds.XUnitTest.Controller
{
    public class TestMenuController
    {
        private Mock<IMenuService> mockMenuService;
        private IMapper mapper;
        private Mock<ISession> mockSession;

        public TestMenuController()
        {
            mockMenuService = new Mock<IMenuService>();
            mockSession = new Mock<ISession>();
            Initialize();
        }
        /// <summary>
        /// Test { public IActionResult SubMenus(int id) }
        /// </summary>
        [Fact]
        public void SubMenus()
        {
            var menus = FindAll();

            mockMenuService.Setup(m => m.FindAll(It.IsAny<Expression<Func<Menu, bool>>>(),
                It.IsAny<Expression<Func<Menu, Object>>[]>()))
                 .Returns(menus);

            var controller = new MenuController(mockMenuService.Object, mapper);
            OkObjectResult result = controller.SubMenus(1) as OkObjectResult;
            List<Menu> model = (List < Menu > )result.Value;

            Assert.Equal(200, result.StatusCode);
            Assert.Equal(6, model.Count);

        }
        /// <summary>
        /// Test for public IActionResult Create
        /// </summary>
        [Fact]
        public void Create_GET()
        {

            mockMenuService.Setup(m => m.FindAll(It.IsAny<Expression<Func<Menu, bool>>>(),
                It.IsAny<Expression<Func<Menu, object>>[]>())).Returns(FindAll());

            var controller = new MenuController(mockMenuService.Object, mapper);

            var result = controller.Create() as PartialViewResult;
            var menus = result.ViewData["Menus"] as List<SelectListItem>;

            Assert.Equal(6, menus.Count);
            
        }
        /// <summary>
        /// Test for public IActionResult Create
        /// </summary>
        [Fact]
        public void Create_ModelStateIsInvalid_POST()
        {
            MenuViewModel model = new MenuViewModel
            {
                Desc = "menu1 description",
                Url = "/Admin/Menu1",
                 ParentID = 3
            };
            mockMenuService.Setup(m => m.FindAll(It.IsAny<Expression<Func<Menu, bool>>>(),
                It.IsAny<Expression<Func<Menu, object>>[]>())).Returns(FindAll());

            var controller = new MenuController(mockMenuService.Object, mapper);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ModelState.AddModelError("Name", "Name is required");

            var result = controller.Create(model) as PartialViewResult;
            var statusCode = controller.HttpContext.Response.StatusCode;
            var menus = result.ViewData["Menus"] as List<SelectListItem>;
            var resultModel = result.Model as MenuViewModel;

            Assert.Equal(200, statusCode);
            Assert.Equal(6,menus.Count);
            Assert.True(menus[2].Selected);
            Assert.Equal("menu1 description", resultModel.Desc);
            Assert.Equal("menu1 description", model.Desc);
            Assert.Null(resultModel.Name);
        }
        /// <summary>
        /// Test for public IActionResult Create(MenuViewModel model)
        /// </summary>
        [Fact]
        public void CreateModelIsValid_POST()
        {
            MenuViewModel model = new MenuViewModel
            {
                Name="menu1", Desc="menu1 description", Url="/Admin/Menu1"
            };

            mockMenuService.Setup(m => m.FindAll(It.IsAny<Expression<Func<Menu, bool>>>(),
                It.IsAny<Expression<Func<Menu, object>>[]>())).Returns(FindAll());
           

            var controller = new MenuController(mockMenuService.Object, mapper);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Session = mockSession.Object;

            var result = controller.Create(model) as JsonResult;
            var statusCode = controller.HttpContext.Response.StatusCode;


            Assert.Equal(201, statusCode);
            Assert.Equal("Menu Created!", result.Value);
        }
        [Fact]
        public void Admin()
        {
            Menu menu = new Menu
            {
                ID = 2,
                Name = "menu1",
                Desc = "menu1 description",
                Url = "/Admin/Menu1",
                Admin = 0
            };

            mockMenuService.Setup(m => m.Find(It.IsAny<long>())).Returns(menu);

            var controller = new MenuController(mockMenuService.Object, mapper);

            var result = controller.Admin(2, true) as JsonResult;

            Assert.Equal("Saved!", result.Value);
        }
        /// <summary>
        /// Test for public IActionResult Edit(long id)
        /// </summary>
        [Fact]
        public void Edit_GET()
        {
            Menu menu = new Menu
            {
                ID = 6,
                Name = "Home",
                Desc = "Home Menu",
                ParentID = 2
            };

            mockMenuService.Setup(m => m.FindAll(It.IsAny<Expression<Func<Menu, bool>>>(),
                It.IsAny<Expression<Func<Menu, object>>[]>())).Returns(FindAllParent);

            mockMenuService.Setup(m => m.Find(It.IsAny<long>())).Returns(menu);

            var controller = new MenuController(mockMenuService.Object,
                mapper);

            var result = controller.Edit(1) as PartialViewResult;
            var model = result.Model as MenuViewModel;
            var menus = result.ViewData["Menus"] as List<SelectListItem>;

            Assert.Equal("Home", model.Name);
            Assert.Equal("Home Menu", model.Desc);
            Assert.Equal(3, menus.Count);
        }
        /// <summary>
        /// Test for public IActionResult Edit(long id) post
        /// Test when ModelState.Valid = false
        /// </summary>
        [Fact]
        public void Edit_InvalidModelState_POST()
        {
            MenuViewModel model = new MenuViewModel
            {

                ID = 6,
                Name = "Home",
                Desc = "Home Menu",
                ParentID = 2
            };

        
            mockMenuService.Setup(m => m.FindAll(It.IsAny<Expression<Func<Menu, bool>>>(),
                It.IsAny<Expression<Func<Menu, object>>[]>())).Returns(FindAllParent());

            var controller = new MenuController(mockMenuService.Object,
                mapper);

            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ModelState.AddModelError("Name", "Name is required");

            var result = controller.Edit(model) as PartialViewResult;
            var menus = result.ViewData["Menus"] as List<SelectListItem>;

            Assert.Equal(200, controller.HttpContext.Response.StatusCode);
            Assert.Equal(3, menus.Count);
            Assert.True(menus[1].Selected);
        }
        /// <summary>
        /// Test for public IActionResult Edit(long id) post
        /// Test when ModelState.Valid = true
        /// </summary>
        [Fact]
        public void Edit_ValidModelState_POST()
        {
            MenuViewModel model = new MenuViewModel
            {

                ID = 6,
                Name = "Home",
                Desc = "Home Menu",
                ParentID = 2
            };

            var controller = new MenuController(mockMenuService.Object,
                mapper);

            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var result = controller.Edit(model) as JsonResult;

            Assert.Equal("Edit Successful!", result.Value);
            Assert.Equal(201, controller.HttpContext.Response.StatusCode);
        }
        /// <summary>
        /// Initialize Mapper
        /// </summary>
        private void Initialize()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MenuViewModel, Menu>();
            });

            mapper = configuration.CreateMapper();

        }
        private IEnumerable<Menu> FindAll()
        {
            var menus = new List<Menu>
            {
                new Menu{ID=1, Name="Home",Type="HOME"},
                new Menu{ID=2, Name="Settings", Type="HOME"},
                new Menu{ID=3, Name="Manage",Type="SIDEBAR"},
                new Menu{ID=4, Name="Users",Type="SUBMENU",ParentID=3},
                new Menu{ID=5, Name="Categories",Type="SUBMENU",ParentID=3}
            };

            return menus;
        }
        private IEnumerable<Menu> FindAllParent()
        {
            var menus = new List<Menu>
            {
                new Menu{ID=1, Name="Home",Type="HOME"},
                new Menu{ID=2, Name="Settings", Type="HOME"},
                new Menu{ID=3, Name="Manage",Type="SIDEBAR"},
                new Menu{ID=4, Name="Users",Type="SUBMENU",ParentID=3},
                new Menu{ID=5, Name="Categories",Type="SUBMENU",ParentID=3}
            };

            return menus.GetRange(0,3);
        }
    }
}
