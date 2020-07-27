using Sample.Commerce.Engine.Connect.Entities;
using Sitecore.Commerce.Engine.Connect.Entities;
using Sitecore.Commerce.Engine.Connect.Pipelines.Arguments;
using Sitecore.Commerce.Engine.Connect.Pipelines.Carts;
using Sitecore.Commerce.Entities;
using Sitecore.Commerce.Plugin.Sample.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sample.Commerce.Engine.Connect.Pipelines
{
    public class CustomTranslateCartToEntity : TranslateCartToEntity
    {
        public CustomTranslateCartToEntity(IEntityFactory entityFactory) : base(entityFactory)
        {
        }

        protected override void Translate(TranslateCartToEntityRequest request, Sitecore.Commerce.Plugin.Carts.Cart source, CommerceCart destination)
        {
            base.Translate(request, source, destination);
            TranslateLinePaymentType(source, destination);
        }

        private void TranslateLinePaymentType(Sitecore.Commerce.Plugin.Carts.Cart source, CommerceCart destination)
        {
            var simplePayment = (SimplePaymentComponent)source.Components.FirstOrDefault(c => c is SimplePaymentComponent);
            if (simplePayment != null)
            {
                destination.Payment.Add(new SimplePaymentInfo
                {
                    PaymentMethodID = simplePayment.PaymentMethod.EntityTarget,
                    ExternalId = simplePayment.Id,
                    Amount = simplePayment.Amount.Amount
                });
            }
        }
    }
}