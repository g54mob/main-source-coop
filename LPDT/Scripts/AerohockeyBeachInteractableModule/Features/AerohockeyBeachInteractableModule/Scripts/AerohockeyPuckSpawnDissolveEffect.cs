using System.Collections.Generic;
using UnityEngine;

namespace Features.AerohockeyBeachInteractableModule.Scripts
{
	public class AerohockeyPuckSpawnDissolveEffect : MonoBehaviour
	{
		private static readonly int DissolveAmountId = Shader.PropertyToID("_DissolveAmount");

		[SerializeField]
		private List<Renderer> _renderers;

		[SerializeField]
		private float _dissolveDuration = 0.5f;

		private float _elapsedTime;

		private bool _isPlaying;

		private bool _appearArmed;

		private void Awake()
		{
			SetDissolveAmount(0f);
		}

		private void OnDisable()
		{
			_appearArmed = true;
			_isPlaying = false;
		}

		private void OnEnable()
		{
			if (!_appearArmed)
			{
				SetDissolveAmount(0f);
				return;
			}
			_appearArmed = false;
			_elapsedTime = 0f;
			_isPlaying = true;
			SetDissolveAmount(1f);
		}

		private void Update()
		{
			if (_isPlaying)
			{
				_elapsedTime += Time.deltaTime;
				float num = Mathf.Clamp01(_elapsedTime / _dissolveDuration);
				SetDissolveAmount(1f - num);
				if (num >= 1f)
				{
					_isPlaying = false;
				}
			}
		}

		private void SetDissolveAmount(float amount)
		{
			foreach (Renderer renderer in _renderers)
			{
				renderer.sharedMaterial.SetFloat(DissolveAmountId, amount);
			}
		}
	}
}
