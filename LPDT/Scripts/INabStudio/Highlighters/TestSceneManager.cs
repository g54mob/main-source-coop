using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Highlighters
{
	public class TestSceneManager : MonoBehaviour
	{
		public List<GameObject> objectsToShow = new List<GameObject>();

		public Text text;

		private int currentObjectActiveID;

		private void Start()
		{
			foreach (GameObject item in objectsToShow)
			{
				item.SetActive(value: false);
			}
			if (objectsToShow.Count > 0)
			{
				objectsToShow[0].SetActive(value: true);
			}
		}

		private void Update()
		{
			if (objectsToShow.Count >= 1)
			{
				if (text != null)
				{
					text.text = currentObjectActiveID + 1 + " / " + objectsToShow.Count;
				}
				if (Input.GetKeyDown(KeyCode.LeftArrow))
				{
					Previous();
				}
				if (Input.GetKeyDown(KeyCode.RightArrow))
				{
					Next();
				}
			}
		}

		public void Next()
		{
			foreach (GameObject item in objectsToShow)
			{
				item.SetActive(value: false);
			}
			currentObjectActiveID++;
			currentObjectActiveID %= objectsToShow.Count;
			objectsToShow[currentObjectActiveID].SetActive(value: true);
		}

		public void Previous()
		{
			foreach (GameObject item in objectsToShow)
			{
				item.SetActive(value: false);
			}
			currentObjectActiveID--;
			if (currentObjectActiveID < 0)
			{
				currentObjectActiveID = objectsToShow.Count - 1;
			}
			objectsToShow[currentObjectActiveID].SetActive(value: true);
		}
	}
}
