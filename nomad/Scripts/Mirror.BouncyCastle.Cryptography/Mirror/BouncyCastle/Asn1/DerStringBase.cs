namespace Mirror.BouncyCastle.Asn1
{
	public abstract class DerStringBase : Asn1Object
	{
		public abstract string GetString();

		public override string ToString()
		{
			return GetString();
		}

		protected override int Asn1GetHashCode()
		{
			return GetString().GetHashCode();
		}
	}
}
