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
    public class MedicineTypeRepoTests : DatabaseTestBase
    {
        private ITypeRepo _typeRepo;
        private MedicineDbContext _context;

        [TestInitialize]
        public void Initialize()
        {
            _context = this.CreateMedicineTestContext();
            _typeRepo = new TypeRepo(_context);
        }

        [TestMethod]
        public void AddType_NewType_ReturnsTrueResult()
        {
            Result result = _typeRepo.AddType("New Type");
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void AddType_ExistingType_ReturnsFalseResult()
        {
            Result result = _typeRepo.AddType("Tablet");
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void FindType_ExistingType_ReturnsType()
        {
            MedicineType? result = _typeRepo.FindType("Tablet");
            if (result != null)
            {
                Assert.AreEqual("Tablet", result.Type);
            }
            else
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void FindType_NonExistingType_ReturnsNull()
        {
            MedicineType? result = _typeRepo.FindType("asdjkasjdijask");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetTypes_ReturnsAllTypes()
        {
            var typeCount = _context.Medicine_Types.Count();

            IEnumerable<MedicineType> types = _typeRepo.GetTypes();

            Assert.AreEqual(types.Count(), typeCount);
        }

        [TestMethod]
        public void RemoveType_ExistingType_ReturnsTrueResult()
        {
            Result result = _typeRepo.RemoveType("Tablet");
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void RemoveType_NonExistingType_ReturnsFalseResult()
        {
            Result result = _typeRepo.RemoveType("djsdjasjdas");
            Assert.IsFalse(result.Success);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this._context.Dispose();
        }
    }
}
