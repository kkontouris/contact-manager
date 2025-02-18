using Microsoft.AspNetCore.Mvc.Filters;

namespace _16CrudExample.Filters.ActionFilters
{
	public class PersonsListActionFilter : IActionFilter
	{
		private readonly ILogger<PersonsListActionFilter> _logger;

        public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger)
        {  
			_logger=logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
		{
			_logger.LogInformation("PersonListActtionFilter OnActionExecuted method");

		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
			_logger.LogInformation("PersonListActtionFilter OnActionExecuting method");

		}
	}
}
