using BusinessScheduling.Domain.ValueObjects;
using Xunit;

namespace BusinessScheduling.Tests.Domain.ValueObjects
{
    public class AddressTests : IDisposable
    {
        // These fields are initialized fresh for EACH test
        // because xUnit creates a new instance of this class per test.
        private readonly string _street;
        private readonly string _city;
        private readonly UsState _state;
        private readonly string _postalCode;

        private readonly Address _baselineAddress;

        /// <summary>
        /// Constructor runs BEFORE EACH test.
        /// This replaces JUnit's @BeforeEach.
        /// </summary>
        /// <remarks>
        /// This is often simpler than uing the MethodData and ClassData attributes for simple shared setup.
        /// </remarks>
        public AddressTests()
        {
            // Arrange shared test data here
            _street = "123 Redmont St";
            _city = "Birmingham";
            _state = UsState.AL;
            _postalCode = "35203";

            _baselineAddress = new Address(_street, _city, _state, _postalCode);
        }

        /// <summary>
        /// Dispose runs AFTER EACH test.
        /// This replaces JUnit's @AfterEach.
        /// Useful for cleanup of unmanaged resources,
        /// temp files, database connections, etc.
        /// </summary>
        public void Dispose()
        {
            // NOTE: Nothing to clean up yet, but included intentionally
            // to demonstrate the lifecycle pattern.
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_WhenGivenValidValues_CreatesInstance()
        {
            // Act
            var address = new Address(_street, _city, _state, _postalCode);

            // Assert
            Assert.Equal(_street, address.Street);
            Assert.Equal(_city, address.City);
            Assert.Equal(_state, address.State);
            Assert.Equal(_postalCode, address.PostalCode);
        }

        // TODO: Implement validation tests for null/empty strings, invalid postal codes, etc.
        // These would require adding validation logic to the Address constructor and then
        // testing that it throws appropriate exceptions.

        #endregion

        #region Equality Tests

        [Fact]
        public void Equals_WhenAllPropertiesMatch_ReturnsTrue()
        {
            var address2 = new Address(_street, _city, _state, _postalCode);

            Assert.Equal(_baselineAddress, address2);
        }

        [Fact]
        public void Equals_WhenStreetDiffers_ReturnsFalse()
        {
            var different = new Address("456 Oak Ave", _city, _state, _postalCode);

            Assert.NotEqual(_baselineAddress, different);
        }

        [Fact]
        public void Equals_WhenCityDiffers_ReturnsFalse()
        {
            var different = new Address(_street, "Hoover", _state, _postalCode);

            Assert.NotEqual(_baselineAddress, different);
        }

        [Fact]
        public void Equals_WhenStateDiffers_ReturnsFalse()
        {
            var different = new Address(_street, _city, UsState.GA, _postalCode);

            Assert.NotEqual(_baselineAddress, different);
        }

        [Fact]
        public void Equals_WhenPostalCodeDiffers_ReturnsFalse()
        {
            var different = new Address(_street, _city, _state, "30004");

            Assert.NotEqual(_baselineAddress, different);
        }

        [Fact]
        public void GetHashCode_WhenAddressesAreEqual_ReturnsSameHashCode()
        {
            var address2 = new Address(_street, _city, _state, _postalCode);

            Assert.Equal(_baselineAddress.GetHashCode(), address2.GetHashCode());
        }

        #endregion
    }
}
