using UnityEngine;

namespace MessagePack.Unity.Extension
{
	public class Vector4ArrayBlitFormatter : UnsafeBlitFormatterBase<Vector4, ReverseEndianessHelperSimpleRepeat<float>>
	{
		protected override sbyte TypeCode => 32;
	}
}
