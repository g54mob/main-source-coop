using Mimicraft.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.Tutorial
{
	[RequireComponent(typeof(Button))]
	public class TutorialLessonButton : MonoBehaviour
	{
		[Tooltip("Bu butonun açtığı ders.")]
		[LessonIdPopup]
		[SerializeField]
		private string lessonId;

		[Tooltip("Dersin adının yazılacağı yazı. Boş bırakılırsa butonun içindeki ilk TMP yazısı.")]
		[SerializeField]
		private TMP_Text label;

		[Tooltip("Açıksa yazıya dersin çevrilmiş adı konur (Lesson.<id>.Name) ve dil değişince güncellenir. Kendi yazını ya da kendi LocalizedText'ini kullanmak istiyorsan kapat.")]
		[SerializeField]
		private bool localizeLabel = true;

		public string LessonId => lessonId;

		public void ApplyLabel()
		{
			if (localizeLabel && !string.IsNullOrEmpty(lessonId))
			{
				TMP_Text tMP_Text = ((label != null) ? label : GetComponentInChildren<TMP_Text>(includeInactive: true));
				if (tMP_Text != null)
				{
					LocalizedText.Attach(tMP_Text, "Lesson." + lessonId + ".Name");
				}
			}
		}
	}
}
