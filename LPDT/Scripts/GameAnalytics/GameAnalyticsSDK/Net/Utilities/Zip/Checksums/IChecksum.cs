namespace GameAnalyticsSDK.Net.Utilities.Zip.Checksums
{
	public interface IChecksum
	{
		long Value { get; }

		void Reset();

		void Update(int bval);

		void Update(byte[] buffer);

		void Update(byte[] buf, int off, int len);
	}
}
