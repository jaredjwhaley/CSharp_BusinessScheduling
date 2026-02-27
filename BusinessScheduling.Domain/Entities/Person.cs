using System;
using System.Net;
using BusinessScheduling.Domain.ValueObjects;

namespace BusinessScheduling.Domain.Entities
{
    /// <summary>
    /// Represents a person in the system, used for clients and employees.
    /// </summary>
    public class Person
    {
        // Identifier
        public Guid Id { get; private set; }

        // Core personal info
        public DateOnly DateOfBirth { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        // Value objects
        public PhotoId PhotoId { get; set; }
        public Address Address { get; set; }

        // Calculated properties
        public int Age {
            get
            {
                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                var age = today.Year - DateOfBirth.Year;
                if (DateOfBirth > today.AddYears(-age)) age--;
                return age;
            }
        }
        public string FullName { get => $"{FirstName} {LastName}".Trim(); }

        /// <summary>
        /// Initializes a new instance of the Person class with the specified personal and contact details.
        /// </summary>
        /// <param name="photoId">The unique identifier for the person's photo. Cannot be null.</param>
        /// <param name="firstName">The first name of the person. Cannot be null.</param>
        /// <param name="lastName">The last name of the person. Cannot be null.</param>
        /// <param name="dateOfBirth">The date of birth of the person. Cannot be null.</param>
        /// <param name="address">The address information for the person. Cannot be null.</param>
        /// <param name="email">The email address of the person. Defaults to an empty string if not provided.</param>
        /// <param name="phoneNumber">The phone number of the person. Defaults to an empty string if not provided.</param>
        /// <param name="id">An optional unique identifier for the person. Included for possible rehydration from a database. If not provided, a new GUID is generated.</param>
        /// <exception cref="ArgumentNullException">Thrown if photoId, firstName, lastName, dateOfBirth, or address is null.</exception>
        public Person(
            PhotoId photoId,
            string firstName,
            string lastName,
            DateOnly ?dateOfBirth,
            Address address,
            string email = "",
            string phoneNumber = "",
            Guid ?id = null)
        {
            PhotoId = photoId ?? throw new ArgumentNullException(nameof(photoId));
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            DateOfBirth = dateOfBirth ?? throw new ArgumentNullException(nameof(dateOfBirth));
            Address = address ?? throw new ArgumentNullException(nameof(address));
            Email = email;
            PhoneNumber = phoneNumber;
            Id = id ?? Guid.NewGuid();
        }
    }
}
