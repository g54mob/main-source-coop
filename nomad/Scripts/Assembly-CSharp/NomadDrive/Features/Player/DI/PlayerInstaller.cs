using System;
using EvilCore.DI.Core;
using NomadDrive.Features.Attachables.UI;
using NomadDrive.Features.Consumables.UI;
using NomadDrive.Features.Planting.UI;
using NomadDrive.Features.Plates.UI;
using NomadDrive.Features.Player.UI;
using NomadDrive.Features.Restoration;
using NomadDrive.Features.Tools;
using NomadDrive.Features.Vehicle.UI;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Player.DI
{
	[Serializable]
	public class PlayerInstaller : MonoInstaller
	{
		[SerializeField]
		private PeakStatusBarDisplayer peakStatusBarDisplayerReference;

		[SerializeField]
		private ObjectInfoPanel objectInfoPanelReference;

		[SerializeField]
		private EquippedItemBatteryInfoPanel equippedItemBatteryInfoPanelReference;

		[SerializeField]
		private SprayPaintInfoPanel sprayPaintInfoPanelReference;

		[SerializeField]
		private PlantPotInfoPanel plantPotInfoPanelReference;

		[SerializeField]
		private EngineHeatInfoPanel engineHeatInfoPanelReference;

		[SerializeField]
		private OrganicFoodInfoPanel organicFoodInfoPanelReference;

		[SerializeField]
		private PlateInfoPanel plateInfoPanelReference;

		[SerializeField]
		private PlayerInfoPanelManager playerInfoPanelManagerReference;

		[SerializeField]
		private PlayerInfoPanelConfig playerInfoPanelConfig;

		public override void Install(IContainerBuilder builder)
		{
			RegisterIfNotNull(builder, peakStatusBarDisplayerReference, delegate(RegistrationBuilder c)
			{
				c.As<PeakStatusBarDisplayer>();
			});
			RegisterIfNotNull(builder, objectInfoPanelReference, delegate(RegistrationBuilder c)
			{
				c.As<ObjectInfoPanel>();
			});
			RegisterIfNotNull(builder, equippedItemBatteryInfoPanelReference);
			RegisterIfNotNull(builder, sprayPaintInfoPanelReference);
			RegisterIfNotNull(builder, plantPotInfoPanelReference);
			RegisterIfNotNull(builder, engineHeatInfoPanelReference);
			RegisterIfNotNull(builder, organicFoodInfoPanelReference);
			RegisterIfNotNull(builder, plateInfoPanelReference, delegate(RegistrationBuilder c)
			{
				c.As<PlateInfoPanel>();
			});
			RegisterIfNotNull(builder, playerInfoPanelManagerReference, delegate(RegistrationBuilder c)
			{
				c.As<PlayerInfoPanelManager>();
			});
			if (playerInfoPanelConfig != null)
			{
				builder.RegisterInstance(playerInfoPanelConfig);
			}
		}
	}
}
