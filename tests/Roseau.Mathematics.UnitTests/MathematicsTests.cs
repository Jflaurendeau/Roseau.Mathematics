namespace Roseau.Mathematics.UnitTests;

[TestClass]
public class MathematicsTests
{
    [TestMethod]
    [DataRow(-2)]
    public void Factorial_AreEqual_ThrowOutOfRange(int n)
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Mathematics.Factorial(n));
    }
    [TestMethod]
    [DataRow(0, 1)]
    [DataRow(2, 2)]
    [DataRow(4, 24)]
    [DataRow(10, 3628800)]
    public void Factorial_AreEqual_ToFeededResult(int n, int result)
    {
        Assert.AreEqual(result, Mathematics.Factorial(n));
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    public void Pow_ValueIsZeroAndExponentIsEqualOrLessThanZero_ThrowException(int n)
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Mathematics.Pow(0m, n));
    }
    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    [DataRow(31)]
    public void Pow_ValueIsZeroAndExponentIsMoreThanZero_EqualsToZero(int n)
    {
        Assert.AreEqual(0m, Mathematics.Pow(0m, n));
    }
    [TestMethod]
    [DataRow(-1)]
    [DataRow(-7)]
    public void Pow_ValueIsExtremeAndExponentIsLessOrEqualToMinusOne_EqualsToZero(int n)
    {
        Assert.AreEqual(0m, Mathematics.Pow(79228162514264337593543950335m, n));
        Assert.AreEqual(0m, Mathematics.Pow(-79228162514264337593543950335m, n));
    }
    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void Pow_ValueIsMaximumAndExponentIsMoreOrEqualToOne_EqualsToMaximum(int n)
    {
        Assert.AreEqual(79228162514264337593543950335m, Mathematics.Pow(79228162514264337593543950335m, n));
    }
    [TestMethod]
    [DataRow(1)]
    [DataRow(7)]
    public void Pow_ValueIsMinimumAndExponentIsOddAndMoreOrEqualToOne_EqualsToMinimum(int n)
    {
        Assert.AreEqual(-79228162514264337593543950335m, Mathematics.Pow(-79228162514264337593543950335m, n));
    }
    [TestMethod]
    [DataRow(2)]
    [DataRow(8)]
    public void Pow_ValueIsMinimumAndExponentIsEvenAndMoreThanOne_EqualsToMinimum(int n)
    {
        Assert.AreEqual(79228162514264337593543950335m, Mathematics.Pow(-79228162514264337593543950335m, n));
    }
    [TestMethod]
    public void Pow_ValueIsNotZeroAndExponentIsZero_EqualsToOne()
    {
        Assert.AreEqual(Decimal.One, Mathematics.Pow(-79228162514264337593543950335m, Decimal.Zero));
        Assert.AreEqual(Decimal.One, Mathematics.Pow(79228162514264337593543950335m, Decimal.Zero));
        Assert.AreEqual(Decimal.One, Mathematics.Pow(-50, Decimal.Zero));
        Assert.AreEqual(Decimal.One, Mathematics.Pow(50, Decimal.Zero));
    }
    [TestMethod]
    [DataRow(-7)]
    [DataRow(2)]
    [DataRow(8)]
    public void Pow_DecimalsToIntPower_EqualsToGoodResult(int n)
    {
        // Arrange
        decimal exactResults = 1;
        decimal theBase = n < 0 ? Mathematics.EInverse : Mathematics.E;

        // Act
        for (int i = 0; i < Math.Abs(n); i++)
            exactResults *= theBase;

        // Assert 
        Assert.AreEqual(exactResults, Mathematics.Pow(Mathematics.E, n));
    }
    [TestMethod]
    public void Pow_DecimalsToDecimalPower_EqualsToGoodResult()
    {
        // Arrange

        // exactResult is E^(3*E) calculated from Wolfram
        // In decimal is: 2.7182818284590452353602874714^(8.154845485377135706080862414)
        // decimal exactResult = 3480.201541713829450221631224m;
        // tempo = E * E =       7.3890560989306502272304274608m
        decimal exactResultIntPart = 1m;

        // The taylor serie result is the result of TaylorSerieOfPowerFunction, for which I compared the result with Wolfram. 
        // For Woflram Alpha: 2.7182818284590452353602874714^(0.154845485377135706080862414) = 1.167_477_554_813_694_402_482_495_731_666_...
        // So difference in last digits is: TaylorSerieOfPowerFunction - Wolfram = ...7315 - ...731_6 = -...0001 = -Mathematics.Epsilon (ce qui représent le plus petit nombre de type decimal)
        decimal taylorSeriePart = 1.1674775548136944024824957315M;

        // Act
        for (int i = 0; i < 8; i++)
        {
            exactResultIntPart *= Mathematics.E;
        }

        // Assert 
        Assert.AreEqual(exactResultIntPart * taylorSeriePart, Mathematics.Pow(Mathematics.E, 3 * Mathematics.E));
    }
    [TestMethod]
    [DataRow(-1)]
    [DataRow(0)]
    public void Log_ValueIsEqualOrLessThanZero_ThrowException(int n)
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Mathematics.Log(n));
    }
    [TestMethod]
    public void Log_ValueIsEqualToOne_ReturnsZero()
    {
        Assert.AreEqual(Decimal.Zero, Mathematics.Log(1));
    }
    [TestMethod]
    public void Log_ValueIsEqualToE_ReturnsOne()
    {
        Assert.AreEqual(Decimal.One, Mathematics.Log(Mathematics.E));
    }
    [TestMethod]
    public void Log_ValueIsEqualOrMoreThan2_ReturnsLnOf2()
    {
        // Arrange
        // Integer part of Log() method: 1
        decimal integerPart = 1m;
        // The taylorserie part of Log() is TaylorSerieOfLogFunction( 2/E ) =  -0.3068528194400546905827678783M
        decimal taylorSeriePart = -0.3068528194400546905827678783M;

        // Wolfram Ln(2) = 0.6931471805599453094172321214581765680755... => 0.6931471805599453094172321215m (in decimal)
        // Log(2) = intergetPart + taylorSeriePart =                        0.6931471805599453094172321217M
        // The difference of the Log() method  and Wolfram is: Log(2)  - Wolfram = ...72321217 - ...72321215 = ...00000002
        // Différence minimum créer par les erreurs d'arrondies.
        decimal exactResult = integerPart + taylorSeriePart;

        Assert.AreEqual(exactResult, Mathematics.Log(2));
    }
    [TestMethod]
    public void Log_ValueIsLessThanTwo_WhereValueIsEqualTo_E_ToPowerOf_MinusOne_ReturnsMinusOne()
    {
        // Arrange
        // Integer part of Log() method: 1
        decimal integerPart = 0m;
        // The taylorserie part of Log() is TaylorSerieOfLogFunction( 1/E ) =  -0.9999999999999999999999999997M
        decimal taylorSeriePart = -0.9999999999999999999999999997M;

        // EInverse = 0.3678794411714423215955237702
        // Wolfram Ln( EInverse ) = -0.9999999999999999999999999998952397762933739670... => -0.9999999999999999999999999999M (in decimal)
        // Log( EInverse ) = intergetPart + taylorSeriePart =                               -0.9999999999999999999999999997M
        // The difference of the Log() method  and Wolfram is: Log(2)  - Wolfram = (-...99997) - (-...99999) = ...00000002
        // Différence minimum créer par les erreurs d'arrondies.
        decimal exactResult = integerPart + taylorSeriePart;

        Assert.AreEqual(exactResult, Mathematics.Log(Mathematics.EInverse));
    }
}