using System.Collections.Generic;
using GRM.Domain;

namespace GRM.Repository
{
    public interface IDistributionPartnerRepository
    {
        List<DistributionPartner> GetDistributionPartners();
    }
}