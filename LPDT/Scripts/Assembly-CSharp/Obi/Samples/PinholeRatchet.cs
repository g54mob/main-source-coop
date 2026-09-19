using UnityEngine;

namespace Obi.Samples
{
	public class PinholeRatchet : MonoBehaviour
	{
		public ObiPinhole pinhole;

		public bool direction;

		public float teethSeparation = 0.1f;

		public float distanceToNextTooth { get; private set; }

		private void Update()
		{
			if (pinhole == null || pinhole.rope == null)
			{
				return;
			}
			float restLength = (pinhole.rope as ObiRopeBase).restLength;
			float num = Mathf.Max(0.001f, teethSeparation / restLength);
			Vector2 range = pinhole.range;
			if (direction)
			{
				for (distanceToNextTooth = (range.y - pinhole.position) * restLength; distanceToNextTooth > teethSeparation; distanceToNextTooth -= teethSeparation)
				{
					range.y -= num;
				}
			}
			else
			{
				for (distanceToNextTooth = (pinhole.position - range.x) * restLength; distanceToNextTooth > teethSeparation; distanceToNextTooth -= teethSeparation)
				{
					range.x += num;
				}
			}
			pinhole.range = range;
		}

		public void OnDisable()
		{
			if (pinhole != null)
			{
				pinhole.range = new Vector2(0f, 1f);
			}
		}
	}
}
