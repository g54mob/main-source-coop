using UnityEngine;

namespace MessagePack.Unity.Extension
{
	public class RectArrayBlitFormatter : UnsafeBlitFormatterBase<Rect, ReverseEndianessHelperSimpleRepeat<float>>
	{
		protected override sbyte TypeCode => 36;
	}
}
