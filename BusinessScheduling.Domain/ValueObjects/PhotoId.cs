namespace BusinessScheduling.Domain.ValueObjects;

/// <summary>
/// Represents a government issued photo id.
/// </summary>
/// <remarks>A PhotoId encapsulates the information required to reference a photo and determine its validity based
/// on an expiration date. Instances of this class are considered equal if both the filename and expiration date are
/// the same. The filename must be a non-empty string when creating a new instance.</remarks>
public sealed class PhotoId : IEquatable<PhotoId>
{
    /// <summary>
    /// Gets the filename of the image associated with this object.
    /// </summary>
    public string FileName { get; }
    // TODO: Eventually make this a derived property that pulls the expirationdate from the filename.
    // Would need to be enforced in the constructor that the filename contains a valid expiration date
    // in a specific format. This would eliminate the need for an explicit ExpirationDate property and
    // reduce the risk of inconsistencies between the filename and expiration date.
    public DateOnly ExpirationDate { get; }
    // TODO: Consider adding a property for the type of photo ID (e.g., driver's license, passport) if
    // needed for future functionality. This should be a derived property based on the filename, and will
    // require its own separate enum for the different types of photo IDs. This would allow for more specific
    // handling of different ID types in the future without requiring changes to the core PhotoId class.
    // public PhotoIdType Type { get; }

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
    /// Initializes a new instance of the PhotoId class using the specified filename and optional expiration date.
    /// </summary>
    /// <param name="fileName">The filename of the photo ID. This value must not be null, empty, or consist only of white-space characters.</param>
    /// <param name="expirationDate">Expiration date for the photo ID. Must not be null.</param>
    /// <exception cref="ArgumentException">Thrown if the filename is null, empty, or consists only of white-space characters, or if the expiration date is null.</exception>
    public PhotoId(string fileName, DateOnly? expirationDate)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("Photo ID filename is required.");

        FileName = fileName;
        ExpirationDate = expirationDate ?? throw new ArgumentException("Expiration date must not be null.");
    }


    #region Equality by Value

    public bool Equals(PhotoId? otherId)
    {
        if (otherId is null) return false;

        return FileName == otherId.FileName;
    }

    public override bool Equals(object? obj) => Equals(obj as PhotoId);

    public override int GetHashCode()
        => HashCode.Combine(FileName);

    #endregion
}