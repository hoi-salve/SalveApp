using System;
using System.Collections.Generic;
using System.Linq;
using SalveApp.Clinics.Core.Models;

namespace SalveApp.Clinics.Tests.Common;

public class DummyData
{
    public static IEnumerable<Clinic> Clinics()
    {
        return GetClinics();
    }

    public static IEnumerable<Clinic> GetClinics()
    {
        yield return new Clinic { Id = 1, Name = "ABC" };
        yield return new Clinic { Id = 2, Name = "XYZ" };
    }

    public static IEnumerable<Patient> Patients(int clinicId)
    {
        return GetPatients()
            .Where(r => r.ClinicId == clinicId);
    }

    public static IEnumerable<Patient> GetPatients()
    {
        yield return new Patient
        {
            Id = 1,
            DateOfBirth = new DateTime(1991, 1, 1),
            FirstName = "A",
            LastName = "B",
            ClinicId = 1
        };
        yield return new Patient
        {
            Id = 2,
            DateOfBirth = new DateTime(1992, 2, 2),
            FirstName = "C",
            LastName = "D",
            ClinicId = 1
        };

        yield return new Patient
        {
            Id = 3,
            DateOfBirth = new DateTime(1993, 3, 3),
            FirstName = "E",
            LastName = "F",
            ClinicId = 2
        };
        yield return new Patient
        {
            Id = 4,
            DateOfBirth = new DateTime(1994, 4, 4),
            FirstName = "G",
            LastName = "H",
            ClinicId = 2
        };
    }
}