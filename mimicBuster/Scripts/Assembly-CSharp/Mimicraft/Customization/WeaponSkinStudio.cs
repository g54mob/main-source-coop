using UnityEngine;

namespace Mimicraft.Customization
{
	public class WeaponSkinStudio : PortraitStudio, IPortraitStudio<WeaponSkinPortrait>
	{
		[Tooltip("Fotoğrafı çekilecek silahlar - bu sahneye elle koyduğun modeller, her birinin üzerinde WeaponSkinAssembler. Özelleştirme ekranındaki listeyle aynı silahlar olmalı: burada olmayan bir silahın skinine görsel çekilemez.\n\nBoş bırakılırsa bu objenin altındaki bütün WeaponSkinAssembler'lar kullanılır.")]
		[SerializeField]
		private WeaponSkinAssembler[] weapons;

		private WeaponSkinAssembler dressed;

		public static WeaponSkinStudio Instance { get; private set; }

		protected override Component Stage => dressed;

		protected override string MissingStageMessage => "Sahnede giydirilmiş bir silah yok - Weapons listesi boş olabilir.";

		protected override void Awake()
		{
			if (weapons == null || weapons.Length == 0)
			{
				weapons = GetComponentsInChildren<WeaponSkinAssembler>(includeInactive: true);
			}
			base.Awake();
			WeaponSkinAssembler[] array = weapons;
			foreach (WeaponSkinAssembler weaponSkinAssembler in array)
			{
				if (weaponSkinAssembler != null)
				{
					weaponSkinAssembler.gameObject.SetActive(value: false);
				}
			}
			Instance = this;
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}

		public void Dress(WeaponSkinPortrait portrait)
		{
			dressed = null;
			if (portrait == null || weapons == null)
			{
				return;
			}
			WeaponSkinAssembler[] array = weapons;
			foreach (WeaponSkinAssembler weaponSkinAssembler in array)
			{
				if (!(weaponSkinAssembler == null))
				{
					bool flag = weaponSkinAssembler.WeaponId == portrait.WeaponId;
					weaponSkinAssembler.gameObject.SetActive(flag);
					if (flag)
					{
						dressed = weaponSkinAssembler;
						weaponSkinAssembler.Apply(portrait.Skin ?? weaponSkinAssembler.DefaultSkin);
					}
				}
			}
			if (dressed == null)
			{
				Debug.LogWarning("[WeaponSkinStudio] '" + portrait.WeaponId + "' bu studyoda yok - Weapons listesine bu silahin bir modelini ekle. Gorsel cekilemeyecek.", this);
			}
		}
	}
}
