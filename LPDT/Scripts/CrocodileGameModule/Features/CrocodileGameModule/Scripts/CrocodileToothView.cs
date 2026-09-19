using UnityEngine;

namespace Features.CrocodileGameModule.Scripts
{
	public class CrocodileToothView : MonoBehaviour
	{
		[SerializeField]
		private Transform _toothTransform;

		[SerializeField]
		private Vector3 _pressedLocalOffset = new Vector3(0f, -0.03f, 0.05f);

		private Transform _rootTransform;

		private Rigidbody _rigidbody;

		private Vector3 _rootRestLocalPosition;

		private Vector3 _meshRestLocalPosition;

		private bool _isInitialized;

		private bool _isPressed;

		public void Initialize()
		{
			if (_rigidbody == null)
			{
				_rigidbody = GetComponent<Rigidbody>();
			}
			if (_toothTransform == null)
			{
				_toothTransform = base.transform;
			}
			_rootTransform = ((_rigidbody != null) ? _rigidbody.transform : base.transform);
			_rootRestLocalPosition = _rootTransform.localPosition;
			_meshRestLocalPosition = _toothTransform.localPosition;
			_isInitialized = true;
			_isPressed = false;
		}

		public void SetPressed(bool isPressed)
		{
			if (!_isInitialized)
			{
				Initialize();
			}
			if (_isPressed != isPressed)
			{
				_isPressed = isPressed;
				if (_rigidbody != null)
				{
					_rigidbody.isKinematic = true;
					_rigidbody.linearVelocity = Vector3.zero;
					_rigidbody.angularVelocity = Vector3.zero;
				}
				_rootTransform.localPosition = _rootRestLocalPosition;
				_toothTransform.localPosition = (isPressed ? (_meshRestLocalPosition + _pressedLocalOffset) : _meshRestLocalPosition);
			}
		}

		private void OnValidate()
		{
			if (_toothTransform == null)
			{
				_toothTransform = base.transform;
			}
		}
	}
}
