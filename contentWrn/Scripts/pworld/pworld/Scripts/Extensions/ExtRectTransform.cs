using UnityEngine;

namespace pworld.Scripts.Extensions
{
	public static class ExtRectTransform
	{
		public static void EncapsulateChildrenOf(this RectTransform me, RectTransform parent)
		{
			Vector2 vector = new Vector2(float.MinValue, float.MinValue);
			Vector2 vector2 = new Vector2(float.MaxValue, float.MaxValue);
			foreach (RectTransform item in parent)
			{
				vector.x = Mathf.Max(item.offsetMax.x, vector.x);
				vector.y = Mathf.Max(item.offsetMax.y, vector.y);
				vector2.y = Mathf.Min(item.offsetMin.y, vector2.y);
				vector2.x = Mathf.Min(item.offsetMin.x, vector2.x);
			}
			vector = parent.TransformPoint(vector);
			vector2 = parent.TransformPoint(vector2);
			me.offsetMax = me.parent.InverseTransformPoint(vector);
			me.offsetMin = me.parent.InverseTransformPoint(vector2);
		}

		public static void FitHeightOfChildren(this RectTransform me, RectTransform parent)
		{
			Vector2 vector = parent.GetChild(0).GetComponent<RectTransform>().offsetMax - parent.GetLastChild().GetComponent<RectTransform>().offsetMin;
			Vector2 sizeDelta = me.sizeDelta;
			sizeDelta.y = Mathf.Abs(vector.y);
			me.sizeDelta = sizeDelta;
		}

		public static void FitWidthOfChildren(this RectTransform me, RectTransform parent)
		{
			Vector2 vector = parent.GetChild(0).GetComponent<RectTransform>().offsetMax - parent.GetLastChild().GetComponent<RectTransform>().offsetMin;
			Vector2 sizeDelta = me.sizeDelta;
			sizeDelta.x = Mathf.Abs(vector.x);
			me.sizeDelta = sizeDelta;
		}

		public static bool Contains(this RectTransform me, Vector3 worldPositon, Camera cam = null)
		{
			cam = ((cam == null) ? Camera.main : cam);
			Vector3 vector = cam.WorldToViewportPoint(worldPositon);
			Vector3 position = me.TransformPoint(me.rect.min);
			Vector3 position2 = me.TransformPoint(me.rect.max);
			position = cam.WorldToViewportPoint(position);
			position2 = cam.WorldToViewportPoint(position2);
			if (vector.x > position.x && vector.x < position2.x && vector.y > position.y && vector.y < position2.y)
			{
				return true;
			}
			return false;
		}

		public static Vector3 PHeight(this RectTransform me)
		{
			return me.transform.TransformVector(me.rect.height * me.transform.up);
		}

		public static Vector3 PWidth(this RectTransform me)
		{
			return me.transform.TransformVector(me.rect.width * me.transform.right);
		}
	}
}
