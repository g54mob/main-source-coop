using UnityEngine;

namespace NomadDrive.Features.ObjectPlacement
{
	[CreateAssetMenu(fileName = "PlacementGridSettings", menuName = "NomadDrive/Object Placement/Placement Grid Settings")]
	public class PlacementGridSettings : ScriptableObject
	{
		[Header("Material Reference")]
		[Tooltip("Material using the ObjectPlacementGrid shader")]
		public Material gridMaterial;

		[Header("Grid")]
		[Tooltip("Size of each grid cell in meters")]
		public float gridSize = 0.2f;

		[Tooltip("Width of grid lines in meters")]
		public float lineWidth = 0.01f;

		[Header("Falloff")]
		[Tooltip("Visible radius of the grid")]
		public float radius = 1.64f;

		[Tooltip("Softness of the edge falloff")]
		public float falloffSoftness = 0.4f;

		[Header("Appearance")]
		[Tooltip("Color of the grid lines")]
		public Color gridColor = new Color(1f, 1f, 1f, 0.298f);

		[Tooltip("Emission intensity multiplier")]
		public float emissionIntensity = 1.1f;

		[Header("Custom Texture")]
		[Tooltip("Use a custom texture instead of procedural grid")]
		public bool useTexture = true;

		[Tooltip("Custom grid texture (only used when Use Texture is enabled)")]
		public Texture2D gridTexture;
	}
}
