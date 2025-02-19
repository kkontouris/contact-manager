using ContactsManager.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
	public class ApplicationDbContext :IdentityDbContext<ApplicationUser,ApplicationRole, Guid>
	{

		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
			
		}


		public virtual DbSet<Country> Countries { get; set; }
		public virtual DbSet<Person> Persons { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Country>().ToTable("Countries");
			modelBuilder.Entity<Person>().ToTable("Persons");

           


            //Seed To Countries
            string countriesJson = File.ReadAllText("countries.json");
			List<Country>? countries=System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);
			foreach(Country country in countries)
			{
				modelBuilder.Entity<Country>().HasData(country);
			}
			//seed to Persons
			string personsJson = File.ReadAllText("persons.json");
			List<Person>? persons=System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);
			foreach (Person person in persons)
			{
				modelBuilder.Entity<Person>().HasData(person);
			}

			//fluent api
			modelBuilder.Entity<Person>().Property(property => property.TaxIdentificationNumber)
				.HasColumnName("TaxIdentificationNumber")
				.HasColumnType("varchar(9)");


			modelBuilder.Entity<Person>().HasCheckConstraint("CHK_TaxIdentificationNumber", "CHAR_LENGTH(TaxIdentificationNumber)=9");


		}
		public List<Person> sp_GetAllPersons()
		{
			return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
		}

		public int sp_InsertPerson(Person person)
		{
			SqlParameter[] parameters = new SqlParameter[]
			{
				new SqlParameter("@PersonId", person.PersonId),
				new SqlParameter("@PersonName", person.PersonName),
				new SqlParameter("@Email", person.Email),
				new SqlParameter("@DateOfBirth", person.DateOfBirth),
				new SqlParameter("@Gender", person.Gender),
				new SqlParameter("@Address", person.Address),
				new SqlParameter("@ReceiveNewsLeters", person.ReceiveNewsLeters),
				new SqlParameter("@TaxIdentificationNumber", person.TaxIdentificationNumber),
				new SqlParameter("@UserId", person.UserId)
			};
			return Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson] @PersonId,@PersonName, @Email,@DateOfBirth" +
				"@Gender,@Address,@ReceiveNewsLeters,@TaxIdentificationNumber, @UserId",parameters);
		}
	}
}
