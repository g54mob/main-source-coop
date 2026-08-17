using FishNet.Object;
using UnityEngine;

namespace FishNet.Demo.HashGrid
{
	public class MoveRandomly : NetworkBehaviour
	{
		[SerializeField]
		private Renderer _renderer;

		private float _moveRate = 0.5f;

		public const float Range = 25f;

		private Vector3 _goal;

		private Vector3 _start;

		private bool NetworkInitialize___EarlyFishNet_002EDemo_002EHashGrid_002EMoveRandomlyFishNet_002EDemos_002Edll_Excuted;

		private bool NetworkInitialize__LateFishNet_002EDemo_002EHashGrid_002EMoveRandomlyFishNet_002EDemos_002Edll_Excuted;

		private void Update()
		{
			if (base.IsOwner || base.IsServer)
			{
				base.transform.position = Vector3.MoveTowards(base.transform.position, _goal, _moveRate * Time.deltaTime);
				if (base.transform.position == _goal)
				{
					RandomizeGoal();
				}
			}
		}

		public override void OnStartNetwork()
		{
			_start = base.transform.position;
			if (base.Owner.IsLocalClient)
			{
				_renderer.material.color = Color.green;
				_moveRate *= 3f;
				base.transform.position -= new Vector3(0f, 0f, 1f);
				Camera main = Camera.main;
				main.transform.SetParent(base.transform);
				main.transform.localPosition = new Vector3(0f, 0f, -5f);
			}
			else
			{
				_renderer.material.color = Color.gray;
				base.transform.position = _start + RandomInsideRange();
			}
			RandomizeGoal();
		}

		public override void OnStopNetwork()
		{
			Camera main = Camera.main;
			if (main != null && base.Owner.IsLocalClient)
			{
				main.transform.SetParent(null);
			}
		}

		private void RandomizeGoal()
		{
			_goal = _start + RandomInsideRange();
		}

		private Vector3 RandomInsideRange()
		{
			Vector3 result = Random.insideUnitSphere * 25f;
			result.z = base.transform.position.z;
			return result;
		}

		public virtual void NetworkInitialize___Early()
		{
			if (!NetworkInitialize___EarlyFishNet_002EDemo_002EHashGrid_002EMoveRandomlyFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize___EarlyFishNet_002EDemo_002EHashGrid_002EMoveRandomlyFishNet_002EDemos_002Edll_Excuted = true;
			}
		}

		public virtual void NetworkInitialize__Late()
		{
			if (!NetworkInitialize__LateFishNet_002EDemo_002EHashGrid_002EMoveRandomlyFishNet_002EDemos_002Edll_Excuted)
			{
				NetworkInitialize__LateFishNet_002EDemo_002EHashGrid_002EMoveRandomlyFishNet_002EDemos_002Edll_Excuted = true;
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
