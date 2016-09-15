using GRM.Domain;
using GRM.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GRM.Service
{
    public class ContractAvailablityChecker : IContractAvailablityChecker
    {
        IMusicContractsRepository _repository;

        public ContractAvailablityChecker(IMusicContractsRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<MusicContract> GetContracts(string deliveryPartnerName, DateTime effectiveDate)
        {
            var contracts = (
                                from contract in _repository.GetMusicContracts()                  
                                where contract.HasADeliveryPartnerNamed(deliveryPartnerName)
                                where contract.IsCurrentAsOf(effectiveDate)
                                orderby contract.Artist
                                select contract
                             ).ToList();

            return contracts;
        }

        
    }
}
