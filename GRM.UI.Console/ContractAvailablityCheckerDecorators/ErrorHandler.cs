using GRM.ApplicationInfrastructure;
using GRM.Domain;
using GRM.Service;
using System;
using System.Collections.Generic;

namespace GRM.UI.Console.ProductsAvailablityCheckerFactory
{    /// <summary>
     /// The role of this class is to handle any errors produced by by ProductsAvailablityChecker. It decorates 
     /// ProductsAvailablityChecker which means that it takes any object that also looks like IProductsAvailablityChecker and wraps itself 
     /// around it. this is done so that we can maintain the single responsibility principle. 
     /// Why do we do this? To make the objects simpler.
     /// If you wish to add to this class please ensure that its single purpose does not change reason. If you do require additional faunctionality that does not 
     /// fit the criteria above, please create another decorator
     /// </summary>
    public class ErrorHandler : IContractAvailablityChecker
    {
        ILogger _logger;
        IContractAvailablityChecker _productsAvailablityChecker;
        Exception exception;
        string _deliveryPartnerName;
        DateTime _effectiveDate;

        public ErrorHandler(IContractAvailablityChecker productsAvailablityChecker, ILogger logger)
        {
            _logger = logger;
            _productsAvailablityChecker = productsAvailablityChecker;
        }

        public IEnumerable<MusicContract> GetContracts(string deliveryPartnerName, DateTime effectiveDate)
        {
            _deliveryPartnerName = deliveryPartnerName;
            _effectiveDate = effectiveDate;

            try
            {
                return GetProductsFromService();
            }
            catch (Exception exception)
            {
                this.exception = exception;
                LogError();
                InformEndUser();
                return new List<MusicContract>();
            }
        }

        private IEnumerable<MusicContract> GetProductsFromService()
        {
            return _productsAvailablityChecker.GetContracts(_deliveryPartnerName, _effectiveDate);
        }

        private void LogError()
        {
            _logger.Error(exception.Message, exception);
        }

        private void InformEndUser()
        {
            ConsoleWrite.Error("Apologies, An error has occurred. Please check the logs for more information.");
        }
    }
}
