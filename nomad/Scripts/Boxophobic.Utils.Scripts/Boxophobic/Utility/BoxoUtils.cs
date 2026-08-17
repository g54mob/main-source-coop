using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Rendering;

namespace Boxophobic.Utility
{
	public static class BoxoUtils
	{
		public class BoxoGlobals
		{
			public static string userFolder = "Assets/BOXOPHOBIC+";
		}

		[Serializable]
		public class ProjectData
		{
			public string pipeline = "";

			public string minimum = "";

			public string package = "";

			public bool isSupported = true;

			public bool isTechRelease;

			public bool isAlphaOrBetaRelease;
		}

		public static ProjectData GetProjectData()
		{
			ProjectData projectData = new ProjectData();
			string text = "Standard";
			if (GraphicsSettings.defaultRenderPipeline != null)
			{
				if (GraphicsSettings.defaultRenderPipeline.GetType().ToString().Contains("Universal"))
				{
					text = "Universal";
				}
				if (GraphicsSettings.defaultRenderPipeline.GetType().ToString().Contains("HD"))
				{
					text = "High Definition";
				}
			}
			if (QualitySettings.renderPipeline != null)
			{
				if (QualitySettings.renderPipeline.GetType().ToString().Contains("Universal"))
				{
					text = "Universal";
				}
				if (QualitySettings.renderPipeline.GetType().ToString().Contains("HD"))
				{
					text = "High Definition";
				}
			}
			projectData.pipeline = text;
			string unityVersion = Application.unityVersion;
			if (unityVersion.Contains("a") || unityVersion.Contains("b"))
			{
				projectData.isAlphaOrBetaRelease = true;
			}
			unityVersion = unityVersion.Replace("f", "x").Replace("a", "x").Replace("b", "x");
			if (text != "Standard")
			{
				string[] array = unityVersion.Split(".");
				int num = int.Parse(array[0], CultureInfo.InvariantCulture);
				int num2 = int.Parse(array[1], CultureInfo.InvariantCulture);
				int num3 = int.Parse(array[2].Split("x")[0], CultureInfo.InvariantCulture);
				if (num == 2022)
				{
					int num4 = int.Parse("2022.3.18".Split(".")[2], CultureInfo.InvariantCulture);
					if (num2 != 3)
					{
						projectData.isSupported = false;
					}
					else if (num3 < num4)
					{
						projectData.isSupported = false;
					}
					projectData.package = "2022.3+";
				}
				if (num == 6000)
				{
					if (num2 == 0)
					{
						int num5 = int.Parse("6000.0.23".Split(".")[2], CultureInfo.InvariantCulture);
						if (num3 < num5)
						{
							projectData.isSupported = false;
						}
						projectData.package = "6000.0+";
					}
					if (num2 == 1)
					{
						int num6 = int.Parse("6000.1.0".Split(".")[2], CultureInfo.InvariantCulture);
						if (num3 < num6)
						{
							projectData.isSupported = false;
						}
						projectData.isTechRelease = true;
						projectData.package = "6000.1+";
					}
					if (num2 == 2)
					{
						int num7 = int.Parse("6000.2.0".Split(".")[2], CultureInfo.InvariantCulture);
						if (num3 < num7)
						{
							projectData.isSupported = false;
						}
						projectData.isTechRelease = true;
						projectData.package = "6000.2+";
					}
					if (num2 == 3)
					{
						int num8 = int.Parse("6000.2.0".Split(".")[2], CultureInfo.InvariantCulture);
						if (num3 < num8)
						{
							projectData.isSupported = false;
						}
						projectData.package = "6000.3+";
					}
					if (num2 == 4)
					{
						int num9 = int.Parse("6000.2.0".Split(".")[2], CultureInfo.InvariantCulture);
						if (num3 < num9)
						{
							projectData.isSupported = false;
						}
						projectData.isTechRelease = true;
						projectData.package = "6000.4+";
					}
				}
				string minimum = "2021.3.35";
				if (num == 2022)
				{
					minimum = "2022.3.18";
				}
				if (num == 6000)
				{
					minimum = "6000.0.23";
				}
				if (num == 6001)
				{
					minimum = "6000.1.0";
				}
				if (num == 6002)
				{
					minimum = "6000.2.0";
				}
				if (num == 6003)
				{
					minimum = "6000.3.0";
				}
				if (num == 6004)
				{
					minimum = "6000.4.0";
				}
				projectData.minimum = minimum;
			}
			return projectData;
		}

