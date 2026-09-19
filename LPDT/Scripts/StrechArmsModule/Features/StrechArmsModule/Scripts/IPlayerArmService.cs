using Fusion;

namespace Features.StrechArmsModule.Scripts
{
	public interface IPlayerArmService
	{
		void TearOffPlayerArm(PlayerRef player, Arm arm);

		bool IsCanTearOfArm(PlayerRef player, Arm arm);
	}
}
