using FishNet.Object;
using UnityEngine;

namespace FishNet.Demo.NetworkLod
{
	public class MoveRandomly : NetworkBehaviour
	{
		[SerializeField]
		private float _moveRate = 3f;

		[SerializeField]
		private Renderer _renderer;

		[SerializeField]
		private bool _updateRotation;

		private const float _range = 10f;

		private Vector3 _goalPosition;

		private Quaternion _goalRotation;

		private Vector3 _startPosition;

		private bool NetworkInitialize___EarlyFishNet_002EDemo_002ENetworkLod_002EMoveRandomlyFishNet_002EDemos_002Edll_Excuted;

		private bool NetworkInitialize__LateFishNet_002EDemo_002ENetworkLod_002EMoveRandomlyFishNet_002EDemos_002Edll_Excuted;

		private void Update()
		{
			if (!base.IsClientOnly && !base.Owner.IsValid)
			{
				base.transform.position = Vector3.MoveTowards(base.transform.position, _goalPosition, _moveRate * Time.deltaTime);
				if (_updateRotation)
				{
					base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, _goalRotation, 15f * Time.deltaTime);
				}
				if (base.transform.position == _goalPosition)
				{
					RandomizeGoal();
				}
			}
		}

		public override void OnStartNetwork()
		{
			_startPosition = base.transform.position;
			RandomizeGoal();
			if (_renderer != null && base.Owner.IsActive)
			{
				_renderer.material.color = Color.green;
			}
			if (!base.Owner.IsValid)
			{
				base.gameObject.name = "LOD " + base.ObjectId;
			}
			else
			{
				base.gameObject.name = "Owned " + base.ObjectId;
			}
		}

		private void RandomizeGoal()
		{
			_goalPosition = _startPosition + Random.insideUnitSphere * 10f;
			if (_updateRotation)
			{
				if (Random.Range(0f, 1f) <= 0.33f)
				{
					Vector3 euler = Random.insideUnitSphere * 180f;
					_goalRotation = Quaternion.Euler(euler);
				}
				else
				{
					_goalRotation = base.transform.rotation;
				}
			}
		}

		public virtual void NetworkInitialize___Early()
		{
			if (!NetworkInitialize___EarlyFishNet_002EDemo_002ENetworkLod_002EMoveRandomlyFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize___EarlyFishNet_002EDemo_002ENetworkLod_002EMoveRandomlyFishNet_002EDemos_002Edll_Excuted = true;
			}
		}

		public virtual void NetworkInitialize__Late()
		{
			if (!NetworkInitialize__LateFishNet_002EDemo_002ENetworkLod_002EMoveRandomlyFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize__LateFishNet_002EDemo_002ENetworkLod_002EMoveRandomlyFishNet_002EDemos_002Edll_Excuted = true;
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
