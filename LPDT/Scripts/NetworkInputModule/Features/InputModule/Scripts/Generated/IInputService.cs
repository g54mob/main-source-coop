using RSG.Muffin.InputSubmodule.InputModule.Core.Scripts;

namespace Features.InputModule.Scripts.Generated
{
	public interface IInputService
	{
		InputDefaultActions SpectatorLeft { get; set; }

		InputDefaultActions SpectatorRight { get; set; }

		InputDefaultActions OpenSettings { get; set; }

		InputVector2Actions Movement { get; set; }

		InputDefaultActions Jump { get; set; }

		InputVector2Actions Rotation { get; set; }

		InputDefaultActions ChangeMouseVisibility { get; set; }

		InputDefaultActions Crouch { get; set; }

		InputDefaultActions Sprint { get; set; }

		InputDefaultActions GrabItem { get; set; }

		InputDefaultActions DropAllFromArms { get; set; }

		InputVector2Actions ArmItemDistanceChange { get; set; }

		InputDefaultActions ItemInteract { get; set; }

		InputDefaultActions MouseForward { get; set; }

		InputDefaultActions MouseBack { get; set; }

		InputDefaultActions BodyEmote { get; set; }

		InputDefaultActions FaceEmote { get; set; }

		InputDefaultActions HandEmote { get; set; }

		InputVector2Actions EmoteNavigation { get; set; }

		InputDefaultActions HotBar_1 { get; set; }

		InputDefaultActions HotBar_2 { get; set; }

		InputDefaultActions HotBar_3 { get; set; }

		InputDefaultActions HotBar_4 { get; set; }

		InputDefaultActions HotBar_5 { get; set; }

		InputDefaultActions HotBar_6 { get; set; }

		InputDefaultActions HotBar_7 { get; set; }

		InputDefaultActions HotBar_8 { get; set; }

		InputDefaultActions HotBar_9 { get; set; }

		InputDefaultActions HotBar_0 { get; set; }

		InputDefaultActions TurnOnOffOverlay { get; set; }

		InputVector2Actions AnyVectorChange { get; set; }

		InputDefaultActions UIApply { get; set; }

		InputDefaultActions UIApplyWindow { get; set; }

		InputDefaultActions UIBack { get; set; }

		InputVector2Actions Navigate { get; set; }

		InputDefaultActions Submit { get; set; }

		InputDefaultActions Cancel { get; set; }

		InputVector2Actions Point { get; set; }

		InputDefaultActions Click { get; set; }

		InputVector2Actions ScrollWheel { get; set; }

		InputDefaultActions MiddleClick { get; set; }

		InputDefaultActions RightClick { get; set; }

		InputDefaultActions TrackedDevicePosition { get; set; }

		InputDefaultActions TrackedDeviceOrientation { get; set; }

		InputDefaultActions UpgradesBack { get; set; }

		InputDefaultActions AdditionalNavigationLeft { get; set; }

		InputDefaultActions AdditionalNavigationRight { get; set; }

		InputDefaultActions ExtraAdditionalNavigationLeft { get; set; }

		InputDefaultActions ExtraAdditionalNavigationRight { get; set; }

		InputDefaultActions HoldApply { get; set; }

		InputDefaultActions PushToTalk { get; set; }

		InputDefaultActions GameplayApply { get; set; }

		void Enable();

		void Disable();

		void EnableUI();

		void DisableUI();

		void EnableMovementMap();

		void DisableMovementMap();

		void EnableArmMap();

		void DisableArmMap();

		void EnableEmotions();

		void DisableEmotions();

		void EnableHotBar();

		void DisableHotBar();

		void EnableDebugMap();

		void DisableDebugMap();

		void EnableAnyVectorValueMap();

		void DisableAnyVectorValueMap();

		void EnableUIMap();

		void DisableUIMap();

		void EnableVoice();

		void DisableVoice();

		void EnableGameplayInteraction();

		void DisableGameplayInteraction();
	}
}
