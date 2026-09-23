using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mimicraft.Export
{
	public static class FbxBinaryWriter
	{
		public sealed class Texture
		{
			public string FileName = "";

			public byte[] EmbeddedPng;
		}

		private const int Version = 7400;

		private static readonly byte[] FileId = new byte[16]
		{
			40, 179, 42, 235, 182, 36, 204, 194, 191, 200,
			176, 42, 169, 43, 252, 241
		};

		private const string CreationTime = "1970-01-01 10:00:00:000";

		private static readonly byte[] FooterId = new byte[16]
		{
			250, 188, 171, 9, 208, 200, 212, 102, 177, 118,
			251, 131, 28, 247, 38, 126
		};

		private static readonly byte[] FooterMagic = new byte[16]
		{
			248, 90, 140, 106, 222, 245, 217, 126, 236, 233,
			12, 227, 117, 143, 41, 11
		};

		private const int SentinelLength = 13;

		private const string ClassSeparator = "\0\u0001";

		public static void Write(Stream output, IReadOnlyList<ExportMesh> meshes, string materialName, Texture texture, string creator)
		{
			List<FbxNode> list = BuildDocument(meshes, materialName, texture, creator);
			using BinaryWriter binaryWriter = new BinaryWriter(output, Encoding.UTF8, leaveOpen: true);
			binaryWriter.Write(Encoding.ASCII.GetBytes("Kaydara FBX Binary  "));
			binaryWriter.Write((byte)0);
			binaryWriter.Write((byte)26);
			binaryWriter.Write((byte)0);
			binaryWriter.Write(7400u);
			for (int i = 0; i < list.Count; i++)
			{
				WriteNode(binaryWriter, list[i], isLast: false);
			}
			binaryWriter.Write(new byte[13]);
			binaryWriter.Write(FooterId);
			binaryWriter.Write(new byte[4]);
			long position = binaryWriter.BaseStream.Position;
			int num = (int)(((position + 15) & -16) - position);
			if (num == 0)
			{
				num = 16;
			}
			binaryWriter.Write(new byte[num]);
			binaryWriter.Write(7400u);
			binaryWriter.Write(new byte[120]);
			binaryWriter.Write(FooterMagic);
		}

		private static List<FbxNode> BuildDocument(IReadOnlyList<ExportMesh> meshes, string materialName, Texture texture, string creator)
		{
			long nextId = 1000000L;
			creator = (string.IsNullOrEmpty(creator) ? "Mimic Busters" : creator);
			materialName = (string.IsNullOrEmpty(materialName) ? "Material" : materialName);
			bool flag = texture != null && !string.IsNullOrEmpty(texture.FileName);
			List<FbxNode> list = new List<FbxNode>();
			DateTime now = DateTime.Now;
			FbxNode fbxNode = new FbxNode("FBXHeaderExtension");
			fbxNode.Add("FBXHeaderVersion", 1003);
			fbxNode.Add("FBXVersion", 7400);
			fbxNode.Add("EncryptionType", 0);
			FbxNode fbxNode2 = fbxNode.Add("CreationTimeStamp");
			fbxNode2.Add("Version", 1000);
			fbxNode2.Add("Year", now.Year);
			fbxNode2.Add("Month", now.Month);
			fbxNode2.Add("Day", now.Day);
			fbxNode2.Add("Hour", now.Hour);
			fbxNode2.Add("Minute", now.Minute);
			fbxNode2.Add("Second", now.Second);
			fbxNode2.Add("Millisecond", now.Millisecond);
			fbxNode.Add("Creator", creator);
			list.Add(fbxNode);
			list.Add(new FbxNode("FileId", FileId));
			list.Add(new FbxNode("CreationTime", "1970-01-01 10:00:00:000"));
			list.Add(new FbxNode("Creator", creator));
			FbxNode fbxNode3 = new FbxNode("GlobalSettings");
			fbxNode3.Add("Version", 1000);
			FbxNode fbxNode4 = fbxNode3.Add("Properties70");
			fbxNode4.P("UpAxis", "int", "Integer", "", 1);
			fbxNode4.P("UpAxisSign", "int", "Integer", "", 1);
			fbxNode4.P("FrontAxis", "int", "Integer", "", 2);
			fbxNode4.P("FrontAxisSign", "int", "Integer", "", 1);
			fbxNode4.P("CoordAxis", "int", "Integer", "", 0);
			fbxNode4.P("CoordAxisSign", "int", "Integer", "", 1);
			fbxNode4.P("OriginalUpAxis", "int", "Integer", "", -1);
			fbxNode4.P("OriginalUpAxisSign", "int", "Integer", "", 1);
			fbxNode4.P("UnitScaleFactor", "double", "Number", "", 100.0);
			fbxNode4.P("OriginalUnitScaleFactor", "double", "Number", "", 100.0);
			list.Add(fbxNode3);
			FbxNode fbxNode5 = new FbxNode("Documents");
			fbxNode5.Add("Count", 1);
			FbxNode fbxNode6 = fbxNode5.Add("Document", NewId(), "Scene", "Scene");
			fbxNode6.Add("Properties70");
			fbxNode6.Add("RootNode", 0L);
			list.Add(fbxNode5);
			list.Add(new FbxNode("References"));
			int num = (flag ? 2 : 0);
			FbxNode fbxNode7 = new FbxNode("Definitions");
			fbxNode7.Add("Version", 100);
			fbxNode7.Add("Count", 1 + meshes.Count * 2 + 1 + num);
			fbxNode7.Add("ObjectType", "GlobalSettings").Add("Count", 1);
			fbxNode7.Add("ObjectType", "Geometry").Add("Count", meshes.Count);
			fbxNode7.Add("ObjectType", "Model").Add("Count", meshes.Count);
			fbxNode7.Add("ObjectType", "Material").Add("Count", 1);
			if (flag)
			{
				fbxNode7.Add("ObjectType", "Texture").Add("Count", 1);
				fbxNode7.Add("ObjectType", "Video").Add("Count", 1);
			}
			list.Add(fbxNode7);
			FbxNode fbxNode8 = new FbxNode("Objects");
			FbxNode fbxNode9 = new FbxNode("Connections");
			long num2 = NewId();
			for (int i = 0; i < meshes.Count; i++)
			{
				ExportMesh exportMesh = meshes[i];
				string name = (string.IsNullOrEmpty(exportMesh.Name) ? $"Piece{i + 1}" : exportMesh.Name);
				long num3 = NewId();
				long num4 = NewId();
				fbxNode8.Children.Add(Geometry(exportMesh, num3, name));
				fbxNode8.Children.Add(Model(num4, name));
				fbxNode9.Add("C", "OO", num3, num4);
				fbxNode9.Add("C", "OO", num4, 0L);
				fbxNode9.Add("C", "OO", num2, num4);
			}
			fbxNode8.Children.Add(Material(num2, materialName));
			if (flag)
			{
				long num5 = NewId();
				long num6 = NewId();
				fbxNode8.Children.Add(TextureNode(num5, materialName, texture));
				fbxNode8.Children.Add(Video(num6, materialName, texture));
				fbxNode9.Add("C", "OP", num5, num2, "DiffuseColor");
				fbxNode9.Add("C", "OO", num6, num5);
			}
			list.Add(fbxNode8);
			list.Add(fbxNode9);
			FbxNode fbxNode10 = new FbxNode("Takes");
			fbxNode10.Add("Current", "");
			list.Add(fbxNode10);
			return list;
			long NewId()
			{
				return nextId++;
			}
		}

		private static FbxNode Geometry(ExportMesh mesh, long id, string name)
		{
			FbxNode fbxNode = new FbxNode("Geometry", id, name + "\0\u0001Geometry", "Mesh");
			fbxNode.Add("Properties70");
			fbxNode.Add("GeometryVersion", 124);
			double[] array = new double[mesh.Positions.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = mesh.Positions[i];
			}
			fbxNode.Add("Vertices", array);
			int[] array2 = new int[mesh.Corners.Count];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = ((j % 4 == 3) ? (~mesh.Corners[j]) : mesh.Corners[j]);
			}
			fbxNode.Add("PolygonVertexIndex", array2);
			double[] array3 = new double[mesh.Normals.Count];
			for (int k = 0; k < array3.Length; k++)
			{
				array3[k] = mesh.Normals[k];
			}
			FbxNode fbxNode2 = fbxNode.Add("LayerElementNormal", 0);
			fbxNode2.Add("Version", 101);
			fbxNode2.Add("Name", "");
			fbxNode2.Add("MappingInformationType", "ByPolygonVertex");
			fbxNode2.Add("ReferenceInformationType", "Direct");
			fbxNode2.Add("Normals", array3);
			double[] array4 = new double[mesh.Uvs.Count];
			for (int l = 0; l < array4.Length; l++)
			{
				array4[l] = mesh.Uvs[l];
			}
			int[] array5 = new int[mesh.Corners.Count];
			for (int m = 0; m < array5.Length; m++)
			{
				array5[m] = m;
			}
			FbxNode fbxNode3 = fbxNode.Add("LayerElementUV", 0);
			fbxNode3.Add("Version", 101);
			fbxNode3.Add("Name", "UVMap");
			fbxNode3.Add("MappingInformationType", "ByPolygonVertex");
			fbxNode3.Add("ReferenceInformationType", "IndexToDirect");
			fbxNode3.Add("UV", array4);
			fbxNode3.Add("UVIndex", array5);
			FbxNode fbxNode4 = fbxNode.Add("LayerElementMaterial", 0);
			fbxNode4.Add("Version", 101);
			fbxNode4.Add("Name", "");
			fbxNode4.Add("MappingInformationType", "AllSame");
			fbxNode4.Add("ReferenceInformationType", "IndexToDirect");
			fbxNode4.Add("Materials", new int[1]);
			FbxNode fbxNode5 = fbxNode.Add("Layer", 0);
			fbxNode5.Add("Version", 100);
			AddLayerElement(fbxNode5, "LayerElementNormal");
			AddLayerElement(fbxNode5, "LayerElementUV");
			AddLayerElement(fbxNode5, "LayerElementMaterial");
			return fbxNode;
		}

		private static void AddLayerElement(FbxNode layer, string type)
		{
			FbxNode fbxNode = layer.Add("LayerElement");
			fbxNode.Add("Type", type);
			fbxNode.Add("TypedIndex", 0);
		}

		private static FbxNode Model(long id, string name)
		{
			FbxNode fbxNode = new FbxNode("Model", id, name + "\0\u0001Model", "Mesh");
			fbxNode.Add("Version", 232);
			FbxNode fbxNode2 = fbxNode.Add("Properties70");
			fbxNode2.P("Lcl Translation", "Lcl Translation", "", "A", 0.0, 0.0, 0.0);
			fbxNode2.P("Lcl Rotation", "Lcl Rotation", "", "A", 0.0, 0.0, 0.0);
			fbxNode2.P("Lcl Scaling", "Lcl Scaling", "", "A", 1.0, 1.0, 1.0);
			fbxNode2.P("DefaultAttributeIndex", "int", "Integer", "", 0);
			fbxNode2.P("InheritType", "enum", "", "", 1);
			fbxNode.Add("MultiLayer", 0);
			fbxNode.Add("MultiTake", 0);
			fbxNode.Add("Shading", true);
			fbxNode.Add("Culling", "CullingOff");
			return fbxNode;
		}

		private static FbxNode Material(long id, string name)
		{
			FbxNode fbxNode = new FbxNode("Material", id, name + "\0\u0001Material", "");
			fbxNode.Add("Version", 102);
			fbxNode.Add("ShadingModel", "lambert");
			fbxNode.Add("MultiLayer", 0);
			FbxNode fbxNode2 = fbxNode.Add("Properties70");
			fbxNode2.P("DiffuseColor", "Color", "", "A", 1.0, 1.0, 1.0);
			fbxNode2.P("DiffuseFactor", "Number", "", "A", 1.0);
			fbxNode2.P("AmbientColor", "Color", "", "A", 0.0, 0.0, 0.0);
			fbxNode2.P("EmissiveColor", "Color", "", "A", 0.0, 0.0, 0.0);
			fbxNode2.P("TransparencyFactor", "Number", "", "A", 0.0);
			return fbxNode;
		}

		private static FbxNode TextureNode(long id, string name, Texture texture)
		{
			FbxNode fbxNode = new FbxNode("Texture", id, name + "\0\u0001Texture", "");
			fbxNode.Add("Type", "TextureVideoClip");
			fbxNode.Add("Version", 202);
			fbxNode.Add("TextureName", name + "\0\u0001Texture");
			fbxNode.Add("Properties70").P("UseMaterial", "bool", "", "", 1);
			fbxNode.Add("Media", name + "\0\u0001Video");
			fbxNode.Add("FileName", texture.FileName);
			fbxNode.Add("RelativeFilename", texture.FileName);
			fbxNode.Add("ModelUVTranslation", 0.0, 0.0);
			fbxNode.Add("ModelUVScaling", 1.0, 1.0);
			fbxNode.Add("Texture_Alpha_Source", "None");
			fbxNode.Add("Cropping", 0, 0, 0, 0);
			return fbxNode;
		}

		private static FbxNode Video(long id, string name, Texture texture)
		{
			FbxNode fbxNode = new FbxNode("Video", id, name + "\0\u0001Video", "Clip");
			fbxNode.Add("Type", "Clip");
			fbxNode.Add("Properties70").P("Path", "KString", "XRefUrl", "", texture.FileName);
			fbxNode.Add("UseMipMap", 0);
			fbxNode.Add("Filename", texture.FileName);
			fbxNode.Add("RelativeFilename", texture.FileName);
			if (texture.EmbeddedPng != null && texture.EmbeddedPng.Length != 0)
			{
				fbxNode.Add("Content", texture.EmbeddedPng);
			}
			return fbxNode;
		}

		private static void WriteNode(BinaryWriter writer, FbxNode node, bool isLast)
		{
			Stream baseStream = writer.BaseStream;
			long position = baseStream.Position;
			writer.Write(0u);
			writer.Write((uint)node.Properties.Count);
			writer.Write(0u);
			byte[] bytes = Encoding.ASCII.GetBytes(node.Name);
			writer.Write((byte)bytes.Length);
			writer.Write(bytes);
			long position2 = baseStream.Position;
			foreach (object property in node.Properties)
			{
				WriteProperty(writer, property);
			}
			long position3 = baseStream.Position;
			if (node.Children.Count > 0)
			{
				for (int i = 0; i < node.Children.Count; i++)
				{
					WriteNode(writer, node.Children[i], i == node.Children.Count - 1);
				}
				writer.Write(new byte[13]);
			}
			else if (node.Properties.Count == 0 && !isLast)
			{
				writer.Write(new byte[13]);
			}
			long position4 = baseStream.Position;
			baseStream.Position = position;
			writer.Write((uint)position4);
			baseStream.Position = position + 8;
			writer.Write((uint)(position3 - position2));
			baseStream.Position = position4;
		}

		private static void WriteProperty(BinaryWriter writer, object value)
		{
			if (!(value is bool flag))
			{
				if (!(value is int value2))
				{
					if (!(value is long value3))
					{
						if (!(value is float value4))
						{
							if (!(value is double value5))
							{
								if (!(value is string s))
								{
									if (!(value is byte[] array))
									{
										if (!(value is double[] array2))
										{
											if (!(value is int[] array3))
											{
												throw new NotSupportedException("No FBX property type for " + (value?.GetType().Name ?? "null") + ".");
											}
											WriteArrayHeader(writer, 'i', array3.Length, 4);
											int[] array4 = array3;
											foreach (int value6 in array4)
											{
												writer.Write(value6);
											}
										}
										else
										{
											WriteArrayHeader(writer, 'd', array2.Length, 8);
											double[] array5 = array2;
											foreach (double value7 in array5)
											{
												writer.Write(value7);
											}
										}
									}
									else
									{
										writer.Write((byte)82);
										writer.Write((uint)array.Length);
										writer.Write(array);
									}
								}
								else
								{
									byte[] bytes = Encoding.UTF8.GetBytes(s);
									writer.Write((byte)83);
									writer.Write((uint)bytes.Length);
									writer.Write(bytes);
								}
							}
							else
							{
								writer.Write((byte)68);
								writer.Write(value5);
							}
						}
						else
						{
							writer.Write((byte)70);
							writer.Write(value4);
						}
					}
					else
					{
						writer.Write((byte)76);
						writer.Write(value3);
					}
				}
				else
				{
					writer.Write((byte)73);
					writer.Write(value2);
				}
			}
			else
			{
				writer.Write((byte)67);
				writer.Write((byte)(flag ? 1u : 0u));
			}
		}

		private static void WriteArrayHeader(BinaryWriter writer, char type, int length, int itemSize)
		{
			writer.Write((byte)type);
			writer.Write((uint)length);
			writer.Write(0u);
			writer.Write((uint)(length * itemSize));
		}
	}
}
