using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;
using StockApp.Controllers;
using StockApp.Models;

namespace StockApp.Filters.ActionFilters
{
    public class CreateOrderActionFilter :IAsyncActionFilter
    {
        //Logging
        private readonly ILogger<CreateOrderActionFilter> _logger;
        
        //Constructor
        public CreateOrderActionFilter(ILogger<CreateOrderActionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            /*Before logic*/

            //First checks if it has been applied TradeController.
            if(context.Controller is TradeController tradeController)
            {
                //If so, then it receives the action argument called "orderRequest".
                var orderRequest = context.ActionArguments["orderRequest"] as IOrderRequest;

                //It perform model level validations on that model object.
                if (orderRequest != null)
                {
                    //update date of order
                    orderRequest.DateAndTimeOfOrder = DateTime.Now;

                    //re-validate the model object after updating the date
                    tradeController.ModelState.Clear();
                    tradeController.TryValidateModel(orderRequest);
                    if(!tradeController.ModelState.IsValid)
                    {
                        //adds model errors to ViewBag.Errors and then reinvokes the "TradeController.Index" view.
                        tradeController.ViewBag.Errors = tradeController.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                        //In case if it has one or more model errors, it has to create object of StockTrade model class with essential data,
                        StockTrade stockTrade = new StockTrade() { StockName = orderRequest.StockName, Quantity = orderRequest.Quantity, StockSymbol = orderRequest.StockSymbol };
                        context.Result = tradeController.View(nameof(TradeController.Index), stockTrade);//short-circuits or skips the subsequent action filters & action method
                    }

                    else
                    {
                        //In case of no model errors, it doesn't nothing - just invokes the subsequent filter in the filter pipeline.
                        _logger.LogInformation("{FilterName}.{MethodName} method", nameof(CreateOrderActionFilter), nameof(OnActionExecutionAsync));
                        await next(); //invokes the subsequent filter or action method
                    }
                }
                else
                {
                    _logger.LogInformation("{FilterName}.{MethodName} method", nameof(CreateOrderActionFilter), nameof(OnActionExecutionAsync));
                    await next(); //invokes the subsequent filter or action method
                }
            }
            else
            {
                await next(); //invokes the subsequent filter or action method
            }

            /*After Logic*/
        }
    }
}
