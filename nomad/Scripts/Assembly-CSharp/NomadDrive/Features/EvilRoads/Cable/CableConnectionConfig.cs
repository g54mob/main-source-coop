using NomadDrive.Features.Rope;
using UnityEngine;

namespace NomadDrive.Features.EvilRoads.Cable
{
	[CreateAssetMenu(menuName = "NomadDrive/World Generation/Cable Connection Config", fileName = "CableConnectionConfig")]
	public class CableConnectionConfig : ScriptableObject
	{
		[Header("Rope Prefab")]
		[Tooltip("The rope prefab to instantiate for connections. Must have a Rope component.")]
		[SerializeField]
		private GameObject _ropePrefab;

		[Header("Rope Parameters")]
		[Tooltip("Extra slack added to the rope length (creates more sag). Rope length = distance + slack.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float _slack = 0.5f;

		[Tooltip("Rope stretch factor. Higher values = tighter rope.")]
		[SerializeField]
		[Range(1f, 20f)]
		private float _stretch = 8f;

		[Header("Animation")]
		[Tooltip("Swing angle in radians. Higher values = more swing.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float _swingAngle = 0.3f;

		[Tooltip("Swing frequency. Higher values = faster swing.")]
		[SerializeField]
		[Range(0f, 5f)]
		private float _swingFreq = 1.5f;

		[Header("Visual")]
		[Tooltip("Width scale at the start of the rope.")]
		[SerializeField]
		[Range(0f, 2f)]
		private float _widthStart = 1f;

		[Tooltip("Width scale at the end of the rope.")]
		[SerializeField]
		[Range(0f, 2f)]
		private float _widthEnd = 1f;

		[Header("Optional Overrides")]
		[Tooltip("Override rope mesh and material. Leave null to use prefab defaults.")]
		[SerializeField]
		private RopeType _ropeType;

		[Tooltip("Rope end attachment at start point. Leave null for no end object.")]
		[SerializeField]
		private RopeEnd _startEnd;

		[Tooltip("Rope end attachment at end point. Leave null for no end object.")]
		[SerializeField]
		private RopeEnd _endEnd;

		[Header("Global Manager Influence")]
		[Tooltip("Allow CableManager's swing multiplier to affect this cable type.")]
		[SerializeField]
		private bool _affectedByGlobalSwing = true;

		[Tooltip("Allow CableManager's slack multiplier to affect this cable type.")]
		[SerializeField]
		private bool _affectedByGlobalSlack = true;

		[Header("Sequence Behavior")]
		[Tooltip("How to handle the first object in sequence (no previous object to connect from).")]
		[SerializeField]
		private CableEndBehavior _startBehavior;

		[Tooltip("How to handle the last object in sequence (no next object to connect to).")]
		[SerializeField]
		private CableEndBehavior _endBehavior;

		public GameObject RopePrefab => _ropePrefab;

		public float Slack => _slack;

		public float Stretch => _stretch;

		public float SwingAngle => _swingAngle;

		public float SwingFreq => _swingFreq;

		public float WidthStart => _widthStart;

		public float WidthEnd => _widthEnd;

		public RopeType RopeType => _ropeType;

		public RopeEnd StartEnd => _startEnd;

		public RopeEnd EndEnd => _endEnd;

		public CableEndBehavior StartBehavior => _startBehavior;

		public CableEndBehavior EndBehavior => _endBehavior;

		public bool AffectedByGlobalSwing => _affectedByGlobalSwing;

		public bool AffectedByGlobalSlack => _affectedByGlobalSlack;

		public bool Validate(out string errorMessage)
		{
			errorMessage = string.Empty;
			if (_ropePrefab == null)
			{
				errorMessage = "Rope prefab is not assigned";
				return false;
			}
			if (_ropePrefab.GetComponent<NomadDrive.Features.Rope.Rope>() == null)
			{
				errorMessage = "Rope prefab does not have a Rope component";
				return false;
			}
			return true;
		}

		private void OnValidate()
		{
			if (_ropePrefab != null && _ropePrefab.GetComponent<NomadDrive.Features.Rope.Rope>() == null)
			{
				Debug.LogWarning("[CableConnectionConfig] " + base.name + ": Rope prefab does not have a Rope component");
			}
		}
	}
}
