using Features.GrabModule.Scripts;
using Features.PhysicsVolumeModule.Scripts;
using UnityEngine;

namespace Features.ContainersModule.Scripts
{
	public class CauldronV2 : MonoBehaviour
	{
		[SerializeField]
		private PhysicsInfluenceVolume _volume;

		[SerializeField]
		private SimplePointGrabable _grabable;

		[SerializeField]
		private float _heavyItemMultiplierWhileOccupied = 1f;

		public bool IsOccupied;

		private bool _emptyIsHeavy;

		private float _emptyMultiplier;

		private void Awake()
		{
			_emptyIsHeavy = _grabable.IsHeavyItem;
			_emptyMultiplier = _grabable.HeavyItemMultiplier;
		}

		private void FixedUpdate()
		{
			UpdateLoadSensedWeight();
		}

		private void UpdateLoadSensedWeight()
		{
			IsOccupied = _volume.HasRiderOtherThan(_grabable.GrabbedByPlayers);
			if (IsOccupied != _grabable.IsHeavyItem || _emptyIsHeavy)
			{
				_grabable.SetHeavyItem(IsOccupied || _emptyIsHeavy);
				_grabable.HeavyItemMultiplier = (IsOccupied ? _heavyItemMultiplierWhileOccupied : _emptyMultiplier);
			}
		}
	}
}
