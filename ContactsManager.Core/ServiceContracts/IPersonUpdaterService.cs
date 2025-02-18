using ServiceContracts.Dto;
using ServiceContracts.Enums;


namespace ServiceContracts
{
	public interface IPersonUpdaterService
	{



		/// <summary>
		/// Updates the specified given Person object based on the specified Person Id
		/// </summary>
		/// <param name="request">Person Details to Update</param>
		/// <returns>The PersonResponse object after update</returns>
		public Task<PersonResponse> UpdatePerson(PersonUpdateRequest? request);




	}

}
