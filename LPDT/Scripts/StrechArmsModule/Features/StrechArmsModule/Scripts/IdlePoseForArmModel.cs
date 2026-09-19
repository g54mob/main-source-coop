using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;
using Fusion;
using UnityEngine;

namespace Features.StrechArmsModule.Scripts
{
	public class IdlePoseForArmModel : ISessionCleanup
	{
		public Dictionary<PlayerRef, Dictionary<Arm, Transform>> _idlePoseByArm = new Dictionary<PlayerRef, Dictionary<Arm, Transform>>();

		public Transform GetIdlePose(Arm armOrientation, PlayerRef objectInputAuthority)
		{
			if (_idlePoseByArm.ContainsKey(objectInputAuthority) && _idlePoseByArm[objectInputAuthority].ContainsKey(armOrientation))
			{
				return _idlePoseByArm[objectInputAuthority][armOrientation];
			}
			return null;
		}

		public void RegisterIdlePose(Arm armOrientation, Transform idlePoseTransform, PlayerRef objectInputAuthority)
		{
			if (!_idlePoseByArm.ContainsKey(objectInputAuthority))
			{
				_idlePoseByArm.Add(objectInputAuthority, new Dictionary<Arm, Transform> { { armOrientation, idlePoseTransform } });
			}
			else if (!_idlePoseByArm[objectInputAuthority].ContainsKey(armOrientation))
			{
				_idlePoseByArm[objectInputAuthority].Add(armOrientation, idlePoseTransform);
			}
		}

		public void UnregisterIdlePose(Arm armOrientation, PlayerRef objectInputAuthority)
		{
			if (_idlePoseByArm.ContainsKey(objectInputAuthority) && _idlePoseByArm[objectInputAuthority].ContainsKey(armOrientation))
			{
				_idlePoseByArm[objectInputAuthority].Remove(armOrientation);
			}
		}

		public void Cleanup()
		{
			_idlePoseByArm.Clear();
		}
	}
}
