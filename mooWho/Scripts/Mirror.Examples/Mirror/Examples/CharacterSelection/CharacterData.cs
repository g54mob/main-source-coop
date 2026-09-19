using UnityEngine;

namespace Mirror.Examples.CharacterSelection
{
	public class CharacterData : MonoBehaviour
	{
		public GameObject[] characterPrefabs;

		public string[] characterTitles;

		public int[] characterHealths;

		public float[] characterSpeeds;

		public int[] characterAttack;

		public string[] characterAbilities;

		public static CharacterData characterDataSingleton { get; private set; }

		public void Awake()
		{
			characterDataSingleton = this;
		}
	}
}
