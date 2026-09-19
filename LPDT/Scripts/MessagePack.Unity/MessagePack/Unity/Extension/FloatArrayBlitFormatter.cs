namespace MessagePack.Unity.Extension
{
	public class FloatArrayBlitFormatter : UnsafeBlitFormatterBase<float, ReverseEndianessHelperSimpleSingle>
	{
		protected override sbyte TypeCode => 38;
	}
}
