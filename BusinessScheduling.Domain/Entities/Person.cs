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
        /// <summary>
        /// The unique identifier for the <see cref="PhotoId"> on file for this client.
        /// </summary>
        public Guid PhotoIdGuid { get; set; }
        /// <summary>
        /// Gets or sets the street address information associated with the entity.
        /// </summary>
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
        /// Initializes a new instance of the Person class with the specified personal details, contact information, and
        /// optional identifiers.
        /// </summary>
        /// <param name="firstName">The first name of the person. Cannot be null.</param>
        /// <param name="lastName">The last name of the person. Cannot be null.</param>
        /// <param name="dateOfBirth">The date of birth of the person. Cannot be null.</param>
        /// <param name="address">The address associated with the person. Cannot be null.</param>
        /// <param name="email">The email address of the person. If not specified, defaults to an empty string.</param>
        /// <param name="phoneNumber">The phone number of the person. If not specified, defaults to an empty string.</param>
        /// <param name="photoIdGuid">An optional GUID representing the person's photo ID. If not specified, defaults to an empty GUID. This is required for appointments.</param>
        /// <param name="id">An optional unique identifier for the person. If not specified, a new GUID is generated.</param>
        /// <exception cref="ArgumentNullException">Thrown if firstName, lastName, dateOfBirth, or address is null.</exception>
        public Person(
            string firstName,
            string lastName,
            DateOnly ?dateOfBirth,
            Address address,
            string email = "",
            string phoneNumber = "",
            Guid? photoIdGuid = null,
            Guid? id = null)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            DateOfBirth = dateOfBirth ?? throw new ArgumentNullException(nameof(dateOfBirth));
            Address = address ?? throw new ArgumentNullException(nameof(address));
            Email = email;
            PhoneNumber = phoneNumber;
            PhotoIdGuid = photoIdGuid ?? Guid.Empty;
            Id = id ?? Guid.NewGuid();
        }
    }
}
