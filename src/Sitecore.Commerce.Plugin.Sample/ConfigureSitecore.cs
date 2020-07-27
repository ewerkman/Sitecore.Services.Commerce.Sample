// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigureSitecore.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2017
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Sitecore.Framework.Rules;

namespace Sitecore.Commerce.Plugin.Sample
{
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.EntityViews;
    using Sitecore.Commerce.Plugin.Orders;
    using Sitecore.Commerce.Plugin.Sample.Pipelines;
    using Sitecore.Commerce.Plugin.Sample.Pipelines.Blocks;
    using Sitecore.Framework.Configuration;
    using Sitecore.Framework.Pipelines.Definitions.Extensions;

    /// <summary>
    /// The configure sitecore class.
    /// </summary>
    public class ConfigureSitecore : IConfigureSitecore
    {
        /// <summary>
        /// The configure services.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);

            services.Sitecore().Rules(config => config.Registry(registry => registry.RegisterAssembly(assembly)));

            services.Sitecore().Pipelines(config => config
                .AddPipeline<IExportOrderPipeline, ExportOrderPipeline>( c => c
                    .Add<ExportOrderBlock>()
                    )
                .ConfigurePipeline<IOrderPlacedPipeline>(c => c
                   .Add<AddOrderToExportableOrdersListBlock>().After<OrderPlacedAssignConfirmationIdBlock>()
                   )
                .ConfigurePipeline<IConfigureServiceApiPipeline>(c => c
                    .Add<ConfigureServiceApiBlock>()
                    )
                .ConfigurePipeline<IGetEntityViewPipeline>( c => c
                    .Add<GetOrderPaymentDetailsViewBlock>().Before<IFormatEntityViewPipeline>()
                    )
                );

            services.RegisterAllCommands(assembly);
        }
    }
}