using Application.Common.Models;
using Application.Repositories;
using Application.Repositories.Interfaces;
using dataservice.Tests;
using Domain.Entities.MedicalData;
using Infrastructure.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dataservice.Tests.Repositories
{
    [TestClass()]
    public class CareWorkerRepoTests : DatabaseTestBase
    {
        private ICareWorkerRepo CareWorkerRepo { get; set; }
        private ICareWorkerFunctionRepo careWorkerFunctionRepo { get; set; }

        private MedicineDbContext _context { get; set; }

        [TestInitialize()]
        public void Setup()
        {
            _context = CreateMedicineTestContext();
            careWorkerFunctionRepo = new CareWorkerFunctionRepo(_context);
            CareWorkerRepo = new CareWorkerRepo(_context, careWorkerFunctionRepo);

        }

        [TestMethod()]
        public void AddCareWorkerTest()
        {
            Result expected = new Result(false, "Given firstname, lastname or function was empty!");
            Result actual = CareWorkerRepo.AddCareWorker("", "", "");

            Assert.AreEqual(expected.Success, actual.Success);
        }

        [TestMethod()]
        public void FindCareWorkerTest()
        {
            CareWorker? expected = null;
            CareWorker? actual = CareWorkerRepo.FindCareWorker(10);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetCareWorkersTest()
        {
            var expected = 2;
            var actual = CareWorkerRepo.GetCareWorkers().Count;

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod()]
        public void RemoveCareWorkerTest()
        {
            var expected = new Result(false);
            var actual = CareWorkerRepo.RemoveCareWorker(10);

            Assert.AreEqual(expected.Success, actual.Success);
        }
    }
}