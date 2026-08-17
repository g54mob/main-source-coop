using Mirror.BouncyCastle.Asn1.Anssi;
using Mirror.BouncyCastle.Asn1.CryptoPro;
using Mirror.BouncyCastle.Asn1.GM;
using Mirror.BouncyCastle.Asn1.Sec;
using Mirror.BouncyCastle.Asn1.TeleTrust;

namespace Mirror.BouncyCastle.Asn1.X9
{
	public class ECNamedCurveTable
	{
		public static X9ECParameters GetByOid(DerObjectIdentifier oid)
		{
			X9ECParameters byOid = X962NamedCurves.GetByOid(oid);
			if (byOid == null)
			{
				byOid = SecNamedCurves.GetByOid(oid);
			}
			if (byOid == null)
			{
				byOid = TeleTrusTNamedCurves.GetByOid(oid);
			}
			if (byOid == null)
			{
				byOid = AnssiNamedCurves.GetByOid(oid);
			}
			if (byOid == null)
			{
				byOid = ECGost3410NamedCurves.GetByOid(oid);
			}
			if (byOid == null)
			{
				byOid = GMNamedCurves.GetByOid(oid);
			}
			return byOid;
		}
	}
}
