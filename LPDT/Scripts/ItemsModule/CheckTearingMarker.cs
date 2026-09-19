using UnityEngine;

public class CheckTearingMarker : MonoBehaviour
{
	[SerializeField]
	private CoinMarker _coinMarker;

	[Tooltip("Minimum speed to start having a chance to tear off")]
	public float minTearSpeed = 1f;

	[Tooltip("Speed at which tear chance reaches maxTearChance")]
	public float maxTearSpeed = 5f;

	public AnimationCurve _tearChanceCurve;

	private Vector3 _previousPosition;

	private void OnEnable()
	{
		_coinMarker.Grabbable = GetComponentInParent<RigidBodySmallGrabable>();
		_previousPosition = base.transform.position;
	}

	private void FixedUpdate()
	{
		if (_coinMarker.Grabbable.ConnectedRigidbody == null)
		{
			return;
		}
		if (_coinMarker.Grabbable == null)
		{
			_previousPosition = base.transform.position;
			return;
		}
		float fixedDeltaTime = Time.fixedDeltaTime;
		if (fixedDeltaTime <= 0f)
		{
			_previousPosition = base.transform.position;
			return;
		}
		float num = (base.transform.position - _previousPosition).magnitude / fixedDeltaTime;
		if (num >= minTearSpeed)
		{
			Mathf.InverseLerp(minTearSpeed, maxTearSpeed, num);
			_tearChanceCurve.Evaluate(num);
			if ((float)Random.Range(0, 100) < 0.1f)
			{
				_coinMarker.Grabbable.UnjoinAll();
			}
		}
		_previousPosition = base.transform.position;
	}
}
