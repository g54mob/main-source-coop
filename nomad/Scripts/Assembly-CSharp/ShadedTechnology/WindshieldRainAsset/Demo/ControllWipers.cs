using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace ShadedTechnology.WindshieldRainAsset.Demo
{
	public class ControllWipers : MonoBehaviour
	{
		public Animator m_WipersAnimator;

		public AudioSource m_AudioSource;

		public float m_AnimationSpeed = 1f;

		public KeyCode m_SingleWipeKey = KeyCode.E;

		public KeyCode m_TurnOffWipersKey = KeyCode.Alpha0;

		public WipingPreset[] m_WipingPresets;

		private int _wiperMode;

		private float _lastWipedTime;

		private bool _goingToWipe;

		private readonly int wipeTriggerHash = Animator.StringToHash("Wipe");

		private readonly int wipingSpeedHash = Animator.StringToHash("WipingSpeed");

		public void PlayWipersAudio()
		{
			m_AudioSource.Play();
			_lastWipedTime = Time.time;
			_goingToWipe = false;
		}

		public void OnSingleWipeKey(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				m_WipersAnimator.SetFloat(wipingSpeedHash, 1f);
				m_WipersAnimator.SetTrigger(wipeTriggerHash);
			}
		}

		public void OnTurnOffKey(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				_wiperMode = 0;
			}
		}

		public void OnWipingModeKey(InputAction.CallbackContext context)
		{
			if (!context.performed || !(context.control is KeyControl keyControl))
			{
				return;
			}
			for (int i = 0; i < m_WipingPresets.Length; i++)
			{
				string text = m_WipingPresets[i].keyCode.ToString();
				text = text.Replace("Alpha", "");
				if (keyControl.name == text)
				{
					_wiperMode = i + 1;
				}
			}
		}

		private void Update()
		{
			if (_wiperMode > 0 && _wiperMode <= m_WipingPresets.Length && !_goingToWipe)
			{
				WipingPreset wipingPreset = m_WipingPresets[_wiperMode - 1];
				if (Time.time - _lastWipedTime > wipingPreset.delay)
				{
					m_WipersAnimator.SetFloat(wipingSpeedHash, wipingPreset.animSpeed);
					m_WipersAnimator.SetTrigger(wipeTriggerHash);
					_goingToWipe = true;
				}
			}
			m_WipersAnimator.speed = m_AnimationSpeed;
		}
	}
}
