using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace NomadDrive.Features.Vehicle
{
	public static class VehiclePlayerContactFilter
	{
		private static readonly HashSet<int> _vehicleBodyIds = new HashSet<int>();

		private static int _playerBodyId;

		private static bool _isSubscribed;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStateOnPlayModeStart()
		{
			if (_isSubscribed)
			{
				Physics.ContactModifyEvent -= OnContactModify;
				_isSubscribed = false;
			}
			_vehicleBodyIds.Clear();
			_playerBodyId = 0;
		}

		public static void RegisterVehicle(Rigidbody vehicleRigidbody)
		{
			if (!(vehicleRigidbody == null))
			{
				_vehicleBodyIds.Add(vehicleRigidbody.GetInstanceID());
				EnsureSubscribed();
			}
		}

		public static void UnregisterVehicle(Rigidbody vehicleRigidbody)
		{
			if (!(vehicleRigidbody == null))
			{
				_vehicleBodyIds.Remove(vehicleRigidbody.GetInstanceID());
				MaybeUnsubscribe();
			}
		}

		public static void RegisterPlayer(Rigidbody playerRigidbody)
		{
			if (!(playerRigidbody == null))
			{
				_playerBodyId = playerRigidbody.GetInstanceID();
				EnsureSubscribed();
			}
		}

		public static void UnregisterPlayer(Rigidbody playerRigidbody)
		{
			if (!(playerRigidbody == null))
			{
				if (_playerBodyId == playerRigidbody.GetInstanceID())
				{
					_playerBodyId = 0;
				}
				MaybeUnsubscribe();
			}
		}

		private static void EnsureSubscribed()
		{
			if (!_isSubscribed)
			{
				Physics.ContactModifyEvent += OnContactModify;
				_isSubscribed = true;
			}
		}

		private static void MaybeUnsubscribe()
		{
			if (_isSubscribed && _playerBodyId == 0 && _vehicleBodyIds.Count <= 0)
			{
				Physics.ContactModifyEvent -= OnContactModify;
				_isSubscribed = false;
			}
		}

		private static void OnContactModify(PhysicsScene scene, NativeArray<ModifiableContactPair> pairs)
		{
			if (_playerBodyId == 0 || _vehicleBodyIds.Count == 0)
			{
				return;
			}
			int length = pairs.Length;
			for (int i = 0; i < length; i++)
			{
				ModifiableContactPair modifiableContactPair = pairs[i];
				int bodyInstanceID = modifiableContactPair.bodyInstanceID;
				int otherBodyInstanceID = modifiableContactPair.otherBodyInstanceID;
				if ((bodyInstanceID == _playerBodyId || otherBodyInstanceID == _playerBodyId) && (_vehicleBodyIds.Contains(bodyInstanceID) || _vehicleBodyIds.Contains(otherBodyInstanceID)))
				{
					int contactCount = modifiableContactPair.contactCount;
					for (int j = 0; j < contactCount; j++)
					{
						modifiableContactPair.SetMaxImpulse(j, 0f);
					}
				}
			}
		}
	}
}
