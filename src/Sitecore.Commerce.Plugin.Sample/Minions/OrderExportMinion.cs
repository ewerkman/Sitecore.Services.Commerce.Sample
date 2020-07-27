using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Orders;
using Sitecore.Commerce.Plugin.Sample.Pipelines;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Sitecore.Commerce.Plugin.Sample.Minions
{
    public class OrderExportMinion : Minion
    {
        private IExportOrderPipeline exportOrderPipeline;

        public override void Initialize(IServiceProvider serviceProvider,
            ILogger logger,
            MinionPolicy policy,
            CommerceEnvironment environment,
            CommerceContext globalContext)
        {
            base.Initialize(serviceProvider, logger, policy, environment, globalContext);

            this.exportOrderPipeline = serviceProvider.GetService<IExportOrderPipeline>();
        }

        [Obsolete]
        public override Task<MinionRunResultsModel> Run()
        {
            throw new NotImplementedException();
        }

        protected override async Task<MinionRunResultsModel> Execute()
        {
            var listCount = await GetListCount(Policy.ListToWatch);
            if(listCount > 0)
            {
                foreach (var order in (await GetListItems<Order>(Policy.ListToWatch, Policy.ItemsPerBatch)).ToList())
                {
                    await exportOrderPipeline.Run(order,
                        new CommercePipelineExecutionContextOptions(new CommerceContext(Logger, MinionContext.TelemetryClient)
                            { Environment = Environment }));
                }
            }
            return new MinionRunResultsModel { DidRun = true, ItemsProcessed = (int)listCount };
        }
    }
}
