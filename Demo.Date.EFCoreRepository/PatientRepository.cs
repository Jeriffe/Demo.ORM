using Demo.Data.Models;
using Demo.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Date.EFCoreRepository
{
    public interface IPatientRepository : IRepoDBRepository<TPatient>
    {
    }
    
}
