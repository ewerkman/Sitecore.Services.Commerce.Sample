using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Orders;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Sitecore.Commerce.Plugin.Sample.Pipelines.Blocks
{
    public class ExportOrderBlock : PipelineBlock<Order, Order, CommercePipelineExecutionContext>
    {
        protected CommerceCommander Commander { get; set; }

        public ExportOrderBlock(CommerceCommander commander)
        {
            this.Commander = commander;
        }

        public override async Task<Order> Run(Order arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{this.Name}: The argument can not be null");

            // Export order 
            File.WriteAllText($"c:\\tmp\\Orders\\{arg.OrderConfirmationId}.json", JsonConvert.SerializeObject(arg));

            var knownOrderListsPolicy = context.GetPolicy<Policies.KnownOrderListsPolicy>();
            var listEntitiesArgument = await Commander.Pipeline<IRemoveListEntitiesPipeline>()
                   .Run(new ListEntitiesArgument(new List<string>() { arg.Id }, knownOrderListsPolicy.ExportOrdersListName), context)
                   .ConfigureAwait(false);

            return arg;
        }
    }
}
