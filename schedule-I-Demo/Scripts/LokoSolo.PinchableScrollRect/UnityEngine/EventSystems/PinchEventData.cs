namespace UnityEngine.EventSystems
{
	public class PinchEventData
	{
		public PointerEventData targetPointerData;

		public PointerEventData unchangedPointerData;

		public float distanceDelta;

		public Vector2 midPoint => (targetPointerData.position + unchangedPointerData.position) / 2f;

		public PinchEventData(PointerEventData target, PointerEventData unchanged, float distanceDelta = 0f)
		{
			targetPointerData = target;
			unchangedPointerData = unchanged;
			this.distanceDelta = distanceDelta;
		}
	}
}
