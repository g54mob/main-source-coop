using UnityEngine;

namespace Mimicraft.Gameplay
{
	public class WeaponPickup : MonoBehaviour
	{
		[Tooltip("Bu objeye bakıp F'ye basınca alınacak silah.")]
		[SerializeField]
		private WeaponDefinition weapon;

		[Tooltip("Oyuncuya gösterilecek ad. Boş bırakılırsa silahın kendi adı kullanılır - normalde boş bırak, iki yerde iki isim tutmanın bir faydası yok.")]
		[SerializeField]
		private string promptOverride = "";

		[Tooltip("Bakıldığında çevresinde çıkacak kontur rengi.")]
		[SerializeField]
		private Color outlineColor = new Color(1f, 0.85f, 0.3f, 1f);

		[Tooltip("Konturun kalınlığı, metre. Dünya birimi olduğu için küçük bir tabanca ile uzun bir tüfek aynı kalınlıkta çerçevelenir.")]
		[SerializeField]
		[Range(0.001f, 0.1f)]
		private float outlineWidth = 0.012f;

		private const string OutlineShaderName = "Mimicraft/HighlightOutline";

		private static readonly int OutlineColorId = Shader.PropertyToID("_OutlineColor");

		private static readonly int OutlineWidthId = Shader.PropertyToID("_OutlineWidth");

		private bool highlighted;

		private Renderer[] renderers;

		private Material[][] originalMaterials;

		private Material outlineMaterial;

		private static bool warnedAboutShader;

		public WeaponDefinition Weapon => weapon;

		public string Prompt
		{
			get
			{
				if (string.IsNullOrWhiteSpace(promptOverride))
				{
					if (!(weapon != null))
					{
						return "";
					}
					return weapon.DisplayName;
				}
				return promptOverride;
			}
		}

		public void SetHighlighted(bool highlighted)
		{
			if (this.highlighted == highlighted)
			{
				return;
			}
			this.highlighted = highlighted;
			CacheRenderers();
			for (int i = 0; i < renderers.Length; i++)
			{
				Renderer renderer = renderers[i];
				if (!(renderer == null))
				{
					renderer.sharedMaterials = (highlighted ? WithOutline(originalMaterials[i]) : originalMaterials[i]);
				}
			}
		}

		private Material[] WithOutline(Material[] originals)
		{
			if (outlineMaterial == null)
			{
				Shader shader = Shader.Find("Mimicraft/HighlightOutline");
				if (shader == null)
				{
					if (!warnedAboutShader)
					{
						warnedAboutShader = true;
						Debug.LogWarning("[WeaponPickup] 'Mimicraft/HighlightOutline' shader'i bulunamadi - kontur cizilmeyecek. Build'de Always Included Shaders'a eklenmesi gerekebilir.", this);
					}
					return originals;
				}
				outlineMaterial = new Material(shader);
				outlineMaterial.SetColor(OutlineColorId, outlineColor);
				outlineMaterial.SetFloat(OutlineWidthId, outlineWidth);
			}
			Material[] array = new Material[originals.Length + 1];
			originals.CopyTo(array, 0);
			array[originals.Length] = outlineMaterial;
			return array;
		}

		private void CacheRenderers()
		{
			if (renderers == null)
			{
				renderers = GetComponentsInChildren<Renderer>(includeInactive: true);
				originalMaterials = new Material[renderers.Length][];
				for (int i = 0; i < renderers.Length; i++)
				{
					originalMaterials[i] = ((renderers[i] != null) ? renderers[i].sharedMaterials : new Material[0]);
				}
			}
		}

		private void OnDestroy()
		{
			if (outlineMaterial != null)
			{
				Object.Destroy(outlineMaterial);
			}
		}

		private void OnValidate()
		{
			if (!(weapon != null))
			{
				Debug.LogWarning("[WeaponPickup] '" + base.name + "' bir WeaponDefinition tasimiyor - bu objeye bakip F'ye basmak hicbir sey yapmaz.", this);
			}
		}
	}
}
