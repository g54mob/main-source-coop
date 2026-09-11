using TMPro;
using UnityEngine;

public class DistanceSFX : MonoBehaviour
{
	public SFX_Instance sfx;

	private TextMeshProUGUI tm;

	private string prev;

	private void Start()
	{
		tm = GetComponent<TextMeshProUGUI>();
	}

	private void Update()
	{
		if (prev != tm.text)
		{
			sfx.Play(base.transform.position);
		}
		prev = tm.text;
	}
}
