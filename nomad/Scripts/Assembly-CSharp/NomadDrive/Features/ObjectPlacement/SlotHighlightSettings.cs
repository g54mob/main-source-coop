using UnityEngine;

namespace NomadDrive.Features.ObjectPlacement
{
	[CreateAssetMenu(fileName = "SlotHighlightSettings", menuName = "NomadDrive/Object Placement/Slot Highlight Settings")]
	public class SlotHighlightSettings : ScriptableObject
	{
		[Header("Material Reference")]
		[Tooltip("Material using the SweepHighlight shader")]
		public Material sweepHighlightMaterial;

		[Header("Sweep Band")]
		[Tooltip("Color of the sweep highlight")]
		public Color color = new Color(1f, 1f, 1f, 0.8f);

		[Tooltip("Width of the sweep band")]
		[Range(0.01f, 1f)]
		public float bandWidth = 0.01f;

		[Tooltip("Softness of the band edges")]
		[Range(0.01f, 1f)]
		public float bandSoftness = 0.283f;

		[Tooltip("Speed of the sweep animation")]
		public float speed = 0.5f;

		[Header("Emission")]
		[Tooltip("Emission intensity multiplier")]
		public float emissionIntensity = 1f;

		[Header("Fresnel")]
		[Tooltip("Fresnel edge glow power")]
		public float fresnelPower = 10.74f;

		[Tooltip("Fresnel edge glow intensity")]
		public float fresnelIntensity = 6.14f;

		[Header("Direction")]
		[Tooltip("Sweep direction vector")]
		public Vector4 direction = new Vector4(0f, 1f, 0f, 0f);
	}
}
