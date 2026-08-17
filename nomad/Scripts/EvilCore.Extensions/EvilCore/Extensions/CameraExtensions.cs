using UnityEngine;
using UnityEngine.UI;

namespace EvilCore.Extensions
{
	public static class CameraExtensions
	{
		public static void SetBackgroundColorR(this Camera camera, float r)
		{
			camera.backgroundColor = camera.backgroundColor.WithR(r);
		}

		public static void SetBackgroundColorG(this Camera camera, float g)
		{
			camera.backgroundColor = camera.backgroundColor.WithG(g);
		}

		public static void SetBackgroundColorB(this Camera camera, float b)
		{
			camera.backgroundColor = camera.backgroundColor.WithB(b);
		}

		public static void SetBackgroundColorA(this Camera camera, float a)
		{
			camera.backgroundColor = camera.backgroundColor.WithA(a);
		}

		public static void SetBackgroundColorRg(this Camera camera, float r, float g)
		{
			camera.backgroundColor = camera.backgroundColor.WithRg(r, g);
		}

		public static void SetBackgroundColorRb(this Camera camera, float r, float b)
		{
			camera.backgroundColor = camera.backgroundColor.WithRb(r, b);
		}

		public static void SetBackgroundColorRa(this Camera camera, float r, float a)
		{
			camera.backgroundColor = camera.backgroundColor.WithRa(r, a);
		}

		public static void SetBackgroundColorGb(this Camera camera, float g, float b)
		{
			camera.backgroundColor = camera.backgroundColor.WithGb(g, b);
		}

		public static void SetBackgroundColorGa(this Camera camera, float g, float a)
		{
			camera.backgroundColor = camera.backgroundColor.WithGa(g, a);
		}

		public static void SetBackgroundColorBa(this Camera camera, float b, float a)
		{
			camera.backgroundColor = camera.backgroundColor.WithBa(b, a);
		}

		public static void SetBackgroundColorRGB(this Camera camera, float r, float g, float b)
		{
			camera.backgroundColor = camera.backgroundColor.WithRGB(r, g, b);
		}

		public static void SetBackgroundColorRga(this Camera camera, float r, float g, float a)
		{
			camera.backgroundColor = camera.backgroundColor.WithRga(r, g, a);
		}

		public static void SetBackgroundColorRba(this Camera camera, float r, float b, float a)
		{
			camera.backgroundColor = camera.backgroundColor.WithRba(r, b, a);
		}

		public static void SetBackgroundColorGba(this Camera camera, float g, float b, float a)
		{
			camera.backgroundColor = camera.backgroundColor.WithGba(g, b, a);
		}

		public static void SetLensShiftX(this Camera camera, float x)
		{
			camera.lensShift = camera.lensShift.WithX(x);
		}

		public static void SetLensShiftY(this Camera camera, float y)
		{
			camera.lensShift = camera.lensShift.WithY(y);
		}

		public static void SetPixelRectCenter(this Camera camera, Vector2 center)
		{
			camera.pixelRect = camera.pixelRect.WithCenter(center);
		}

		public static void SetPixelRectPosition(this Camera camera, Vector2 position)
		{
			camera.pixelRect = camera.pixelRect.WithPosition(position);
		}

		public static void SetPixelRectHeight(this Camera camera, float height)
		{
			camera.pixelRect = camera.pixelRect.WithHeight(height);
		}

		public static void SetPixelRectWidth(this Camera camera, float width)
		{
			camera.pixelRect = camera.pixelRect.WithWidth(width);
		}

		public static void SetPixelRectMax(this Camera camera, Vector2 max)
		{
			camera.pixelRect = camera.pixelRect.WithMax(max);
		}

		public static void SetPixelRectMin(this Camera camera, Vector2 min)
		{
			camera.pixelRect = camera.pixelRect.WithMin(min);
		}

		public static void SetPixelRectSize(this Camera camera, Vector2 size)
		{
			camera.pixelRect = camera.pixelRect.WithSize(size);
		}

		public static void SetPixelRectX(this Camera camera, float x)
		{
			camera.pixelRect = camera.pixelRect.WithX(x);
		}

		public static void SetPixelRectY(this Camera camera, float y)
		{
			camera.pixelRect = camera.pixelRect.WithY(y);
		}

		public static void SetPixelRectXMax(this Camera camera, float xMax)
		{
			camera.pixelRect = camera.pixelRect.WithXMax(xMax);
		}

