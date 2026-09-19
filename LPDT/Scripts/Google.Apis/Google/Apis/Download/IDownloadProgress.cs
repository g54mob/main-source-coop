using System;

namespace Google.Apis.Download
{
	public interface IDownloadProgress
	{
		DownloadStatus Status { get; }

		long BytesDownloaded { get; }

		Exception Exception { get; }
	}
}
