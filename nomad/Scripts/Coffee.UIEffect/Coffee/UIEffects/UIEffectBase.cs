using System;
using Coffee.UIEffectInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace Coffee.UIEffects
{
	[ExecuteAlways]
	[DisallowMultipleComponent]
	public abstract class UIEffectBase : UIBehaviour, IMeshModifier, IMaterialModifier, ICanvasRaycastFilter, ITimeControl
	{
		private static readonly InternalObjectPool<UIEffectContext> s_ContextPool = new InternalObjectPool<UIEffectContext>(() => new UIEffectContext(), (UIEffectContext x) => true, delegate(UIEffectContext x)
		{
			x.Reset();
		});

		private Graphic _graphic;

		private Material _material;

		private UIEffectContext _context;

		private Action _onBeforeCanvasRebuild;

		private Action _onAfterCanvasRebuild;

		private bool _canModifyMesh;

		private Matrix4x4 _prevTransformHash;

		private Canvas _canvas;

		private bool _canvasCached;

		public Material effectMaterial => _material;

		public Graphic graphic
		{
			get
			{
				if (!_graphic)
				{
					return _graphic = GetComponent<Graphic>();
				}
				return _graphic;
			}
		}

		public virtual uint effectId => (uint)GetInstanceID();

		public virtual float actualSamplingScale => 1f;

		public virtual bool canModifyShape => true;

		protected Canvas canvas
		{
			get
			{
				if (_canvasCached)
				{
					return _canvas;
				}
				_canvasCached = true;
				_canvas = GetComponentInParent<Canvas>();
				if (!_canvas || !_canvas.isActiveAndEnabled)
				{
					_canvas = null;
					_canvasCached = false;
				}
				return _canvas;
			}
		}

		public virtual UIEffectContext context
		{
			get
			{
				if (_context == null)
				{
					_context = s_ContextPool.Rent();
					UpdateContext(_context);
				}
				return _context;
			}
		}

		public virtual RectTransform transitionRoot => base.transform as RectTransform;

		protected Material GetCurrentMaterial()
		{
			if (!base.isActiveAndEnabled)
			{
				return null;
			}
			Graphic graphic = this.graphic;
			if (!graphic)
			{
				return null;
			}
			CanvasRenderer canvasRenderer = graphic.canvasRenderer;
			if (!canvasRenderer || canvasRenderer.materialCount == 0)
			{
				return null;
			}
			return canvasRenderer.GetMaterial();
		}

		protected override void OnEnable()
		{
			_canModifyMesh = true;
			if (_onBeforeCanvasRebuild == null)
			{
				_onBeforeCanvasRebuild = OnBeforeCanvasRebuild;
			}
			if (_onAfterCanvasRebuild == null)
			{
				_onAfterCanvasRebuild = OnAfterCanvasRebuild;
			}
			UIExtraCallbacks.onBeforeCanvasRebuild += _onBeforeCanvasRebuild;
			UIExtraCallbacks.onAfterCanvasRebuild += _onAfterCanvasRebuild;
			UpdateContext(context);
			SetMaterialDirty();
			SetVerticesDirty();
		}

		protected override void OnDisable()
		{
			if (_onBeforeCanvasRebuild != null)
			{
				UIExtraCallbacks.onBeforeCanvasRebuild -= _onBeforeCanvasRebuild;
			}
			if (_onAfterCanvasRebuild != null)
			{
				UIExtraCallbacks.onAfterCanvasRebuild -= _onAfterCanvasRebuild;
			}
			MaterialRepository.Release(ref _material);
			SetMaterialDirty();
			SetVerticesDirty();
		}

		protected override void OnDestroy()
		{
			_onBeforeCanvasRebuild = null;
			_graphic = null;
			s_ContextPool.Return(ref _context);
		}

		private void OnBeforeCanvasRebuild()
		{
			if (!_canModifyMesh && CanModifyMesh())
			{
				_canModifyMesh = true;
				SetVerticesDirty();
			}
		}

		private void OnAfterCanvasRebuild()
		{
			if ((bool)_material && (bool)graphic && (bool)canvas && context != null && context.useViewMatrix)
			{
				context.UpdateViewMatrix(GetCurrentMaterial(), transitionRoot, canvas.rootCanvas);
			}
		}

		protected override void OnCanvasHierarchyChanged()
		{
			_canvasCached = false;
			_canvas = null;
		}

		public void ModifyMesh(Mesh mesh)
		{
		}

		public virtual void ModifyMesh(VertexHelper vh)
		{
			if (base.isActiveAndEnabled && context != null)
			{
				_canModifyMesh = CanModifyMesh();
				if (_canModifyMesh)
				{
					context.ModifyMesh(graphic, canvas, transitionRoot, vh, canModifyShape);
				}
			}
		}

		private bool CanModifyMesh()
		{
			if (!graphic || !graphic.isActiveAndEnabled)
			{
				return false;
			}
			if (!base.isActiveAndEnabled)
			{
				return false;
			}
			RectTransform rectTransform = transitionRoot;
			if (base.transform == rectTransform)
			{
				return true;
			}
			Vector3 lossyScale = rectTransform.lossyScale;
			Vector3 lossyScale2 = base.transform.lossyScale;
			return !Mathf.Approximately(lossyScale.x * lossyScale.y * lossyScale2.x * lossyScale2.y, 0f);
		}

		public virtual Material GetModifiedMaterial(Material baseMaterial)
		{
			if (baseMaterial == null || !base.isActiveAndEnabled || context == null || !context.willModifyMaterial)
			{
				MaterialRepository.Release(ref _material);
				return baseMaterial;
			}
			uint u32_ = (uint)(Mathf.InverseLerp(0.01f, 100f, actualSamplingScale) * 4.2949673E+09f);
			uint u32_2 = (transitionRoot ? ((uint)transitionRoot.GetInstanceID()) : 0u);
			Hash128 hash = new Hash128((uint)baseMaterial.GetInstanceID(), effectId, u32_, u32_2);
			if (!MaterialRepository.Valid(hash, _material))
			{
				MaterialRepository.Get(hash, ref _material, (Material x) => new Material(x)
				{
					shader = UIEffectProjectSettings.shaderRegistry.FindOptionalShader(x.shader, "(UIEffect)", "Hidden/{0} (UIEffect)", "Hidden/UI/Default (UIEffect)"),
					hideFlags = HideFlags.HideAndDontSave
				}, baseMaterial);
			}
			_material.CopyPropertiesFromMaterial(baseMaterial);
			ApplyContextToMaterial(_material);
			return _material;
		}

		protected override void OnRectTransformDimensionsChange()
		{
			if (base.isActiveAndEnabled)
			{
				SetVerticesDirty();
			}
		}

		protected override void OnDidApplyAnimationProperties()
		{
			UpdateContext(context);
			SetVerticesDirty();
			SetMaterialDirty();
		}

		public virtual void SetVerticesDirty()
		{
			if ((bool)graphic)
			{
				graphic.SetVerticesDirty();
				GraphicProxy.Find(graphic).SetVerticesDirty(graphic, base.enabled);
			}
		}

		public virtual void SetMaterialDirty()
		{
			if ((bool)graphic)
			{
				graphic.SetMaterialDirty();
			}
		}

		internal void ReleaseMaterial()
		{
			MaterialRepository.Release(ref _material);
		}

		internal abstract void UpdateContext(UIEffectContext c);

		public virtual void ApplyContextToMaterial(Material material)
		{
			if (base.isActiveAndEnabled && context != null && (bool)material)
			{
				context.ApplyToMaterial(material, actualSamplingScale);
			}
		}

		public abstract void SetRate(float rate, UIEffectTweener.CullingMask cullingMask);

		public abstract bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera);

		public void SetTime(double _)
		{
		}

		public void OnControlTimeStart()
		{
			if ((bool)this)
			{
				base.enabled = true;
			}
		}

		public void OnControlTimeStop()
		{
			if ((bool)this)
			{
				base.enabled = false;
			}
		}
	}
}
