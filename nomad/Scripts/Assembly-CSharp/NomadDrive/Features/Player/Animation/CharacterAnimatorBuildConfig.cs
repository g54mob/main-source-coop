using System;
using System.Collections.Generic;
using NomadDrive.Features.Player.PlayerStateMachine;
using UnityEngine;

namespace NomadDrive.Features.Player.Animation
{
	[CreateAssetMenu(fileName = "CharacterAnimatorBuildConfig", menuName = "NomadDrive/Player/Character Animator Build Config")]
	public class CharacterAnimatorBuildConfig : ScriptableObject
	{
		public enum ClipMode
		{
			Single = 0,
			BlendTree2D = 1
		}

		[Serializable]
		public class BaseStateEntry
		{
			public PlayerState state;

			public ClipMode mode;

			public bool isDefaultState;

			public AnimationClip singleClip;

			public AnimationClip forwardClip;

			public AnimationClip backwardClip;

			public AnimationClip leftClip;

			public AnimationClip rightClip;

			public AnimationClip forwardLeftClip;

			public AnimationClip forwardRightClip;

			public AnimationClip backwardLeftClip;

			public AnimationClip backwardRightClip;

			[Tooltip("Optional avatar mask. When set, this entry's state is built on a dedicated additional layer that respects the mask, and is NOT added to the base layer. Only the bones inside the mask animate from this state; unmasked bones continue whatever the base layer is playing.")]
			public AvatarMask layerMask;

			public float transitionDuration = 0.15f;

			public float playbackSpeed = 1f;
		}

		public string controllerOutputPath = "Assets/_Project/Features/Player/Animations/CharacterAnimatorController.controller";

		public GameObject playerPrefab;

		public AvatarMask upperBodyMask;

		public List<BaseStateEntry> baseStates = new List<BaseStateEntry>();

		public AnimationClip pickUpClip;

		public float pickUpEnterDuration = 0.1f;

		[Range(0f, 1f)]
		public float pickUpExitTime = 0.9f;

		public float pickUpExitDuration = 0.15f;

		public float emoteEnterDuration = 0.2f;

		public float emoteExitDuration = 0.2f;
	}
}
