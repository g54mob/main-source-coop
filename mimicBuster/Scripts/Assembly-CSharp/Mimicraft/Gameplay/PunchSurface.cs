using System;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	[Serializable]
	public class PunchSurface
	{
		[Tooltip("Sadece Inspector'daki listeyi okunur kılmak için - 'Metal', 'Tahta', 'Cam'. Kod bunu hiç kullanmıyor, eşleştirme tamamen Layers alanından yapılıyor.")]
		public string name = "";

		[Tooltip("Bu yüzeye ait katmanlar. Birden fazla seçebilirsin. Listede YUKARIDAN AŞAĞI bakılır ve eşleşen ilk kayıt kullanılır, yani aynı katmanı iki kayda koyarsan üstteki kazanır.")]
		public LayerMask layers;

		[Tooltip("Temas noktasında doğan efekt. +Z'si yüzeyden DIŞARI bakar - duvar toz efektiyle aynı yön kuralı. Boş bırakılırsa kod kendi küçük toz patlamasını kullanır.")]
		public GameObject effectPrefab;

		[Tooltip("Darbe sesleri. Birden fazla koyarsan her vuruşta rastgele biri çalar - aynı klibin art arda tekrarlanması, dinleyicinin fark ettiği tek şeydir. Boşsa ortak yumruk darbe sesi çalar.")]
		public AudioClip[] clips = new AudioClip[0];

		[Tooltip("Bu yüzeyin sesinin yüksekliği. Tok bir duvar sesi genelde bir bedene inen yumruktan (0.7) biraz daha yüksek olur.")]
		[Range(0f, 1.5f)]
		public float volume = 0.8f;

		[Tooltip("Perdenin her vuruşta ne kadar oynayacağı. Küçük bir oynama, aynı sesin üst üste duyulmasını tek bir klip gibi değil, ayrı ayrı vuruşlar gibi duyurur. 0 = hiç oynama.")]
		[Range(0f, 0.5f)]
		public float pitchJitter = 0.06f;

		public bool Matches(int layer)
		{
			return (layers.value & (1 << layer)) != 0;
		}

		public AudioClip PickClip()
		{
			if (clips == null || clips.Length == 0)
			{
				return null;
			}
			return clips[UnityEngine.Random.Range(0, clips.Length)];
		}

		public float PickPitch()
		{
			return 1f + UnityEngine.Random.Range(0f - pitchJitter, pitchJitter);
		}
	}
}
