using BusinessScheduling.Domain.ValueObjects;
using BusinessScheduling.Domain.Entities;
using Xunit;

namespace BusinessScheduling.Tests.Domain.Entities
{
    public class PersonTests : IDisposable
    {
        #region Setup and Teardown

        // Identifier
        private Guid _id;

        // Core personal info
        private DateOnly _dob;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _phoneNumber;

        // Value objects
        private PhotoId? _photoId;
        private Address _address;

        public PersonTests() {
            _id = Guid.NewGuid();
            _firstName = "John";
            _lastName = "Doe";
            _dob = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-30)); // 30 years old
            _email = "johndoe@somesite.org";
            _phoneNumber = "555-123-4567";
            _photoId = new PhotoId("validFilename.jpg", DateOnly.FromDateTime(DateTime.UtcNow.AddYears(2)));
            _address = new Address("123 Main St", "Anytown", UsState.AZ, "12345");
        }
        public void Dispose()
        {
            // TODO: ChatGPT, explain this to me please. I get that it's for
            // teardown, but I'm not sure what, if anything, needs to be
            // explicitly removed as part of the dispose method.
        }
        #endregion

        #region Constructor Tests
        [Fact]
        public void Constructor_WhenGivenValidValues_CreatesInstance()
        {
            // Act
            var person = new Person(
                firstName: _firstName,
                lastName: _lastName,
                dateOfBirth: _dob,
                address: _address,
                email: _email,
                phoneNumber: _phoneNumber,
                photoId: _photoId,
                id: _id
            );
            // Assert
            Assert.Equal(_id, person.Id);
            Assert.Equal(_firstName, person.FirstName);
            Assert.Equal(_lastName, person.LastName);
            Assert.Equal(_dob, person.DateOfBirth);
            Assert.Equal(_email, person.Email);
            Assert.Equal(_phoneNumber, person.PhoneNumber);
            Assert.Equal(_photoId, person.PhotoId);
            Assert.Equal(_address, person.Address);
        }

        // Enumerable data for invalid constructor tests.
        // Required to test multiple null parameters utilizing MemberData
        // without needing to write multiple [Fact] tests
        public static IEnumerable<object[]> InvalidConstructorData()
        {
            var validDob = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-30));
            var validAddress = new Address("123 Main St", "Anytown", UsState.AZ, "12345");
            var validPhotoId = new PhotoId("valid.jpg", DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1)));
            var validId = Guid.NewGuid();

            yield return new object[] { null, "Doe", validDob, validAddress, "a@b.com", "555", validPhotoId, validId };
            yield return new object[] { "John", null, validDob, validAddress, "a@b.com", "555", validPhotoId, validId };
            yield return new object[] { "John", "Doe", null, validAddress, "a@b.com", "555", validPhotoId, validId };
            yield return new object[] { "John", "Doe", validDob, null, "a@b.com", "555", validPhotoId, validId };
        }

        [Theory]
        [MemberData(nameof(InvalidConstructorData))]
        public void Constructor_WhenRequiredArgumentsAreNull_ThrowsArgumentNullException(
            string firstName, string lastName, DateOnly? dateOfBirth, Address address,
            string email, string phoneNumber, PhotoId? photoId, Guid id)
        {
            Assert.Throws<ArgumentNullException>(() =>
                new Person( firstName, lastName, dateOfBirth, address,
                    email, phoneNumber, photoId, id
                ));
        }

        #endregion

        #region ???
        #endregion
    }
}
