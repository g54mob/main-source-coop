namespace Mirror.BouncyCastle.Math.Field
{
	public interface IPolynomialExtensionField : IFiniteField
	{
		IPolynomial MinimalPolynomial { get; }
	}
}
