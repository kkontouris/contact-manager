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
using Exceptions;

namespace Services
{
	public class PersonUpdaterService : IPersonUpdaterService
	{
		private readonly IPersonsRepository _personsRepository;


		public PersonUpdaterService(IPersonsRepository personsRepository)
        {
			_personsRepository = personsRepository;			
        }

		public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
		{
			if (personUpdateRequest == null)
			{
				throw new ArgumentNullException(nameof(Person));
			}


			//validation
			ValidationHelpers.ModelValidation(personUpdateRequest);

			//seek in the database person object with the same id
			Person? matchingPerson=await _personsRepository.GetPersonByPersonId(personUpdateRequest.PersonId);

			if(matchingPerson==null)
			{
				throw new InvalidPersonIdException("Given Person Id does not exist");
			}

			//update all details
			matchingPerson.PersonName = personUpdateRequest.PersonName;
			matchingPerson.Address = personUpdateRequest.Address;
			matchingPerson.Email = personUpdateRequest.Email;
			matchingPerson.Gender = personUpdateRequest.Gender.ToString();
			matchingPerson.CountryId = personUpdateRequest.CountryId;
			matchingPerson.DateOfBirth= personUpdateRequest.DateOfBirth;
			matchingPerson.ReceiveNewsLeters = personUpdateRequest.ReceiveNewsLetters;
			matchingPerson.TaxIdentificationNumber = personUpdateRequest.TaxIdentificationNumber;

			await _personsRepository.UpdatePerson(matchingPerson);

			return matchingPerson.ToPersonResponse();
		}

	}
}
