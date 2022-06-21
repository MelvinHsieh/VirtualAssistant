using Application.Common.Models;
using Application.Repositories;
using Application.Repositories.Interfaces;
using Domain.Entities.MedicalData;
using Infrastructure.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace dataservice.Tests.Repositories
{
    [TestClass]
    public class MedicineColorRepoTests : DatabaseTestBase
    {
        private IColorRepo _colorRepo;
        private MedicineDbContext _context;

        [TestInitialize]
        public void Initialize()
        {
            _context = CreateMedicineTestContext();
            _colorRepo = new ColorRepo(_context);
        }

        [TestMethod]
        public void AddColor_NewColor_ReturnsTrueResult()
        {
            Result result = _colorRepo.AddColor("New Color");
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void AddColor_ExistingColor_ReturnsFalseResult()
        {
            Result result = _colorRepo.AddColor("Rood");
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void FindColor_ExistingColor_ReturnsColor()
        {
            MedicineColor? result = _colorRepo.FindColor("Rood");
            if (result != null)
            {
                Assert.AreEqual("Rood", result.Color);
            }
            else
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void FindColor_NonExistingColor_ReturnsNull()
        {
            MedicineColor? result = _colorRepo.FindColor("asdjkasjdijask");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetColors_ReturnsAllColors()
        {
            var colorCount = _context.Medicine_Colors.Count();

            IEnumerable<MedicineColor> colors = _colorRepo.GetColors();

            Assert.AreEqual(colors.Count(), colorCount);
        }

        [TestMethod]
        public void RemoveColor_ExistingColor_ReturnsTrueResult()
        {
            Result result = _colorRepo.RemoveColor("Rood");
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void RemoveColor_NonExistingColor_ReturnsFalseResult()
        {
            Result result = _colorRepo.RemoveColor("djsdjasjdas");
            Assert.IsFalse(result.Success);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _context.Dispose();
        }
    }
}
