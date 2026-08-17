using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace NWH.Common.Demo
{
	[RequireComponent(typeof(Text))]
	public class DemoCameraNameDisplay : MonoBehaviour
	{
		private Text cameraText;

		private void Awake()
		{
			cameraText = GetComponent<Text>();
			StartCoroutine(CameraNameCoroutine());
		}

		private IEnumerator CameraNameCoroutine()
		{
			while (true)
			{
				Camera main = Camera.main;
				if (main != null)
				{
					cameraText.text = main.name;
				}
				else
				{
					cameraText.text = "[no main camera]";
				}
				yield return new WaitForSeconds(0.1f);
			}
		}

		private void OnDestroy()
		{
			StopAllCoroutines();
		}
	}
}
