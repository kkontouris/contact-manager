using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ContactsManager.Core.Enums;
using Microsoft.AspNetCore.Mvc;


namespace ContactsManager.Core.DTO
{
	public class RegisterDTO
	{
		[Required(ErrorMessage ="Name can't be blanc")]
        public string? PersonName { get; set; }

		[Required(ErrorMessage = "Email can't be blanc")]
		[EmailAddress(ErrorMessage ="Email should be in a proper email address format")]
		[Remote(action: "IsEmailAlreadyRegistered", controller:"Account", ErrorMessage="Email" +
			" is Already In Use")]    //Remote validation idf email is already exists
		public string? Email { get; set; }
		
		[Required(ErrorMessage = "Phone can't be blanc")]
		[RegularExpression("^[0-9]*$", ErrorMessage ="Phone number shoyuld contain only numbers")]
		[DataType(DataType.PhoneNumber)]
		public string? Phone { get; set; }

		[Required(ErrorMessage = "Password can't be blanc")]
		[DataType(DataType.Password)]
		public string? Password { get; set; }

		[Required(ErrorMessage = "Confirm Password can't be blanc")]
		[DataType(DataType.Password)]
		[Compare("Password",ErrorMessage="Password and Confirm Password do not match")]
		public string? ConfirmPassword { get; set; }

		public UserTypeOptions userType { get; set; } =UserTypeOptions.User;

	}
}
