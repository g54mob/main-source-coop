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
				if (!(target != null))
				{
					if (!(preset != null))
					{
						return (uint)GetHashCode();
					}
					return (uint)preset.GetHashCode();
				}
				return target.effectId;
			}
		}

		public override UIEffectContext context
		{
			get
			{
				if (preset != null || (target != null && !isTargetInScene))
				{
					return base.context;
				}
				if (!(target != null) || !target.isActiveAndEnabled || !isTargetInScene)
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
					if (preset != null && m_CustomRoot == null && base.graphic != null && base.canvas != null)
					{
						return base.canvas.transform as RectTransform;
					}
					if (isTargetInScene)
					{
						return target.transitionRoot;
					}
				}
				if (!(m_CustomRoot != null))
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
				if (target != null)
				{
					return target.gameObject.scene.IsValid();
				}
				return false;
			}
		}

		protected override void OnEnable()
		{
			if (preset == null)
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
			if (_currentTarget != null)
			{
				_currentTarget.replicas.Remove(this);
			}
			if (newTarget != null)
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
			if (preset != null)
			{
				preset.UpdateContext(dst);
			}
			else if (target != null && !isTargetInScene)
			{
				target.UpdateContext(dst);
			}
		}

		public override void ApplyContextToMaterial(Material material)
		{
			if (base.isActiveAndEnabled || !(preset == null) || !(target == null))
			{
				if (preset != null || (target != null && !isTargetInScene))
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
			if (m_Preset != null)
			{
				m_Target = null;
			}
			else if (m_Target != null)
			{
				m_Preset = null;
			}
		}

		public void SetTarget(UIEffectBase effect)
		{
			if (effect is UIEffect uIEffect)
			{
				target = uIEffect;
				preset = null;
				useTargetTransform = true;
				customRoot = null;
				samplingScale = uIEffect.samplingScale;
			}
			else if (effect is UIEffectReplica uIEffectReplica)
			{
				target = uIEffectReplica.target;
				preset = uIEffectReplica.preset;
				useTargetTransform = uIEffectReplica.useTargetTransform;
				customRoot = uIEffectReplica.customRoot;
				samplingScale = uIEffectReplica.samplingScale;
			}
			else
			{
				target = null;
				preset = null;
				useTargetTransform = true;
				customRoot = null;
				samplingScale = 1f;
			}
		}
	}
}
