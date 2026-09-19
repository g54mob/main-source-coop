using Features.CartUpgradesModule.Scripts.Core;
using Features.HingeModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.CartCannonModule.Scripts
{
	public class CartCannonMount : MonoBehaviour
	{
		private const float LOWERED_EPSILON = 0.001f;

		private const float MIN_LOWER_DURATION = 0.05f;

		[SerializeField]
		private Transform _cartRoot;

		[SerializeField]
		private Transform _barrel;

		[SerializeField]
		private Collider[] _barrelColliders;

		[SerializeField]
		private float _loweredAngle = 90f;

		private CartUpgradeConfiguration _cartUpgradeConfiguration;

		private BoatHingeControllerData[] _boatHinges;

		private Quaternion _authoredLocalRotation = Quaternion.identity;

		private float _loweredProgress;

		private bool _areCollidersEnabled = true;

		public bool IsLowered => _loweredProgress > 0.001f;

		[Inject]
		public void InjectDependencies(CartUpgradeConfiguration cartUpgradeConfiguration)
		{
			_cartUpgradeConfiguration = cartUpgradeConfiguration;
		}

		private void Awake()
		{
			if (_barrel != null)
			{
				_authoredLocalRotation = _barrel.localRotation;
			}
			_boatHinges = _cartRoot.GetComponentsInChildren<BoatHingeControllerData>(includeInactive: true);
		}

		private void Update()
		{
			float num = ((_cartUpgradeConfiguration != null) ? Mathf.Max(_cartUpgradeConfiguration.CannonLowerDurationSeconds, 0.05f) : 0.05f);
			_loweredProgress = Mathf.MoveTowards(_loweredProgress, IsDockedToBoat() ? 1f : 0f, Time.deltaTime / num);
			ApplyBarrelPose();
			ApplyColliderState();
		}

		private bool IsDockedToBoat()
		{
			BoatHingeControllerData[] boatHinges = _boatHinges;
			foreach (BoatHingeControllerData boatHingeControllerData in boatHinges)
			{
				if (boatHingeControllerData != null && boatHingeControllerData.IsHingeConnected)
				{
					return true;
				}
			}
			return false;
		}

		private void ApplyBarrelPose()
		{
			if (!(_barrel == null) && !(_cartRoot == null))
			{
				Quaternion quaternion = Quaternion.Euler(_loweredAngle * _loweredProgress, 0f, 0f);
				_barrel.rotation = LevelRotation() * quaternion * _authoredLocalRotation;
			}
		}

		private void ApplyColliderState()
		{
			bool flag = !IsLowered;
			if (flag == _areCollidersEnabled)
			{
				return;
			}
			_areCollidersEnabled = flag;
			Collider[] barrelColliders = _barrelColliders;
			foreach (Collider collider in barrelColliders)
			{
				if (collider != null)
				{
					collider.enabled = flag;
				}
			}
		}

		private Quaternion LevelRotation()
		{
			Vector3 vector = Vector3.Cross(Vector3.ProjectOnPlane(_cartRoot.right, Vector3.up), Vector3.up);
			Vector3 vector2 = Vector3.ProjectOnPlane(_cartRoot.forward, Vector3.up);
			return Quaternion.LookRotation(((vector.sqrMagnitude >= vector2.sqrMagnitude) ? vector : vector2).normalized, Vector3.up);
		}
	}
}
