using UnityEngine;

namespace Mirror.Examples.PickupsDropsChilds
{
	public class EquippedBall : MonoBehaviour, IEquipped
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
				usages = 3,
				maxUsages = 3
			};
		}

		public void Use()
		{
			if (equippedItemConfig.maxUsages == 0)
			{
				Debug.Log("Ball used");
			}
			else if (equippedItemConfig.usages > 0)
			{
				Debug.Log("Ball used");
			}
			else
			{
				Debug.Log("Ball is out of uses");
			}
		}

		public void AddUsages(byte usages)
		{
			Debug.Log($"Ball added {usages} usages");
		}

		public void ResetUsages()
		{
			Debug.Log("Ball reset");
		}

		public void ResetUsages(byte usages)
		{
			Debug.Log($"Ball reset usages to {usages}");
		}
	}
}
