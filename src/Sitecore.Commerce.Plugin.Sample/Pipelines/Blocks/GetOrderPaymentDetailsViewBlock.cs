using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Orders;
using Sitecore.Commerce.Plugin.Payments;
using Sitecore.Commerce.Plugin.Sample.Components;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Commerce.Plugin.Sample.Pipelines.Blocks
{
    public class GetOrderPaymentDetailsViewBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            var request = context.CommerceContext.GetObject<EntityViewArgument>();
            if (string.IsNullOrEmpty(request?.ViewName)
                || (!request.ViewName.Equals(context.GetPolicy<KnownPaymentsViewsPolicy>().OrderPayments, StringComparison.OrdinalIgnoreCase)
                    && !request.ViewName.Equals(context.GetPolicy<KnownOrderViewsPolicy>().Master, StringComparison.OrdinalIgnoreCase)
                    && !request.ViewName.Equals(context.GetPolicy<KnownPaymentsViewsPolicy>().OrderPaymentDetails, StringComparison.OrdinalIgnoreCase))
                || !(request.Entity is Order)
                || !string.IsNullOrEmpty(request.ForAction))
            {
                return Task.FromResult(arg);
            }

            var order = (Order)request.Entity;
            if (!order.HasComponent<PaymentComponent>())
            {
                return Task.FromResult(arg);
            }

            var payments = order.Components.OfType<SimplePaymentComponent>().ToList();

            if (request.ViewName.Equals(context.GetPolicy<KnownOrderViewsPolicy>().Master, StringComparison.OrdinalIgnoreCase)
                || request.ViewName.Equals(context.GetPolicy<KnownPaymentsViewsPolicy>().OrderPayments, StringComparison.OrdinalIgnoreCase))
            {
                var paymentsViews = arg.ChildViews.Cast<EntityView>().FirstOrDefault(ev => ev.Name.Equals(context.GetPolicy<KnownPaymentsViewsPolicy>().OrderPayments, StringComparison.OrdinalIgnoreCase));
                paymentsViews?.ChildViews.Where(cv => cv.Name.Equals(context.GetPolicy<KnownPaymentsViewsPolicy>().OrderPaymentDetails, StringComparison.OrdinalIgnoreCase)).Cast<EntityView>().ToList().ForEach(
                    paymentView =>
                    {
                        var simplePayment = payments.FirstOrDefault(s => s.Id.Equals(paymentView.ItemId, StringComparison.OrdinalIgnoreCase)) as SimplePaymentComponent;
                        if (simplePayment == null)
                        {
                            return;
                        }

                        paymentView.Properties.Add(new ViewProperty
                        {
                            Name = "ItemId",
                            IsReadOnly = true,
                            IsHidden = true,
                            RawValue = simplePayment.Id
                        });
                        paymentView.Properties.Add(new ViewProperty
                        {
                            Name = "Type",
                            IsReadOnly = true,
                            RawValue = simplePayment.GetType().Name
                        });
                        paymentView.Properties.Add(new ViewProperty
                        {
                            Name = "Amount",
                            IsReadOnly = true,
                            RawValue = simplePayment.Amount.Amount
                        });
                        paymentView.Properties.Add(new ViewProperty
                        {
                            Name = "Currency",
                            IsReadOnly = true,
                            RawValue = simplePayment.Amount.CurrencyCode
                        });
                    });

                return Task.FromResult(arg);
            }

            return Task.FromResult(arg);

        }
    }
}
