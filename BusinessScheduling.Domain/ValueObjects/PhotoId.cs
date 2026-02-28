namespace BusinessScheduling.Domain.ValueObjects;

/// <summary>
/// Represents a government issued photo id.
/// </summary>
/// <remarks>A PhotoId encapsulates the information required to reference a photo and determine its validity based
/// on an expiration date. Instances of this class are considered equal if both the file path and expiration date are
/// the same. The file path must be a non-empty string when creating a new instance.</remarks>
public sealed class PhotoId : IEquatable<PhotoId>
{
    /// <summary>
    /// Gets the file path of the image associated with this object.
    /// </summary>
    public string FilePath { get; }
    public DateOnly ExpirationDate { get; }

    /// <summary>
    /// Determines whether the current item has expired based on its expiration date.
    /// </summary>
    /// <remarks>This method compares the expiration date with the current date in UTC. Ensure that the
    /// expiration date is set correctly to avoid unexpected results.</remarks>
    /// <returns>true if the expiration date is earlier than the current UTC date; otherwise, false.</returns>
    public bool IsExpired()
    {
        return ExpirationDate < DateOnly.FromDateTime(DateTime.UtcNow);
    }

    /// <summary>
    /// Initializes a new instance of the PhotoId class using the specified file path and optional expiration date.
    /// </summary>
    /// <param name="filePath">The file path of the photo ID. This value must not be null, empty, or consist only of white-space characters.</param>
    /// <param name="expirationDate">Expiration date for the photo ID. Must not be null.</param>
    /// <exception cref="ArgumentException">Thrown if the file path is null, empty, or consists only of white-space characters, or if the expiration date is null.</exception>
    public PhotoId(string filePath, DateOnly? expirationDate)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Photo ID file path is required.");

        FilePath = filePath;
        ExpirationDate = expirationDate ?? throw new ArgumentException("Expiration date must not be null.");
    }


    #region Equality by Value

    public bool Equals(PhotoId? other)
    {
        if (other is null) return false;

        return FilePath == other.FilePath &&
               ExpirationDate == other.ExpirationDate;
    }

    public override bool Equals(object? obj) => Equals(obj as PhotoId);

    public override int GetHashCode()
        => HashCode.Combine(FilePath, ExpirationDate);

    #endregion
}