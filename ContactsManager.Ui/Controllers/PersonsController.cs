using _16CrudExample.Filters.ActionFilters;
using _16CrudExample.Filters.AuthorizationFilters;
using _16CrudExample.Filters.ResultFilters;
using ContactsManager.Core.Domain.IdentityEntities;
using DinkToPdf;
using DinkToPdf.Contracts;
using Entities;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.Dto;
using ServiceContracts.Enums;
using System.Security.Claims;

namespace _16CrudExample.Controllers
{
	//attribute routing
	[Route("[controller]")]
	public class PersonsController : Controller
	{

		//private field
		private readonly ICountriesService _countriesService;
		private readonly IPersonGetterService _personGetterService;
		private readonly IPersonAdderService _personAdderService;
		private readonly IPersonSorterService _personSorterService;
		private readonly IPersonDeleterService _personDeleterService;
		private readonly IPersonUpdaterService _personUpdaterService;
		private readonly ILogger<PersonsController> _logger;

		private readonly UserManager<ApplicationUser> _userManager;



		public PersonsController(IPersonGetterService personGetterService, IPersonAdderService personAdderService, IPersonSorterService personSorterService,
			IPersonUpdaterService personUpdaterService, IPersonDeleterService personDeleterService, ICountriesService countriesService,
			ILogger<PersonsController> logger, UserManager<ApplicationUser> userManager)
		{
			_personGetterService = personGetterService;
			_personAdderService = personAdderService;
			_personSorterService = personSorterService;
			_personDeleterService = personDeleterService;
			_personUpdaterService = personUpdaterService;
			_countriesService = countriesService;
			_logger = logger;

			_userManager = userManager;


		}

		//Url: persons/index
		[Route("[action]")]
		[Route("/")]
		[TypeFilter(typeof(PersonsListActionFilter))]

