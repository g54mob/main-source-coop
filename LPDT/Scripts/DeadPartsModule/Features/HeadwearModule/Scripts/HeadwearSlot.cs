using Features.DeadPartsModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.HeadwearModule.Scripts
{
	[RequireComponent(typeof(Collider))]
	public class HeadwearSlot : MonoBehaviour
	{
		[SerializeField]
		private NetworkObject _bearer;

		[SerializeField]
		private PlayerAlivePart _playerAlivePart;

		private Headwear _occupant;

		public Transform Anchor => base.transform;

		public NetworkObject Bearer
		{
			get
			{
				if (!(_bearer != null))
				{
					if (!(_playerAlivePart != null))
					{
						return null;
					}
					return _playerAlivePart.Object;
				}
				return _bearer;
			}
		}

		public bool HasPlayerOwner => _playerAlivePart != null;

		public PlayerRef PlayerOwner
		{
			get
			{
				if (!(_playerAlivePart != null))
				{
					return PlayerRef.None;
				}
				return _playerAlivePart.Object.InputAuthority;
			}
		}

		public bool IsTakenByOther(Headwear headwear)
		{
			if (_occupant != null && _occupant != headwear)
			{
				return _occupant.IsWorn;
			}
			return false;
		}

		public bool RefusesClaim(Headwear headwear)
		{
			if (IsTakenByOther(headwear))
			{
				return !headwear.HasEquipPriorityOver(_occupant);
			}
			return false;
		}

		public void Claim(Headwear headwear)
		{
			if (!RefusesClaim(headwear))
			{
				_occupant = headwear;
			}
		}

		public void Release(Headwear headwear)
		{
			if (!(_occupant != headwear))
			{
				_occupant = null;
			}
		}

		private void Reset()
		{
			_playerAlivePart = GetComponentInParent<PlayerAlivePart>();
			_bearer = GetComponentInParent<NetworkObject>();
		}
	}
}
