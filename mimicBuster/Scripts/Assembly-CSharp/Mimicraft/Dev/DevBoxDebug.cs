using System.Collections.Generic;
using System.Text;
using Mimicraft.Customization;
using UnityEngine;
using UnityEngine.Rendering;

namespace Mimicraft.Dev
{
	public class DevBoxDebug : MonoBehaviour
	{
		private static DevBoxDebug instance;

		private static readonly Color DefaultBox = new Color(0.35f, 0.75f, 1f, 0.1f);

		private static readonly Color LimitBox = new Color(1f, 0.65f, 0.15f, 0.1f);

		private static readonly Color RequiredCell = new Color(1f, 0.25f, 0.3f, 0.35f);

		private readonly List<GameObject> drawn = new List<GameObject>();

		public static bool Active => instance != null;

		public static void SetActive(bool active)
		{
			if (instance != null)
			{
				Object.Destroy(instance.gameObject);
				instance = null;
			}
			if (active)
			{
				GameObject obj = new GameObject("DevBoxDebug");
				Object.DontDestroyOnLoad(obj);
				instance = obj.AddComponent<DevBoxDebug>();
			}
		}

		private void OnDestroy()
		{
			foreach (GameObject item in drawn)
			{
				if (item != null)
				{
					Object.Destroy(item);
				}
			}
			if (instance == this)
			{
				instance = null;
			}
		}

		public static string BuildAndDescribe()
		{
			if (instance == null)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			CharacterAssembler[] array = Object.FindObjectsByType<CharacterAssembler>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
			foreach (CharacterAssembler assembler in array)
			{
				num += instance.DrawCharacter(assembler, stringBuilder);
			}
			WeaponSkinAssembler[] array2 = Object.FindObjectsByType<WeaponSkinAssembler>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
			foreach (WeaponSkinAssembler weapon in array2)
			{
				num += instance.DrawWeapon(weapon, stringBuilder);
			}
			if (num == 0)
			{
				return "Cizilecek kutu bulunamadi - bir customization ekrani acik mi?";
			}
			return $"{num} kutu:\n{stringBuilder}";
		}

		private int DrawCharacter(CharacterAssembler assembler, StringBuilder text)
		{
			int num = 0;
			foreach (CharacterAssembler.BuiltPart builtPart in assembler.BuiltParts)
			{
				if (!(builtPart.Definition == null) && !(builtPart.Model == null))
				{
					Describe(text, builtPart.Definition.DisplayName, builtPart.Definition, builtPart.Model.VoxelSize);
					Draw(builtPart.Model.transform, builtPart.Definition, builtPart.Model.VoxelSize);
					num++;
				}
			}
			return num;
		}

		private int DrawWeapon(WeaponSkinAssembler weapon, StringBuilder text)
		{
			WeaponSkinBox box = weapon.Box;
			if (box == null || weapon.Model == null)
			{
				return 0;
			}
			Describe(text, weapon.name, box, box.VoxelSize);
			Draw(weapon.Model.transform, box, box.VoxelSize);
			return 1;
		}

		private static void Describe(StringBuilder text, string label, IVoxelBox box, float voxelSize)
		{
			Vector3Int boxSize = box.BoxSize;
			Vector3Int limitMin = box.LimitMin;
			Vector3Int limitSize = box.LimitSize;
			text.Append("  " + label + "\n");
			text.Append($"    varsayilan {boxSize.x}x{boxSize.y}x{boxSize.z}" + $"   sinir {limitSize.x}x{limitSize.y}x{limitSize.z} (kose {limitMin.x},{limitMin.y},{limitMin.z})" + $"   voxel {voxelSize:0.####} m\n");
			text.Append($"    metre: varsayilan {(float)boxSize.x * voxelSize:0.###}x{(float)boxSize.y * voxelSize:0.###}" + $"x{(float)boxSize.z * voxelSize:0.###}" + $"   sinir {(float)limitSize.x * voxelSize:0.###}x{(float)limitSize.y * voxelSize:0.###}" + $"x{(float)limitSize.z * voxelSize:0.###}\n");
			if (!box.HasRequiredCore)
			{
				text.Append("    zorunlu cekirdek yok - parca tamamen bosaltilabilir\n");
				return;
			}
			int num = 0;
			for (int i = limitMin.x; i < limitMin.x + limitSize.x; i++)
			{
				for (int j = limitMin.y; j < limitMin.y + limitSize.y; j++)
				{
					for (int k = limitMin.z; k < limitMin.z + limitSize.z; k++)
					{
						if (box.IsRequired(new Vector3Int(i, j, k)))
						{
							num++;
						}
					}
				}
			}
			text.Append($"    zorunlu {num} hucre " + $"({100f * (float)num / (float)Mathf.Max(1, boxSize.x * boxSize.y * boxSize.z):0.#}% varsayilan kutunun)\n");
		}

