using ServiceContracts.Dto;

namespace ServiceContracts
{
	/// <summary>
	/// Represents the business logic of manipulating Country Entity
	/// </summary>
	public interface ICountriesService
	{
		/// <summary>
		/// adds a country object to the list of countries
		/// </summary>
		/// <param name="countryAddRequest">country object to add</param>
		/// <returns>country object to list</returns>
		public Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);

		public Task<List<CountryResponse>> GetAllCountries();

		public Task<CountryResponse?> GetCountryByCountryId(Guid? CountryId);
	}
}