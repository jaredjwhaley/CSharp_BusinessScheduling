using BusinessScheduling.Domain.ValueObjects;
using System.Globalization;
using Xunit;

namespace BusinessScheduling.Tests.Domain.ValueObjects
{
    public class PhotoIdTests
    {
        #region Constructor Tests
        [Fact]
        public void Constructor_WhenFileNameIsNull_ThrowsArgumentException()
        {
            // Arrange
            string? fileName = null;
            var expirationDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1);

            // Act
            // NOTE: "!" added after the fileName variable to suppress nullable warning since we're intentionally
            // passing null to test the constructor's behavior.
            Action act = () => new PhotoId(fileName!, expirationDate);

            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void Constructor_WhenFileNameIsEmpty_ThrowsArgumentException()
        {
            // Arrange
            string fileName = "";
            var expirationDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1);
            // Act
            Action act = () => new PhotoId(fileName, expirationDate);
            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void Constructor_WhenFileNameIsWhitespace_ThrowsArgumentException()
        {
            // Arrange
            string fileName = "   ";
            var expirationDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1);
            // Act
            Action act = () => new PhotoId(fileName, expirationDate);
            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void Constructor_WhenExpirationDateIsNull_ThrowsArgumentException()
        {
            // Arrange
            string fileName = "valid_filename.jpg";
            DateOnly? expirationDate = null;
            // Act
            Action act = () => new PhotoId(fileName, expirationDate);
            // Assert
            Assert.Throws<ArgumentException>(act);
        }
        #endregion

        #region Expiration Tests
        [Fact]
        public void IsExpired_WhenExpirationDateIsInThePast_ReturnsTrue()
        {
            // Arrange
            var expirationDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-1);
            var photoId = new PhotoId("valid_filename.jpg", expirationDate);
            // Act
            var isExpired = photoId.IsExpired();
            // Assert
            Assert.True(isExpired);
        }

        [Fact]
        public void IsExpired_WhenExpirationDateIsInTheFuture_ReturnsFalse()
        {
            // Arrange
            var expirationDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1);
            var photoId = new PhotoId("valid_filename.jpg", expirationDate);
            // Act
            var isExpired = photoId.IsExpired();
            // Assert
            Assert.False(isExpired);
        }

        [Fact]
        public void IsExpired_WhenExpirationDateIsToday_ReturnsFalse()
        {
            // Arrange
            var expirationDate = DateOnly.FromDateTime(DateTime.UtcNow);
            var photoId = new PhotoId("valid_filename.jpg", expirationDate);
            // Act
            var isExpired = photoId.IsExpired();
            // Assert
            Assert.False(isExpired);
        }
        #endregion

        #region Equality Tests
        [Fact]
        public void Equals_WhenFileNameAndExpirationDateAreEqual_ReturnsTrue()
        {
            // Arrange
            var expirationDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1);
            var photoId1 = new PhotoId("valid_filename.jpg", expirationDate);
            var photoId2 = new PhotoId("valid_filename.jpg", expirationDate);
            // Act
            var areEqual = photoId1.Equals(photoId2);
            // Assert
            Assert.True(areEqual);
        }

        [Fact]
        public void Equals_WhenFileNameIsDifferent_ReturnsFalse()
        {
            // Arrange
            var expirationDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1);
            var photoId1 = new PhotoId("valid_filename.jpg", expirationDate);
            var photoId2 = new PhotoId("different_filename.jpg", expirationDate);
            // Act
            var areEqual = photoId1.Equals(photoId2);
            // Assert
            Assert.False(areEqual);
        }

        [Fact]
        public void Equals_WhenExpirationDateIsDifferent_ReturnsFalse()
        {
            // Arrange
            var photoId1 = new PhotoId("valid_filename.jpg", DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1));
            var photoId2 = new PhotoId("valid_filename.jpg", DateOnly.FromDateTime(DateTime.UtcNow).AddDays(2));
            // Act
            var areEqual = photoId1.Equals(photoId2);
            // Assert
            Assert.False(areEqual);
        }
        #endregion
    }
}
