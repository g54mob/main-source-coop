namespace MessagePack.Unity.Extension
{
	public class IntArrayBlitFormatter : UnsafeBlitFormatterBase<int, ReverseEndianessHelperSimpleSingle>
	{
		protected override sbyte TypeCode => 37;
	}
}
