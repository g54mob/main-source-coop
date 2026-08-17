using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BuildingBlocks : MonoBehaviour
{
	[SerializeReference]
	public List<IShape> inventory;

	[SerializeReference]
	public object bin;

	[SerializeReference]
	public List<object> bins;

	private void OnEnable()
	{
		if (inventory == null)
		{
			inventory = new List<IShape>
			{
				new Cube
				{
					size = new Vector3(1f, 1f, 1f)
				}
			};
		}
		if (bins == null)
		{
			bins = new List<object>
			{
				new Cube(),
				new Thing()
			};
		}
		_ = bin;
	}
}
