using UnityEngine;

namespace Mimicraft.Customization
{
	public class CharacterPortraitStudio : PortraitStudio, IPortraitStudio<CharacterData>
	{
		[Tooltip("Fotoğrafı çekilecek gövde - bu sahnedeki CharacterAssembler. Boş bırakılırsa bu objenin altında aranır.")]
		[SerializeField]
		private CharacterAssembler subject;

		public static CharacterPortraitStudio Instance { get; private set; }

		protected override Component Stage => subject;

		protected override string MissingStageMessage => "Subject (CharacterAssembler) yok.";

		protected override void Awake()
		{
			if (subject == null)
			{
				subject = GetComponentInChildren<CharacterAssembler>(includeInactive: true);
			}
			base.Awake();
			Instance = this;
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}

		public void Dress(CharacterData data)
		{
			if (base.IsUsable)
			{
				subject.Apply(data);
			}
		}
	}
}
