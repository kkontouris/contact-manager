using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts;
using ServiceContracts.Dto;
using Entities;
using ServiceContracts.Enums;
using System.Collections;
using Services.Helpers;
using System.Threading.Tasks;
using RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace Services
{
	public class PersonGetterService : IPersonGetterService
	{
		private readonly IPersonsRepository _personsRepository;
		private readonly ILogger<PersonGetterService> _logger;
		public PersonGetterService(IPersonsRepository personsRepository, ILogger<PersonGetterService> logger)
        {
			_personsRepository = personsRepository;
			_logger = logger;
        }



		//Include to fetch data of Person with data of country
		public async Task<List<PersonResponse>> GetAllPersons()
		{
			_logger.LogInformation("GetAllPersons of person service");
			var persons=await _personsRepository.GetAllPersons();


			return persons.Select(temp=>temp.ToPersonResponse()).ToList();
		}


		public async Task<PersonResponse?> GetPersonByPersonId(Guid? personId)
		{
			if (personId == null)
			{
				return null;
			}

			Person? personWithPersonId = await _personsRepository.GetPersonByPersonId(personId);
			if(personWithPersonId == null)
			{
				return null;
			}
			return personWithPersonId.ToPersonResponse();	
		}


		public async Task<List<PersonResponse>> GetFilteredPersons(string userId,string searchBy, string? searchString)
		{

            // Φιλτράρισμα των εγγραφών του συγκεκριμένου χρήστη
            List<Person> persons = await _personsRepository.GetPersonsByUserId(userId);
            //List<Person> persons = await _personsRepository.GetPersonsByUserId(userId);
            persons = searchBy switch
			{
				nameof(PersonResponse.PersonName) =>
					await _personsRepository.GetFilteredPersons(userId,temp =>
					 temp.PersonName.Contains(searchString)),

				nameof (PersonResponse.Email) =>
					await _personsRepository.GetFilteredPersons(userId,temp =>
					temp.Email.Contains(searchString)),


				nameof (PersonResponse.TaxIdentificationNumber) =>
					await _personsRepository.GetFilteredPersons(userId,temp =>
					temp.TaxIdentificationNumber.Contains(searchString)),

				nameof (PersonResponse.Gender) =>
					await _personsRepository.GetFilteredPersons(userId,temp =>
					temp.Gender.Contains(searchString)),

				nameof(PersonResponse.Country) =>
				 await _personsRepository.GetFilteredPersons(userId,temp =>
				 temp.Country!=null &&temp.Country.CountryName!=null&&temp.Country.CountryName.Contains(searchString)),

				nameof (PersonResponse.Address) =>
					await _personsRepository.GetFilteredPersons(userId,temp =>
					temp.Address.Contains(searchString)),

                _ => persons // Επιστρέφουμε τη λίστα των ατόμων χωρίς περαιτέρω φίλτρα
            };
			return persons.Select(temp => temp.ToPersonResponse()).ToList();
		}



		
	}
}
