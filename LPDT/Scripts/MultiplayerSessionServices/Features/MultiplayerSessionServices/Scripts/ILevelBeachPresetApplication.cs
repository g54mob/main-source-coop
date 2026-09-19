using Cysharp.Threading.Tasks;

namespace Features.MultiplayerSessionServices.Scripts
{
	public interface ILevelBeachPresetApplication
	{
		UniTask ApplyForCurrentLevelAsync();

		void ClearActivePreset();
	}
}
