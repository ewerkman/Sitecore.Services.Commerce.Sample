using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.ManagedLists;
using Sitecore.Commerce.Plugin.Orders;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Sitecore.Commerce.Plugin.Sample.Pipelines.Blocks
{
    public class AddOrderToExportableOrdersListBlock : PipelineBlock<Order, Order, CommercePipelineExecutionContext>
    {
        public override Task<Order> Run(Order arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{this.Name}: The argument can not be null");

            Policies.KnownOrderListsPolicy knownOrderListsPolicy = context.GetPolicy<Policies.KnownOrderListsPolicy>();

            arg.GetComponent<TransientListMembershipsComponent>().Memberships.Add(knownOrderListsPolicy.ExportOrdersListName);
            context.Logger.LogInformation($"{PipelineConstants.Block.EXECUTION_STARTED} AddOrderToExportableOrdersListBlock: order '{arg.Id}' is now added to 'OrdersForExport' list status.", Array.Empty<object>());

            return Task.FromResult(arg);
        }
    }
}
