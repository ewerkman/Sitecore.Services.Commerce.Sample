using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Orders;
using Sitecore.Framework.Pipelines;

namespace Sitecore.Commerce.Plugin.Sample.Pipelines
{
    public class ExportOrderPipeline : CommercePipeline<Order, Order>, IExportOrderPipeline
    {
        public ExportOrderPipeline(IPipelineConfiguration<IExportOrderPipeline> configuration, ILoggerFactory loggerFactory)
                : base(configuration, loggerFactory)
        {
        }
    }
}
