using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ShadedTechnology.WindshieldRainAsset.Demo
{
	public class SimpleCameraController : MonoBehaviour
	{
		public bool flipForward;

		public float backupRotationThreshold = 0.2f;

		public Rigidbody carRigidbody;

		public Transform mainTarget;

		public Transform[] cameraViews;

		public Component vCamMain;

		public Component vCamView;

		public KeyCode lookBackKey = KeyCode.B;

		public KeyCode changeCamKey = KeyCode.C;

		private Quaternion _originalMainTargetRot;

		private Quaternion[] _originalCameraViewsRot;

		private int _viewsCount;

		private int _currentView;

		private bool _isLookingBack;

		private void Start()
		{
			_originalMainTargetRot = mainTarget.localRotation;
			_originalCameraViewsRot = new Quaternion[cameraViews.Length];
			for (int i = 0; i < cameraViews.Length; i++)
			{
				if (!(cameraViews[i] == null))
				{
					_originalCameraViewsRot[i] = cameraViews[i].transform.rotation;
				}
			}
			_viewsCount = cameraViews.Length + 1;
		}

		private void SetComponentActive(Component cam, bool active)
		{
			if (cam != null)
			{
				cam.gameObject.SetActive(active);
			}
		}

		private void SetFollow(Component cam, Transform target)
		{
			if (cam == null || target == null)
			{
				return;
			}
			Type type = cam.GetType();
			if (type.FullName.Contains("Cinemachine.CinemachineVirtualCamera"))
			{
				PropertyInfo property = type.GetProperty("Follow");
				if (property != null)
				{
					property.SetValue(cam, target);
				}
			}
		}

		public void OnLookBackKey(InputAction.CallbackContext context)
		{
			_isLookingBack = context.ReadValueAsButton();
			if (_isLookingBack)
			{
				mainTarget.localRotation = _originalMainTargetRot * Quaternion.Euler(0f, 180f, 0f);
			}
			else
			{
				mainTarget.localRotation = _originalMainTargetRot;
			}
		}

		public void OnChangeCameraKey(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				_currentView = (_currentView + 1) % _viewsCount;
				if (_currentView == 0)
				{
					SetComponentActive(vCamMain, active: true);
					SetComponentActive(vCamView, active: false);
				}
				else
				{
					SetComponentActive(vCamView, active: true);
					SetComponentActive(vCamMain, active: false);
					SetFollow(vCamView, cameraViews[_currentView - 1]);
				}
			}
		}

		private void Update()
		{
			if (!_isLookingBack && (bool)carRigidbody)
			{
				float num = Vector3.Dot(carRigidbody.linearVelocity, ((!flipForward) ? 1 : (-1)) * carRigidbody.transform.forward);
				if (num < 0f - backupRotationThreshold)
				{
					mainTarget.localRotation = _originalMainTargetRot * Quaternion.Euler(0f, 180f, 0f);
				}
				else if (num > backupRotationThreshold)
				{
					mainTarget.localRotation = _originalMainTargetRot;
				}
			}
		}
	}
}
