using UnityEngine;

namespace Coffee.UIEffects
{
	public class UIEffectReplica : UIEffectBase, ISerializationCallbackReceiver
	{
		[SerializeField]
		private UIEffect m_Target;

		[SerializeField]
		private UIEffectPreset m_Preset;

		[SerializeField]
		private bool m_UseTargetTransform = true;

		[SerializeField]
		protected float m_SamplingScale = 1f;

		[SerializeField]
		protected bool m_AllowToModifyMeshShape = true;

		[SerializeField]
		protected RectTransform m_CustomRoot;

		private UIEffect _currentTarget;

		public UIEffect target
		{
			get
			{
				return m_Target;
			}
			set
			{
				if (!(m_Target == value))
				{
					m_Target = value;
					m_Preset = null;
					RefreshTarget(m_Target);
					SetVerticesDirty();
					SetMaterialDirty();
				}
			}
		}

		public UIEffectPreset preset
		{
			get
			{
				return m_Preset;
			}
			set
			{
				if (!(m_Preset == value))
				{
					m_Preset = value;
					RefreshTarget(null);
					SetVerticesDirty();
					SetMaterialDirty();
				}
			}
		}

		public bool useTargetTransform
		{
			get
			{
				return m_UseTargetTransform;
			}
			set
			{
				if (m_UseTargetTransform != value)
				{
					m_UseTargetTransform = value;
					SetVerticesDirty();
				}
			}
		}

		public RectTransform customRoot
		{
			get
			{
				return m_CustomRoot;
			}
			set
			{
				if (!(m_CustomRoot == value))
				{
					m_CustomRoot = value;
					SetVerticesDirty();
				}
			}
		}

		public float samplingScale
		{
			get
			{
				return m_SamplingScale;
			}
			set
			{
				value = Mathf.Clamp(value, 0.01f, 100f);
				if (!Mathf.Approximately(m_SamplingScale, value))
				{
					m_SamplingScale = value;
					SetMaterialDirty();
				}
			}
		}

		public bool allowToModifyMeshShape
		{
			get
			{
				return m_AllowToModifyMeshShape;
			}
			set
			{
				if (m_AllowToModifyMeshShape != value)
				{
					m_AllowToModifyMeshShape = value;
					SetVerticesDirty();
				}
			}
		}

		public override float actualSamplingScale => Mathf.Clamp(m_SamplingScale, 0.01f, 100f);

		public override bool canModifyShape => m_AllowToModifyMeshShape;

		public override uint effectId
		{
			get
			{
				if (!target)
				{
					if (!preset)
					{
						return (uint)GetInstanceID();
					}
					return (uint)preset.GetInstanceID();
				}
				return target.effectId;
			}
		}

		public override UIEffectContext context
		{
			get
			{
				if ((bool)preset || ((bool)target && !isTargetInScene))
				{
					return base.context;
				}
				if (!target || !target.isActiveAndEnabled || !isTargetInScene)
				{
					return null;
				}
				return target.context;
			}
		}

		public override RectTransform transitionRoot
		{
			get
			{
				if (useTargetTransform)
				{
					if ((bool)preset && !m_CustomRoot && (bool)base.graphic && (bool)base.canvas)
					{
						return base.canvas.transform as RectTransform;
					}
					if (isTargetInScene)
					{
						return target.transitionRoot;
					}
				}
				if (!m_CustomRoot)
				{
					return base.transform as RectTransform;
				}
				return m_CustomRoot;
			}
		}

		private bool isTargetInScene
		{
			get
			{
				if ((bool)target)
				{
					return target.gameObject.scene.IsValid();
				}
				return false;
			}
		}

		protected override void OnEnable()
		{
			if (!preset)
			{
				RefreshTarget(target);
			}
			base.OnEnable();
		}

		protected override void OnDisable()
		{
			RefreshTarget(null);
			base.OnDisable();
		}

		protected override void OnDestroy()
		{
			_currentTarget = null;
			base.OnDestroy();
		}

		private void RefreshTarget(UIEffect newTarget)
		{
			if (_currentTarget == newTarget)
			{
				return;
			}
			if ((bool)_currentTarget)
			{
				_currentTarget.replicas.Remove(this);
			}
			if ((bool)newTarget)
			{
				_currentTarget = newTarget;
				if (isTargetInScene)
				{
					_currentTarget.replicas.Add(this);
				}
			}
			else
			{
				_currentTarget = null;
			}
		}

		internal override void UpdateContext(UIEffectContext dst)
		{
			if ((bool)preset)
			{
				preset.UpdateContext(dst);
			}
			else if ((bool)target && !isTargetInScene)
			{
				target.UpdateContext(dst);
			}
		}

		public override void ApplyContextToMaterial(Material material)
		{
			if (base.isActiveAndEnabled || (bool)preset || (bool)target)
			{
				if ((bool)preset || ((bool)target && !isTargetInScene))
				{
					base.ApplyContextToMaterial(material);
				}
				else if (isTargetInScene && target.isActiveAndEnabled)
				{
					base.ApplyContextToMaterial(material);
				}
			}
		}

		public override void SetRate(float rate, UIEffectTweener.CullingMask mask)
		{
		}

		public override bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
		{
			if (!base.isActiveAndEnabled || !isTargetInScene)
			{
				return true;
			}
			return target.IsRaycastLocationValid(sp, eventCamera);
		}

		public void OnBeforeSerialize()
		{
		}

		public void OnAfterDeserialize()
		{
			if ((bool)m_Preset)
			{
				m_Target = null;
			}
			else if ((bool)m_Target)
			{
				m_Preset = null;
			}
		}
	}
}