		private void Draw(Transform space, IVoxelBox box, float voxelSize)
		{
			Vector3Int boxSize = box.BoxSize;
			Vector3Int limitMin = box.LimitMin;
			Vector3Int limitSize = box.LimitSize;
			AddBox(space, Vector3.zero, boxSize, DefaultBox, "DefaultBox");
			if (limitSize != boxSize || limitMin != Vector3Int.zero)
			{
				AddBox(space, limitMin, limitSize, LimitBox, "LimitBox");
			}
			if (!box.HasRequiredCore)
			{
				return;
			}
			List<Vector3Int> list = new List<Vector3Int>();
			for (int i = limitMin.x; i < limitMin.x + limitSize.x; i++)
			{
				for (int j = limitMin.y; j < limitMin.y + limitSize.y; j++)
				{
					for (int k = limitMin.z; k < limitMin.z + limitSize.z; k++)
					{
						Vector3Int vector3Int = new Vector3Int(i, j, k);
						if (box.IsRequired(vector3Int))
						{
							list.Add(vector3Int);
						}
					}
				}
			}
			AddCells(space, list, RequiredCell, "RequiredCells");
		}

		private void AddBox(Transform space, Vector3 origin, Vector3Int size, Color colour, string name)
		{
			List<Vector3> vertices = new List<Vector3>();
			List<int> triangles = new List<int>();
			AddCube(origin, size, vertices, triangles);
			Spawn(space, name, vertices, triangles, colour);
		}

		private void AddCells(Transform space, List<Vector3Int> cells, Color colour, string name)
		{
			List<Vector3> vertices = new List<Vector3>();
			List<int> triangles = new List<int>();
			foreach (Vector3Int cell in cells)
			{
				AddCube(cell + Vector3.one * 0.06f, Vector3.one * 0.88f, vertices, triangles);
			}
			Spawn(space, name, vertices, triangles, colour);
		}

		private void Spawn(Transform space, string name, List<Vector3> vertices, List<int> triangles, Color colour)
		{
			if (vertices.Count != 0)
			{
				GameObject gameObject = new GameObject(name);
				gameObject.transform.SetParent(space, worldPositionStays: false);
				Mesh mesh = new Mesh
				{
					name = name
				};
				mesh.indexFormat = ((vertices.Count > 65000) ? IndexFormat.UInt32 : IndexFormat.UInt16);
				mesh.SetVertices(vertices);
				mesh.SetTriangles(triangles, 0);
				mesh.RecalculateBounds();
				gameObject.AddComponent<MeshFilter>().sharedMesh = mesh;
				Shader shader = Shader.Find("Mimicraft/HighlightUnlit");
				MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
				if (shader != null)
				{
					meshRenderer.sharedMaterial = new Material(shader)
					{
						color = colour
					};
				}
				drawn.Add(gameObject);
			}
		}

		private static void AddCube(Vector3 origin, Vector3 size, List<Vector3> vertices, List<int> triangles)
		{
			int count = vertices.Count;
			Vector3 vector = origin + size;
			vertices.Add(new Vector3(origin.x, origin.y, origin.z));
			vertices.Add(new Vector3(vector.x, origin.y, origin.z));
			vertices.Add(new Vector3(vector.x, vector.y, origin.z));
			vertices.Add(new Vector3(origin.x, vector.y, origin.z));
			vertices.Add(new Vector3(origin.x, origin.y, vector.z));
			vertices.Add(new Vector3(vector.x, origin.y, vector.z));
			vertices.Add(new Vector3(vector.x, vector.y, vector.z));
			vertices.Add(new Vector3(origin.x, vector.y, vector.z));
			int[] array = new int[36]
			{
				0, 2, 1, 0, 3, 2, 5, 6, 7, 5,
				7, 4, 4, 7, 3, 4, 3, 0, 1, 2,
				6, 1, 6, 5, 3, 7, 6, 3, 6, 2,
				4, 0, 1, 4, 1, 5
			};
			foreach (int num in array)
			{
				triangles.Add(count + num);
			}
		}
	}
}
