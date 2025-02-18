using ServiceContracts.Dto;
using ServiceContracts.Enums;


namespace ServiceContracts
{
	public interface IPersonAdderService
	{
		/// <summary>
		/// Adds a new person into the list of persons
		/// </summary>
		/// <param name="request">Person to add</param>
		/// <returns>Returns the same person details, with PersonId</returns>
		/// <exception cref="NotImplementedException"></exception>
		public Task<PersonResponse> AddPerson(PersonAddRequest request);

		
	}

}
