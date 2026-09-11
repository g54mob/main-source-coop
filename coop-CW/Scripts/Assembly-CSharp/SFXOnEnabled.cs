using UnityEngine;

public class SFXOnEnabled : MonoBehaviour
{
	public GameObject reference;

	public SFX_Instance onSFX;

	public SFX_Instance offSFX;

	private bool t;

	private void Update()
	{
		if ((bool)reference)
		{
			if (reference.activeInHierarchy && !t)
			{
				onSFX.Play(base.transform.position);
				t = true;
			}
			if (!reference.activeInHierarchy && t)
			{
				offSFX.Play(base.transform.position);
				t = false;
			}
		}
	}
}
