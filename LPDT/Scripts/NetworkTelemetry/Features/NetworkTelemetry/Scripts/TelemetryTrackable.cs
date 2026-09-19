using Fusion;
using UnityEngine;
using Zenject;

namespace Features.NetworkTelemetry.Scripts
{
	public class TelemetryTrackable : MonoBehaviour
	{
		[SerializeField]
		private NetworkObject _networkObject;

		private TelemetryTrackablesModel _trackablesModel;

		private bool _requireRegistration;

		public string TrackableType { get; private set; }

		public NetworkObject NetworkObject => _networkObject;

		[Inject]
		public void InjectDependencies(TelemetryTrackablesModel trackablesModel)
		{
			_trackablesModel = trackablesModel;
		}

		private void Awake()
		{
			if (_networkObject == null)
			{
				_networkObject = GetComponentInParent<NetworkObject>();
			}
			if (_networkObject == null)
			{
				Debug.LogError("TelemetryTrackable on '" + base.name + "' could not resolve a NetworkObject. Assign one in the Inspector or attach this component to a GameObject that is a child of a NetworkObject.", this);
			}
			else
			{
				TrackableType = _networkObject.gameObject.name;
			}
		}

		private void OnEnable()
		{
			_requireRegistration = true;
		}

		private void OnDisable()
		{
			_trackablesModel?.Trackables.Remove(this);
		}

		private void LateUpdate()
		{
			if (_requireRegistration)
			{
				_requireRegistration = false;
				if (!(_networkObject == null))
				{
					_trackablesModel?.Trackables.Add(this);
				}
			}
		}

		private void Reset()
		{
			if (_networkObject == null)
			{
				_networkObject = GetComponentInParent<NetworkObject>();
			}
		}
	}
}
