using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Google.Apis.Download
{
	public interface IMediaDownloader
	{
		int ChunkSize { get; set; }

		event Action<IDownloadProgress> ProgressChanged;

		IDownloadProgress Download(string url, Stream stream);

		Task<IDownloadProgress> DownloadAsync(string url, Stream stream);

		Task<IDownloadProgress> DownloadAsync(string url, Stream stream, CancellationToken cancellationToken);
	}
}