		public static string GetProjectPipeline()
		{
			string result = "Standard";
			if (GraphicsSettings.defaultRenderPipeline != null)
			{
				if (GraphicsSettings.defaultRenderPipeline.GetType().ToString().Contains("Universal"))
				{
					result = "Universal";
				}
				if (GraphicsSettings.defaultRenderPipeline.GetType().ToString().Contains("HD"))
				{
					result = "High Definition";
				}
			}
			if (QualitySettings.renderPipeline != null)
			{
				if (QualitySettings.renderPipeline.GetType().ToString().Contains("Universal"))
				{
					result = "Universal";
				}
				if (QualitySettings.renderPipeline.GetType().ToString().Contains("HD"))
				{
					result = "High Definition";
				}
			}
			return result;
		}

		public static void SetMaterialBool(Material material, string valueProp, string internalProp)
		{
			if (material.HasProperty(valueProp))
			{
				if (material.GetFloat(valueProp) > 0f)
				{
					material.SetInt(internalProp, 1);
				}
				else
				{
					material.SetInt(internalProp, 0);
				}
			}
		}

		public static void SetMaterialInt(Material material, string valueProp, string internalProp)
		{
			if (material.HasProperty(valueProp))
			{
				material.SetInt(internalProp, material.GetInt(valueProp));
			}
		}

		public static void SetMaterialFloat(Material material, string valueProp, string internalProp)
		{
			if (material.HasProperty(valueProp))
			{
				material.SetFloat(internalProp, material.GetFloat(valueProp));
			}
		}

		public static void SetMaterialVector(Material material, string valueProp, string internalProp)
		{
			if (material.HasProperty(valueProp))
			{
				material.SetVector(internalProp, material.GetVector(valueProp));
			}
		}

		public static void SetMaterialTexture(Material material, string valueProp, string internalProp)
		{
			if (material.HasProperty(valueProp))
			{
				material.SetTexture(internalProp, material.GetTexture(valueProp));
			}
		}

		public static void SetMaterialCoords(Material material, string modeProp, string valueProp, string internalProp)
		{
			if (material.HasProperty(modeProp) && material.HasProperty(valueProp))
			{
				int num = material.GetInt(modeProp);
				Vector4 vector = material.GetVector(valueProp);
				switch (num)
				{
				case 0:
					material.SetVector(internalProp, vector);
					break;
				case 1:
					material.SetVector(internalProp, new Vector4(1f / vector.x, 1f / vector.y, vector.z, vector.w));
					break;
				}
			}
		}

		public static void SetMaterialBounds(Material material, string modeProp, string valueProp, string internalProp)
		{
			float num = 0f;
			if (material.HasProperty(modeProp) && material.HasProperty(valueProp))
			{
				if (material.GetInt(modeProp) == 1)
				{
					num = 0.5f;
				}
				Vector4 vector = material.GetVector(valueProp);
				Vector2 vector2 = new Vector2(1f / vector.z, 1f / vector.w);
				Vector2 vector3 = new Vector2(vector.x * vector2.x - num, vector.y * vector2.y - num) * -1f;
				material.SetVector(internalProp, new Vector4(vector2.x, vector2.y, vector3.x, vector3.y));
			}
		}

		public static void SetMaterialOptions(Material material, string modeProp, string valueProp)
		{
			if (material.HasProperty(modeProp))
			{
				switch (material.GetInt(modeProp))
				{
				case 0:
					material.SetVector(valueProp, new Vector4(1f, 0f, 0f, 0f));
					break;
				case 1:
					material.SetVector(valueProp, new Vector4(0f, 1f, 0f, 0f));
					break;
				case 2:
					material.SetVector(valueProp, new Vector4(0f, 0f, 1f, 0f));
					break;
				case 3:
					material.SetVector(valueProp, new Vector4(0f, 0f, 0f, 1f));
					break;
				}
			}
		}

		public static void SetMaterialOptions(Material material, string modeProp, string valuePropA, string valuePropB)
		{
			if (material.HasProperty(modeProp))
			{
				switch (material.GetInt(modeProp))
				{
				case 0:
					material.SetVector(valuePropA, new Vector4(1f, 0f, 0f, 0f));
					material.SetVector(valuePropB, Vector4.zero);
					break;
				case 1:
					material.SetVector(valuePropA, new Vector4(0f, 1f, 0f, 0f));
					material.SetVector(valuePropB, Vector4.zero);
					break;
				case 2:
					material.SetVector(valuePropA, new Vector4(0f, 0f, 1f, 0f));
					material.SetVector(valuePropB, Vector4.zero);
					break;
				case 3:
					material.SetVector(valuePropA, new Vector4(0f, 0f, 0f, 1f));
					material.SetVector(valuePropB, Vector4.zero);
					break;
				case 4:
					material.SetVector(valuePropA, Vector4.zero);
					material.SetVector(valuePropB, new Vector4(1f, 0f, 0f, 0f));
					break;
				case 5:
					material.SetVector(valuePropA, Vector4.zero);
					material.SetVector(valuePropB, new Vector4(0f, 1f, 0f, 0f));
					break;
				case 6:
					material.SetVector(valuePropA, Vector4.zero);
					material.SetVector(valuePropB, new Vector4(0f, 0f, 1f, 0f));
					break;
				case 7:
					material.SetVector(valuePropA, Vector4.zero);
					material.SetVector(valuePropB, new Vector4(0f, 0f, 0f, 1f));
					break;
				}
			}
		}

