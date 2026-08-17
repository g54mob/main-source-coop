using System;
using Den.Tools;
using Den.Tools.GUI;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.ObjectsGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Objects/Initial", name = "Get by Tag", iconName = "GeneratorIcons/Position", disengageable = true, colorType = typeof(TransitionsList), helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/ObjectsGenerators/GetByTag")]
	public class GetByTag211 : Generator, IOutlet<TransitionsList>, IUnit, IPrepare
	{
		public string tag;

		[Val("Add.Margin", "Advanced")]
		public float additionalMargins;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Objects\\Runtime\\ObjectsInitial.cs", 191);
		}

		public void Prepare(TileData data, Terrain terrain)
		{
			GameObject[] src = GameObject.FindGameObjectsWithTag(tag);
			Vector3[] item = src.Select((GameObject p) => p.transform.position);
			Quaternion[] item2 = src.Select((GameObject p) => p.transform.rotation);
			Vector3[] item3 = src.Select((GameObject p) => p.transform.localScale);
			(Vector3[], Quaternion[], Vector3[]) tuple = (item, item2, item3);
			data.StorePrepare(id, tuple);
		}

		public override void Generate(TileData data, StopToken stop)
		{
			if (!enabled)
			{
				return;
			}
			Vector3 vector = (Vector3)data.area.full.worldPos - new Vector3(additionalMargins, 0f, additionalMargins);
			Vector3 vector2 = (Vector3)data.area.full.worldSize + new Vector3(additionalMargins * 2f, 0f, additionalMargins * 2f);
			Vector3 vector3 = vector;
			Vector3 vector4 = vector + vector2;
			TransitionsList transitionsList = new TransitionsList();
			(Vector3[], Quaternion[], Vector3[]) obj = ((Vector3[], Quaternion[], Vector3[]))data.ReadPrepare(id);
			Vector3[] item = obj.Item1;
			Quaternion[] item2 = obj.Item2;
			Vector3[] item3 = obj.Item3;
			for (int i = 0; i < item.Length; i++)
			{
				Vector3 vector5 = item[i];
				if (vector5.x > vector3.x && vector5.z > vector3.z && vector5.x < vector4.x && vector5.z < vector4.z)
				{
					Transition trs = new Transition(vector5.x, vector5.z);
					trs.rotation = item2[i];
					trs.scale = item3[i];
					transitionsList.Add(trs);
				}
			}
			data.StoreProduct(this, transitionsList);
		}
	}
}
