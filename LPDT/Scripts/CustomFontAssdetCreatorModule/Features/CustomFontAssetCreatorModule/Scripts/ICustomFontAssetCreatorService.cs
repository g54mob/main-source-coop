using Cysharp.Threading.Tasks;

namespace Features.CustomFontAssetCreatorModule.Scripts
{
	public interface ICustomFontAssetCreatorService
	{
		UniTask GenerateFonts();

		UniTask GenerateFontsWithWindow();
	}
}
