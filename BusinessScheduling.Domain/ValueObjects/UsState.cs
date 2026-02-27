namespace BusinessScheduling.Domain.ValueObjects;

/// <summary>
/// Specifies the two-letter postal abbreviations for the U.S. states.
/// </summary>
/// <remarks>This enumeration provides a standardized set of values for identifying U.S. states using their
/// official postal codes. It is useful for applications that require state validation, storage, or display in
/// accordance with postal standards.</remarks>
public enum UsState
{
    AL, AK, AZ, AR, CA, CO, CT, DE, FL, GA,
    HI, ID, IL, IN, IA, KS, KY, LA, ME, MD,
    MA, MI, MN, MS, MO, MT, NE, NV, NH, NJ,
    NM, NY, NC, ND, OH, OK, OR, PA, RI, SC,
    SD, TN, TX, UT, VT, VA, WA, WV, WI, WY
}