using UnityEngine;
using UnityEngine.UI;

namespace Zorro.UI.Effects
{
	[ExecuteInEditMode]
	public abstract class EffectBase : MonoBehaviour
	{
		public Shader m_shader;

		protected Material m_material;

		private Image m_image;

		protected virtual void Start()
		{
			DestroyMaterial();
			InitializeMaterial();
		}

		private void InitializeMaterial()
		{
			if (m_shader != null)
			{
				m_image = GetComponent<Image>();
				m_material = new Material(m_shader);
				m_image.material = m_material;
			}
		}

		private void OnDestroy()
		{
			DestroyMaterial();
		}

		private void DestroyMaterial()
		{
			if (!(m_material == null) && !(m_material.shader != m_shader))
			{
				if (Application.isPlaying)
				{
					Object.Destroy(m_material);
				}
				else
				{
					Object.DestroyImmediate(m_material);
				}
				m_material = null;
			}
		}

		protected virtual void Update()
		{
			if (m_material == null && m_shader != null)
			{
				InitializeMaterial();
			}
			m_material = m_image.GetModifiedMaterial(m_material);
			m_image.material = m_material;
		}
	}
}