		public static void SetPixelRectYMax(this Camera camera, float yMax)
		{
			camera.pixelRect = camera.pixelRect.WithYMax(yMax);
		}

		public static void SetPixelRectXMin(this Camera camera, float xMin)
		{
			camera.pixelRect = camera.pixelRect.WithXMin(xMin);
		}

		public static void SetPixelRectYMin(this Camera camera, float yMin)
		{
			camera.pixelRect = camera.pixelRect.WithYMin(yMin);
		}

		public static void SetRectCenter(this Camera camera, Vector2 center)
		{
			camera.rect = camera.rect.WithCenter(center);
		}

		public static void SetRectPosition(this Camera camera, Vector2 position)
		{
			camera.rect = camera.rect.WithPosition(position);
		}

		public static void SetRectHeight(this Camera camera, float height)
		{
			camera.rect = camera.rect.WithHeight(height);
		}

		public static void SetRectWidth(this Camera camera, float width)
		{
			camera.rect = camera.rect.WithWidth(width);
		}

		public static void SetRectMax(this Camera camera, Vector2 max)
		{
			camera.rect = camera.rect.WithMax(max);
		}

		public static void SetRectMin(this Camera camera, Vector2 min)
		{
			camera.rect = camera.rect.WithMin(min);
		}

		public static void SetRectSize(this Camera camera, Vector2 size)
		{
			camera.rect = camera.rect.WithSize(size);
		}

		public static void SetRectX(this Camera camera, float x)
		{
			camera.rect = camera.rect.WithX(x);
		}

		public static void SetRectY(this Camera camera, float y)
		{
			camera.rect = camera.rect.WithY(y);
		}

		public static void SetRectXMax(this Camera camera, float xMax)
		{
			camera.rect = camera.rect.WithXMax(xMax);
		}

		public static void SetRectYMax(this Camera camera, float yMax)
		{
			camera.rect = camera.rect.WithYMax(yMax);
		}

		public static void SetRectXMin(this Camera camera, float xMin)
		{
			camera.rect = camera.rect.WithXMin(xMin);
		}

		public static void SetRectYMin(this Camera camera, float yMin)
		{
			camera.rect = camera.rect.WithYMin(yMin);
		}

		public static void SetSensorSizeX(this Camera camera, float x)
		{
			camera.sensorSize = camera.sensorSize.WithX(x);
		}

		public static void SetSensorSizeY(this Camera camera, float y)
		{
			camera.sensorSize = camera.sensorSize.WithY(y);
		}

		public static void SetTransparencySortAxisX(this Camera camera, float x)
		{
			camera.transparencySortAxis = camera.transparencySortAxis.WithX(x);
		}

		public static void SetTransparencySortAxisY(this Camera camera, float y)
		{
			camera.transparencySortAxis = camera.transparencySortAxis.WithY(y);
		}

		public static void SetTransparencySortAxisZ(this Camera camera, float z)
		{
			camera.transparencySortAxis = camera.transparencySortAxis.WithZ(z);
		}

		public static void SetTransparencySortAxisXY(this Camera camera, float x, float y)
		{
			camera.transparencySortAxis = camera.transparencySortAxis.WithXY(x, y);
		}

		public static void SetTransparencySortAxisXY(this Camera camera, Vector2 vector)
		{
			camera.transparencySortAxis = camera.transparencySortAxis.WithXY(vector.x, vector.y);
		}

		public static void SetTransparencySortAxisXZ(this Camera camera, float x, float z)
		{
			camera.transparencySortAxis = camera.transparencySortAxis.WithXZ(x, z);
		}

		public static void SetTransparencySortAxisXZ(this Camera camera, Vector2 vector)
		{
			camera.transparencySortAxis = camera.transparencySortAxis.WithXZ(vector.x, vector.y);
		}

		public static void SetTransparencySortAxisYZ(this Camera camera, float y, float z)
		{
			camera.transparencySortAxis = camera.transparencySortAxis.WithYZ(y, z);
		}

		public static void SetTransparencySortAxisYZ(this Camera camera, Vector2 vector)
		{
			camera.transparencySortAxis = camera.transparencySortAxis.WithYZ(vector.x, vector.y);
		}

		public static Vector3 WorldToScreenPoint(this Camera camera, Vector3 position, CanvasScaler canvasScaler)
		{
			Vector3 result = camera.WorldToScreenPoint(position);
			float scaleFactor = canvasScaler.GetScaleFactor();
			result.x /= scaleFactor;
			result.y /= scaleFactor;
			return result;
		}
	}
}
