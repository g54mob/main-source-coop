using UnityEngine;
using pworld.Scripts.Extensions;

namespace pworld.Scripts
{
	public class PKeepInScreen : MonoBehaviour
	{
		private RectTransform rectT_g;

		private void Awake()
		{
			rectT_g = GetComponent<RectTransform>();
		}

		private void Start()
		{
		}

		private void Update()
		{
			Vector3[] array = new Vector3[4];
			rectT_g.GetWorldCorners(array);
			for (int i = 0; i < 4; i++)
			{
				array[i] = Camera.main.WorldToScreenPoint(array[i]);
				string me = "Corner " + i + " ";
				if (Screen.safeArea.Contains(array[i]))
				{
					me.PDebug();
				}
			}
		}

		private void OnDestroy()
		{
		}
	}
}
