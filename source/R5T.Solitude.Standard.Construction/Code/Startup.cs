using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using R5T.Bath.Console.Standard;
using R5T.Chalandri.DropboxRivetTestingData;
using R5T.Evosmos.CDriveTemp;
using R5T.Odense.Standard;
using R5T.Richmond;
using R5T.Solgene.VS2017.Standard;


namespace R5T.Solitude.Standard.Construction
{
    class Startup : StartupBase
    {
        public Startup(ILogger<Startup> logger)
            : base(logger)
        {
        }

        protected override void ConfigureServicesBody(IServiceCollection services)
        {
            services
                .AddSingleton<Program>()
                .AddConsoleHumanOutput()
                .AddTemporaryDirectoryFilePathProvider()
                .AddTestingDataDirectoryContentPathsProvider()
                .AddVisualStudioSolutionFileOperator(services.AddVisualStudioSolutionFileGeneratorAction())
                .AddVisualStudioSolutionFileProjectTypeGuidProvider()
                ;
        }
    }
}
