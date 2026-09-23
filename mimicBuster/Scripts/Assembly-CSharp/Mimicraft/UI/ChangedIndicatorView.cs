using UnityEngine;

namespace Mimicraft.UI
{
	public class ChangedIndicatorView : MonoBehaviour
	{
		[Tooltip("Kaydedilmemiş değişiklik varken açılacak obje. Boş bırakılırsa bu objenin ÇOCUKLARI açılıp kapanır - bu bileşen kapalı bir objede kalırsa onu geri açacak kimse olmazdı.")]
		[SerializeField]
		private GameObject indicator;

		private void OnEnable()
		{
			ModelEditSession.DirtyChanged += Apply;
			Apply();
		}

		private void OnDisable()
		{
			ModelEditSession.DirtyChanged -= Apply;
		}

		private void Apply()
		{
			SetShown(ModelEditSession.HasUnsavedChanges);
		}

		private void SetShown(bool shown)
		{
			if (indicator != null && indicator != base.gameObject)
			{
				if (indicator.activeSelf != shown)
				{
					indicator.SetActive(shown);
				}
				return;
			}
			for (int i = 0; i < base.transform.childCount; i++)
			{
				GameObject gameObject = base.transform.GetChild(i).gameObject;
				if (gameObject.activeSelf != shown)
				{
					gameObject.SetActive(shown);
				}
			}
		}
	}
}
