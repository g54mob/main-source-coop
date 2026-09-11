using UnityEngine;

namespace pworld.Scripts
{
	public class PAwake : MonoBehaviour
	{
		private void Awake()
		{
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		private void OnDestroy()
		{
		}

		public void FakeAwake()
		{
			MonoBehaviour[] components = GetComponents<MonoBehaviour>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].SendMessage("Awake");
			}
		}
	}
}
