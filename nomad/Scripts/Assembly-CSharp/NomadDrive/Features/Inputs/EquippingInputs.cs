using EvilCore.Inputs;
using Rewired;

namespace NomadDrive.Features.Inputs
{
	public static class EquippingInputs
	{
		private const string HeldItemPrimaryUse = "HeldItemPrimaryUse";

		private const string HeldItemSecondaryUse = "HeldItemSecondaryUse";

		private const string ThrowObject = "ThrowObject";

		private const string EnterPlacementMode = "EnterPlacementMode";

		private const string LiquidTransfer = "LiquidTransfer";

		private static Rewired.Player Player => InputHelper<DrivingInputsMarker>.Player;

		static EquippingInputs()
		{
			InputHelper<EquippingInputsMarker>.SetMapId(4);
		}

		public static void Enable()
		{
			InputHelper<EquippingInputsMarker>.EnableMap();
		}

		public static void Disable()
		{
			InputHelper<EquippingInputsMarker>.DisableMap();
		}

		public static bool IsUseObjectButton(HeldItemUseInputType type)
		{
			return Player.GetButton((type == HeldItemUseInputType.Primary) ? "HeldItemPrimaryUse" : "HeldItemSecondaryUse");
		}

		public static bool IsUseObjectButtonDown(HeldItemUseInputType type)
		{
			return Player.GetButtonDown((type == HeldItemUseInputType.Primary) ? "HeldItemPrimaryUse" : "HeldItemSecondaryUse");
		}

		public static bool IsUseObjectButtonUp(HeldItemUseInputType type)
		{
			return Player.GetButtonUp((type == HeldItemUseInputType.Primary) ? "HeldItemPrimaryUse" : "HeldItemSecondaryUse");
		}

		public static bool IsThrowObjectButtonDown()
		{
			return Player.GetButtonDown("ThrowObject");
		}

		public static bool IsThrowObjectButtonUp()
		{
			return Player.GetButtonUp("ThrowObject");
		}

		public static bool IsEnterPlacementModeButtonDown()
		{
			return Player.GetButtonDown("EnterPlacementMode");
		}

		public static bool IsLiquidTransferButton()
		{
			return Player.GetButton("LiquidTransfer");
		}

		public static bool IsLiquidTransferButtonDown()
		{
			return Player.GetButtonDown("LiquidTransfer");
		}

		public static bool IsLiquidTransferButtonUp()
		{
			return Player.GetButtonUp("LiquidTransfer");
		}
	}
}
