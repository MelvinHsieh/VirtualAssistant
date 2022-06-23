using Application.Common.Models;
using Application.Repositories;
using Application.Repositories.Interfaces;
using Domain.Entities.MedicalData;
using Infrastructure.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace dataservice.Tests.Repositories;

[TestClass]
public class DoseUnitRepoTests : DatabaseTestBase
{
    private IDoseUnitRepo _doseUnitRepo;
    private MedicineDbContext _context;

    [TestInitialize]
    public void Initialize()
    {
        _context = CreateMedicineTestContext();
        _doseUnitRepo = new DoseUnitRepo(_context);
    }

    [TestMethod]
    public void AddDoseUnit_NewDoseUnit_ReturnsTrueResult()
    {
        Result result = _doseUnitRepo.AddDoseUnit("New DoseUnit");
        Assert.IsTrue(result.Success);
    }

    [TestMethod]
    public void AddDoseUnit_ExistingDoseUnit_ReturnsFalseResult()
    {
        Result result = _doseUnitRepo.AddDoseUnit("mg");
        Assert.IsFalse(result.Success);
    }

    [TestMethod]
    public void FindDoseUnit_ExistingDoseUnit_ReturnsDoseUnit()
    {
        DoseUnit? result = _doseUnitRepo.FindDoseUnit("mg");
        if (result != null)
        {
            Assert.AreEqual("mg", result.Unit);
        }
        else
        {
            Assert.Fail();
        }

    }

    [TestMethod]
    public void FindDoseUnit_NonExistingDoseUnit_ReturnsNull()
    {
        DoseUnit? result = _doseUnitRepo.FindDoseUnit("asdjkasjdijask");
        Assert.IsNull(result);
    }

    [TestMethod]
    public void GetDoseUnits_ReturnsAllDoseUnits()
    {
        var doseUnitCount = _context.DoseUnits.Count();

        IEnumerable<DoseUnit> doseUnits = _doseUnitRepo.GetDoseUnits();

        Assert.AreEqual(doseUnits.Count(), doseUnitCount);
    }

    [TestMethod]
    public void RemoveDoseUnit_ExistingDoseUnit_ReturnsTrueResult()
    {
        Result result = _doseUnitRepo.RemoveDoseUnit("mg");
        Assert.IsTrue(result.Success);
    }

    [TestMethod]
    public void RemoveDoseUnit_NonExistingDoseUnit_ReturnsFalseResult()
    {
        Result result = _doseUnitRepo.RemoveDoseUnit("djsdjasjdas");
        Assert.IsFalse(result.Success);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        _context.Dispose();
    }
}
