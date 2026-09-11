using System.Collections;
using UnityEngine;
using pworld.Scripts.Extensions;

public class SetLayerAfterTime : MonoBehaviour
{
	public float seconds = 0.5f;

	public int layer;

	private IEnumerator Start()
	{
		yield return null;
		yield return new WaitForSeconds(seconds);
		base.gameObject.SetLayer(layer, includeChildren: true);
	}
}
