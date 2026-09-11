using UnityEngine;

public class HandGizmo : MonoBehaviour
{
	private void Start()
	{
		base.transform.GetChild(0).gameObject.SetActive(value: false);
	}
}
