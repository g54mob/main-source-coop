using EvilCore.EvilPack.EvilLogger;
using NomadDrive.Features.LiquidTransferSystem;
using NomadDrive.Features.ObjectPlacement;
using UnityEngine;

namespace NomadDrive.Features.Restoration
{
	public class CleaningSnappingPlane : SnappingPlane
	{
		[Header("Water Level Tracking")]
		[SerializeField]
		[Range(-0.5f, 0.5f)]
		private float _minLocalY = 0.02f;

		[SerializeField]
		[Range(-0.5f, 0.5f)]
		private float _maxLocalY = 0.15f;

		private LiquidContainerComponent _liquidContainer;

		protected override void Awake()
		{
			base.Awake();
			_liquidContainer = GetComponentInParent<LiquidContainerComponent>();
			if (_liquidContainer == null)
			{
				EvilLogger.LogError("[CleaningSnappingPlane] No LiquidContainerComponent found on parent!", "Awake", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Restoration\\Scripts\\Networked\\CleaningSnappingPlane.cs", 22);
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (_liquidContainer != null)
			{
				_liquidContainer.OnLiquidAmountChangedEvent.AddListener(OnLiquidAmountChanged);
			}
		}

		private void OnDisable()
		{
			if (_liquidContainer != null)
			{
				_liquidContainer.OnLiquidAmountChangedEvent.RemoveListener(OnLiquidAmountChanged);
			}
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			UpdatePlaneY();
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			UpdatePlaneY();
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			UpdatePlaneY();
		}

		private void OnLiquidAmountChanged(float oldAmount, float newAmount)
		{
			UpdatePlaneY();
		}

		public void UpdatePlaneY()
		{
			if (!(_liquidContainer == null))
			{
				float y = Mathf.Lerp(_minLocalY, _maxLocalY, _liquidContainer.FillRatio);
				Vector3 localPosition = base.transform.localPosition;
				localPosition.y = y;
				base.transform.localPosition = localPosition;
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
