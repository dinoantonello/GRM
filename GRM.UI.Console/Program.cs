using Autofac;
using GRM.ApplicationInfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;

namespace GRM.UI.Console
{
    public class Program
    {
        static string _deliveryPartnerName;
        static DateTime _effectiveDate;
        static IEnumerable<Domain.MusicContract> _contracts;
        static ContractAvailablityCheckerDecorators.DependancyResolver _contractAvailablityChecker;
        static List<string> validationErrors;

        public static void Main(string[] args)
        {
            DefaultMembers();
            SetDeliveryPartnerName(args);
            SetEffectiveDate(args);

            ValidateInputs();

            if (InputsAreInvalid()) {
                OutputErrorsToConsole();
                return;
            }

            GetContracts(); 

            OutputContractsToConsole();
        }

        private static void DefaultMembers()
        {
            _deliveryPartnerName = String.Empty;
            _effectiveDate = new DateTime();
            _contracts = new List<Domain.MusicContract>();
            _contractAvailablityChecker = new ContractAvailablityCheckerDecorators.DependancyResolver();
            validationErrors = new List<string>();
        }
        private static void OutputErrorsToConsole() {
            ConsoleWrite.Error(validationErrors);
        }

        private static void ValidateInputs()
        {
            if (_effectiveDate.Ticks == 0) {
                validationErrors.Add("Unable to find a valid effective date");
               
            }

            if (String.IsNullOrEmpty(_deliveryPartnerName))
            {
                validationErrors.Add("Unable to find a valid delivery partner name");
            }

            
        }

        private static bool InputsAreInvalid() {
            return validationErrors.Count != 0;
        }

        private static void GetContracts()
        {
            _contracts = _contractAvailablityChecker.GetContracts(_deliveryPartnerName, _effectiveDate);            
        }

        private static void OutputContractsToConsole()
        {
            ConsoleWrite.Info("Artist|Title|Usages|StartDate|EndDate");

            foreach (var contract in _contracts)
            {
                ConsoleWrite.Info(FormatForOutput(contract));
            }

            ConsoleWrite.Success(_contracts.Count().ToString() + " Records Returned");
        }

        private static string FormatForOutput(Domain.MusicContract contract)
        {
            var line = new StringBuilder();
            line.Append(contract.Artist);
            line.Append("|");
            line.Append(contract.Title);
            line.Append("|");
            int i = 1;
            foreach (var usage in contract.Usages)
            {
                line.Append(usage.Usage);
                if (i < contract.Usages.Count())
                {
                    i++;
                    line.Append(",");
                }                
            }
            line.Append("|");
            if (contract.StartDate.Ticks != 0)
            {
                line.Append(String.Format("{0} {1:MMM yyyy}", contract.StartDate.Day.Ordinalize(), contract.StartDate));
            }
            line.Append("|");
            if (contract.EndDate.Ticks != 0)
            {
                line.Append(String.Format("{0} {1:MMM yyyy}", contract.EndDate.Day.Ordinalize(), contract.EndDate));
            }

            return line.ToString();
        }

        private static void SetEffectiveDate(string[] args)
        {
            if (args.Length >= 4)
            {
                _effectiveDate = (args[args.Length - 3] + " " + args[args.Length - 2] + " " + args[args.Length - 1]).ConvertToDateTime();
            }                      
        }

        private static void SetDeliveryPartnerName(string[] args)
        {
            var deliveryPartnerNameBuilder = new StringBuilder();
      
            if (args.Length >= 4)
            {                
                for (int i = 0; i < args.Length - 3; i++)
                {
                    deliveryPartnerNameBuilder.Append(args[i]);
                    deliveryPartnerNameBuilder.Append(" ");     
                }

                _deliveryPartnerName = deliveryPartnerNameBuilder.ToString();
            }
        }
    }
}
