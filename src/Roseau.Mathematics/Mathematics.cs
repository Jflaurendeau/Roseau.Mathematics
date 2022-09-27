using CommunityToolkit.Diagnostics;

namespace Roseau.Mathematics;

public static class Mathematics
{
    // Precision of decimal type is 28-29 digits
    #region public Fields
    public const decimal E = 2.7182818284590452353602874714m;
    public const decimal EInverse = 1 / E;
    public const decimal Epsilon = 0.0000000000000000000000000001m;
    #endregion

    #region private Fields
    private const int iterations = 300;
    #endregion

    #region public methods
    public static int Factorial(int n)
    {
        Guard.IsGreaterThanOrEqualTo(n, 0);
        if (n <= 1) return 1;
        return n * Factorial(n - 1);
    }
    public static decimal Pow(decimal value, int n)
    {
        if (value.Equals(Decimal.Zero) && n <= 0) throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(value)} must be different than {value} if {nameof(n)} is equal of less than 0.");
        if (value.Equals(Decimal.Zero) && n > 0) return Decimal.Zero;
        if (IsExtremumValue(value) && n < 0) return Decimal.Zero;
        if (IsMaxDecimalValue(value) && n > 0) return Decimal.MaxValue;
        if (IsMinDecimalValue(value) && n > 0) return IsOdd(n) ? Decimal.MinValue : Decimal.MaxValue;
        if (n == 0) return Decimal.One;
        if (n > 0) return value * Pow(value, n - 1);
        return 1 / value * Pow(value, n + 1);
    }
    public static decimal Pow(decimal theBase, decimal theExponent)
    {
        decimal exponentDecimalPart = theExponent - (int)theExponent;
        if (exponentDecimalPart.Equals(Decimal.Zero)) return Pow(theBase, (int)theExponent);

        decimal power = Pow(theBase, (int)theExponent);
        decimal taylorSerie = TaylorSerieOfPowerFunction(theBase, exponentDecimalPart);
        return power * taylorSerie;
    }
    public static decimal Exp(decimal theExponent) => Pow(E, theExponent);
    public static decimal Log10(decimal value) => Log(10m, value);
    public static decimal Log(decimal theBase, decimal value) => Log(value) / Log(theBase);
    public static decimal Log(decimal value)
    {
        Guard.IsGreaterThan(value, Decimal.Zero);
        if (value.Equals(1)) return Decimal.Zero;
        if (value == E) return Decimal.One;
        if (value < 2) return TaylorSerieOfLogFunction(value);

        int integerPartOfExponent = 0;
        for (; value >= 2; integerPartOfExponent++)
            value /= E;

        return integerPartOfExponent + TaylorSerieOfLogFunction(value);
    }
    #region Helpers
    public static bool IsExtremumValue(decimal value) => IsMaxDecimalValue(value) || IsMinDecimalValue(value);
    public static bool IsMaxDecimalValue(decimal value) => value == Decimal.MaxValue;
    public static bool IsMinDecimalValue(decimal value) => value == Decimal.MinValue;
    public static bool IsOdd(int n) => n % 2 == 1;
    public static bool IsEven(int n) => !IsOdd(n);
    public static bool RepresentSameValue(decimal value1, decimal value2) => (value1 - value2).Equals(Decimal.Zero);
    #endregion
    #endregion

    #region Private methods
    private static decimal TaylorSerieOfPowerFunction(decimal theBase, decimal theExponent)
    {
        Guard.IsBetween(theExponent, -1, 1);
        decimal sum = Decimal.One;
        decimal lastTerm = Decimal.One;
        decimal logBase = Log(theBase);

        for (int i = 1; i < iterations; i++)
        {
            lastTerm *= logBase * theExponent / i;
            sum += lastTerm;
        }
        return sum;
    }
    private static decimal TaylorSerieOfLogFunction(decimal value)
    {
        Guard.IsBetween(value, 0, 2);
        decimal sum = Decimal.Zero;
        decimal lastTerm = Decimal.MinusOne;
        for (int i = 1; i < iterations; i++)
        {
            lastTerm *= -(value - Decimal.One);
            sum += lastTerm / i;
        }
        return sum;
    }
    #endregion
}