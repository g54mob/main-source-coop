using EvilCore.EvilPack.EvilLogger;
using EvilCore.UI.Scripts;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.LiquidTransferSystem.UI;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.LiquidTransferSystem
{
	[RequireComponent(typeof(LiquidContainerComponent))]
	public class HeldItemLiquidContainer : HeldItem
	{
		[Inject]
		protected LiquidContainerInfoPanel LiquidContainerInfoPanel;

		[Inject]
		private IGameUIManager _guiManager;

		private LiquidSnapTarget _snapTarget;

		[field: SerializeField]
		public LiquidContainerComponent LiquidContainer { get; set; }

		protected override void Awake()
		{
			base.Awake();
			LiquidContainer = GetComponent<LiquidContainerComponent>();
			_snapTarget = GetComponent<LiquidSnapTarget>();
			if (LiquidContainer == null)
			{
				EvilLogger.LogError("LiquidContainerComponent is missing on " + base.gameObject.name, "Awake", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\LiquidTransferSystem\\Scripts\\Core\\HeldItemLiquidContainer.cs", 27);
			}
		}

		protected override void OnHovered()
		{
			base.OnHovered();
			LiquidContainerInfoPanel.OnLiquidContainerHovered(LiquidContainer);
			_snapTarget?.SetSnap(active: true);
		}

		protected override void OnUnhovered()
		{
			base.OnUnhovered();
			LiquidContainerInfoPanel.OnLiquidContainerHovered(null);
			_snapTarget?.SetSnap(active: false);
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			LiquidContainer?.SetLateJoinCompleted();
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
