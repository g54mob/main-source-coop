using UnityEngine;

namespace NomadDrive.Features.ObjectPlacement
{
	[CreateAssetMenu(fileName = "DropIndicatorSettings", menuName = "NomadDrive/Object Placement/Drop Indicator Settings")]
	public class DropIndicatorSettings : ScriptableObject
	{
		[Header("Dashed Line")]
		[Tooltip("Material using the DashedLine shader")]
		public Material dashedLineMaterial;

		[Tooltip("Width of the dashed line")]
		[Range(0.005f, 0.1f)]
		public float lineWidth = 0.02f;

		[Tooltip("Length of each dash segment")]
		[Range(0.05f, 0.5f)]
		public float dashLength = 0.15f;

		[Tooltip("Gap between dash segments")]
		[Range(0.05f, 0.3f)]
		public float gapLength = 0.1f;

		[Tooltip("Speed of dash animation scroll")]
		[Range(0f, 5f)]
		public float scrollSpeed = 1f;

		[Tooltip("Line color in normal state")]
		public Color lineColor = new Color(1f, 1f, 1f, 0.6f);

		[Tooltip("Line color when snapping to a SnappingPlane")]
		public Color lineColorSnapping = new Color(0.2f, 1f, 0.4f, 0.8f);

		[Tooltip("Line color when collision detected")]
		public Color lineColorCollision = new Color(1f, 0.2f, 0.2f, 0.8f);

		[Header("Ghost Preview")]
		[Tooltip("Material using the GhostPreview shader")]
		public Material ghostMaterial;

		[Tooltip("Base alpha transparency for ghost")]
		[Range(0.1f, 0.8f)]
		public float ghostAlpha = 0.4f;

		[Tooltip("Ghost color in normal state")]
		public Color ghostColor = new Color(1f, 1f, 1f, 0.4f);

		[Tooltip("Ghost color when snapping to a SnappingPlane")]
		public Color ghostColorSnapping = new Color(0.2f, 1f, 0.4f, 0.5f);

		[Tooltip("Ghost color when collision detected")]
		public Color ghostColorCollision = new Color(1f, 0.2f, 0.2f, 0.5f);

		[Tooltip("Fresnel edge glow power (higher = sharper edge)")]
		[Range(0.5f, 5f)]
		public float fresnelPower = 2f;

		[Tooltip("Fresnel edge glow intensity")]
		[Range(0f, 1f)]
		public float fresnelIntensity = 0.3f;
	}
}
