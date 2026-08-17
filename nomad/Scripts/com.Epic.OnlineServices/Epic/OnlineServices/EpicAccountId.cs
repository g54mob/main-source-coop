using System;

namespace Epic.OnlineServices
{
	public sealed class EpicAccountId : Handle
	{
		public Result ToString(out Utf8String outBuffer)
		{
			int inOutBufferLength = 33;
			IntPtr value = Helper.AddAllocation(inOutBufferLength);
			Result result = Bindings.EOS_EpicAccountId_ToString(base.InnerHandle, value, ref inOutBufferLength);
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
