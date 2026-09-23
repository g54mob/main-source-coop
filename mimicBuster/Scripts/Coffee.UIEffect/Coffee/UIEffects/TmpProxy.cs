using System;
using System.Collections.Generic;
using Coffee.UIEffectInternal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Coffee.UIEffects
{
	internal sealed class TmpProxy : GraphicProxy
	{
		private static readonly Func<UIVertex, UIVertex, UIVertex, float, UIVertex> s_OnLerpVertex = null;

		private static readonly Func<UIVertex, float, UIVertex> s_OnMarkAsShadow = delegate(UIVertex vt, float s)
		{
			vt.uv2.x -= s;
			return vt;
		};

		private static readonly Func<UIVertex, Rect, UIVertex> s_OnModifyVertex = delegate(UIVertex vt, Rect uvMask)
		{
			vt.uv2 = new Vector4(uvMask.xMin, uvMask.yMin, uvMask.xMax, uvMask.yMax);
			return vt;
		};

		private static Mesh s_Mesh;

		private static readonly VertexHelper s_VertexHelper = new VertexHelper();

		private static readonly HashSet<TextMeshProUGUI> s_ChangedInstances = new HashSet<TextMeshProUGUI>();

		private static readonly HashSet<TextMeshProUGUI> s_RegisteredInstances = new HashSet<TextMeshProUGUI>();

		private static readonly Dictionary<int, float> s_SdfScaleCache = new Dictionary<int, float>();

		protected override bool IsValid(Graphic graphic)
		{
			if (graphic == null)
			{
				return false;
			}
			if (graphic is TextMeshProUGUI)
			{
				return true;
			}
			if (graphic is TMP_SubMeshUI tMP_SubMeshUI)
			{
				if (!(tMP_SubMeshUI.spriteAsset == null))
				{
					return tMP_SubMeshUI.sharedMaterial != tMP_SubMeshUI.spriteAsset.material;
				}
				return true;
			}
			return false;
		}

		public override bool IsText(Graphic graphic)
		{
			return true;
		}

		public override void OnPreModifyMesh(Graphic graphic, Canvas canvas)
		{
			UIVertexUtil.onLerpVertex = s_OnLerpVertex;
			ShadowUtil.onMarkAsShadow = s_OnMarkAsShadow;
			UIEffectContext.onModifyVertex = s_OnModifyVertex;
			if (canvas != null)
			{
				canvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord2;
			}
		}

		public override void SetVerticesDirty(Graphic graphic, bool enabled)
		{
			if (graphic is TextMeshProUGUI { isActiveAndEnabled: not false } textMeshProUGUI)
			{
				if (enabled)
				{
					s_ChangedInstances.Add(textMeshProUGUI);
				}
				else if (0 < textMeshProUGUI.textInfo?.meshInfo?.Length && 0 < textMeshProUGUI.textInfo.meshInfo[0].vertexCount)
				{
					textMeshProUGUI.UpdateVertexData();
				}
			}
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void RuntimeInitializeOnLoadMethod()
		{
			s_ChangedInstances.Clear();
			s_RegisteredInstances.Clear();
			s_SdfScaleCache.Clear();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void InitializeOnLoad()
		{
			GraphicProxy.Register(new TmpProxy());
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(delegate(UnityEngine.Object obj)
			{
				if (obj is TextMeshProUGUI { isActiveAndEnabled: not false } textMeshProUGUI && textMeshProUGUI.TryGetComponent<UIEffectBase>(out var _))
				{
					s_ChangedInstances.Add(textMeshProUGUI);
				}
			});
			UIExtraCallbacks.onBeforeCanvasRebuild += delegate
			{
				List<TextMeshProUGUI> toRelease = InternalListPool<TextMeshProUGUI>.Rent();
				foreach (TextMeshProUGUI s_RegisteredInstance in s_RegisteredInstances)
				{
					if (s_RegisteredInstance != null && s_RegisteredInstance.isActiveAndEnabled && !s_RegisteredInstance.isTextObjectScaleStatic)
					{
						int hashCode = s_RegisteredInstance.GetHashCode();
						float y = s_RegisteredInstance.transform.lossyScale.y;
						if (s_SdfScaleCache.TryGetValue(hashCode, out var value) && !Mathf.Approximately(value, y))
						{
							s_ChangedInstances.Add(s_RegisteredInstance);
						}
						s_SdfScaleCache[hashCode] = y;
					}
					else if ((object)s_RegisteredInstance != null)
					{
						toRelease.Add(s_RegisteredInstance);
					}
				}
				foreach (TextMeshProUGUI item in toRelease)
				{
					s_SdfScaleCache.Remove(item.GetHashCode());
					s_RegisteredInstances.Remove(item);
				}
				InternalListPool<TextMeshProUGUI>.Return(ref toRelease);
			};
			UIExtraCallbacks.onAfterCanvasRebuild += delegate
			{
				foreach (TextMeshProUGUI s_ChangedInstance in s_ChangedInstances)
				{
					if (!(s_ChangedInstance == null) && s_ChangedInstance.isActiveAndEnabled && s_ChangedInstance.TryGetComponent<IMeshModifier>(out var _))
					{
						s_RegisteredInstances.Add(s_ChangedInstance);
						ModifyMesh(s_ChangedInstance);
					}
				}
				s_ChangedInstances.Clear();
			};
		}

		private static void ModifyMesh(TextMeshProUGUI textMeshProUGUI)
		{
			if (s_Mesh == null)
			{
				s_Mesh = new Mesh();
				s_Mesh.MarkDynamic();
			}
			List<TMP_SubMeshUI> toRelease = InternalListPool<TMP_SubMeshUI>.Rent();
			List<IMeshModifier> toRelease2 = InternalListPool<IMeshModifier>.Rent();
			List<IMeshModifier> toRelease3 = InternalListPool<IMeshModifier>.Rent();
			textMeshProUGUI.TryGetComponent<UIEffectBase>(out var component);
			textMeshProUGUI.GetComponentsInChildren(toRelease, 1);
			textMeshProUGUI.GetComponents(toRelease2);
			for (int i = 0; i < textMeshProUGUI.textInfo.meshInfo.Length; i++)
			{
				TMP_MeshInfo tMP_MeshInfo = textMeshProUGUI.textInfo.meshInfo[i];
				if (tMP_MeshInfo.vertexCount == 0)
				{
					s_Mesh.Clear(keepVertexLayout: false);
					if (i == 0)
					{
						textMeshProUGUI.canvasRenderer.SetMesh(s_Mesh);
						continue;
					}
					TMP_SubMeshUI subMeshUI = GetSubMeshUI(toRelease, tMP_MeshInfo.material, tMP_MeshInfo.mesh, i - 1);
					if (subMeshUI != null)
					{
						subMeshUI.canvasRenderer.SetMesh(s_Mesh);
					}
					continue;
				}
				s_VertexHelper.Clear();
				tMP_MeshInfo.mesh.CopyTo(s_VertexHelper);
				if (i == 0)
				{
					foreach (IMeshModifier item in toRelease2)
					{
						item.ModifyMesh(s_VertexHelper);
					}
					s_VertexHelper.FillMesh(s_Mesh);
					textMeshProUGUI.canvasRenderer.SetMesh(s_Mesh);
					continue;
				}
				if (i - 1 >= toRelease.Count)
				{
					break;
				}
				foreach (IMeshModifier item2 in toRelease2)
				{
					if (!(item2 is UIEffectBase))
					{
						item2.ModifyMesh(s_VertexHelper);
					}
				}
				TMP_SubMeshUI subMeshUI2 = GetSubMeshUI(toRelease, tMP_MeshInfo.material, tMP_MeshInfo.mesh, i - 1);
				if (subMeshUI2 == null)
				{
					break;
				}
				if (!subMeshUI2.TryGetComponent<UIEffectReplica>(out var component2))
				{
					component2 = subMeshUI2.gameObject.AddComponent<UIEffectReplica>();
					textMeshProUGUI.RegisterDirtyMaterialCallback(delegate
					{
						if (subMeshUI2 != null)
						{
							subMeshUI2.SetMaterialDirty();
						}
					});
				}
				component2.SetTarget(component);
				subMeshUI2.GetComponents(toRelease3);
				foreach (IMeshModifier item3 in toRelease3)
				{
					item3.ModifyMesh(s_VertexHelper);
				}
				s_VertexHelper.FillMesh(s_Mesh);
				subMeshUI2.canvasRenderer.SetMesh(s_Mesh);
			}
			InternalListPool<TMP_SubMeshUI>.Return(ref toRelease);
			InternalListPool<IMeshModifier>.Return(ref toRelease2);
			InternalListPool<IMeshModifier>.Return(ref toRelease3);
			s_Mesh.Clear(keepVertexLayout: false);
		}

		private static TMP_SubMeshUI GetSubMeshUI(List<TMP_SubMeshUI> subMeshes, Material material, Mesh mesh, int start)
		{
			int count = subMeshes.Count;
			if (mesh != null)
			{
				for (int i = 0; i < count; i++)
				{
					TMP_SubMeshUI tMP_SubMeshUI = subMeshes[(i + start + count) % count];
					if (!(tMP_SubMeshUI == null) && tMP_SubMeshUI.mesh == mesh)
					{
						return tMP_SubMeshUI;
					}
				}
			}
			for (int j = 0; j < count; j++)
			{
				TMP_SubMeshUI tMP_SubMeshUI2 = subMeshes[(j + start + count) % count];
				if (!(tMP_SubMeshUI2 == null) && tMP_SubMeshUI2.sharedMaterial == material)
				{
					return tMP_SubMeshUI2;
				}
			}
			return null;
		}
	}
}
