using GRM.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRM.Service
{
    public interface IContractAvailablityChecker
    {
        IEnumerable<MusicContract> GetContracts(string deliveryPartnerName, DateTime effectiveDate);
    }
}
