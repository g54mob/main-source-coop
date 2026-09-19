using UnityEngine;

namespace Features.EditorToolsModule.Scripts.DecalBaker
{
	[DisallowMultipleComponent]
	[AddComponentMenu("Tools/Decal Baker/Beach Decal Baker")]
	public sealed class BeachDecalBaker : MonoBehaviour
	{
		[Header("Beach Target")]
		[Tooltip("Beach mesh renderer. Its shared material is the assignment target unless overridden below.")]
		[SerializeField]
		private Renderer _beachMeshRenderer;

		[Tooltip("Optional override material to receive the splatmap. If null, uses the beach renderer's shared material.")]
		[SerializeField]
		private Material _beachMaterialOverride;

		[Tooltip("Padding (meters) added to the auto-computed decal union bounds. Use 0.5–2 m so decals near the edge are not clipped.")]
		[SerializeField]
		private float _bakeBoundsPaddingMeters = 1f;

		[Header("Material Properties")]
		[Tooltip("Material property that receives the baked splatmap texture.")]
		[SerializeField]
		private string _splatmapPropertyName = "_DecalSplatmap";

		[Tooltip("Material Vector property receiving (minX, minZ) of the bake bounds.")]
		[SerializeField]
		private string _boundsMinXZPropertyName = "_BakeBoundsMinXZ";

		[Tooltip("Material Vector property receiving (sizeX, sizeZ) of the bake bounds.")]
		[SerializeField]
		private string _boundsSizeXZPropertyName = "_BakeBoundsSizeXZ";

		[Tooltip("Texture property on each decal's preview material that holds its alpha mask (read at bake time).")]
		[SerializeField]
		private string _decalSourceTextureProperty = "_BaseMap";

		[Header("Output")]
		[Tooltip("Output texture asset path. Saved as EXR (16-bit half float) regardless of extension; .exr is recommended.")]
		[SerializeField]
		private string _outputTexturePath = "Assets/Global/GameObjects/ART/Materials/Beach/T_Beach_DecalSplatmap.exr";

		[Tooltip("Square resolution of the baked splatmap.")]
		[SerializeField]
		private int _resolution = 2048;

		public Renderer BeachMeshRenderer => _beachMeshRenderer;

		public Material BeachMaterial
		{
			get
			{
				if (!(_beachMaterialOverride != null))
				{
					if (!(_beachMeshRenderer != null))
					{
						return null;
					}
					return _beachMeshRenderer.sharedMaterial;
				}
				return _beachMaterialOverride;
			}
		}

		public float BakeBoundsPaddingMeters => _bakeBoundsPaddingMeters;

		public string SplatmapPropertyName => _splatmapPropertyName;

		public string BoundsMinXZPropertyName => _boundsMinXZPropertyName;

		public string BoundsSizeXZPropertyName => _boundsSizeXZPropertyName;

		public string DecalSourceTextureProperty => _decalSourceTextureProperty;

		public string OutputTexturePath => _outputTexturePath;

		public int Resolution => _resolution;
	}
}
