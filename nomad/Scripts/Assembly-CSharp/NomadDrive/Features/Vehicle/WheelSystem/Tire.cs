using Ami.BroAudio;
using NomadDrive.Features.Attachables;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.WheelSystem
{
	[RequireComponent(typeof(ConditionComponent))]
	public class Tire : AttachableObject
	{
		public TireConfig tireConfig;

		[SerializeField]
		private MeshRenderer tireMeshRenderer;

		[SerializeField]
		private int rustMaterialIndex;

		public Transform rotatingTransform;

		public Vector3 rotatingOffset;

		[Header("Audio")]
		[SerializeField]
		private SoundID repairSound;

		private static readonly int RustIntensityId = Shader.PropertyToID("_RustIntensity");

		private MaterialPropertyBlock _tirePropertyBlock;

		private Quaternion _defaultModelRotation;

		public override Transform HighlightSource
		{
			get
			{
				if (!(base.ModelTransform != null))
				{
					return base.transform;
				}
				return base.ModelTransform;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			_defaultModelRotation = base.ModelTransform.localRotation;
			_tirePropertyBlock = new MaterialPropertyBlock();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			base.ConditionComponent?.OnConditionChanged.AddListener(UpdateTireMaterialByCondition);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			base.ConditionComponent?.OnConditionChanged.RemoveListener(UpdateTireMaterialByCondition);
		}

		protected override void OnDetach()
		{
			base.OnDetach();
			ResetModelTransformRotation();
		}

		protected override void OnRepair()
		{
			base.OnRepair();
			if (repairSound.IsValid())
			{
				NetworkAudioRelay?.PlayOneShot(repairSound, base.transform.position);
			}
		}

		private void UpdateTireMaterialByCondition(float _, float newCondition)
		{
			if (!(tireMeshRenderer == null))
			{
				if (_tirePropertyBlock == null)
				{
					_tirePropertyBlock = new MaterialPropertyBlock();
				}
				float num = ((base.ConditionComponent != null && base.ConditionComponent.MaxCondition > 0f) ? base.ConditionComponent.MaxCondition : 100f);
				float value = Mathf.Clamp01(1f - newCondition / num);
				tireMeshRenderer.GetPropertyBlock(_tirePropertyBlock, rustMaterialIndex);
				_tirePropertyBlock.SetFloat(RustIntensityId, value);
				tireMeshRenderer.SetPropertyBlock(_tirePropertyBlock, rustMaterialIndex);
			}
		}

		public void SetModelTransformRotation(Vector3 rotation)
		{
			base.ModelTransform.localRotation = Quaternion.Euler(rotation);
		}

		public void ResetModelTransformRotation()
		{
			base.ModelTransform.localRotation = _defaultModelRotation;
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
