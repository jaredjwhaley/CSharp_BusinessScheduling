namespace BusinessScheduling.Domain.ValueObjects;

public sealed class Address : IEquatable<Address>
{
    public string Street { get; }
    public string City { get; }
    public UsState State { get; }
    public string PostalCode { get; }

    public Address(string street, string city, UsState state, string postalCode)
    {
        Street = street;
        City = city;
        State = state;
        PostalCode = postalCode;
    }


    #region Equality by Value

    public bool Equals(Address? other)
    {
        if (other is null) return false;

        return Street == other.Street &&
               City == other.City &&
               State == other.State &&
               PostalCode == other.PostalCode;
    }

    public override bool Equals(object? obj) => Equals(obj as Address);

    public override int GetHashCode()
        => HashCode.Combine(Street, City, State, PostalCode);

    #endregion
}