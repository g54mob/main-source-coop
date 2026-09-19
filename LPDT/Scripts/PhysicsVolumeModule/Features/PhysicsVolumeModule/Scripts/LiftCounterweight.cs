using UnityEngine;

namespace Features.PhysicsVolumeModule.Scripts
{
	[ExecuteAlways]
	public class LiftCounterweight : MonoBehaviour
	{
		[SerializeField]
		private Lift _lift;

		[Tooltip("Offset of the weight relative to the shaft top — where it sits when the deck is at the bottom.")]
		[SerializeField]
		private Vector3 _offset = Vector3.zero;

		private void LateUpdate()
		{
			if (!(_lift == null))
			{
				float num = Mathf.Clamp(_lift.DeckPosition.y - _lift.BottomPoint.y, 0f, _lift.ShaftHeight);
				Vector3 position = _lift.TopPoint + _offset;
				position.y -= num;
				base.transform.position = position;
			}
		}
	}
}
