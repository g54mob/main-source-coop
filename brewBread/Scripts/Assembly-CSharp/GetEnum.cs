using UnityEngine;

public class GetEnum : MonoBehaviour
{
	[field: SerializeField]
	public UIStateMachineEvents State { get; private set; }
}
