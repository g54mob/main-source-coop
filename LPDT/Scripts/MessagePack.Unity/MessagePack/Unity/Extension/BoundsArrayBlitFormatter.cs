using UnityEngine;

namespace MessagePack.Unity.Extension
{
	public class BoundsArrayBlitFormatter : UnsafeBlitFormatterBase<Bounds, ReverseEndianessHelperSimpleRepeat<float>>
	{
		protected override sbyte TypeCode => 35;
	}
}
