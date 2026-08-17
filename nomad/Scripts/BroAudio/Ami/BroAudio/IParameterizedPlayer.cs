namespace Ami.BroAudio
{
	public interface IParameterizedPlayer
	{
		void SetParameter(string name, bool value);

		void SetParameter(string name, int value);

		void SetParameter(string name, float value);

		bool TryGetParameter(string name, out bool value);

		bool TryGetParameter(string name, out int value);

		bool TryGetParameter(string name, out float value);

		void RequestLoopRegionExit();
	}
}
