using Application.Common.Models;
using Application.Repositories;
using Application.Repositories.Interfaces;
using Domain.Entities.PatientData;
using Infrastructure.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace dataservice.Tests.Application.Services
{
    [TestClass]
    internal class PatientRepoTests : DatabaseTestBase
    {
        private PatientDbContext _patientDbContext;
        private MedicineDbContext _medcontext;
        private IPatientRepo _patientRepo;
        private ICareWorkerRepo _careWorkerRepo;
        private ICareWorkerFunctionRepo _careWorkerFunctionRepo;

        [TestInitialize]
        public void Initialize()
        {
            _patientDbContext = this.CreatePatientTestContext();
            _medcontext = this.CreateMedicineTestContext();

            _careWorkerRepo = new CareWorkerRepo(_medcontext, _careWorkerFunctionRepo);
            _patientRepo = new PatientRepo(_patientDbContext, _careWorkerRepo);
            _careWorkerFunctionRepo = new CareWorkerFunctionRepo(_medcontext);
        }

        [TestMethod]
        public void AddPatient_ValidData_ReturnsTrueResult()
        {
            Result result = _patientRepo.AddPatient("Test", "Testerman", new DateTime(1992, 12, 12), "1234AB", "43", "tester@test.nl", "0612345678");
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void AddPatient_InvalidData_ReturnsFalseResult()
        {
            Result result = _patientRepo.AddPatient(null, null, new DateTime(), null, null, null, null);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void GetPatient_ExistingPatientId_ReturnsPatient()
        {
            Patient? result = _patientRepo.GetPatient(1);
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
        public void GetPatient_NonExistingPatientId_ReturnsNull()
        {
            Patient? result = _patientRepo.GetPatient(-1);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void RemovePatient_ExistingPatient_ReturnsTrueResult()
        {
            Result result = _patientRepo.RemovePatient(1);
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void RemovePatient_NonExistingPatient_ReturnsFalseResult()
        {
            Result result = _patientRepo.RemovePatient(-1);
            Assert.IsFalse(result.Success);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this._patientDbContext.Dispose();
        }
    }
}
