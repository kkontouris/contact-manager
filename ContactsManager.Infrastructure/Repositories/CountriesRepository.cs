using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories
{
	public class CountriesRepository : ICountriesRepository
	{
		private readonly ApplicationDbContext _dB;

        public CountriesRepository(ApplicationDbContext dB)
        {
			_dB = dB;	
        }
        public async Task<Country> AddCountry(Country country)
		{
			_dB.Countries.Add(country);
			await _dB.SaveChangesAsync();

			return country;
		}


		public async Task<List<Country>> GetAllCountries()
		{
			return await _dB.Countries.ToListAsync();
		}

		public async Task<Country?> GetCountryByCountryID(Guid countryID)
		{
			return await _dB.Countries.FirstOrDefaultAsync(x=>x.CountryId== countryID);
		}

		public async Task<Country?> GetCountryByCountryName(string countryName)
		{
			return await _dB.Countries.FirstOrDefaultAsync(temp=>temp.CountryName== countryName);
		}
	}
}
