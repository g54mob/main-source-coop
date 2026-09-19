using UnityEngine;

namespace MessagePack.Unity.Extension
{
	public class QuaternionArrayBlitFormatter : UnsafeBlitFormatterBase<Quaternion, ReverseEndianessHelperSimpleRepeat<float>>
	{
		protected override sbyte TypeCode => 33;
	}
}
