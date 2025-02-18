using ContactsManager.Core.Domain.IdentityEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
	public class Person
	{
		[Key]
		public Guid PersonId { get; set; }

		public string? PersonName { get; set; }
		public string? Email { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string? Gender { get; set; }
		public Guid? CountryId { get; set; }
		public string? Address { get;  set; }
		public bool ReceiveNewsLeters { get; set; }
		public string? TaxIdentificationNumber { get; set; }

		[ForeignKey("CountryId")]
		public Country? Country { get; set; }

        // Foreign Key for User
        [Required]
        public Guid UserId { get; set; }


        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }

        // Constructor με validation
        public Person(string? personName, string? email, DateTime? dateOfBirth, string? gender, Guid? countryId,
					  string? address, bool receiveNewsLeters, string? taxIdentificationNumber,
					  Guid userId)
		{
			// Validation για PersonName
			if (string.IsNullOrWhiteSpace(personName) || personName.Length > 40)
			{
				throw new ArgumentException("Το όνομα δεν πρέπει να είναι κενό και δεν πρέπει να υπερβαίνει τους 40 χαρακτήρες.");
			}

			// Validation για Email
			if (string.IsNullOrWhiteSpace(email) || email.Length > 40)
			{
				throw new ArgumentException("Το email δεν πρέπει να είναι κενό και δεν πρέπει να υπερβαίνει τους 40 χαρακτήρες.");
			}

			// Validation για Gender
			if (gender != null && gender.Length > 10)
			{
				throw new ArgumentException("Το φύλο δεν πρέπει να υπερβαίνει τους 10 χαρακτήρες.");
			}

			// Validation για Address
			if (address != null && address.Length > 200)
			{
				throw new ArgumentException("Η διεύθυνση δεν πρέπει να υπερβαίνει τους 200 χαρακτήρες.");
			}

			// Validation για TaxIdentificationNumber
			if (taxIdentificationNumber != null && taxIdentificationNumber.Length != 9)
			{
				throw new ArgumentException("Ο ΑΦΜ πρέπει να έχει ακριβώς 9 χαρακτήρες.");
			}

			//// Έλεγχος για το PersonId
			//if (PersonId == Guid.Empty)
			//{
			//	PersonId = Guid.NewGuid();  // Δημιουργία νέου Guid μόνο αν το PersonId είναι κενό
			//}

			PersonName = personName;
			Email = email;
			DateOfBirth = dateOfBirth;
			Gender = gender;
			CountryId = countryId;
			Address = address;
			ReceiveNewsLeters = receiveNewsLeters;
			TaxIdentificationNumber = taxIdentificationNumber;
            UserId = userId; 
        }
	}
}