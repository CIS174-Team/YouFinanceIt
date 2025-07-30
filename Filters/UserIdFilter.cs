// Filters/UserIdFilter.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace YouFinanceIt.Filters
{
    // This filter can be used to automatically inject the current user's ID
    // into action methods that have a parameter named 'userId' of type 'string'.
    // Example: public IActionResult MyAction(string userId) { ... }
    public class UserIdFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Get the user's ID from the claims.
            // ClaimTypes.NameIdentifier holds the unique ID for the authenticated user (which is a string for IdentityUser).
            var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if the user is authenticated and their ID is available
            if (!string.IsNullOrEmpty(userId))
            {
                // If the action method has a parameter named "userId" of type string,
                // this will inject the current user's ID into that parameter.
                // This is an alternative to calling User.FindFirstValue(ClaimTypes.NameIdentifier)
                // directly inside each action method.
                if (context.ActionArguments.ContainsKey("userId"))
                {
                    context.ActionArguments["userId"] = userId;
                }
            }
            else
            {
                // If no user ID is found (e.g., user is not authenticated),
                // you might want to redirect them to the login page or return an unauthorized result.
                // Note: The [Authorize] attribute typically handles this before the filter runs.
                context.Result = new UnauthorizedResult();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // This method runs after the action method has executed.
            // No specific logic needed here for this filter's purpose.
        }
    }
}
