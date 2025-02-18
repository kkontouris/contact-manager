using ServiceContracts.Dto;
using ServiceContracts.Enums;


namespace ServiceContracts
{
	public interface IPersonDeleterService
	{
		

		/// <summary>
		/// Deletes a person based on the given person id
		/// </summary>
		/// <param name="PersonId">PersonId for the person to delete</param>
		/// <returns>true if the deletion is successfull or false</returns>
		public Task<bool> DeletePerson(Guid? PersonId);

	}

}
