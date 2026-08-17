using UnityEngine;

namespace PaintCore
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwDestroyAfterTime")]
	[AddComponentMenu("CW/Paint Core/CW Destroy After Time")]
	public class CwDestroyAfterTime : MonoBehaviour
	{
		[SerializeField]
		private float seconds = 5f;

		[SerializeField]
		private float age;

		public float Seconds
		{
			get
			{
				return seconds;
			}
			set
			{
				seconds = value;
			}
		}

		[ContextMenu("Destroy Now")]
		public void DestroyNow()
		{
			Object.Destroy(base.gameObject);
		}

		protected virtual void Update()
		{
			if (seconds >= 0f)
			{
				age += Time.deltaTime;
				if (age >= seconds)
				{
					DestroyNow();
				}
			}
		}
	}
}
