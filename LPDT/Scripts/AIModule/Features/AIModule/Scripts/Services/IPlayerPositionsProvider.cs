using System.Collections.Generic;
using UnityEngine;

namespace Features.AIModule.Scripts.Services
{
	public interface IPlayerPositionsProvider
	{
		List<Vector3> GetPlayerPositions();
	}
}
