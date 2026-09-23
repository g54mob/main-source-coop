using System;
using System.Collections.Generic;
using Mimicraft.Customization;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	public class FirstPersonArmSlots : MonoBehaviour
	{
		[Serializable]
		public class Slot
		{
			[Tooltip("Hangi vücut parçası. Asset referansı - id'yi elle yazmıyorsun, yanlış yazamıyorsun.")]
			public CharacterPartDefinition Part;

			[Tooltip("Parçanın takılacağı transform. Karşılık gelen kemiği verirsen parça karakterdeki yerine oturur; ince ayar istiyorsan kemiğin altına boş bir obje koyup onu ver ve istediğin gibi kaydır.")]
			public Transform Mount;

			[Tooltip("Bu parça geldiğinde kapatılacak varsayılan renderer'lar - yerini aldığı kol mesh'i. Boş bırakabilirsin; o zaman hiçbir şey gizlenmez.")]
			public Renderer[] Replaces;
		}

		[Tooltip("Bu view model'in giyebileceği parçalar.")]
		[SerializeField]
		private List<Slot> slots = new List<Slot>();

		public IReadOnlyList<Slot> Slots => slots;

		public Slot Find(string partId)
		{
			if (string.IsNullOrEmpty(partId))
			{
				return null;
			}
			foreach (Slot slot in slots)
			{
				if (slot?.Part != null && slot.Mount != null && slot.Part.PartId == partId)
				{
					return slot;
				}
			}
			return null;
		}
	}
}
