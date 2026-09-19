using Cysharp.Threading.Tasks;

namespace Features.SessionManagementModule.Models
{
	public interface IShopPlayerControl
	{
		UniTask ReleaseGrabsAsync();

		UniTask EnsureBodyAuthorityAsync();

		UniTask ResetRagdollAsync();

		UniTask SeatAtStoreSeatAsync();

		void LeaveStoreSeat();
	}
}
