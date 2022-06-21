using Application.Common.Models;
using Application.Repositories;
using Application.Repositories.Interfaces;
using dataservice.Tests;
using Domain.Entities.MedicalData;
using Infrastructure.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace dataservice.Tests.Repositories
{
    [TestClass()]
    public class CareWorkerFunctionRepoTests : DatabaseTestBase
    {
        private ICareWorkerFunctionRepo _repo { get; set; }

        private MedicineDbContext _context;

        [TestInitialize]
        public void Initialize()
        {
            _context = CreateMedicineTestContext();
            _repo = new CareWorkerFunctionRepo(_context);
        }

        [TestMethod()]
        public void AddCareWorkerFunctionTest()
        {
            Result expected = new Result(false);
            Result actual = _repo.AddCareWorkerFunction("");

            Assert.AreEqual(expected.Success, actual.Success);
        }

        [TestMethod()]
        public void FindCareWorkerFunctionTest()
        {
            object expected = null;
            var actual = _repo.FindCareWorkerFunction("");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetCareWorkerFunctionsTest()
        {
            var expected = 2;
            var actual = _repo.GetCareWorkerFunctions();

            Assert.AreEqual(expected, actual.Count);
        }

        [TestMethod()]
        public void RemoveCareWorkerFunctionTest()
        {
            Result expected = new Result(false);
            var actual = _repo.RemoveCareWorkerFunction("");

            Assert.AreNotEqual(expected, actual);
        }
    }
}