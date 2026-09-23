using Mimicraft.Localization;
using Mimicraft.Networking;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	public class DuelDoor : MonoBehaviour
	{
		[Tooltip("Hangi kapı. Queue = avcı odasındaki, sıraya giren kapı. Exit = arenadaki, düellodan çıkaran kapı.")]
		[SerializeField]
		private DuelDoorKind kind;

		[Tooltip("Bakılınca yanan parça. Boş bırakılırsa bu objenin Renderer'ı kullanılır.")]
		[SerializeField]
		private Renderer highlight;

		[Tooltip("Bakılırken uygulanan renk.")]
		[SerializeField]
		private Color highlightColor = new Color(1f, 0.85f, 0.3f);

		private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

		private MaterialPropertyBlock block;

		private Color restColor;

		private bool hasRestColor;

		public DuelDoorKind Kind => kind;

		private static RoundManager Round => GameModeController.Current as RoundManager;

		public bool IsUsable
		{
			get
			{
				RoundManager round = Round;
				if (round == null || round.CurrentPhase.Value != RoundPhase.Prep || round.LocalRole != PlayerRole.Hunter)
				{
					return false;
				}
				if (kind != DuelDoorKind.Queue)
				{
					return round.LocalDuelState != DuelState.None;
				}
				return true;
			}
		}

		public string Prompt
		{
			get
			{
				RoundManager round = Round;
				DuelState duelState = ((round != null) ? round.LocalDuelState : DuelState.None);
				if (kind == DuelDoorKind.Exit)
				{
					return Loc.Get((duelState == DuelState.Fighting) ? "Duel.Cancel" : "Duel.Leave");
				}
				return Loc.Get((duelState == DuelState.None) ? "Duel.Join" : "Duel.Leave");
			}
		}

		public void Interact()
		{
			RoundManager round = Round;
			if (!(round == null))
			{
				bool flag = kind == DuelDoorKind.Queue && round.LocalDuelState == DuelState.None;
				round.RequestDuel(flag);
			}
		}

		private void OnEnable()
		{
			if (highlight == null)
			{
				highlight = GetComponent<Renderer>();
			}
		}

		private void OnDisable()
		{
			SetHighlighted(highlighted: false);
		}

		public void SetHighlighted(bool highlighted)
		{
			if (!(highlight == null))
			{
				if (block == null)
				{
					block = new MaterialPropertyBlock();
				}
				if (!hasRestColor)
				{
					hasRestColor = true;
					restColor = ((highlight.sharedMaterial != null && highlight.sharedMaterial.HasProperty(BaseColor)) ? highlight.sharedMaterial.GetColor(BaseColor) : Color.white);
				}
				highlight.GetPropertyBlock(block);
				block.SetColor(BaseColor, highlighted ? highlightColor : restColor);
				highlight.SetPropertyBlock(block);
			}
		}
	}
}
