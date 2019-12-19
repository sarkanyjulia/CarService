using System;
using System.Collections.Generic;
using System.Linq;
using CarService.Persistence;
using CarService.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace CarService.Test
{
    [TestFixture]
    public class WorkItemsControllerTests
    {
        private CarServiceContext _context { get; set; }

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CarServiceContext>()
                .UseInMemoryDatabase("CarServiceTest")
                .Options;

            _context = new CarServiceContext(options);
            _context.Database.EnsureCreated();            
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void GetWorkItemsTest()
        {
            //given      
            var testData = new List<WorkItem> {
                new WorkItem { Item = "item 1", Price=1000 },
                new WorkItem { Item = "item 2", Price=1000 },
                new WorkItem { Item = "item 3", Price=1000 }
            };
            _context.WorkItems.AddRange(testData);
            _context.SaveChanges();
            WorkItemsController underTest = new WorkItemsController(_context);

            //when
            var result = underTest.GetWorkItems();

            //then
            Assert.IsNotNull(result);
            var okResult = result as OkObjectResult;           
            Assert.IsInstanceOf<OkObjectResult>(okResult, result.GetType().ToString());
            var collectionResult = okResult.Value as IEnumerable<WorkItem>;
            Assert.IsInstanceOf<IEnumerable<WorkItem>>(collectionResult, result.GetType().ToString());
            Assert.AreEqual(3, collectionResult.Count());
            Assert.AreEqual(testData, collectionResult);
        }

        [Test]
        public void GetWorkItemsTest_Empty()
        {
            //given      
            var testData = new List<WorkItem>();
            WorkItemsController underTest = new WorkItemsController(_context);

            //when
            var result = underTest.GetWorkItems();

            //then
            Assert.IsNotNull(result);
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOf<OkObjectResult>(okResult, result.GetType().ToString());
            var collectionResult = okResult.Value as IEnumerable<WorkItem>;
            Assert.IsInstanceOf<IEnumerable<WorkItem>>(collectionResult, result.GetType().ToString());
            Assert.AreEqual(0, collectionResult.Count());            
        }
    }
}
