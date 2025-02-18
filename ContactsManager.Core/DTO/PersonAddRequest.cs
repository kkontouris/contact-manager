﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.Enums;
using Entities;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.Dto
{
	/// <summary>
	/// Acts as a DTO for inserting a new Person
	/// </summary>
	public class PersonAddRequest
	{
		[Required(ErrorMessage ="Person name can not be blanc")]
		[StringLength(40, ErrorMessage = "Το όνομα δεν πρέπει να υπερβαίνει τους 40 χαρακτήρες.")]
		public string? PersonName { get; set; }

		[DataType(DataType.EmailAddress)]
		[Required(ErrorMessage = "Email can not be blanc")]
		[EmailAddress(ErrorMessage ="Email address should be in specific format")]
		public string? Email { get; set; }

		[DataType(DataType.Date)]
		public DateTime? DateOfBirth { get; set; }

        public GenderOptions? Gender { get; set; }

		[Required(ErrorMessage ="Please select a country")]
		public Guid? CountryId { get; set; }

		public string? Address { get; set; }
		[StringLength(9, MinimumLength = 9, ErrorMessage = "Ο ΑΦΜ πρέπει να έχει ακριβώς 9 χαρακτήρες.")]
		public string? TaxIdentificationNumber { get; set; }

		public bool ReceiveNewsLetters { get; set; }


        public Guid UserId { get; set; }


        /// <summary>
        /// Converts the current object of PersonAddRequest into a new
        /// Object of Person Type for the Domain Model
        /// </summary>
        /// <returns></returns>
        public Person ToPerson()
		{
			return new Person (
				personName : PersonName,
				email : Email,
				dateOfBirth: DateOfBirth,
				gender: Gender.ToString(),
				countryId: CountryId,
				address: Address,
				receiveNewsLeters: ReceiveNewsLetters,
				taxIdentificationNumber: TaxIdentificationNumber,
				userId:UserId
			);

		}
    }
}
