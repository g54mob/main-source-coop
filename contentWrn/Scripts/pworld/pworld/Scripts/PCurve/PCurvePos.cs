using System;
using UnityEngine;
using UnityEngine.Animations;

namespace pworld.Scripts.PCurve
{
	public class PCurvePos : PCurveBase
	{
		public AnimationCurve Curve;

		public Axis axis;

		public bool playing = true;

		public bool loop;

		private float elapsedTime;

		private Vector3 startPos;

		private Vector3 Axis => axis switch
		{
			UnityEngine.Animations.Axis.None => Vector3.zero, 
			UnityEngine.Animations.Axis.X => Vector3.right, 
			UnityEngine.Animations.Axis.Y => Vector3.up, 
			UnityEngine.Animations.Axis.Z => Vector3.forward, 
			_ => throw new ArgumentOutOfRangeException(), 
		};

		private void Start()
		{
			startPos = base.transform.position;
			base.transform.position = startPos + base.transform.TransformVector(Curve.Evaluate(elapsedTime) * Axis);
		}

		private void Update()
		{
			if (playing)
			{
				elapsedTime += Time.deltaTime;
				if (loop)
				{
					elapsedTime %= Curve.keys[^1].time;
				}
				base.transform.position = startPos + base.transform.TransformVector(Curve.Evaluate(elapsedTime) * Axis);
			}
		}

		public override void Play()
		{
			elapsedTime = 0f;
			playing = true;
		}
	}
}
