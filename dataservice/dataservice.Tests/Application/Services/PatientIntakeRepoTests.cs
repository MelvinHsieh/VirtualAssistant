using Application.Common.Models;
using Application.Repositories;
using Application.Repositories.Interfaces;
using Domain.Entities.MedicalData;
using Infrastructure.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dataservice.Tests.Application.Services
{
    [TestClass]
    internal class PatientIntakeTests : DatabaseTestBase
    {
        private IPatientIntakeRepo _intakeRepo;
        private MedicineDbContext _medicineDbContext;
        private PatientDbContext _patientDbContext;

        [TestInitialize]
        public void Initialize()
        {
            _medicineDbContext = this.CreateMedicineTestContext();
            _patientDbContext = this.CreatePatientTestContext();
            _intakeRepo = new PatientIntakeRepo(_medicineDbContext, new MedicineRepo(_medicineDbContext, new ShapeRepo(_medicineDbContext), new ColorRepo(_medicineDbContext), new TypeRepo(_medicineDbContext), new DoseUnitRepo(_medicineDbContext)), new PatientRepo(_patientDbContext));
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
            this._medicineDbContext.Dispose();
            this._patientDbContext.Dispose();
        }
    }
}
