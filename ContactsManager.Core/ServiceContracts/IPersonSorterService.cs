using ServiceContracts.Dto;
using ServiceContracts.Enums;


namespace ServiceContracts
{
	public interface IPersonSorterService
	{
		

		/// <summary>
		/// return sorted list of person
		/// </summary>
		/// <param name="allPersons">represent all objects of person in list</param>
		/// <param name="sortBy">name of the property (key), based on which the persons 
		/// should be sorted</param>
		/// <param name="sortOrder">Asc or Desc</param>
		/// <returns>returns sorted list of persons as PersonResponse</returns>
		public Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);


	}

}
