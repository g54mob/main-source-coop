using UnityEngine;
using UnityEngine.EventSystems;

namespace QFSW.QC.UI
{
	[DisallowMultipleComponent]
	public class ResizableUI : MonoBehaviour, IDragHandler, IEventSystemHandler
	{
		[SerializeField]
		private RectTransform _resizeRoot;

		[SerializeField]
		private Canvas _resizeCanvas;

		[SerializeField]
		private bool _lockInScreen = true;

		[SerializeField]
		private Vector2 _minSize;

		public void OnDrag(PointerEventData eventData)
		{
			Vector2 vector = (_resizeRoot.offsetMin + _minSize) * _resizeCanvas.scaleFactor;
			Vector2 vector2 = (_lockInScreen ? new Vector2(Screen.width, Screen.height) : new Vector2(1f / 0f, 1f / 0f));
			Vector2 delta = eventData.delta;
			Vector2 position = eventData.position;
			Vector2 vector3 = position - delta;
			Vector2 vector4 = new Vector2(Mathf.Clamp(position.x, vector.x, vector2.x), Mathf.Clamp(position.y, vector.y, vector2.y));
			Vector2 vector5 = new Vector2(Mathf.Clamp(vector3.x, vector.x, vector2.x), Mathf.Clamp(vector3.y, vector.y, vector2.y));
			Vector2 vector6 = vector4 - vector5;
			_resizeRoot.offsetMax += vector6 / _resizeCanvas.scaleFactor;
		}
	}
}
