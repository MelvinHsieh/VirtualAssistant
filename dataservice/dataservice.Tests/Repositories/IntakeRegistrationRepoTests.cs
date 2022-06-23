using Microsoft.VisualStudio.TestTools.UnitTesting;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Repositories.Interfaces;
using Infrastructure.Persistence;
using dataservice.Tests;
using Application.Common.Models;
using Domain.Entities.MedicalData;

namespace dataservice.Tests.Repositories
{
    [TestClass()]
    public class IntakeRegistrationRepoTests : DatabaseTestBase
    {
        private IIntakeRegistrationRepo _repo { get; set; }

        private MedicineDbContext _context;

        [TestInitialize]
        public void Initialize()
        {
            _context = CreateMedicineTestContext();
            _repo = new IntakeRegistrationRepo(_context);
        }


        [TestMethod()]
        public void AddIntakeRegistrationTest()
        {
            var expected = new Result(false, "The intake date should not be in the past");
            var actual = _repo.AddIntakeRegistration(DateTime.Parse("Jan 1, 2000"), 1);

            Assert.AreEqual(expected.Message, actual.Message);
        }

        [TestMethod()]
        public void GetIntakeRegistrationTest()
        {
            object expected = null;
            var actual = _repo.GetIntakeRegistration(-1);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetIntakeRegistrationForDateTest()
        {
            var expected = 0;
            var actual = _repo.GetIntakeRegistrationForDate(-1, DateOnly.MaxValue).Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetIntakeRegistrationForPatientTest()
        {
            var expected = 0;
            var actual = _repo.GetIntakeRegistrationForPatient(-1).Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void RemoveIntakeRegistrationTest()
        {
            var expected = new Result(false);
            var actual = _repo.RemoveIntakeRegistration(-1);

            Assert.AreEqual(expected.Success, actual.Success);
        }

        [TestMethod()]
        public void ValidateIntakeRegistrationTest()
        {
            var testobject = new IntakeRegistration()
            {
                TakenOn = DateTime.Parse("Jan 9, 2000"),
                PatientIntakeId = -1
            };
            var expected = new Result(false);
            var actual = _repo.ValidateIntakeRegistration(testobject);

            Assert.AreEqual(expected.Success, actual.Success);
        }
    }
}