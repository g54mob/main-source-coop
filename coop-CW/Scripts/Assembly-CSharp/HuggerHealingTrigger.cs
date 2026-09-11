using UnityEngine;

public class HuggerHealingTrigger : MonoBehaviour
{
	private ItemHugger itemHugger;

	private void Start()
	{
		itemHugger = GetComponentInParent<ItemHugger>();
		if (itemHugger == null)
		{
			Debug.LogError("ItemHugger not found in parent of HuggerHealingTrigger");
		}
	}

	private void OnTriggerStay(Collider other)
	{
		itemHugger.TriggerSaysTriggerStay(other);
	}

	private void Update()
	{
	}
}
