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

        // TODO: Add format verification for email and phone number if needed in the future.
        // This could be done through a value object or by adding validation logic in the setters for these properties.
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        // Value objects
        /// <summary>
        /// The optional <see cref="PhotoId">photo ID</see> associated with the person,
        /// which is required for certain operations such as appointments.
        /// </summary>
        public PhotoId? PhotoId { get; set; }
        /// <summary>
        /// The <see cref="Address">physical address</see> associated with the entity.
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
        /// <param name="photoId">A technically optional <see cref="PhotoId"/>. This is required for appointments in our current design.</param>
        /// <param name="id">An optional unique identifier for the person. If not specified, a new GUID is generated.</param>
        /// <exception cref="ArgumentNullException">Thrown if firstName, lastName, dateOfBirth, or address is null.</exception>
        public Person(
            string firstName,
            string lastName,
            DateOnly? dateOfBirth,
            Address address,
            string email = "",
            string phoneNumber = "",
            PhotoId? photoId = null,
            Guid? id = null)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            DateOfBirth = dateOfBirth ?? throw new ArgumentNullException(nameof(dateOfBirth));
            Address = address ?? throw new ArgumentNullException(nameof(address));
            Email = email;
            PhoneNumber = phoneNumber;
            PhotoId = photoId;
            Id = id ?? Guid.NewGuid();
        }
    }
}
