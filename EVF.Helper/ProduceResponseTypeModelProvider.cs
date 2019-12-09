using EVF.Helper.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace EVF.Helper
{
    public class ProduceResponseTypeModelProvider : IApplicationModelProvider
    {
        public int Order => 3;

        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
        }

        /// <summary>
        /// Add produces response type to attribute filter.
        /// </summary>
        /// <param name="context">The context object for <see cref="IApplicationModelProvider" /></param>
        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
            foreach (ControllerModel controller in context.Result.Controllers)
            {
                foreach (ActionModel action in controller.Actions)
                {
                    action.Filters.Add(new ProducesResponseTypeAttribute(UtilityService.InitialResultError(MessageValue.InternalServerError).GetType(), 
                                                                         StatusCodes.Status400BadRequest));
                    action.Filters.Add(new ProducesResponseTypeAttribute(UtilityService.InitialResultError(MessageValue.InternalServerError).GetType(),
                                                                         StatusCodes.Status401Unauthorized));
                    action.Filters.Add(new ProducesResponseTypeAttribute(UtilityService.InitialResultError(MessageValue.InternalServerError).GetType(),
                                                                         StatusCodes.Status403Forbidden));
                    action.Filters.Add(new ProducesResponseTypeAttribute(UtilityService.InitialResultError(MessageValue.InternalServerError).GetType(), 
                                                                         StatusCodes.Status500InternalServerError));
                }
            }
        }
    }
}
