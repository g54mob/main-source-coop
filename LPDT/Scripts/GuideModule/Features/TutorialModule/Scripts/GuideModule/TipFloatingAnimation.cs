using UnityEngine;

namespace Features.TutorialModule.Scripts.GuideModule
{
	public class TipFloatingAnimation : MonoBehaviour
	{
		[SerializeField]
		private float _floatingSpeed;

		[SerializeField]
		private float _maxDeltaFloat;

		[SerializeField]
		private TipEntity _tipEntity;

		private float? _initialPositionY;

		private void OnEnable()
		{
			if (_tipEntity.IsInitialized)
			{
				StartAnimation(_tipEntity.IsInitialized);
			}
			else
			{
				_tipEntity.OnInitialized += StartAnimation;
			}
		}

		private void OnDisable()
		{
			_tipEntity.OnInitialized -= StartAnimation;
		}

		private void Update()
		{
			if (_initialPositionY.HasValue)
			{
				base.transform.localPosition = new Vector3(base.transform.localPosition.x, _initialPositionY.Value + _maxDeltaFloat * Mathf.Sin(Time.time * _floatingSpeed), base.transform.localPosition.z);
			}
		}

		private void StartAnimation(bool isInitialized)
		{
			if (isInitialized)
			{
				_initialPositionY = base.transform.localPosition.y;
			}
		}
	}
}
