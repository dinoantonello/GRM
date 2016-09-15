using Autofac;
using GRM.ApplicationInfrastructure;
using GRM.Repository;
using GRM.Service;
using GRM.UI.Console.ProductsAvailablityCheckerFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRM.UI.Console
{
    public static class DependencyResolution
    {
        public static IContainer GetContainer()
        {
            var builder = new ContainerBuilder();

            builder.Register(c => new Logger()).As<ILogger>();
            builder.Register(c => new CsvReaderFactory()).As<ICsvReaderFactory>();
            builder.Register(c => new DistributionPartnerRepository("Data/DistributionPartnerContracts.data", c.Resolve<ICsvReaderFactory>())).As<IDistributionPartnerRepository>();
            builder.Register(c => new MusicContractsRepository("Data/MusicContracts.data", c.Resolve<IDistributionPartnerRepository>(), c.Resolve<ICsvReaderFactory>())).As<IMusicContractsRepository>();


            builder.RegisterType<ContractAvailablityChecker>().Named<IContractAvailablityChecker>("realProductsAvailablityChecker");
            builder.RegisterDecorator<IContractAvailablityChecker>((c, inner) => new ErrorHandler(inner, c.Resolve<ILogger>()), fromKey: "realProductsAvailablityChecker");

            return builder.Build();
        }
    }
}
