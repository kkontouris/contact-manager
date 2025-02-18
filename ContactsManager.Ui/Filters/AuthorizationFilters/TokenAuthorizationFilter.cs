using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace _16CrudExample.Filters.AuthorizationFilters
{
	public class TokenAuthorizationFilter : IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			if (context.HttpContext.Request.Cookies.ContainsKey("Auth-Key")==false)
			{
				context.Result=new StatusCodeResult(StatusCodes.Status401Unauthorized);
				return;
			}
		}
	}
}
