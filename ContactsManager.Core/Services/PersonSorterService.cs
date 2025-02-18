﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts;
using ServiceContracts.Dto;
using Entities;
using ServiceContracts.Enums;
using System.Collections;
using Services.Helpers;
using System.Threading.Tasks;
using RepositoryContracts;

namespace Services
{
	public class PersonSorterService : IPersonSorterService
	{
		private readonly IPersonsRepository _personsRepository;


		public PersonSorterService(IPersonsRepository personsRepository)
        {
			_personsRepository = personsRepository;		
        }


		public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
		{
			if (string.IsNullOrEmpty(sortBy))
			{
				return allPersons;
			}
			List<PersonResponse> sortedPersons = (sortBy, sortOrder)
			switch
			{
				(nameof(PersonResponse.PersonName), (SortOrderOptions.ASC))
				=> allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
				
				(nameof(PersonResponse.PersonName), (SortOrderOptions.DESC))
				=> allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Age), (SortOrderOptions.ASC))
				=> allPersons.OrderBy(temp => temp.Age).ToList(),

				(nameof(PersonResponse.Age), (SortOrderOptions.DESC))
				=> allPersons.OrderByDescending(temp => temp.Age).ToList(),

				(nameof(PersonResponse.Address), (SortOrderOptions.ASC))
				=> allPersons.OrderBy(temp => temp.Address,StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Address), (SortOrderOptions.DESC))
				=> allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Email), (SortOrderOptions.ASC))
				=> allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Email), (SortOrderOptions.DESC))
				=> allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.DateOfBirth), (SortOrderOptions.ASC))
				=> allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),

				(nameof(PersonResponse.DateOfBirth), (SortOrderOptions.DESC))
				=> allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

				(nameof(PersonResponse.Gender), (SortOrderOptions.ASC))
				=> allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Gender), (SortOrderOptions.DESC))
				=> allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.ReceiveNewsLetters), (SortOrderOptions.ASC))
				=> allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),

				(nameof(PersonResponse.ReceiveNewsLetters),(SortOrderOptions.DESC))
				=> allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

				_=>allPersons

			};
			return sortedPersons;
		}

		
	}
}
