using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Rules;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Commerce.Plugin.Sample.Conditions
{
    [EntityIdentifier(nameof(HappyHourCondition))]
    public class HappyHourCondition : ICondition
    {
        public IRuleValue<string> StartOfHappyHour { get; set; }
        public IRuleValue<string> EndOfHappyHour { get; set; }

        public bool Evaluate(IRuleExecutionContext context)
        {
            var commerceContext = context.Fact<CommerceContext>();

            var cart = commerceContext?.GetObject<Cart>();
            if(cart == null || !cart.Lines.Any())
            {
                return false;
            }

            var startOfHappyHour = DateTime.ParseExact(StartOfHappyHour.Yield(context), "HH:mm", CultureInfo.InvariantCulture);
            var endOfHappyHour = DateTime.ParseExact(EndOfHappyHour.Yield(context), "HH:mm", CultureInfo.InvariantCulture);

            var now = DateTime.Now;
            return (now >= startOfHappyHour && now <= endOfHappyHour);
        }
    }
}
