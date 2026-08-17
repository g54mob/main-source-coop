using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.POISpawning
{
	public class PoiDefinition : MonoBehaviour
	{
		[Header("Identity")]
		[SerializeField]
		private POICategory _category;

		[SerializeField]
		private string _displayName;

		[Header("Terrain Defaults")]
		[SerializeField]
		private TerrainDeformMode _defaultDeformMode = TerrainDeformMode.RaiseToMax;

		[SerializeField]
		[Range(5f, 100f)]
		private float _defaultDeformRadius = 10f;

		[SerializeField]
		[Range(0f, 1f)]
		private float _defaultDeformFalloff = 0.3f;

		[Tooltip("Sinks the POI below terrain surface to prevent z-fighting. The building floor embeds into the ground. Value in meters.")]
		[SerializeField]
		[Range(0f, 0.5f)]
		private float _defaultTerrainHeightOffset = 0.05f;

		[Tooltip("Raises POI above the foundation platform to prevent terrain clipping. Only used in Foundation mode. Value in meters.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float _defaultFoundationYOffset = 0.1f;

		[SerializeField]
		private bool _vegetationCleaning = true;

		public POICategory Category => _category;

		public string DisplayName
		{
			get
			{
				if (!string.IsNullOrEmpty(_displayName))
				{
					return _displayName;
				}
				return base.gameObject.name;
			}
		}

		public TerrainDeformMode DefaultDeformMode => _defaultDeformMode;

		public float DefaultDeformRadius => _defaultDeformRadius;

		public float DefaultDeformFalloff => _defaultDeformFalloff;

		public float DefaultTerrainHeightOffset => _defaultTerrainHeightOffset;

		public float DefaultFoundationYOffset => _defaultFoundationYOffset;

		public bool VegetationCleaning => _vegetationCleaning;
	}
}
