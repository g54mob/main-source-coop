using System;
using UnityEngine;

namespace Features.PhysicsVolumeModule.Scripts
{
	[Serializable]
	public class LiftCallStop
	{
		[Tooltip("The floor-side lever that calls the cabin to this stop.")]
		[SerializeField]
		private LiftCallLever _lever;

		[Tooltip("Height of this stop above the shaft base, in metres. Clamped to the shaft's travel.")]
		[SerializeField]
		private float _heightMeters;

		[Tooltip("Name shown on the shaft gizmo — the floor this stop serves.")]
		[SerializeField]
		private string _label = "Floor";

		public LiftCallLever Lever => _lever;

		public float HeightMeters => _heightMeters;

		public string Label
		{
			get
			{
				if (!string.IsNullOrEmpty(_label))
				{
					return _label;
				}
				return "Floor";
			}
		}
	}
}
