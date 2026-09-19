using System.Runtime.InteropServices;
using Fusion;
using UnityEngine;

namespace Features.NetworkInputModule.Scripts
{
	[StructLayout(LayoutKind.Explicit, Size = 236)]
	[NetworkInputWeaved(2882575978u, 59)]
	public struct NetworkInputActions : INetworkInput
	{
		[FieldOffset(0)]
		public NetworkButtons SpectatorLeftPhase;

		[FieldOffset(4)]
		public NetworkButtons SpectatorRightPhase;

		[FieldOffset(8)]
		public NetworkButtons OpenSettingsPhase;

		[FieldOffset(12)]
		public Vector2 MovementInput;

		[FieldOffset(20)]
		public NetworkButtons JumpPhase;

		[FieldOffset(24)]
		public Vector2 RotationInput;

		[FieldOffset(32)]
		public NetworkButtons ChangeMouseVisibilityPhase;

		[FieldOffset(36)]
		public NetworkButtons CrouchPhase;

		[FieldOffset(40)]
		public NetworkButtons SprintPhase;

		[FieldOffset(44)]
		public NetworkButtons GrabItemPhase;

		[FieldOffset(48)]
		public NetworkButtons DropAllFromArmsPhase;

		[FieldOffset(52)]
		public Vector2 ArmItemDistanceChangeInput;

		[FieldOffset(60)]
		public NetworkButtons ItemInteractPhase;

		[FieldOffset(64)]
		public NetworkButtons MouseForwardPhase;

		[FieldOffset(68)]
		public NetworkButtons BodyEmotePhase;

		[FieldOffset(72)]
		public NetworkButtons FaceEmotePhase;

		[FieldOffset(76)]
		public NetworkButtons HandEmotePhase;

		[FieldOffset(80)]
		public Vector2 EmoteNavigationInput;

		[FieldOffset(88)]
		public NetworkButtons HotBar_1Phase;

		[FieldOffset(92)]
		public NetworkButtons HotBar_2Phase;

		[FieldOffset(96)]
		public NetworkButtons HotBar_3Phase;

		[FieldOffset(100)]
		public NetworkButtons HotBar_4Phase;

		[FieldOffset(104)]
		public NetworkButtons HotBar_5Phase;

		[FieldOffset(108)]
		public NetworkButtons HotBar_6Phase;

		[FieldOffset(112)]
		public NetworkButtons HotBar_7Phase;

		[FieldOffset(116)]
		public NetworkButtons HotBar_8Phase;

		[FieldOffset(120)]
		public NetworkButtons HotBar_9Phase;

		[FieldOffset(124)]
		public NetworkButtons HotBar_0Phase;

		[FieldOffset(128)]
		public NetworkButtons TurnOnOffOverlayPhase;

		[FieldOffset(132)]
		public Vector2 AnyVectorChangeInput;

		[FieldOffset(140)]
		public NetworkButtons UIApplyPhase;

		[FieldOffset(144)]
		public NetworkButtons UIApplyWindowPhase;

		[FieldOffset(148)]
		public NetworkButtons UIBackPhase;

		[FieldOffset(152)]
		public Vector2 NavigateInput;

		[FieldOffset(160)]
		public NetworkButtons SubmitPhase;

		[FieldOffset(164)]
		public NetworkButtons CancelPhase;

		[FieldOffset(168)]
		public Vector2 PointInput;

		[FieldOffset(176)]
		public NetworkButtons ClickPhase;

		[FieldOffset(180)]
		public Vector2 ScrollWheelInput;

		[FieldOffset(188)]
		public NetworkButtons MiddleClickPhase;

		[FieldOffset(192)]
		public NetworkButtons RightClickPhase;

		[FieldOffset(196)]
		public NetworkButtons TrackedDevicePositionPhase;

		[FieldOffset(200)]
		public NetworkButtons TrackedDeviceOrientationPhase;

		[FieldOffset(204)]
		public NetworkButtons UpgradesBackPhase;

		[FieldOffset(208)]
		public NetworkButtons AdditionalNavigationLeftPhase;

		[FieldOffset(212)]
		public NetworkButtons AdditionalNavigationRightPhase;

		[FieldOffset(216)]
		public NetworkButtons ExtraAdditionalNavigationLeftPhase;

		[FieldOffset(220)]
		public NetworkButtons ExtraAdditionalNavigationRightPhase;

		[FieldOffset(224)]
		public NetworkButtons HoldApplyPhase;

		[FieldOffset(228)]
		public NetworkButtons PushToTalkPhase;

		[FieldOffset(232)]
		public NetworkButtons GameplayApplyPhase;
	}
}
