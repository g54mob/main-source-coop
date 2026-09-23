using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	[RequireComponent(typeof(Button))]
	public class PracticeLaunchButton : MonoBehaviour
	{
		[Tooltip("Acikken bu buton Tutorial olarak baslatir (rehber katmani acilir). Kapaliyken duz Sandbox. Iki buton ayni bileseni kullanir, sadece bu kutu farklidir.")]
		[SerializeField]
		private bool startTutorial;

		[Tooltip("Baslatilacak oyun modunun id'si - Resources/GameModes altindaki asset'in Mode Id alaniyla ayni olmali. \"practice\" = Serbest Mod / Ogretici (modelleme). \"range\" = Atis Poligonu (Avci odasi). Ikisi de menuden dogrudan baslayan, sunucu tarayicisinda gorunen siradan oturumlar - aralarindaki tek fark bu id.")]
		[SerializeField]
		private string practiceModeId = "practice";

		private void Awake()
		{
			Button component = GetComponent<Button>();
			component.onClick.RemoveAllListeners();
			component.onClick.AddListener(Launch);
		}

		private void Launch()
		{
			PracticeSession.Start(startTutorial, practiceModeId);
		}
	}
}
