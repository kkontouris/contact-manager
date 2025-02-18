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

namespace Services
{
	public class PersonAdderService : IPersonAdderService
	{
		private readonly IPersonsRepository _personsRepository;

		public PersonAdderService(IPersonsRepository personsRepository)
        {
			_personsRepository = personsRepository;

			
        }

		public async Task<PersonResponse> AddPerson(PersonAddRequest? request)
		{
			if (request == null) throw new ArgumentNullException(nameof(request));

			//Model validation
			ValidationHelpers.ModelValidation(request);

			Person person = request.ToPerson();
			person.PersonId=Guid.NewGuid();
			await _personsRepository.AddPerson(person);
			return person.ToPersonResponse();
		}


		
	}
}
