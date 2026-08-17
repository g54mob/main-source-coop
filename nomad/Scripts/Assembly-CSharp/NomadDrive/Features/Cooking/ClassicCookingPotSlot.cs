using Ami.BroAudio;
using Ami.BroAudio.Data;
using EvilCore.Audio;
using PrimeTween;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Cooking
{
	public class ClassicCookingPotSlot : CookingPotSlot, ICookingPotSlotActions
	{
		private static readonly Vector3 FireFullScale = new Vector3(0.077f, 0.077f, 0.077f);

		[SerializeField]
		private GameObject heatDistrotionObject;

		[SerializeField]
		private SoundID burnerSound;

		[SerializeField]
		private AudioParameter ignitionParameter;

		[Inject]
		private IAudioManager audioManager;

		private ParticleSystem _fireEffectParticleSystem;

		private AudioHandle _burnerAudioHandle = AudioHandle.Invalid;

		protected override void Awake()
		{
			base.Awake();
			_fireEffectParticleSystem = GetComponentInChildren<ParticleSystem>();
			_fireEffectParticleSystem.Stop();
			heatDistrotionObject.SetActive(value: false);
			base.OnCookingPotSlotIgnited.AddListener(OnIgniteLive);
			base.OnCookingPotSlotExtinguished.AddListener(OnExtinguishLive);
			base.OnCookingPotSlotLateJoinerIgnited.AddListener(OnIgniteLateJoiner);
			foreach (Transform item in _fireEffectParticleSystem.transform)
			{
				if (!(item == _fireEffectParticleSystem.transform))
				{
					item.localScale = Vector3.zero;
				}
			}
		}

		private void OnDisable()
		{
			base.OnCookingPotSlotIgnited.RemoveListener(OnIgniteLive);
			base.OnCookingPotSlotExtinguished.RemoveListener(OnExtinguishLive);
			base.OnCookingPotSlotLateJoinerIgnited.RemoveListener(OnIgniteLateJoiner);
			if (_burnerAudioHandle.IsValid)
			{
				audioManager?.ReleaseInstance(_burnerAudioHandle);
				_burnerAudioHandle = AudioHandle.Invalid;
			}
		}

		public void OnIgniteActions()
		{
			_fireEffectParticleSystem.Play();
			foreach (Transform item in _fireEffectParticleSystem.transform)
			{
				if (!(item == _fireEffectParticleSystem.transform))
				{
					Tween.Scale(item, FireFullScale, 0.75f, Ease.OutExpo);
				}
			}
			heatDistrotionObject.SetActive(value: true);
		}

		public void OnExtinguishActions()
		{
			foreach (Transform item in _fireEffectParticleSystem.transform)
			{
				if (!(item == _fireEffectParticleSystem.transform))
				{
					Tween.Scale(item, Vector3.zero, 0.75f, Ease.OutExpo);
				}
			}
			heatDistrotionObject.SetActive(value: false);
		}

		private void OnIgniteLive()
		{
			OnIgniteActions();
			StartBurnerAudio(skipIntro: false);
		}

		private void OnExtinguishLive()
		{
			OnExtinguishActions();
			StopBurnerAudio();
		}

		private void OnIgniteLateJoiner()
		{
			_fireEffectParticleSystem.Play();
			foreach (Transform item in _fireEffectParticleSystem.transform)
			{
				if (!(item == _fireEffectParticleSystem.transform))
				{
					item.localScale = FireFullScale;
				}
			}
			heatDistrotionObject.SetActive(value: true);
			StartBurnerAudio(skipIntro: true);
		}

		private void StartBurnerAudio(bool skipIntro)
		{
			if (audioManager != null && burnerSound.IsValid())
			{
				if (_burnerAudioHandle.IsValid)
				{
					audioManager.StopEvent(_burnerAudioHandle, AudioStopMode.AllowFadeout, 0.05f);
					_burnerAudioHandle = AudioHandle.Invalid;
				}
				_burnerAudioHandle = audioManager.PlayLoopRegion(burnerSound, base.gameObject, skipIntro);
				audioManager.SetParameter(_burnerAudioHandle, ignitionParameter, value: true);
			}
		}

		private void StopBurnerAudio()
		{
			if (audioManager != null && _burnerAudioHandle.IsValid)
			{
				audioManager.StopLoopRegion(_burnerAudioHandle);
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
