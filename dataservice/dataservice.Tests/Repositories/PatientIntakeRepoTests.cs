using Application.Common.Models;
using Application.Repositories;
using Application.Repositories.Interfaces;
using Domain.Entities.MedicalData;
using Infrastructure.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dataservice.Tests.Repositories
{
    [TestClass]
    internal class PatientIntakeTests : DatabaseTestBase
    {
        private MedicineDbContext _medicineDbContext;
        private PatientDbContext _patientDbContext;
        private ICareWorkerRepo _careWorkerRepo;
        private IPatientRepo _patientRepo;
        private ICareWorkerFunctionRepo _careWorkerFunctionRepo;
        private IPatientIntakeRepo _intakeRepo;

        [TestInitialize]
        public void Initialize()
        {
            _medicineDbContext = CreateMedicineTestContext();
            _patientDbContext = CreatePatientTestContext();
            _careWorkerFunctionRepo = new CareWorkerFunctionRepo(_medicineDbContext);
            _patientRepo = new PatientRepo(_patientDbContext, _careWorkerRepo);
            _careWorkerRepo = new CareWorkerRepo(_medicineDbContext, _careWorkerFunctionRepo);

            var _medicineRepo = new MedicineRepo(_medicineDbContext, new ShapeRepo(_medicineDbContext), new ColorRepo(_medicineDbContext), new TypeRepo(_medicineDbContext), new DoseUnitRepo(_medicineDbContext));
            var _intakeRegistrationRepo = new IntakeRegistrationRepo(_medicineDbContext);
            _intakeRepo = new PatientIntakeRepo(_medicineDbContext, _medicineRepo, _patientRepo, _intakeRegistrationRepo);
        }

        [TestMethod]
        public void AddIntake_ValidData_ReturnsTrueResult()
        {
            Result result = _intakeRepo.AddIntake(1, 1, 1, new TimeOnly(12, 0), new TimeOnly(23, 59));
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void AddIntake_InvalidData_ReturnsFalseResult()
        {
            Result result = _intakeRepo.AddIntake(0, 0, 0, new TimeOnly(), new TimeOnly());
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void GetIntake_ExistingIntakeId_ReturnsIntake()
        {
            PatientIntake? result = _intakeRepo.GetIntakeById(1);
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
        public void GetIntake_NonExistingIntakeId_ReturnsNull()
        {
            PatientIntake? result = _intakeRepo.GetIntakeById(-1);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAllIntake_ValidPatientId_ReturnsAllIntake()
        {
            var medicineCount = _medicineDbContext.PatientIntakes.Where(x => x.PatientId == 1).Count();

            IEnumerable<PatientIntake> medicines = _intakeRepo.GetIntakesByPatientId(1);

            Assert.AreEqual(medicines.Count(), medicineCount);
        }

        [TestMethod]
        public void GetAllIntake_InValidPatientId_ReturnsNull()
        {
            IEnumerable<PatientIntake> medicines = _intakeRepo.GetIntakesByPatientId(-1);

            Assert.IsNull(medicines);
        }

        [TestMethod]
        public void RemoveIntake_ExistingIntake_ReturnsTrueResult()
        {
            Result result = _intakeRepo.RemoveIntake(1);
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void RemoveIntake_NonExistingIntake_ReturnsFalseResult()
        {
            Result result = _intakeRepo.RemoveIntake(-1);
            Assert.IsFalse(result.Success);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _medicineDbContext.Dispose();
            _patientDbContext.Dispose();
        }
    }
}