		public async Task<IActionResult> Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName)
			, SortOrderOptions sortOrder = SortOrderOptions.ASC)
		{
			_logger.LogInformation("Index action method of PersonsController");
			_logger.LogDebug($"searchby:{searchBy}, searchstring:{searchString}, sortBy:{sortBy}");

            var userId = _userManager.GetUserId(User); // Λήψη του ID του συνδεδεμένου χρήστη
			Guid userIdGuid = Guid.Parse(userId);

            ViewBag.SearchFields = new Dictionary<string, string>()

			{ {nameof(PersonResponse.PersonName),"Person Name" },
				{nameof(PersonResponse.Gender), "Gender" },
				{nameof(PersonResponse.Address), "Address" },
				{nameof(PersonResponse.Email), "Email" },
				{nameof(PersonResponse.TaxIdentificationNumber), "Tax Identification Number" },
				{nameof(PersonResponse.Country), "Country" }
			};
			//search
			List<PersonResponse> allPersons = await _personGetterService.GetFilteredPersons(userId,searchBy, searchString);
			ViewBag.CurrentSearchBy = searchBy;
			ViewBag.CurrentSearchString = searchString;

            //sort 
            // Φιλτράρισμα των εγγραφών μόνο για τον τρέχοντα χρήστη
            List<PersonResponse> userPersons = allPersons.Where(p => p.UserId == userIdGuid).ToList();
            List<PersonResponse> sortedPersons = await _personSorterService.GetSortedPersons(userPersons, sortBy, sortOrder);
			ViewBag.CurrentSortBy = sortBy;
			ViewBag.CurrentSortOrder = sortOrder.ToString();


			return View(sortedPersons);
		}
		//Executes when the user clicks on "Create Person" hyperlink
		//while opening the create view
		[Route("[action]")]
		[HttpGet]
		public async Task<IActionResult> Create()
		{
			List<CountryResponse> allCountries = await _countriesService.GetAllCountries();
			ViewBag.Countries = allCountries;
			return View();
		}

		//Post request
		//url: person/create
		[HttpPost]
		[Route("[action]")]
		//[TypeFilter(typeof(TokenAuthorizationFilter))]
		public async Task<IActionResult> Create(PersonAddRequest personAddRequest)
		{
			if (!ModelState.IsValid)
			{
				List<CountryResponse> allCountries = await _countriesService.GetAllCountries();
				ViewBag.Countries = allCountries;
				ViewBag.errors = ModelState.Values.SelectMany(v => v.Errors).SelectMany(v => v.ErrorMessage).ToList();
				return View();
			}
            // Ανάκτηση του UserId του τρέχοντος χρήστη
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			Guid userIdGuid=Guid.Parse(userId);

            // Δημιουργία του Person object με validation στον constructor
            var person = new Person(personAddRequest.PersonName, personAddRequest.Email,
									personAddRequest.DateOfBirth, personAddRequest.Gender.ToString(),
									personAddRequest.CountryId, personAddRequest.Address,
									personAddRequest.ReceiveNewsLetters, personAddRequest.TaxIdentificationNumber, userIdGuid);
			personAddRequest.UserId = userIdGuid;
			await _personAdderService.AddPerson(personAddRequest);
			return RedirectToAction("Index", "Persons");
		}
		//Update  (Get) request
		[HttpGet]
		[Route("[action]/{PersonId}")]
		//[TypeFilter(typeof(TokenResultFilter))]
		public async Task<IActionResult> Edit(Guid? PersonId)
		{
			PersonResponse? personResponse = await _personGetterService.GetPersonByPersonId(PersonId);
			if (personResponse == null)
			{
				return RedirectToAction("Index");
			}
			PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
			List<CountryResponse> allCountries = await _countriesService.GetAllCountries();
			ViewBag.Countries = allCountries;
			return View(personUpdateRequest);
		}

		//Update(Post) request
		[HttpPost]
		[Route("[action]/{PersonId}")]
		//[TypeFilter(typeof(TokenAuthorizationFilter))]

		public async Task<IActionResult> Edit(PersonUpdateRequest personUpdateRequest)
		{
			PersonResponse? personResponse = await _personGetterService.GetPersonByPersonId(personUpdateRequest.PersonId);
			if (personResponse == null)
			{
				return RedirectToAction("Index");
			}
			if (!ModelState.IsValid)
			{
				List<CountryResponse> allCountries = await _countriesService.GetAllCountries();
				ViewBag.Countries = allCountries;
				ViewBag.errors = ModelState.Values.SelectMany(v => v.Errors).SelectMany(v => v.ErrorMessage).ToList();
				return View(personResponse.ToPersonUpdateRequest());

			}

            // Retrieve the current user's UserId
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userIdGuid))
            {
                // Handle the case where the UserId is not found or is invalid
                return Unauthorized();
            }

            var person = new Person(personUpdateRequest.PersonName, personUpdateRequest.Email, personUpdateRequest.DateOfBirth,
				personUpdateRequest.Gender.ToString(), personUpdateRequest.CountryId, personUpdateRequest.Address, personUpdateRequest.ReceiveNewsLetters,
				personUpdateRequest.TaxIdentificationNumber,userIdGuid);

			await _personUpdaterService.UpdatePerson(personUpdateRequest);
			return RedirectToAction("Index");

		}




		[HttpGet]
		[Route("[action]/{PersonId}")]
		public async Task<IActionResult> Delete(Guid? PersonId)
		{

            PersonResponse? personResponse = await _personGetterService.GetPersonByPersonId(PersonId);
			if (personResponse == null)
			{
				return RedirectToAction("Index");
			}

			return View(personResponse);
		}

		[HttpPost]
		[Route("[action]/{PersonId}")]
		public async Task<IActionResult> Delete(PersonUpdateRequest personUpdateRequest)
		{
            // Retrieve the current user's UserId
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userIdGuid))
            {
                // Handle the case where the UserId is not found or is invalid
                return Unauthorized();
            }
			
            PersonResponse? personResponse = await _personGetterService.GetPersonByPersonId(personUpdateRequest.PersonId);
			if (personResponse == null)
			{
				return RedirectToAction("Index");
			}
			await _personDeleterService.DeletePerson(personResponse.PersonId);


			return RedirectToAction("Index");
		}

	} 

	
	

}

