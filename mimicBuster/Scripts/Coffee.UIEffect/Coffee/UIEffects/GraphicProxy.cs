using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Coffee.UIEffects
{
	public class GraphicProxy
	{
		private static readonly List<GraphicProxy> s_Proxies = new List<GraphicProxy>
		{
			new GraphicProxy()
		};

		private static readonly Func<UIVertex, UIVertex, UIVertex, float, UIVertex> s_OnLerpVertex = null;

		private static readonly Func<UIVertex, float, UIVertex> s_OnMarkAsShadow = delegate(UIVertex vt, float s)
		{
			vt.uv1.x -= s;
			return vt;
		};

		private static readonly Func<UIVertex, Rect, UIVertex> s_OnModifyVertex = delegate(UIVertex vt, Rect uvMask)
		{
			vt.uv1 = new Vector4(uvMask.xMin, uvMask.yMin, uvMask.xMax, uvMask.yMax);
			return vt;
		};

		public static void Register(GraphicProxy proxy)
		{
			int count = s_Proxies.Count;
			for (int i = 0; i < count; i++)
			{
				if (s_Proxies[i].GetType() == proxy.GetType())
				{
					return;
				}
			}
			s_Proxies.Add(proxy);
		}

		public static GraphicProxy Find(Graphic graphic)
		{
			if (graphic == null)
			{
				return null;
			}
			for (int num = s_Proxies.Count - 1; num >= 0; num--)
			{
				GraphicProxy graphicProxy = s_Proxies[num];
				if (graphicProxy.IsValid(graphic))
				{
					return graphicProxy;
				}
			}
			return null;
		}

		protected virtual bool IsValid(Graphic graphic)
		{
			return graphic != null;
		}

		public virtual bool IsText(Graphic graphic)
		{
			return graphic is Text;
		}

		public virtual void OnPreModifyMesh(Graphic graphic, Canvas canvas)
		{
			UIVertexUtil.onLerpVertex = s_OnLerpVertex;
			ShadowUtil.onMarkAsShadow = s_OnMarkAsShadow;
			UIEffectContext.onModifyVertex = s_OnModifyVertex;
			if (canvas != null)
			{
				canvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1;
			}
		}

		public virtual void SetVerticesDirty(Graphic graphic, bool enabled)
		{
		}

		public virtual Vector4 ModifyExpandSize(Graphic graphic, Vector4 expandSize)
		{
			return expandSize;
		}
	}
}
