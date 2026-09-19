using UnityEngine;

namespace MessagePack.Unity.Extension
{
	public class ColorArrayBlitFormatter : UnsafeBlitFormatterBase<Color, ReverseEndianessHelperSimpleRepeat<float>>
	{
		protected override sbyte TypeCode => 34;
	}
}
