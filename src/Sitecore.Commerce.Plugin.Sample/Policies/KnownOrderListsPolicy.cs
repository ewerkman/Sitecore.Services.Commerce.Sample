using Sitecore.Commerce.Core;

namespace Sitecore.Commerce.Plugin.Sample.Policies
{
    public class KnownOrderListsPolicy : Policy
    {
        public string ExportOrdersListName { get; set; } = "OrdersForExport";
    }
}
