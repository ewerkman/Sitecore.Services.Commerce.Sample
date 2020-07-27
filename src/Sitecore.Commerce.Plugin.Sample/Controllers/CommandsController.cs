// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandsController.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2017
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.Commerce.Plugin.Sample
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http.OData;

    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Plugin.Payments;
    using Sitecore.Commerce.Plugin.Sample.Components;

    /// <inheritdoc />
    /// <summary>
    /// Defines a controller
    /// </summary>
    /// <seealso cref="T:Sitecore.Commerce.Core.CommerceController" />
    public class CommandsController : CommerceController
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sitecore.Commerce.Plugin.Sample.CommandsController" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="globalEnvironment">The global environment.</param>
        public CommandsController(IServiceProvider serviceProvider, CommerceEnvironment globalEnvironment)
            : base(serviceProvider, globalEnvironment)
        {
        }

        [HttpPut]
        [Route("AddSimplePayment()")]
        public async Task<IActionResult> AddSimplePayment([FromBody] ODataActionParameters value)
        {
            if (!this.ModelState.IsValid || value == null)
            {
                return (IActionResult)new BadRequestObjectResult(this.ModelState);
            }

            if (!value.ContainsKey("cartId") ||
                (string.IsNullOrEmpty(value["cartId"]?.ToString()) ||
                !value.ContainsKey("payment")) ||
                string.IsNullOrEmpty(value["payment"]?.ToString()))
            {
                return (IActionResult)new BadRequestObjectResult((object)value);
            }

            string cartId = value["cartId"].ToString();

            var paymentComponent = JsonConvert.DeserializeObject<SimplePaymentComponent>(value["payment"].ToString());
            var command = this.Command<AddPaymentsCommand>();
            await command.Process(this.CurrentContext, cartId, new List<PaymentComponent> { paymentComponent });

            return new ObjectResult(command);
        }

    }
}

