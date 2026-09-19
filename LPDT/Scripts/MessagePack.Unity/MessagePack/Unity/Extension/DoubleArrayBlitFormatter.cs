namespace MessagePack.Unity.Extension
{
	public class DoubleArrayBlitFormatter : UnsafeBlitFormatterBase<double, ReverseEndianessHelperSimpleSingle>
	{
		protected override sbyte TypeCode => 39;
	}
}
