using System;

namespace Google.Apis.Upload
{
	public interface IUploadProgress
	{
		UploadStatus Status { get; }

		long BytesSent { get; }

		Exception Exception { get; }
	}
}
