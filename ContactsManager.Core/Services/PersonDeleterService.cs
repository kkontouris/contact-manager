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
	public class PersonDeleterService : IPersonDeleterService
	{
		private readonly IPersonsRepository _personsRepository;


		public PersonDeleterService(IPersonsRepository personsRepository)
        {
			_personsRepository = personsRepository;

			
        }

		public async Task<bool> DeletePerson(Guid? personId)
		{
			if(personId == null)
			{
				throw new ArgumentNullException(nameof(personId));
			}
			Person? matchingPerson =await _personsRepository.GetPersonByPersonId(personId);
			if (matchingPerson == null)
			{
				return false;
				
			}
			using (CancellationTokenSource cts = new CancellationTokenSource())
			{
				cts.CancelAfter(TimeSpan.FromSeconds(10)); // Ακύρωση μετά από 10 δευτερόλεπτα
				return await _personsRepository.DeletePersonWithPersonIdAsync(personId.Value, cts.Token);
			}

		}
	}
}
