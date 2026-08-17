using System;

namespace Epic.OnlineServices
{
	public sealed class ProductUserId : Handle
	{
		public static ProductUserId FromString(Utf8String productUserIdString)
		{
			IntPtr to = IntPtr.Zero;
			Helper.Set(productUserIdString, ref to);
			IntPtr intPtr = Bindings.EOS_ProductUserId_FromString(to);
			Helper.Dispose(ref to);
			Helper.Get(intPtr, out ProductUserId to2);
			return to2;
		}

		public Result ToString(out Utf8String outBuffer)
		{
			int inOutBufferLength = 33;
			IntPtr value = Helper.AddAllocation(inOutBufferLength);
			Result result = Bindings.EOS_ProductUserId_ToString(base.InnerHandle, value, ref inOutBufferLength);
			Helper.Get(value, out outBuffer);
			Helper.Dispose(ref value);
			return result;
		}

		public override string ToString()
		{
			ToString(out var outBuffer);
			return outBuffer;
		}

		public override string ToString(string format, IFormatProvider formatProvider)
		{
			if (format != null)
			{
				return string.Format(format, ToString());
			}
			return ToString();
		}
	}
}
