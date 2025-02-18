using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryContracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Exceptions;

namespace Repositories
{
	public class PersonsRepository : IPersonsRepository
	{
		private readonly ApplicationDbContext _dB;
		private readonly ILogger<PersonsRepository> _logger;
        public PersonsRepository(ApplicationDbContext dB, ILogger<PersonsRepository> logger)
        {
				_dB= dB;
			_logger= logger;
        }
        public async Task<Person> AddPerson(Person person)
		{
			_dB.Persons.Add(person);
			await _dB.SaveChangesAsync();
			return person;

		}

		public async Task<bool> DeletePersonWithPersonIdAsync(Guid personId, CancellationToken token)
		{
			try {
                var person = await _dB.Persons.SingleOrDefaultAsync(p => p.PersonId == personId, token);
				if (person != null)
				{
					_dB.Persons.Remove(person);
                    int rowsDeleted = await _dB.SaveChangesAsync(token);

                    // Επιστροφή true αν διαγράφηκε τουλάχιστον μία γραμμή
                    return rowsDeleted > 0;


                }
				return false;
            }
			catch (OperationCanceledException)
			{
				// Ακύρωση λειτουργίας
				return false;
			}
			catch (Exception)
			{
				// Διαχείριση άλλων σφαλμάτων
				throw;
			}

		}



		public async Task<List<Person>> GetAllPersons()
		{
			return await _dB.Persons.Include("Country").ToListAsync();
		}

		public async Task<List<Person>> GetFilteredPersons(string userId, Expression<Func<Person, bool>> predicate)
		{
			Guid UserIdGuid=Guid.Parse(userId);
			return await _dB.Persons.Include("Country")
				.Where(p=>p.UserId==UserIdGuid)
				.Where(predicate)
				.ToListAsync();
		}

		public async Task<Person?> GetPersonByPersonId(Guid? personId)
		{
			return await _dB.Persons.Include("Country")
				.Where(temp=>temp.PersonId==personId)
				.FirstOrDefaultAsync();
		}

        public async Task<List<Person>> GetPersonsByUserId(string userId)
        {
            // Μετατροπή του string σε Guid
            Guid userGuid = Guid.Parse(userId);
            return await _dB.Persons.Include("Country")
                .Where(p => p.UserId == userGuid)
                .ToListAsync();
        }


        public async Task<Person> UpdatePerson(Person person)
		{
			Person? matchingPerson = await _dB.Persons.FirstOrDefaultAsync(temp => temp.PersonId == person.PersonId);

			if (matchingPerson == null)
			{
				throw new InvalidPersonIdException("Given Person Id does not exist");
			}

			
			matchingPerson.PersonName = person.PersonName;
			matchingPerson.Email = person.Email;
			matchingPerson.DateOfBirth = person.DateOfBirth;
			matchingPerson.Gender = person.Gender;
			matchingPerson.Address = person.Address;
			matchingPerson.ReceiveNewsLeters = person.ReceiveNewsLeters;
			matchingPerson.TaxIdentificationNumber = person.TaxIdentificationNumber;
			

			int countUpdated=await _dB.SaveChangesAsync();
			return matchingPerson;
		}
	}
}
