using Autofac;
using GRM.Domain;
using GRM.Service;
using System;
using System.Collections.Generic;

namespace GRM.UI.Console.ContractAvailablityCheckerDecorators
{
    /// <summary>
    /// The role of this class is to resolve all the dependancies from autofac that are need by ProductsAvailablityChecker. It decorates 
    /// ProductsAvailablityChecker which means that it takes any object that also looks like IProductsAvailablityChecker and wraps itself 
    /// around it. this is done so that we can maintain the single responsibility principle. 
    /// Why do we do this? To make the objects simpler.
    /// If you wish to add to this class please ensure that its single purpose does not change reason. If you do require additional faunctionality that does not 
    /// fit the criteria above, please create another decorator
    /// </summary>
    public class DependancyResolver : IContractAvailablityChecker
    {
        private static IContainer container;
        
        public DependancyResolver()
        {
            container = DependencyResolution.GetContainer();
        }

        public IEnumerable<MusicContract> GetContracts(string deliveryPartnerName, DateTime effectiveDate)
        {
            using (var scope = container.BeginLifetimeScope())
            {               
                var contractAvailablityChecker = scope.Resolve<IContractAvailablityChecker>();

                return contractAvailablityChecker.GetContracts(deliveryPartnerName, effectiveDate);               
            }
        }        
    }
}
