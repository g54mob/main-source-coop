using UnityEngine;

namespace Mirror.Examples.PickupsDropsChilds
{
	public class EquippedBat : MonoBehaviour, IEquipped
	{
		[Header("Components")]
		public Animator animator;

		public AudioSource audioSource;

		[Header("Equipped Item")]
		[SerializeField]
		private EquippedItemConfig _equippedItemConfig;

		public EquippedItemConfig equippedItemConfig
		{
			get
			{
				return _equippedItemConfig;
			}
			set
			{
				Debug.Log($"{base.transform.root.name} EquippedItemConfig set from {_equippedItemConfig} to {value}", base.gameObject);
				_equippedItemConfig = value;
			}
		}

		private void Reset()
		{
			equippedItemConfig = new EquippedItemConfig
			{
				usages = 5,
				maxUsages = 5
			};
		}

		public void Use()
		{
			if (equippedItemConfig.maxUsages == 0)
			{
				Debug.Log("Bat used");
			}
			else if (equippedItemConfig.usages > 0)
			{
				Debug.Log("Bat used");
			}
			else
			{
				Debug.Log("Bat is out of uses");
			}
		}

		public void AddUsages(byte usages)
		{
			Debug.Log($"Bat added {usages} usages");
		}

		public void ResetUsages()
		{
			Debug.Log("Bat reset");
		}

		public void ResetUsages(byte usages)
		{
			Debug.Log($"Bat reset usages to {usages}");
		}
	}
}
