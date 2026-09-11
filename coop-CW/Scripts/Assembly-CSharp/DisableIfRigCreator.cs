using UnityEngine;

public class DisableIfRigCreator : MonoBehaviour
{
	private void Update()
	{
		if ((bool)base.transform.parent && base.transform.parent.name == "RigCreator")
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
