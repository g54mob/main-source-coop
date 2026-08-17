using System;
using UnityEngine;
using UnityEngine.Events;

namespace NWH.WheelController3D
{
	[Serializable]
	public class Wheel
	{
		[Obsolete]
		[Tooltip("!OBSOLETE! - Check the v11 to v12 upgrade notes! Parent the visual(s) to the rotatingContainer instead.GameObject representing the visual aspect of the wheel / wheel mesh.\r\nShould not have any physics colliders attached to it.")]
		public GameObject visual;

		[Obsolete]
		[Tooltip("!OBSOLETE! - Check the v11 to v12 upgrade notes! Parent the visual to the rotatingContainer instead.Object representing non-rotating part of the wheel. This could be things such as brake calipers, external fenders, etc.")]
		public GameObject nonRotatingVisual;

		[Tooltip("Container to which all the rotating wheel parts should be parented, such as wheels, tires, and brake discs.")]
		public Transform rotatingContainer;

		[Tooltip("Container to which all the non-rotating wheel parts should be parented, such as fenders and brake calipers.")]
		public Transform nonRotatingContainer;

		public Quaternion axleRotation;

		[Tooltip("Collider covering the top half of the wheel. ")]
		public MeshCollider meshCollider;

		[Tooltip("    Current angular velocity of the wheel in rad/s.")]
		public float angularVelocity;

		public float inertia;

		public float invInertia;

		[Tooltip("Mass of the wheel. Inertia is calculated from this.")]
		public float mass = 30f;

		[Tooltip("Total radius of the tire in [m].")]
		[Min(0.001f)]
		public float radius = 0.35f;

		public float invRadius = 3f;

		[NonSerialized]
		[Tooltip("    Current rotation angle of the wheel visual in regards to it's X axis vector.")]
		public float axleAngle;

		[Tooltip("    Width of the tyre.")]
		[Min(0.001f)]
		public float width = 0.25f;

		[Tooltip("Offset of the rim from the axle, in m.")]
		public float rimOffset = 0.1f;

		[NonSerialized]
		[Tooltip("Angular velocity during the previus FixedUpdate().")]
		public float prevAngularVelocity;

		[NonSerialized]
		[Tooltip("Called when either radius or width of the wheel change.")]
		public UnityEvent onWheelDimensionsChange = new UnityEvent();

		public float rpm => angularVelocity * 9.55f;
	}
}
