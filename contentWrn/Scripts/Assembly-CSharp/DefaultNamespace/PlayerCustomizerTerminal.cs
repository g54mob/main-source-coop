using UnityEngine;

namespace DefaultNamespace
{
	public class PlayerCustomizerTerminal : Interactable
	{
		private PlayerCustomizer playerCustomizer_gp;

		protected override void Awake()
		{
			base.Awake();
			playerCustomizer_gp = GetComponentInParent<PlayerCustomizer>();
			base.gameObject.layer = LayerMask.NameToLayer("Interactable");
		}

		private void Start()
		{
			hoverText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Interact);
		}

		public override bool IsValid(Player player)
		{
			return !playerCustomizer_gp.HasPlayerInTerminal;
		}

		public override void Interact(Player player)
		{
			Debug.Log("Tried to interacted with terminal");
			if (IsValid(player))
			{
				playerCustomizer_gp.EnterTerminal(player.refs.view);
			}
		}
	}
}
