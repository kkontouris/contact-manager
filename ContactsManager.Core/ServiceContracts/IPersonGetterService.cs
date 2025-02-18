using ServiceContracts.Dto;
using ServiceContracts.Enums;


namespace ServiceContracts
{
	public interface IPersonGetterService
	{


		/// <summary>
		/// Returns all persons
		/// </summary>
		/// <returns>returns a list of persons with type PersonResponse</returns>
		Task<List<PersonResponse>> GetAllPersons();

		/// <summary>
		/// Returns the person object based on the personId
		/// </summary>
		/// <param name="personId">Person Id to search</param>
		/// <returns>Returns matching Person Object</returns>
		public Task<PersonResponse> GetPersonByPersonId(Guid? personId);

		/// <summary>
		/// Get all the specific persons objects based on the searchBy and searchstrings which is gived 
		/// fromthe user 
		/// </summary>
		/// <param name="searchBy">search field to search</param>
		/// <param name="searchString">search string to search</param>
		/// <returns>List of Person objects</returns>
		public Task<List<PersonResponse>> GetFilteredPersons(string userId,string searchBy, string? searchString);



	}

}
