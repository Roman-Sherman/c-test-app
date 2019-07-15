using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace aspdotnet_managesys.Controllers
{
    public class RestErrorFilter : ActionFilterAttribute
    {
        // @see https://stackoverflow.com/questions/15296069/how-to-figure-out-which-key-of-modelstate-has-error
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid == false)
            {
                IDictionary<string, string> errors = new Dictionary<string, string>();

                foreach (var key in context.ModelState.Keys)
                {
                    foreach (ModelError error in context.ModelState[key].Errors)
                    {
                        errors.Add(key, error.ErrorMessage);
                    }
                }

                JsonResult result = new JsonResult(errors);
                result.StatusCode = (int) HttpStatusCode.BadRequest;
                context.Result = result;
            }
        }        
    }
}