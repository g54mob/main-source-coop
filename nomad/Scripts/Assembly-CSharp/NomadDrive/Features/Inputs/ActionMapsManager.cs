using EvilCore;

namespace NomadDrive.Features.Inputs
{
	public class ActionMapsManager : MonoSingleton<ActionMapsManager>, IActionMapsManager
	{
		protected override bool PersistAcrossScenes => true;

		protected override void Awake()
		{
			base.Awake();
			Init();
		}

		private void Init()
		{
			ResetToDefaultGameplayMaps();
		}

		public static void ResetToDefaultGameplayMaps()
		{
			BaseInputs.Enable();
			OnFootInputs.Enable();
			DrivingInputs.Disable();
			EquippingInputs.Disable();
			PlacingObjectInputs.Disable();
			SittingInputs.Disable();
		}

		void IActionMapsManager.EnterEquippingMode()
		{
			EnterEquippingMode();
		}

		void IActionMapsManager.ExitEquippingMode()
		{
			ExitEquippingMode();
		}

		void IActionMapsManager.EnterDrivingMode()
		{
			EnterDrivingMode();
		}

		void IActionMapsManager.ExitDrivingMode()
		{
			ExitDrivingMode();
		}

		void IActionMapsManager.EnterSittingMode()
		{
			EnterSittingMode();
		}

		void IActionMapsManager.ExitSittingMode()
		{
			ExitSittingMode();
		}

		void IActionMapsManager.EnterPlacingObjectMode()
		{
			EnterPlacingObjectMode();
		}

		void IActionMapsManager.ExitPlacingObjectMode()
		{
			ExitPlacingObjectMode();
		}

		public static void EnterEquippingMode()
		{
			EquippingInputs.Enable();
		}

		public static void ExitEquippingMode()
		{
			EquippingInputs.Disable();
		}

		public static void EnterDrivingMode()
		{
			BaseInputs.Enable();
			OnFootInputs.Disable();
			DrivingInputs.Enable();
			EquippingInputs.Disable();
			PlacingObjectInputs.Disable();
		}

		public static void ExitDrivingMode()
		{
			BaseInputs.Enable();
			OnFootInputs.Enable();
			DrivingInputs.Disable();
			EquippingInputs.Disable();
			PlacingObjectInputs.Disable();
		}

		public static void EnterSittingMode()
		{
			SittingInputs.Enable();
		}

		public static void ExitSittingMode()
		{
			SittingInputs.Disable();
		}

		public static void EnterPlacingObjectMode()
		{
			PlacingObjectInputs.Enable();
		}

		public static void ExitPlacingObjectMode()
		{
			PlacingObjectInputs.Disable();
		}

		public static void EnterDownedMode()
		{
			BaseInputs.Enable();
			OnFootInputs.Disable();
			DrivingInputs.Disable();
			EquippingInputs.Disable();
			PlacingObjectInputs.Disable();
			SittingInputs.Disable();
		}

		public static void ExitDownedMode()
		{
			BaseInputs.Enable();
			OnFootInputs.Enable();
			DrivingInputs.Disable();
			EquippingInputs.Disable();
			PlacingObjectInputs.Disable();
			SittingInputs.Disable();
		}
	}
}
