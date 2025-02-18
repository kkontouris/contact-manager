using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace ServiceContracts.Dto
{

	/// <summary>
	/// Acts as a DTO for inserting a new Person
	/// </summary>
	public class PersonUpdateRequest
	{
		[Required(ErrorMessage = "Person Id can't be blanc")]
		public Guid PersonId { get; set; }

		[Required(ErrorMessage = "Person name can not be blanc")]
		public string? PersonName { get; set; }
		[Required(ErrorMessage = "Email can not be blanc")]
		[EmailAddress(ErrorMessage = "Email address should be in specific format")]
		public string? Email { get; set; }

		public DateTime? DateOfBirth { get; set; }

		public GenderOptions? Gender { get; set; }
		[Required(ErrorMessage = "Please select a country")]
		public Guid? CountryId { get; set; }

		public string? Address { get; set; }
		[StringLength(9, MinimumLength = 9, ErrorMessage = "Ο ΑΦΜ πρέπει να έχει ακριβώς 9 χαρακτήρες.")]
		public string? TaxIdentificationNumber { get; set; }

		public bool ReceiveNewsLetters { get; set; }

        public Guid UserId { get; set; }


        /// <summary>
        /// Converts the current object of PersonAUpdateRequest into a new
        /// Object of Person Type for the Domain Model
        /// </summary>
        /// <returns>Person Object</returns>
        public Person ToPerson()
		{
			return new Person (


				personName: PersonName,
				email: Email,
				dateOfBirth: DateOfBirth,
				gender: Gender.ToString(),
				countryId: CountryId,
				address: Address,
				receiveNewsLeters: ReceiveNewsLetters,
				taxIdentificationNumber: TaxIdentificationNumber,
                userId: UserId // Pass the UserId here

            ) ;



		}
	}
	
}
