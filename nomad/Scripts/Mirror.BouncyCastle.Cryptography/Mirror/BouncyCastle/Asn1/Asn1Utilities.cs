using System;

namespace Mirror.BouncyCastle.Asn1
{
	public abstract class Asn1Utilities
	{
		internal static Asn1TaggedObject CheckContextTagClass(Asn1TaggedObject taggedObject)
		{
			return CheckTagClass(taggedObject, 128);
		}

		internal static Asn1TaggedObject CheckTagClass(Asn1TaggedObject taggedObject, int tagClass)
		{
			if (!taggedObject.HasTagClass(tagClass))
			{
				string tagClassText = GetTagClassText(tagClass);
				string tagClassText2 = GetTagClassText(taggedObject);
				throw new InvalidOperationException("Expected " + tagClassText + " tag but found " + tagClassText2);
			}
			return taggedObject;
		}

		public static string GetTagClassText(Asn1TaggedObject taggedObject)
		{
			return GetTagClassText(taggedObject.TagClass);
		}

		public static string GetTagClassText(int tagClass)
		{
			return tagClass switch
			{
				64 => "APPLICATION", 
				128 => "CONTEXT", 
				192 => "PRIVATE", 
				_ => "UNIVERSAL", 
			};
		}

		public static string GetTagText(Asn1TaggedObject taggedObject)
		{
			return GetTagText(taggedObject.TagClass, taggedObject.TagNo);
		}

		public static string GetTagText(int tagClass, int tagNo)
		{
			return tagClass switch
			{
				64 => $"[APPLICATION {tagNo}]", 
				128 => $"[CONTEXT {tagNo}]", 
				192 => $"[PRIVATE {tagNo}]", 
				_ => $"[UNIVERSAL {tagNo}]", 
			};
		}
	}
}
