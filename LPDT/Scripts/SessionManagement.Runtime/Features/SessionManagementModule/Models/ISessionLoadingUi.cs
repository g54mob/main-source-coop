using Cysharp.Threading.Tasks;

namespace Features.SessionManagementModule.Models
{
	public interface ISessionLoadingUi
	{
		bool IsBlackoutRaised { get; }

		UniTask ShowAsync();

		UniTask ShowRunStartAsync();

		UniTask HideAsync();
	}
}
