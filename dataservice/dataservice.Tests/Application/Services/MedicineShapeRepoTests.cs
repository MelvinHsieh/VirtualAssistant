using Application.Common.Models;
using Application.Repositories;
using Application.Repositories.Interfaces;
using Domain.Entities.MedicalData;
using Infrastructure.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace dataservice.Tests.Application.Services
{
    [TestClass]
    public class MedicineShapeRepoTests : DatabaseTestBase
    {
        private IShapeRepo _shapeRepo;
        private MedicineDbContext _context;

        [TestInitialize]
        public void Initialize()
        {
            _context = this.CreateMedicineTestContext();
            _shapeRepo = new ShapeRepo(_context);
        }

        [TestMethod]
        public void AddShape_NewShape_ReturnsTrueResult()
        {
            Result result = _shapeRepo.AddShape("New Shape");
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void AddShape_ExistingShape_ReturnsFalseResult()
        {
            Result result = _shapeRepo.AddShape("Vierkant");
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void FindShape_ExistingShape_ReturnsShape()
        {
            MedicineShape? result = _shapeRepo.FindShape("Vierkant");
            if (result != null)
            {
                Assert.AreEqual("Vierkant", result.Shape);
            }
            else
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void FindShape_NonExistingShape_ReturnsNull()
        {
            MedicineShape? result = _shapeRepo.FindShape("asdjkasjdijask");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetShapes_ReturnsAllShapes()
        {
            var shapeCount = _context.Medicine_Shapes.Count();

            IEnumerable<MedicineShape> shapes = _shapeRepo.GetShapes();

            Assert.AreEqual(shapes.Count(), shapeCount);
        }

        [TestMethod]
        public void RemoveShape_ExistingShape_ReturnsTrueResult()
        {
            Result result = _shapeRepo.RemoveShape("Vierkant");
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void RemoveShape_NonExistingShape_ReturnsFalseResult()
        {
            Result result = _shapeRepo.RemoveShape("djsdjasjdas");
            Assert.IsFalse(result.Success);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this._context.Dispose();
        }
    }
}
