using System.Collections.Generic;
using UnityEngine;

namespace Features.ItemsModule.Scripts
{
	public class ItemSpawnDissolveEffect : MonoBehaviour
	{
		private static readonly int _dissolveAmount = Shader.PropertyToID("_DissolveAmount");

		[SerializeField]
		private List<Renderer> _renderers;

		[SerializeField]
		private float _dissolveDuration = 1f;

		private readonly List<Material> _dissolveInstances = new List<Material>();

		private float _elapsedTime;

		private bool _isPlaying;

		private void Awake()
		{
			foreach (Renderer renderer in _renderers)
			{
				_dissolveInstances.Add(renderer.material);
			}
		}

		private void OnDestroy()
		{
			foreach (Material dissolveInstance in _dissolveInstances)
			{
				Object.Destroy(dissolveInstance);
			}
		}

		private void OnEnable()
		{
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
			foreach (Material dissolveInstance in _dissolveInstances)
			{
				dissolveInstance.SetFloat(_dissolveAmount, amount);
			}
		}
	}
}
