using System.IO;
using System.IO.Compression;

namespace EvilCore.EvilSave
{
	public class GZipCompressionProcessor : IStreamProcessor
	{
		public byte[] Process(byte[] input)
		{
			using MemoryStream memoryStream = new MemoryStream();
			using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, leaveOpen: true))
			{
				gZipStream.Write(input, 0, input.Length);
			}
			return memoryStream.ToArray();
		}

		public byte[] Unprocess(byte[] input)
		{
			using MemoryStream stream = new MemoryStream(input);
			using GZipStream gZipStream = new GZipStream(stream, CompressionMode.Decompress);
			using MemoryStream memoryStream = new MemoryStream();
			gZipStream.CopyTo(memoryStream);
			return memoryStream.ToArray();
		}
	}
}