		public static void SetMaterialBackface(Material material, string modeProp, string valueProp)
		{
			if (material.HasProperty(modeProp))
			{
				switch (material.GetInt(modeProp))
				{
				case 0:
					material.SetVector(valueProp, new Vector4(1f, 1f, 1f, 0f));
					break;
				case 1:
					material.SetVector(valueProp, new Vector4(-1f, -1f, -1f, 0f));
					break;
				case 2:
					material.SetVector(valueProp, new Vector4(1f, 1f, -1f, 0f));
					break;
				}
			}
		}

		public static void SetMaterialBackfaceLegacy(Material material, string modeProp, string valueProp)
		{
			if (material.HasProperty(modeProp))
			{
				switch (material.GetInt(modeProp))
				{
				case 0:
					material.SetVector(valueProp, new Vector4(-1f, -1f, -1f, 0f));
					break;
				case 1:
					material.SetVector(valueProp, new Vector4(1f, 1f, -1f, 0f));
					break;
				case 2:
					material.SetVector(valueProp, new Vector4(1f, 1f, 1f, 0f));
					break;
				}
			}
		}

		public static void SetMaterialReciprocal(Material material, string valueProp)
		{
			if (material.HasProperty(valueProp))
			{
				Vector4 vector = material.GetVector(valueProp);
				material.SetVector(valueProp, new Vector4(vector.x, vector.y, 1f / (vector.y - vector.x), vector.w));
			}
		}

		public static void SetMaterialKeyword(Material material, string keyword, bool enable)
		{
			if (enable)
			{
				material.EnableKeyword(keyword);
			}
			else
			{
				material.DisableKeyword(keyword);
			}
		}

		public static void SetMaterialKeyword(Material material, string property, string keyword)
		{
			if (material.HasFloat(property))
			{
				if (material.GetFloat(property) == 0f)
				{
					material.DisableKeyword(keyword);
				}
				else
				{
					material.EnableKeyword(keyword);
				}
			}
		}

		public static void SetMaterialKeyword(Material material, string property, string[] keywords)
		{
			if (!material.HasFloat(property))
			{
				return;
			}
			float num = material.GetFloat(property);
			for (int i = 0; i < keywords.Length; i++)
			{
				if ((float)i == num)
				{
					material.EnableKeyword(keywords[i]);
				}
				else
				{
					material.DisableKeyword(keywords[i]);
				}
			}
		}

		public static void SetMaterialKeyword(Material material, string parent, string property, string keyword)
		{
			if (!material.HasFloat(parent) || !material.HasFloat(property))
			{
				return;
			}
			if (material.GetFloat(parent) > 0f)
			{
				if (material.GetFloat(property) == 0f)
				{
					material.DisableKeyword(keyword);
				}
				else
				{
					material.EnableKeyword(keyword);
				}
			}
			else
			{
				material.DisableKeyword(keyword);
			}
		}

		public static void SetMaterialKeyword(Material material, string parent, string property, string[] keywords)
		{
			if (!material.HasFloat(parent) || !material.HasFloat(property))
			{
				return;
			}
			if (material.GetFloat(parent) > 0f)
			{
				float num = material.GetFloat(property);
				for (int i = 0; i < keywords.Length; i++)
				{
					if ((float)i == num)
					{
						material.EnableKeyword(keywords[i]);
					}
					else
					{
						material.DisableKeyword(keywords[i]);
					}
				}
			}
			else
			{
				for (int j = 0; j < keywords.Length; j++)
				{
					material.DisableKeyword(keywords[j]);
				}
			}
		}

		public static void SetMaterialKeyword(Material material, bool allParentsOn, string[] parents, string property, string keyword)
		{
			bool flag = false;
			float num = 0f;
			if (allParentsOn)
			{
				int num2 = 0;
				foreach (string name in parents)
				{
					if (material.HasProperty(name) && material.GetFloat(name) > 0f)
					{
						num2++;
					}
				}
				if (parents.Length == num2)
				{
					flag = true;
				}
			}
			else
			{
				float num3 = 0f;
				foreach (string name2 in parents)
				{
					if (material.HasProperty(name2))
					{
						num3 += material.GetFloat(name2);
					}
				}
				if (num3 > 0f)
				{
					flag = true;
				}
			}
			if (material.HasProperty(property))
			{
				num = material.GetFloat(property);
			}
			if (flag && num > 0f)
			{
				material.EnableKeyword(keyword);
			}
			else
			{
				material.DisableKeyword(keyword);
			}
		}

