using ServiceContracts;
using ServiceContracts.Dto;
using Entities;
using System.Threading.Tasks;
using RepositoryContracts;


namespace Services
{
	public class CountriesService : ICountriesService
	{
		//private field
		private readonly ICountriesRepository _countriesRepository;
        public CountriesService(ICountriesRepository countriesRepository)
        {
				_countriesRepository = countriesRepository;
        }

        #region AddCountry
        /// <summary>
        /// Give the CountryAddRequest and return the matching CountryResponse
        /// </summary>
        /// <param name="countryAddRequest"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
		{

			//Validation: The countryAddRequest can not be null
			if(countryAddRequest == null)
			{
				throw new ArgumentNullException(nameof(countryAddRequest));
			}
			//validation : if countryName is null throw exception
			if(countryAddRequest.CountryName == null)
			{
				throw new ArgumentException();
			}
			//Validation: countryName can not be duplicate throw argument exception

				if (await _countriesRepository.GetCountryByCountryName(countryAddRequest.CountryName)!=null)
				{
					throw new ArgumentException("Given country name already exists");	
				}
			
			Country country=countryAddRequest.ToCountry();

			country.CountryId = Guid.NewGuid();

			await _countriesRepository.AddCountry(country);
			
			return country.ToCountryResponse();
		}
		#endregion

		#region GetAllCountries
		/// <summary>
		/// Return all the countries of list
		/// </summary>
		/// <returns>List of CountryResponse</returns>
		public async Task<List<CountryResponse>> GetAllCountries()
		{
			return (await _countriesRepository.GetAllCountries())
				.Select(temp => temp.ToCountryResponse()).ToList();
		}
		#endregion

		#region GetCountryByCountryId
		/// <summary>
		/// Returns country object based on the given countryId
		/// </summary>
		/// <param name="CountryId">CountryId Guid</param>
		/// <returns>Matching Country as Country Response</returns>
		/// <exception cref="NotImplementedException"></exception>
		public async Task<CountryResponse?> GetCountryByCountryId(Guid? CountryId)
		{
			if (CountryId == null)
			{
				return null;
			}

			Country? country_response_from_list=await (_countriesRepository.GetCountryByCountryID(CountryId.Value));

			if (country_response_from_list == null)
				return null;
			 return (CountryResponse)country_response_from_list.ToCountryResponse();
		}


		#endregion
	}
}