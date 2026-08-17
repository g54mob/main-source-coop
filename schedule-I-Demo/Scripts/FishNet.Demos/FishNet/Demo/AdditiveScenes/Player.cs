using System.Collections.Generic;
using System.Linq;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

namespace FishNet.Demo.AdditiveScenes
{
	public class Player : NetworkBehaviour
	{
		[SerializeField]
		private Transform _ownerObjects;

		[SerializeField]
		private float _moveRate = 2f;

		private List<Waypoint> _wayPoints = new List<Waypoint>();

		private int _goalIndex;

		private Vector3 _goalOffset;

		private bool NetworkInitialize___EarlyFishNet_002EDemo_002EAdditiveScenes_002EPlayerFishNet_002EDemos_002Edll_Excuted;

		private bool NetworkInitialize__LateFishNet_002EDemo_002EAdditiveScenes_002EPlayerFishNet_002EDemos_002Edll_Excuted;

		public override void OnStartServer()
		{
			_wayPoints = UnityEngine.Object.FindObjectsOfType<Waypoint>().ToList();
			if (base.ServerManager.Clients.Count % 2 == 0)
			{
				_goalOffset = new Vector3(-0.5f, 0f, 0f);
				_wayPoints = _wayPoints.OrderBy((Waypoint x) => x.WaypointIndex).ToList();
			}
			else
			{
				_goalOffset = new Vector3(0.5f, 0f, 0f);
				_wayPoints = _wayPoints.OrderByDescending((Waypoint x) => x.WaypointIndex).ToList();
			}
			base.transform.position = _wayPoints[0].transform.position + _goalOffset;
			_goalIndex = 1;
		}

		public override void OnOwnershipClient(NetworkConnection prevOwner)
		{
			_ownerObjects.gameObject.SetActive(base.IsOwner);
		}

		private void Update()
		{
			if (!base.IsServer || _wayPoints.Count == 0 || _goalIndex >= _wayPoints.Count)
			{
				return;
			}
			Vector3 vector = _wayPoints[_goalIndex].transform.position + _goalOffset;
			base.transform.position = Vector3.MoveTowards(base.transform.position, vector, _moveRate * Time.deltaTime);
			if ((vector - base.transform.position).normalized != Vector3.zero)
			{
				Quaternion to = Quaternion.LookRotation((vector - base.transform.position).normalized, base.transform.up);
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, 270f * Time.deltaTime);
			}
			if (base.transform.position == vector)
			{
				_goalIndex++;
				if (_goalIndex >= _wayPoints.Count)
				{
					_goalIndex = 0;
				}
			}
		}

		public virtual void NetworkInitialize___Early()
		{
			if (!NetworkInitialize___EarlyFishNet_002EDemo_002EAdditiveScenes_002EPlayerFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize___EarlyFishNet_002EDemo_002EAdditiveScenes_002EPlayerFishNet_002EDemos_002Edll_Excuted = true;
			}
		}

		public virtual void NetworkInitialize__Late()
		{
			if (!NetworkInitialize__LateFishNet_002EDemo_002EAdditiveScenes_002EPlayerFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize__LateFishNet_002EDemo_002EAdditiveScenes_002EPlayerFishNet_002EDemos_002Edll_Excuted = true;
			}
		}

		public override void NetworkInitializeIfDisabled()
		{
			NetworkInitialize___Early();
			NetworkInitialize__Late();
		}

		public virtual void Awake()
		{
			NetworkInitialize___Early();
			NetworkInitialize__Late();
		}
	}
}