		public static void SetMaterialKeyword(Material material, bool allParentsOn, string[] parents, string property, string[] keywords)
		{
			bool flag = false;
			if (allParentsOn)
			{
				int num = 0;
				foreach (string name in parents)
				{
					if (material.HasProperty(name) && material.GetFloat(name) > 0f)
					{
						num++;
					}
				}
				if (parents.Length == num)
				{
					flag = true;
				}
			}
			else
			{
				float num2 = 0f;
				foreach (string name2 in parents)
				{
					if (material.HasProperty(name2))
					{
						num2 += material.GetFloat(name2);
					}
				}
				if (num2 > 0f)
				{
					flag = true;
				}
			}
			if (!material.HasFloat(property))
			{
				return;
			}
			if (flag)
			{
				int num3 = material.GetInt(property);
				for (int k = 0; k < keywords.Length; k++)
				{
					if (k == num3)
					{
						material.EnableKeyword(keywords[k]);
					}
					else
					{
						material.DisableKeyword(keywords[k]);
					}
				}
			}
			else
			{
				for (int l = 0; l < keywords.Length; l++)
				{
					material.DisableKeyword(keywords[l]);
				}
			}
		}

		public static void SetMaterialKeyword(Material material, bool allParentsOn, string[] properties, string keyword)
		{
			bool flag = false;
			if (allParentsOn)
			{
				int num = 0;
				foreach (string name in properties)
				{
					if (material.HasProperty(name) && material.GetFloat(name) > 0f)
					{
						num++;
					}
				}
				if (properties.Length == num)
				{
					flag = true;
				}
			}
			else
			{
				float num2 = 0f;
				foreach (string name2 in properties)
				{
					if (material.HasProperty(name2))
					{
						num2 += material.GetFloat(name2);
					}
				}
				if (num2 > 0f)
				{
					flag = true;
				}
			}
			if (flag)
			{
				material.EnableKeyword(keyword);
			}
			else
			{
				material.DisableKeyword(keyword);
			}
		}

		public static void SetMaterialKeywordInverted(Material material, string property, string keyword)
		{
			if (material.HasFloat(property))
			{
				if (material.GetFloat(property) == 0f)
				{
					material.EnableKeyword(keyword);
				}
				else
				{
					material.DisableKeyword(keyword);
				}
			}
		}

		public static void SetMaterialKeywordByTexture(Material material, string property, string keyword)
		{
			if (IsMaterialTextureUsed(material, property))
			{
				material.EnableKeyword(keyword);
			}
			else
			{
				material.DisableKeyword(keyword);
			}
		}

		public static float GetMaterialFloat(Material material, string property, float defaultValue)
		{
			float result = defaultValue;
			if (material.HasFloat(property))
			{
				result = material.GetFloat(property);
			}
			return result;
		}

		public static float GetMaterialFloat(Material material, string property)
		{
			return GetMaterialFloat(material, property, 0f);
		}

		public static int GetMaterialInt(Material material, string property, int defaultValue)
		{
			int result = defaultValue;
			if (material.HasFloat(property))
			{
				result = material.GetInt(property);
			}
			return result;
		}

		public static int GetMaterialInt(Material material, string property)
		{
			return GetMaterialInt(material, property, 0);
		}

		public static Texture GetMaterialTexture(Material material, string property)
		{
			Texture result = null;
			if (material.HasTexture(property))
			{
				result = material.GetTexture(property);
			}
			return result;
		}

		public static bool IsMaterialTextureUsed(Material material, string property)
		{
			bool result = false;
			if (material.HasTexture(property) && material.GetTexture(property) != null)
			{
				result = true;
			}
			return result;
		}

		public static float MathRemap(float value, float minOld, float maxOld, float minNew, float maxNew)
		{
			return minNew + (value - minOld) * (maxNew - minNew) / (maxOld - minOld);
		}

		public static float MathRemap(float value, float minOld, float maxOld)
		{
			return (value - minOld) / (maxOld - minOld);
		}

		public static float MathVector2ToFloat(float x, float y)
		{
			Vector2 vector = default(Vector2);
			vector.x = Mathf.Floor(x * 2047f);
			vector.y = Mathf.Floor(y * 2047f);
			return vector.x * 2048f + vector.y;
		}

		public static Vector2 MathFloatFromVector2(float input)
		{
			Vector2 vector = default(Vector2);
			vector.y = input % 2048f;
			vector.x = Mathf.Floor(input / 2048f);
			return vector / 2047f;
		}

		public static void DestryObject(UnityEngine.Object objectToDestory)
		{
			UnityEngine.Object.Destroy(objectToDestory);
		}

		public static void DisableServerExecution()
		{
		}
	}
}
