using CW.Common;
using UnityEngine;

namespace PaintIn3D
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintIn3D#CwTranslate")]
	[AddComponentMenu("CW/Paint in 3D/CW Translate")]
	public class CwTranslate : MonoBehaviour
	{
		[SerializeField]
		private Space space = Space.Self;

		[SerializeField]
		private float multiplier = 1f;

		[SerializeField]
		private float damping = 10f;

		[SerializeField]
		private Vector3 perSecond;

		[SerializeField]
		private Vector3 remainingDelta;

		public Space Space
		{
			get
			{
				return space;
			}
			set
			{
				space = value;
			}
		}

		public float Multiplier
		{
			get
			{
				return multiplier;
			}
			set
			{
				multiplier = value;
			}
		}

		public float Damping
		{
			get
			{
				return damping;
			}
			set
			{
				damping = value;
			}
		}

		public Vector3 PerSecond
		{
			get
			{
				return perSecond;
			}
			set
			{
				perSecond = value;
			}
		}

		public void TranslateX(float magnitude)
		{
			Translate(Vector3.right * magnitude);
		}

		public void TranslateY(float magnitude)
		{
			Translate(Vector3.up * magnitude);
		}

		public void TranslateZ(float magnitude)
		{
			Translate(Vector3.forward * magnitude);
		}

		public void Translate(Vector3 vector)
		{
			if (Space == Space.Self)
			{
				vector = base.transform.TransformVector(vector);
			}
			TranslateWorld(vector);
		}

		public void TranslateWorld(Vector3 vector)
		{
			remainingDelta += vector * Multiplier;
		}

		protected virtual void Update()
		{
			float t = CwHelper.DampenFactor(Damping, Time.deltaTime);
			Vector3 vector = Vector3.Lerp(remainingDelta, Vector3.zero, t);
			base.transform.position += remainingDelta - vector;
			base.transform.Translate(perSecond * Time.deltaTime, space);
			remainingDelta = vector;
		}
	}
}
