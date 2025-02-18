using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace _16CrudExample.Controllers
{
	public class HomeController : Controller
	{
		//attribute routing
		[Route("Error")]
		[AllowAnonymous]
		public IActionResult Error(CancellationToken token)
		{
			IExceptionHandlerPathFeature? exceptionHandlerPathFeature=HttpContext.Features.Get<IExceptionHandlerPathFeature>();
			if(exceptionHandlerPathFeature != null&& exceptionHandlerPathFeature.Error!=null)
			{
				ViewBag.ErrorMessage = exceptionHandlerPathFeature.Error.Message;
			}
			return View();  //Views/Shared/Error
		}
	}
}
