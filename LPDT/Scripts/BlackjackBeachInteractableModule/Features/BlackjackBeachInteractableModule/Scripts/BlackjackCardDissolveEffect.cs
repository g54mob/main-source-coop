using System.Collections;
using UnityEngine;

namespace Features.BlackjackBeachInteractableModule.Scripts
{
	public class BlackjackCardDissolveEffect : MonoBehaviour
	{
		private readonly int _dissolveAmountId = Shader.PropertyToID("_DissolveAmount");

		private readonly string[] _albedoPropertyNames = new string[3] { "_BaseMap", "_MainTex", "_MainTexture" };

		[SerializeField]
		private Renderer[] _renderers;

		[SerializeField]
		private Material _dissolveMaterial;

		private Material[] _restMaterials;

		private Material[] _runtimeMaterials;

		private bool _ownsRuntimeMaterials;

		private void Awake()
		{
			ResolveRenderers();
			CacheRestMaterials();
			EnsureRuntimeMaterials();
			SetDissolveAmount(0f);
		}

		private void OnDestroy()
		{
			DestroyOwnedRuntimeMaterials();
		}

		public void SetVisibleInstant(bool visible)
		{
			ResolveRenderers();
			CacheRestMaterials();
			EnsureRuntimeMaterials();
			SetDissolveAmount(visible ? 0f : 1f);
			for (int i = 0; i < _renderers.Length; i++)
			{
				if (_renderers[i] != null)
				{
					_renderers[i].enabled = visible;
				}
			}
		}

		public IEnumerator PlayDissolveOut(float duration, Material dissolveTemplate = null)
		{
			if (PrepareForDissolve(dissolveTemplate))
			{
				SetDissolveAmount(0f);
				yield return AnimateDissolve(0f, 1f, duration);
				SetDissolveAmount(1f);
			}
		}

		public IEnumerator PlayDissolveIn(float duration, Material dissolveTemplate = null)
		{
			if (PrepareForDissolve(dissolveTemplate))
			{
				SetDissolveAmount(1f);
				yield return AnimateDissolve(1f, 0f, duration);
				SetDissolveAmount(0f);
			}
		}

		private IEnumerator AnimateDissolve(float from, float to, float duration)
		{
			float elapsed = 0f;
			float safeDuration = Mathf.Max(0.01f, duration);
			while (elapsed < safeDuration)
			{
				elapsed += Time.deltaTime;
				float t = Mathf.Clamp01(elapsed / safeDuration);
				SetDissolveAmount(Mathf.Lerp(from, to, t));
				yield return null;
			}
		}

		private bool PrepareForDissolve(Material dissolveTemplate)
		{
			ResolveRenderers();
			CacheRestMaterials();
			if (_renderers == null || _renderers.Length == 0)
			{
				return false;
			}
			Material material = ((dissolveTemplate != null) ? dissolveTemplate : _dissolveMaterial);
			if (material != null && !RestAlreadyUsesDissolveShader())
			{
				ApplyTemporaryDissolveInstances(material);
			}
			else
			{
				EnsureRuntimeMaterials();
			}
			if (_runtimeMaterials == null)
			{
				return false;
			}
			for (int i = 0; i < _renderers.Length; i++)
			{
				if (_renderers[i] != null)
				{
					_renderers[i].enabled = true;
				}
			}
			return true;
		}

		private bool RestAlreadyUsesDissolveShader()
		{
			if (_restMaterials == null)
			{
				return false;
			}
			for (int i = 0; i < _restMaterials.Length; i++)
			{
				Material material = _restMaterials[i];
				if (material != null && material.HasProperty(_dissolveAmountId))
				{
					return true;
				}
			}
			return false;
		}

		private void ApplyTemporaryDissolveInstances(Material template)
		{
			DestroyOwnedRuntimeMaterials();
			_runtimeMaterials = new Material[_renderers.Length];
			_ownsRuntimeMaterials = true;
			for (int i = 0; i < _renderers.Length; i++)
			{
				Renderer renderer = _renderers[i];
				if (!(renderer == null))
				{
					Material source = ((i < _restMaterials.Length) ? _restMaterials[i] : renderer.sharedMaterial);
					Material material = new Material(template);
					CopyAlbedo(source, material);
					_runtimeMaterials[i] = material;
					renderer.sharedMaterial = material;
				}
			}
		}

		private void EnsureRuntimeMaterials()
		{
			if (_runtimeMaterials != null)
			{
				return;
			}
			ResolveRenderers();
			CacheRestMaterials();
			if (_renderers == null || _renderers.Length == 0)
			{
				return;
			}
			_runtimeMaterials = new Material[_renderers.Length];
			_ownsRuntimeMaterials = true;
			for (int i = 0; i < _renderers.Length; i++)
			{
				Renderer renderer = _renderers[i];
				Material material = ((_restMaterials != null && i < _restMaterials.Length) ? _restMaterials[i] : ((renderer != null) ? renderer.sharedMaterial : null));
				if (!(renderer == null) && !(material == null))
				{
					Material material2 = new Material(material);
					_runtimeMaterials[i] = material2;
					renderer.sharedMaterial = material2;
				}
			}
		}

		private void CacheRestMaterials()
		{
			if (_restMaterials != null || _renderers == null)
			{
				return;
			}
			_restMaterials = new Material[_renderers.Length];
			for (int i = 0; i < _renderers.Length; i++)
			{
				if (_renderers[i] != null)
				{
					_restMaterials[i] = _renderers[i].sharedMaterial;
				}
			}
		}

		private void SetDissolveAmount(float amount)
		{
			if (_runtimeMaterials == null)
			{
				return;
			}
			for (int i = 0; i < _runtimeMaterials.Length; i++)
			{
				Material material = _runtimeMaterials[i];
				if (material != null && material.HasProperty(_dissolveAmountId))
				{
					material.SetFloat(_dissolveAmountId, amount);
				}
			}
		}

		private void DestroyOwnedRuntimeMaterials()
		{
			if (!_ownsRuntimeMaterials || _runtimeMaterials == null)
			{
				return;
			}
			for (int i = 0; i < _runtimeMaterials.Length; i++)
			{
				if (_runtimeMaterials[i] != null)
				{
					Object.Destroy(_runtimeMaterials[i]);
				}
			}
			_runtimeMaterials = null;
			_ownsRuntimeMaterials = false;
		}

		private void ResolveRenderers()
		{
			if (_renderers == null || _renderers.Length == 0)
			{
				_renderers = GetComponentsInChildren<Renderer>(includeInactive: true);
			}
		}

		private void CopyAlbedo(Material source, Material destination)
		{
			if (source == null || destination == null)
			{
				return;
			}
			for (int i = 0; i < _albedoPropertyNames.Length; i++)
			{
				string text = _albedoPropertyNames[i];
				if (source.HasProperty(text) && destination.HasProperty(text))
				{
					destination.SetTexture(text, source.GetTexture(text));
					break;
				}
			}
		}
	}
}
