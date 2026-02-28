namespace BusinessScheduling.Domain.ValueObjects;

/// <summary>
/// Represents a physical US address, including street, city, state, and postal code.
/// </summary>
/// <remarks>The Address class encapsulates address information as a value object and supports value equality
/// based on its properties. Two Address instances are considered equal if all their property values are equal. This
/// class is immutable; its properties are set at construction and cannot be changed.</remarks>
public sealed class Address : IEquatable<Address>
{
    public string Street { get; }
    public string City { get; }
    public UsState State { get; }
    public string PostalCode { get; }

    /// <summary>
    /// Initializes a new instance of the Address class using the specified street, city, state, and postal code.
    /// </summary>
    /// <param name="street">The street address. Cannot be null or empty.</param>
    /// <param name="city">The name of the city. Cannot be null or empty.</param>
    /// <param name="state">A value of the UsState enumeration that specifies the state for the address.</param>
    /// <param name="postalCode">The postal code for the address. Must be in a valid postal code format.</param>
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