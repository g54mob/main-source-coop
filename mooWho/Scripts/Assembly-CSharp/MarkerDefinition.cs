using UnityEngine;

[CreateAssetMenu(menuName = "Markers/Marker")]
public class MarkerDefinition : ScriptableObject
{
	public byte id;

	[Space(10f)]
	public GameObject markerLocalObj;

	[Space(10f)]
	public GameObject markerWorldObj;

	public float lifeDuration;

	public bool unlimitedLife;

	public float minFadeAmount;

	public bool fadeByDotProduct;

	public bool fadeByAim;
}
