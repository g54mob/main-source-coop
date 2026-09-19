using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Features.BlackjackBeachInteractableModule.Scripts
{
	[CreateAssetMenu(fileName = "BlackjackConfiguration_Default", menuName = "Configurations/Blackjack/BlackjackConfiguration")]
	public class BlackjackConfiguration : ScriptableObject
	{
		[field: Header("Prefabs")]
		[field: SerializeField]
		public NetworkPrefabRef CardPrefab { get; private set; }

		[field: Header("Card Materials")]
		[field: SerializeField]
		public Material CardBackMaterial { get; private set; }

		[field: SerializeField]
		public Material CardDissolveMaterial { get; private set; }

		[field: SerializeField]
		public List<BlackjackCardMaterialEntry> CardMaterials { get; private set; } = new List<BlackjackCardMaterialEntry>();

		[field: Header("Card Movement")]
		[field: SerializeField]
		public float CardMoveSpeed { get; private set; } = 2.5f;

		[Tooltip("How fast a held card eases yaw toward the grabbing player's table side (deg/sec).")]
		[field: SerializeField]
		public float CardYawTurnSpeedDegrees { get; private set; } = 180f;

		[Tooltip("Optional lift above the spawn point / last-3 deck visual. 0 = exact point pose.")]
		[field: SerializeField]
		public float CardSpawnHeightOffset { get; private set; }

		[field: SerializeField]
		public float CardStackHeightStep { get; private set; } = 0.004f;

		[field: SerializeField]
		public float CardOverlapRadius { get; private set; } = 0.12f;

		[field: Header("Reveal")]
		[field: SerializeField]
		public float RevealDuration { get; private set; } = 0.6f;

		[field: SerializeField]
		public float RevealJumpHeight { get; private set; } = 0.08f;

		[field: SerializeField]
		public float CardRevealFlipAngleDegrees { get; private set; } = 180f;

		[field: Header("Return To Deck")]
		[field: SerializeField]
		public float ReturnDuration { get; private set; } = 0.35f;

		[field: Header("Deck Visual")]
		[field: SerializeField]
		public float DeckVisualDissolveDuration { get; private set; } = 0.35f;

		[field: Header("Bell Button")]
		[field: SerializeField]
		public float BellButtonRestLocalY { get; private set; } = 0.1784569f;

		[field: SerializeField]
		public float BellButtonPressedLocalY { get; private set; } = 0.15f;

		[field: SerializeField]
		public float BellButtonPressDuration { get; private set; } = 0.08f;

		[field: SerializeField]
		public float BellButtonReleaseDuration { get; private set; } = 0.12f;

		public Material GetMaterial(BlackjackCardId cardId)
		{
			for (int i = 0; i < CardMaterials.Count; i++)
			{
				BlackjackCardMaterialEntry blackjackCardMaterialEntry = CardMaterials[i];
				if (blackjackCardMaterialEntry != null && blackjackCardMaterialEntry.CardId == cardId)
				{
					return blackjackCardMaterialEntry.Material;
				}
			}
			return null;
		}
	}
}
