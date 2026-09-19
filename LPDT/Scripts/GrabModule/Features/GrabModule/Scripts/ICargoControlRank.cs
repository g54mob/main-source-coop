using UnityEngine;

namespace Features.GrabModule.Scripts
{
	public interface ICargoControlRank
	{
		bool ControlsOverCarrierOf(GameObject candidate);
	}
}
