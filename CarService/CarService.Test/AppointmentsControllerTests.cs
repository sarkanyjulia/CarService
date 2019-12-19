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
    public class AppointmentsControllerTests
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
        public void GetAppointmets_OwnAppointmentListed()
        {
            //given   
            var testData = new List<Appointment> {
                new Appointment {
                    Mechanic = testMechanic1,
                    Partner = testPartner,
                    Time = DateTime.Now.AddHours(1),
                    WorkType = "maintenance",
                    Note = ""
                }
            };
            _context.Appointments.AddRange(testData);
            _context.SaveChanges();

            AppointmentsController underTest = new AppointmentsController(_context);
            SetupUser(underTest, "mechanic123");

            //when
            var result = underTest.GetAppointments();

            //then
            Assert.IsNotNull(result);
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOf<OkObjectResult>(okResult, result.GetType().ToString());
            var collectionResult = okResult.Value as IEnumerable<AppointmentDTO>;
            Assert.IsInstanceOf<IEnumerable<AppointmentDTO>>(collectionResult, result.GetType().ToString());
            Assert.AreEqual(1, collectionResult.Count());
        }

        [Test]
        public void GetAppointmets_NotOwnAppointmentNotListed()
        {
            //given   
            var testData = new List<Appointment> {
                new Appointment {
                    Mechanic = testMechanic2,
                    Partner = testPartner,
                    Time = DateTime.Now.AddHours(1),
                    WorkType = "maintenance",
                    Note = ""
                }
            };
            _context.Appointments.AddRange(testData);
            _context.SaveChanges();

            AppointmentsController underTest = new AppointmentsController(_context);
            SetupUser(underTest, "mechanic123");

            //when
            var result = underTest.GetAppointments();

            //then
            Assert.IsNotNull(result);
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOf<OkObjectResult>(okResult, result.GetType().ToString());
            var collectionResult = okResult.Value as IEnumerable<AppointmentDTO>;
            Assert.IsInstanceOf<IEnumerable<AppointmentDTO>>(collectionResult, result.GetType().ToString());
            Assert.AreEqual(0, collectionResult.Count());
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
