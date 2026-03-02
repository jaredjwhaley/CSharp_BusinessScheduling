using BusinessScheduling.Domain.Entities;
using BusinessScheduling.Domain.ValueObjects;
using Xunit;

namespace BusinessScheduling.Tests.Domain.Entities
{
    public class PersonTests
    {
        private readonly string _firstName = "John";
        private readonly string _lastName = "Doe";
        private readonly DateOnly _dob = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-30));
        private readonly string _email = "john@doe.com";
        private readonly string _phone = "555-1234";
        private readonly Address _address =
            new Address("123 Main St", "Anytown", UsState.AZ, "12345");

        private readonly PhotoId _photoId =
            new PhotoId("id.jpg", DateOnly.FromDateTime(DateTime.UtcNow.AddYears(2)));

        #region Constructor Tests

        [Fact]
        public void Constructor_WhenValid_CreatesPerson()
        {
            var person = new Person(
                _firstName,
                _lastName,
                _dob,
                _address,
                _email,
                _phone,
                _photoId
            );

            Assert.Equal(_firstName, person.FirstName);
            Assert.Equal(_lastName, person.LastName);
            Assert.Equal(_dob, person.DateOfBirth);
            Assert.Equal(_address, person.Address);
            Assert.Equal(_email, person.Email);
            Assert.Equal(_phone, person.PhoneNumber);
            Assert.Equal(_photoId, person.PhotoId);
            Assert.NotEqual(Guid.Empty, person.Id);
        }

        #endregion

        #region Derived Property Tests

        [Fact]
        public void Age_WhenBirthdayHasPassed_IsCorrect()
        {
            var dob = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-30).AddDays(-1));
            var person = new Person(_firstName, _lastName, dob, _address);

            Assert.Equal(30, person.Age);
        }

        [Fact]
        public void FullName_ReturnsFirstAndLast()
        {
            var person = new Person(_firstName, _lastName, _dob, _address);

            Assert.Equal("John Doe", person.FullName);
        }

        [Fact]
        public void FullName_WhenLastNameMissing_StillFormats()
        {
            var person = new Person("John", "", _dob, _address);

            Assert.Equal("John", person.FullName);
        }

        #endregion

        #region Id Tests

        [Fact]
        public void Constructor_WhenIdProvided_UsesIt()
        {
            var id = Guid.NewGuid();

            var person = new Person(
                _firstName,
                _lastName,
                _dob,
                _address,
                id: id
            );

            Assert.Equal(id, person.Id);
        }

        [Fact]
        public void Constructor_WhenIdNotProvided_GeneratesNewId()
        {
            var person1 = new Person(_firstName, _lastName, _dob, _address);
            var person2 = new Person(_firstName, _lastName, _dob, _address);
            Assert.NotEqual(Guid.Empty, person1.Id);
            Assert.NotEqual(Guid.Empty, person2.Id);
            Assert.NotEqual(person1.Id, person2.Id);
        }

        #endregion
    }
}
