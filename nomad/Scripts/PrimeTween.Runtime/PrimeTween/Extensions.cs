using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace PrimeTween
{
	internal static class Extensions
	{
		internal static float calcDelta(this float val, TweenAnimation.ValueWrapper prevVal)
		{
			return val - prevVal.single;
		}

		internal static Color calcDelta(this Color val, TweenAnimation.ValueWrapper prevVal)
		{
			return val - prevVal.color;
		}

		internal static Color WithAlpha(this Color c, float alpha)
		{
			c.a = alpha;
			return c;
		}

		internal static TweenAnimation.ValueWrapper ToContainer(this float f)
		{
			return new TweenAnimation.ValueWrapper
			{
				single = f
			};
		}

		internal static TweenAnimation.ValueWrapper ToContainer(this Vector2 v)
		{
			return new TweenAnimation.ValueWrapper
			{
				vector2 = v
			};
		}

		internal static TweenAnimation.ValueWrapper ToContainer(this Vector3 v)
		{
			return new TweenAnimation.ValueWrapper
			{
				vector3 = v
			};
		}

		internal static TweenAnimation.ValueWrapper ToContainer(this Vector4 v)
		{
			return new TweenAnimation.ValueWrapper
			{
				vector4 = v
			};
		}

		internal static TweenAnimation.ValueWrapper ToContainer(this Color c)
		{
			return new TweenAnimation.ValueWrapper
			{
				color = c
			};
		}

		internal static TweenAnimation.ValueWrapper ToContainer(this Quaternion q)
		{
			return new TweenAnimation.ValueWrapper
			{
				quaternion = q
			};
		}

		internal static TweenAnimation.ValueWrapper ToContainer(this Rect r)
		{
			return new TweenAnimation.ValueWrapper
			{
				rect = r
			};
		}

		internal static Vector2 WithComponent(this Vector2 v, int index, float val)
		{
			v[index] = val;
			return v;
		}

		internal static Vector3 WithComponent(this Vector3 v, int index, float val)
		{
			v[index] = val;
			return v;
		}

		internal static Vector2 GetFlexibleSize(this LayoutElement target)
		{
			return new Vector2(target.flexibleWidth, target.flexibleHeight);
		}

		internal static void SetFlexibleSize(this LayoutElement target, Vector2 vector2)
		{
			target.flexibleWidth = vector2.x;
			target.flexibleHeight = vector2.y;
		}

		internal static Vector2 GetMinSize(this LayoutElement target)
		{
			return new Vector2(target.minWidth, target.minHeight);
		}

		internal static void SetMinSize(this LayoutElement target, Vector2 vector2)
		{
			target.minWidth = vector2.x;
			target.minHeight = vector2.y;
		}

		internal static Vector2 GetPreferredSize(this LayoutElement target)
		{
			return new Vector2(target.preferredWidth, target.preferredHeight);
		}

		internal static void SetPreferredSize(this LayoutElement target, Vector2 vector2)
		{
			target.preferredWidth = vector2.x;
			target.preferredHeight = vector2.y;
		}

		internal static Vector2 GetNormalizedPosition(this ScrollRect target)
		{
			return new Vector2(target.horizontalNormalizedPosition, target.verticalNormalizedPosition);
		}

		internal static void SetNormalizedPosition(this ScrollRect target, Vector2 vector2)
		{
			target.horizontalNormalizedPosition = vector2.x;
			target.verticalNormalizedPosition = vector2.y;
		}

		internal static Vector2 GetTopLeft(this VisualElement e)
		{
			IResolvedStyle resolvedStyle = e.resolvedStyle;
			return new Vector2(resolvedStyle.left, resolvedStyle.top);
		}

		internal static void SetTopLeft(this VisualElement e, Vector2 c)
		{
			IStyle style = e.style;
			style.left = c.x;
			style.top = c.y;
		}

		internal static Rect GetResolvedStyleRect(this VisualElement e)
		{
			IResolvedStyle resolvedStyle = e.resolvedStyle;
			return new Rect(resolvedStyle.left, resolvedStyle.top, resolvedStyle.width, resolvedStyle.height);
		}

		internal static void SetStyleRect(this VisualElement e, Rect c)
		{
			IStyle style = e.style;
			style.left = c.x;
			style.top = c.y;
			style.width = c.width;
			style.height = c.height;
		}
	}
}
