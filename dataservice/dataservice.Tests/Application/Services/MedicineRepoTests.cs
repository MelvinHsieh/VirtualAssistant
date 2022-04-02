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
    internal class MedicineRepoTests : DatabaseTestBase
    {
        private IMedicineRepo _medicineRepo;
        private MedicineDbContext _context;

        [TestInitialize]
        public void Initialize()
        {
            _context = this.CreateMedicineTestContext();
            _medicineRepo = new MedicineRepo(_context, new ShapeRepo(_context), new ColorRepo(_context), new TypeRepo(_context), new DoseUnitRepo(_context));
        }

        [TestMethod]
        public void AddMedicine_ValidData_ReturnsTrueResult()
        {
            Result result = _medicineRepo.AddMedicine("Paracetamol", "Migraine", 20, "mg", "Tablet", "Wit", "Rond");
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void AddMedicine_InvalidData_ReturnsFalseResult()
        {
            Result result = _medicineRepo.AddMedicine(null, null, 0, null, null, null, null);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void GetMedicine_ExistingMedicineId_ReturnsMedicine()
        {
            Medicine? result = _medicineRepo.GetMedicine(1);
            if (result != null)
            {
                Assert.IsNotNull(result);
                Assert.AreEqual(result.Id, 1);
            }
            else
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void GetMedicine_NonExistingMedicineId_ReturnsNull()
        {
            Medicine? result = _medicineRepo.GetMedicine(-1);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAllMedicine_ReturnsAllMedicine()
        {
            var medicineCount = _context.Medicine.Count();

            IEnumerable<Medicine> medicines = _medicineRepo.GetAllMedicine();

            Assert.AreEqual(medicines.Count(), medicineCount);
        }

        [TestMethod]
        public void RemoveMedicine_ExistingMedicine_ReturnsTrueResult()
        {
            Result result = _medicineRepo.RemoveMedicine(1);
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void RemoveMedicine_NonExistingMedicine_ReturnsFalseResult()
        {
            Result result = _medicineRepo.RemoveMedicine(-1);
            Assert.IsFalse(result.Success);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this._context.Dispose();
        }
    }
}
