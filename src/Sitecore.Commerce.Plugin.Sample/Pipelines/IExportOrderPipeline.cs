using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Orders;
using Sitecore.Framework.Pipelines;

namespace Sitecore.Commerce.Plugin.Sample.Pipelines
{
    public interface IExportOrderPipeline : IPipeline<Order, Order, CommercePipelineExecutionContext>
    {
    }
}
