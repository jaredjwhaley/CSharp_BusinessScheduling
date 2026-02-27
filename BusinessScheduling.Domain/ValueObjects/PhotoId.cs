namespace BusinessScheduling.Domain.ValueObjects;

public sealed class PhotoId : IEquatable<PhotoId>
{
    public string FilePath { get; }
    public DateOnly ExpirationDate { get; }

    public bool IsExpired(DateOnly? asOf = null)
    {
        var date = asOf ?? DateOnly.FromDateTime(DateTime.UtcNow);
        return ExpirationDate < date;
    }

    public PhotoId(string filePath, DateOnly expirationDate)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Photo ID file path is required.");

        FilePath = filePath;
        ExpirationDate = expirationDate;
    }

    public bool Equals(PhotoId? other)
    {
        if (other is null) return false;

        return FilePath == other.FilePath &&
               ExpirationDate == other.ExpirationDate;
    }

    public override bool Equals(object? obj) => Equals(obj as PhotoId);

    public override int GetHashCode()
        => HashCode.Combine(FilePath, ExpirationDate);
}