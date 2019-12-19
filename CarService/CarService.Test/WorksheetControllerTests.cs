using System;
using System.Collections.Generic;
using System.Linq;
using CarService.Data;
using CarService.Persistence;
using CarService.WebAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace CarService.Test
{
    [TestFixture]
    public class WorksheetControllerTests
    {
        private CarServiceContext _context { get; set; }

        private AppUser testMechanic1;
        private AppUser testMechanic2;
        private AppUser testPartner;
       

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CarServiceContext>()
                .UseInMemoryDatabase("CarServiceTest" + DateTime.Now.ToFileTimeUtc())
                .Options;

            _context = new CarServiceContext(options);
            _context.Database.EnsureCreated();
            AddTestUsers();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void PostWorksheet_Ok()
        {
            // GIVEN
            DateTime testTime = DateTime.Now.AddHours(1);
            int testId = 1;
            string testWorkType = "maintenance";
            Appointment testAppointment = new Appointment
            {
                Id = 1,
                Mechanic = testMechanic1,
                Partner = testPartner,
                Time = testTime,
                WorkType = testWorkType,
                Note = ""
            };
            _context.Appointments.Add(testAppointment);
            _context.SaveChanges();

            WorksheetDTO testData = new WorksheetDTO
            {
                Id = testId,
                Appointment = new AppointmentDTO
                {
                    Id = testId,
                    Time = testTime,
                    WorkType = testWorkType,
                    Partner = new UserDTO
                    {
                        Id = 3,
                        Name = "Partner"
                    }                   
                },
                Items = new List<WorkItemDTO>(),
                FinalPrice = 0,
                Closed = true
            };
            WorksheetsController underTest = new WorksheetsController(_context);
            SetupUser(underTest, "mechanic123");
            // WHEN
            var result = underTest.PostWorksheet(testData);
            // THEN
            Assert.IsNotNull(result);            
            var okResult = result as CreatedAtActionResult;
            Assert.IsInstanceOf<CreatedAtActionResult>(okResult, result.GetType().ToString());
        }

        [Test]
        public void PostWorksheet_Unauthorized()
        {
            // GIVEN
            DateTime testTime = DateTime.Now.AddHours(1);
            int testId = 1;
            string testWorkType = "maintenance";
            Appointment testAppointment = new Appointment
            {
                Id = 1,
                Mechanic = testMechanic1,
                Partner = testPartner,
                Time = testTime,
                WorkType = testWorkType,
                Note = ""
            };
            _context.Appointments.Add(testAppointment);
            _context.SaveChanges();

            WorksheetDTO testData = new WorksheetDTO
            {
                Id = testId,
                Appointment = new AppointmentDTO
                {
                    Id = testId,
                    Time = testTime,
                    WorkType = testWorkType,
                    Partner = new UserDTO
                    {
                        Id = 3,
                        Name = "Partner"
                    }
                },
                Items = new List<WorkItemDTO>(),
                FinalPrice = 0,
                Closed = true
            };
            WorksheetsController underTest = new WorksheetsController(_context);
            SetupUser(underTest, "mechanic456");
            // WHEN
            var result = underTest.PostWorksheet(testData);
            // THEN
            Assert.IsNotNull(result);
            var objectResult = result as UnauthorizedResult;
            Assert.IsInstanceOf<UnauthorizedResult>(objectResult, result.GetType().ToString());
        }



        private void SetupUser(ControllerBase controller, string username)
        {
            var mockContext = new Mock<HttpContext>(MockBehavior.Strict);
            mockContext.SetupGet(hc => hc.User.Identity.Name).Returns(username);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = mockContext.Object
            };
        }

        private void AddTestUsers()
        {
            testMechanic1 = new AppUser { UserName = "mechanic123" };
            testMechanic2 = new AppUser { UserName = "mechanic456" };
            testPartner = new AppUser { UserName = "partner123" };          
            _context.Users.AddRange(new List<AppUser> {
                testMechanic1,
                testMechanic2,
                testPartner
            });            
            _context.SaveChanges();
        }

        
    }
}
