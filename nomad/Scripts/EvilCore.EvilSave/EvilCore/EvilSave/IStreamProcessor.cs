namespace EvilCore.EvilSave
{
	public interface IStreamProcessor
	{
		byte[] Process(byte[] input);

		byte[] Unprocess(byte[] input);
	}
}
