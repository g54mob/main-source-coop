using UnityEngine;

namespace pworld.Scripts.Extensions
{
	public static class ExtCamera
	{
		public static bool IsVisibleToCamera(this Camera me, Vector3 position)
		{
			Vector3 vector = me.WorldToViewportPoint(position);
			if (vector.x >= 0f && vector.y >= 0f && vector.x <= 1f && vector.y <= 1f)
			{
				return vector.z >= 0f;
			}
			return false;
		}

		public static Vector3 GetMousePosInWorld(this Camera me)
		{
			Vector3 mousePosition = Input.mousePosition;
			mousePosition.z = me.nearClipPlane;
			return me.ScreenToWorldPoint(mousePosition);
		}

		public static Vector3 ManualWorldToScreenPoint(Vector3 wp, Camera cam)
		{
			Vector4 vector = cam.projectionMatrix * cam.worldToCameraMatrix * new Vector4(wp.x, wp.y, wp.z, 1f);
			if (vector.w == 0f)
			{
				return Vector3.zero;
			}
			vector.x = (vector.x / vector.w + 1f) * 0.5f * (float)cam.pixelWidth;
			vector.y = (vector.y / vector.w + 1f) * 0.5f * (float)cam.pixelHeight;
			return new Vector3(vector.x, vector.y, wp.z);
		}

		public static Vector3 ManualWorldToScreenUV(Vector3 wp, Matrix4x4 projMat, Matrix4x4 worldToCamMat)
		{
			Vector4 vector = projMat * worldToCamMat * new Vector4(wp.x, wp.y, wp.z, 1f);
			if (vector.w == 0f)
			{
				return Vector3.zero;
			}
			vector.x = (vector.x / vector.w + 1f) * 0.5f;
			vector.y = (vector.y / vector.w + 1f) * 0.5f;
			return new Vector3(vector.x, vector.y, wp.z);
		}

		private static int CountCornersVisibleFrom(this RectTransform rectTransform, Camera camera)
		{
			Rect rect = new Rect(0f, 0f, Screen.width, Screen.height);
			Vector3[] array = new Vector3[4];
			rectTransform.GetWorldCorners(array);
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				Vector3 point = camera.WorldToScreenPoint(array[i]);
				if (rect.Contains(point))
				{
					num++;
				}
			}
			return num;
		}

		public static bool PIsFullyVisibleFrom(this RectTransform rectTransform, Camera camera)
		{
			return rectTransform.CountCornersVisibleFrom(camera) == 4;
		}

		public static bool PIsVisibleFrom(this RectTransform rectTransform, Camera camera)
		{
			return rectTransform.CountCornersVisibleFrom(camera) > 0;
		}

		public static bool Overlap(this Camera cam, RectTransform elem, RectTransform viewport = null)
		{
			Vector2 vector;
			Vector2 vector2;
			if (viewport != null)
			{
				Vector3[] array = new Vector3[4];
				viewport.GetWorldCorners(array);
				vector = cam.WorldToScreenPoint(array[0]);
				vector2 = cam.WorldToScreenPoint(array[2]);
			}
			else
			{
				vector = new Vector2(0f, 0f);
				vector2 = new Vector2(Screen.width, Screen.height);
			}
			vector += Vector2.one;
			vector2 -= Vector2.one;
			Vector3[] array2 = new Vector3[4];
			elem.GetWorldCorners(array2);
			Vector2 vector3 = cam.WorldToScreenPoint(array2[0]);
			Vector2 vector4 = cam.WorldToScreenPoint(array2[2]);
			if (vector3.x > vector2.x)
			{
				return false;
			}
			if (vector3.y > vector2.y)
			{
				return false;
			}
			if (vector4.x < vector.x)
			{
				return false;
			}
			if (vector4.y < vector.y)
			{
				return false;
			}
			return true;
		}

		public static bool Contains(this Camera cam, RectTransform elem, RectTransform viewport = null)
		{
			Vector2 vector;
			Vector2 vector2;
			if (viewport != null)
			{
				Vector3[] array = new Vector3[4];
				viewport.GetWorldCorners(array);
				vector = cam.WorldToScreenPoint(array[0]);
				vector2 = cam.WorldToScreenPoint(array[2]);
			}
			else
			{
				vector = new Vector2(0f, 0f);
				vector2 = new Vector2(Screen.width, Screen.height);
			}
			vector += Vector2.one;
			vector2 -= Vector2.one;
			Vector3[] array2 = new Vector3[4];
			elem.GetWorldCorners(array2);
			Vector2 vector3 = cam.WorldToScreenPoint(array2[0]);
			Vector2 vector4 = cam.WorldToScreenPoint(array2[2]);
			Vector2 vector5 = vector - vector3;
			Vector2 vector6 = vector2 - vector4;
			if (vector5.x < 0f && vector5.y < 0f && vector6.x > 0f && vector6.y > 0f)
			{
				return true;
			}
			return false;
		}
	}
}
