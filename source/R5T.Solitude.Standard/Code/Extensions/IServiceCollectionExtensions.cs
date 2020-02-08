using System;

using Microsoft.Extensions.DependencyInjection;

using R5T.Dacia;
using R5T.Solgene;
using R5T.Soltana.Standard;
using R5T.Solutas.Standard;

using R5T.Solitude.Soltana;


namespace R5T.Solitude.Standard
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="VisualStudioSolutionFileOperator"/> implementation of <see cref="IVisualStudioSolutionFileOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddVisualStudioSolutionFileOperator(this IServiceCollection services,
            ServiceAction<IVisualStudioSolutionFileGenerator> addSolutionFileGenerator)
        {
            services.AddSoltanaVisualStudioSolutionFileOperator(
                services.AddInMemoryVisualStudioSolutionFileOperatorAction(addSolutionFileGenerator),
                services.AddVisualStudioSolutionFileSerializerAction());

            return services;
        }

        /// <summary>
        /// Adds the <see cref="VisualStudioSolutionFileOperator"/> implementation of <see cref="IVisualStudioSolutionFileOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static ServiceAction<IVisualStudioSolutionFileOperator> AddVisualStudioSolutionFileOperatorAction(this IServiceCollection services,
            ServiceAction<IVisualStudioSolutionFileGenerator> addSolutionFileGenerator)
        {
            var serviceAction = new ServiceAction<IVisualStudioSolutionFileOperator>(() => services.AddVisualStudioSolutionFileOperator(addSolutionFileGenerator));
            return serviceAction;
        }
    }
}
