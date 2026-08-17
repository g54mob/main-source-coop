using UnityEngine;

namespace UnluckSoftware
{
	[ExecuteAlways]
	public class EnableSelectedGameObject : MonoBehaviour
	{
		[HideInInspector]
		public Transform prevSelect;

		[HideInInspector]
		public Transform prevSelectWrong;

		[Header("! Gizmos no longer required.")]
		public int particleSystems;

		public bool disableMe;

		public string ignoreTag = "^^^^";

		public bool disableOnPlay;

		public bool autoSlideShow;

		public float autoSlideShowDelay = 3f;

		public string nextKey = "n";

		public bool randomEnable;

		public bool repeat;

		private int emitOnEnable = 1;

		private int counter;

		private Transform randomEnablePlaceHolder;

		private GameObject prevRandomBird;

		private bool autoSlideShowStarted;

		private bool selfSelected;

		private void Start()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (disableOnPlay)
			{
				base.gameObject.SetActive(value: false);
				return;
			}
			if (randomEnable)
			{
				DisableAllChildren();
				randomEnablePlaceHolder = new GameObject("randomEnablePlaceHolder").transform;
			}
			if (autoSlideShow)
			{
				DisableAllChildren();
			}
		}

		private void Update()
		{
			if (!Application.isPlaying || autoSlideShowStarted || !Input.GetKeyUp(nextKey))
			{
				return;
			}
			if (autoSlideShow)
			{
				if (randomEnable)
				{
					InvokeRepeating("RandomModel", 0f, autoSlideShowDelay);
				}
				else
				{
					InvokeRepeating("NextModel", 0f, autoSlideShowDelay);
				}
				autoSlideShowStarted = true;
			}
			else if (randomEnable)
			{
				RandomModel();
			}
			else
			{
				NextModel();
			}
		}

		private void RandomModel()
		{
			if (base.transform.childCount == 0 && !repeat)
			{
				return;
			}
			if (base.transform.childCount == 0 && repeat)
			{
				while (randomEnablePlaceHolder.childCount > 0)
				{
					randomEnablePlaceHolder.GetChild(0).parent = base.transform;
				}
			}
			if (prevRandomBird != null)
			{
				prevRandomBird.SetActive(value: false);
				prevRandomBird.transform.SetParent(randomEnablePlaceHolder.transform);
			}
			if (base.transform.childCount != 0)
			{
				int index = Random.Range(0, base.transform.childCount);
				prevRandomBird = base.transform.GetChild(index).gameObject;
				prevRandomBird.SetActive(value: true);
			}
		}

		private void PlayParticles()
		{
			if ((bool)prevSelect)
			{
				ParticleSystem component = prevSelect.GetComponent<ParticleSystem>();
				if ((bool)component)
				{
					component.Clear();
					component.Play();
					component.Emit(emitOnEnable);
				}
			}
		}

		private void DisableAllChildren()
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				Transform child = base.transform.GetChild(i);
				if (child.gameObject.activeInHierarchy && child.parent == base.transform)
				{
					child.gameObject.SetActive(value: false);
				}
			}
		}

		private void NextModel()
		{
			if (base.transform.childCount != 0)
			{
				DisableAllChildren();
				base.transform.GetChild(counter % base.transform.childCount).gameObject.SetActive(value: true);
				counter++;
			}
		}

		private void CountParticles()
		{
			ParticleSystem[] componentsInChildren = base.transform.GetComponentsInChildren<ParticleSystem>(includeInactive: true);
			particleSystems = 0;
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].transform.parent != null && componentsInChildren[i].transform.parent.GetComponent<ParticleSystem>() == null)
				{
					particleSystems++;
				}
			}
		}
	}
}
