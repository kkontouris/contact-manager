using Entities;
namespace RepositoryContracts
{
	/// <summary>
	/// represents data access logic for managing person Entity
	/// </summary>
	public interface ICountriesRepository
	{
		/// <summary>
		/// Adds a new country object to the data store
		/// </summary>
		/// <param name="country">country object to add</param>
		/// <returns>the country object after adding it to the data store</returns>
		Task<Country> AddCountry(Country country);

		/// <summary>
		/// Returns all the countries of the data store
		/// </summary>
		/// <returns>all countries from the table</returns>
		Task<List<Country>> GetAllCountries();


		/// <summary>
		/// Returns a country object based on the given country id; otherwise, it returns null
		/// </summary>
		/// <param name="countryID">CountryID to search</param>
		/// <returns>Matching country or null</returns>
		Task<Country?> GetCountryByCountryID(Guid countryID);


		/// <summary>
		/// Returns a country object based on the given country name
		/// </summary>
		/// <param name="countryName">Country name to search</param>
		/// <returns>Matching country or null</returns>
		Task<Country?> GetCountryByCountryName(string countryName);
	}
}
