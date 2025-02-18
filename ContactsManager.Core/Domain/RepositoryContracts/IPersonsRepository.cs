using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace RepositoryContracts
{
	/// <summary>
	/// reprersents data access logic for managing person entity
	/// </summary>
	public interface IPersonsRepository
	{
		/// <summary>
		/// adds a person object to the data store
		/// </summary>
		/// <param name="person">Person object</param>
		/// <returns>the persons object after adding it to the table</returns>
		Task<Person> AddPerson(Person person);

		/// <summary>
		/// returns a list of person objects from the data store
		/// </summary>
		/// <returns></returns>
		Task<List<Person>> GetAllPersons();


		/// <summary>
		/// Return the person objct with the same id
		/// </summary>
		/// <param name="id">id of the person</param>
		/// <returns>the person object form the table or null</returns>
		Task<Person?> GetPersonByPersonId(Guid? personId);


		/// <summary>
		/// returns all thw person objects based on the given expression
		/// </summary>
		/// <param name="predicate">LINQ expression to check</param>
		/// <returns>list of person objects who match with the given condition</returns>
		Task<List<Person>> GetFilteredPersons(string userId, Expression<Func<Person, bool>> predicate);


		/// <summary>
		/// deletes a person based on the given person id
		/// </summary>
		/// <param name="personId">given person id</param>
		/// <returns>true if the deletion is succeed</returns>
		Task<bool> DeletePersonWithPersonIdAsync(Guid personId, CancellationToken token);

		/// <summary>
		/// updates person object based on the given person id
		/// </summary>
		/// <param name="person">person object to update</param>
		/// <returns>the updated person object</returns>
		Task<Person> UpdatePerson(Person person);

		 Task<List<Person>> GetPersonsByUserId(string userId);

    }
}
