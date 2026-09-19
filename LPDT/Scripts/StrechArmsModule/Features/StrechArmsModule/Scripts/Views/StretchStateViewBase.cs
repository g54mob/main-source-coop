using System.Collections.Generic;
using Features.StrechArmsModule.Scripts.Views.StateVisuals;
using Global.SerializableDictionary;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.StrechArmsModule.Scripts.Views
{
	public abstract class StretchStateViewBase : ViewBehaviour
	{
		[SerializeField]
		private List<ArmStateVisual> _leftArmStateVisuals;

		[SerializeField]
		private List<ArmStateVisual> _rightArmStateVisuals;

		[SerializeField]
		private CanvasGroup _leftHandCanvasGroup;

		[SerializeField]
		private CanvasGroup _rightHandCanvasGroup;

		[SerializeField]
		private GameObject _leftHandKilled;

		[SerializeField]
		private GameObject _rightHandKilled;

		[SerializeField]
		private SerializableDictionary<Arm, GameObject> _grabbedArmVisuals;

		private readonly Dictionary<ArmState, ArmStateVisual> _leftArmStateVisualDictionary = new Dictionary<ArmState, ArmStateVisual>();

		private readonly Dictionary<ArmState, ArmStateVisual> _rightArmStateVisualDictionary = new Dictionary<ArmState, ArmStateVisual>();

		private ArmStateVisual _lastLeftArmStateVisual;

		private ArmStateVisual _lastRightArmStateVisual;

		private void Start()
		{
			foreach (GameObject value in _grabbedArmVisuals.Values)
			{
				value.SetActive(value: false);
			}
		}

		protected override void OnEnable()
		{
			foreach (ArmStateVisual leftArmStateVisual in _leftArmStateVisuals)
			{
				_leftArmStateVisualDictionary.Add(leftArmStateVisual.ArmState, leftArmStateVisual);
				leftArmStateVisual.Disable();
			}
			foreach (ArmStateVisual rightArmStateVisual in _rightArmStateVisuals)
			{
				_rightArmStateVisualDictionary.Add(rightArmStateVisual.ArmState, rightArmStateVisual);
				rightArmStateVisual.Disable();
			}
		}

		public void UpdateArmState(Arm armOrientation, ArmState state, float progress = 0f)
		{
			if (armOrientation == Arm.Right)
			{
				ArmStateVisual armStateVisual = _rightArmStateVisualDictionary[state];
				if (_lastRightArmStateVisual == null)
				{
					armStateVisual.Enable();
				}
				else if (_lastRightArmStateVisual != armStateVisual)
				{
					_lastRightArmStateVisual.Disable();
					armStateVisual.Enable();
				}
				armStateVisual.SetProgress(progress);
				_lastRightArmStateVisual = armStateVisual;
			}
			if (armOrientation == Arm.Left)
			{
				ArmStateVisual armStateVisual2 = _leftArmStateVisualDictionary[state];
				if (_lastLeftArmStateVisual == null)
				{
					armStateVisual2.Enable();
				}
				else if (_lastLeftArmStateVisual != armStateVisual2)
				{
					_lastLeftArmStateVisual.Disable();
					armStateVisual2.Enable();
				}
				armStateVisual2.SetProgress(progress);
				_lastLeftArmStateVisual = armStateVisual2;
			}
		}

		public void SetActiveLeftArm(bool isActive)
		{
			_leftHandCanvasGroup.alpha = (isActive ? 1 : 0);
			_leftHandKilled.SetActive(!isActive);
		}

		public void SetActiveRightArm(bool isActive)
		{
			_rightHandCanvasGroup.alpha = (isActive ? 1 : 0);
			_rightHandKilled.SetActive(!isActive);
		}

		public void SetIsGrabbed(Arm arm, bool isGrabbed)
		{
			_grabbedArmVisuals[arm].SetActive(isGrabbed);
		}
	}
}
