using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using Rewired.Utils.Attributes;
using Rewired.Utils.Interfaces;
using Rewired.Utils.Libraries.TinyJson;
using UnityEngine;

namespace Rewired.Utils.Classes.Data
{
	[Preserve]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal sealed class SerializedObject : IEnumerable<SerializedObject.Field>, IEnumerable, IExportToXml, IExportToJson, IAddValue<object>, IAddKeyValue<string, object>
	{
		[CustomObfuscation(rename = false)]
		public enum ObjectType
		{
			[CustomObfuscation(rename = false)]
			Object = 0,
			[CustomObfuscation(rename = false)]
			List = 1
		}

		[Flags]
		[CustomObfuscation(rename = false)]
		public enum FieldOptions
		{
			[CustomObfuscation(rename = false)]
			None = 0,
			[CustomObfuscation(rename = false)]
			ExculdeFromXml = 1
		}

		private struct ysQjoUnlrACbUCKuNmYYPurbTSdW
		{
			public Type VvTjeMCwzNLIMnXeumTjjfaYHKUO;

			public object ykDdLPMhayTettlvvLMxtApaKiCX;

			public FieldOptions niLiZtIBeVjKsAgtlTaXIlYWtTqh;

			public ysQjoUnlrACbUCKuNmYYPurbTSdW(Type P_0, object P_1, FieldOptions P_2)
			{
				VvTjeMCwzNLIMnXeumTjjfaYHKUO = P_0;
				ykDdLPMhayTettlvvLMxtApaKiCX = P_1;
				niLiZtIBeVjKsAgtlTaXIlYWtTqh = P_2;
			}

			public string HsTJfTMGFLLYIqNiixWPwMfaiFFU()
			{
				return string.Concat(string.Concat("" + "type = " + ((VvTjeMCwzNLIMnXeumTjjfaYHKUO != null) ? VvTjeMCwzNLIMnXeumTjjfaYHKUO.Name : "NULL") + "\n", "value = ", (ykDdLPMhayTettlvvLMxtApaKiCX != null) ? ykDdLPMhayTettlvvLMxtApaKiCX.ToString() : "NULL", "\n"), "options = ", niLiZtIBeVjKsAgtlTaXIlYWtTqh.ToString(), "\n");
			}
		}

		[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
		[CustomObfuscation(rename = false)]
		public struct Field
		{
			public string name;

			public object value;

			public Type type;

			public FieldOptions options;

			public Field(string P_0, object P_1, Type P_2, FieldOptions P_3)
			{
				name = P_0;
				value = P_1;
				type = P_2;
				options = P_3;
			}

			public override string ToString()
			{
				return string.Concat(string.Concat(string.Concat("name = " + ((name != null) ? name : "NULL") + "\n", "value = ", (value != null) ? value.ToString() : "NULL", "\n"), "type = ", (type != null) ? type.Name : "NULL", "\n"), "options = ", options.ToString(), "\n");
			}
		}

		[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
		[CustomObfuscation(rename = false)]
		public class XmlInfo
		{
			public abstract class yNlNhbgspAXUCpVrAdsXgrpzrKKV
			{
			}

			public class JISccXeaCjmCeBJbWJlLPGJisilo : yNlNhbgspAXUCpVrAdsXgrpzrKKV
			{
				public string XCTmYYtBCCzTuZmlJPszaypSgJdS;

				public string qItGqcAiFsVuEXTeGzrduYLlUPFM;

				public string GpDRMyFZBJdjlvWjrvVYAzZnhbYW;

				public string vLOpmXQkMsPmsBAJNcXkcKfWoznZ;

				public virtual string adWmaRQSCDdxNBAbICVEtvyNXEYA()
				{
					return string.Concat(string.Concat(string.Concat("" + "prefix = " + XCTmYYtBCCzTuZmlJPszaypSgJdS + "\n", "localName = ", qItGqcAiFsVuEXTeGzrduYLlUPFM, "\n"), "ns = ", GpDRMyFZBJdjlvWjrvVYAzZnhbYW, "\n"), "value = ", vLOpmXQkMsPmsBAJNcXkcKfWoznZ, "\n");
				}
			}

			private List<yNlNhbgspAXUCpVrAdsXgrpzrKKV> isMhGVtkbfLyExXoVEywLMZgASNr;

			public List<yNlNhbgspAXUCpVrAdsXgrpzrKKV> attributes => isMhGVtkbfLyExXoVEywLMZgASNr ?? (isMhGVtkbfLyExXoVEywLMZgASNr = new List<yNlNhbgspAXUCpVrAdsXgrpzrKKV>());

			public override string ToString()
			{
				string text = "Attributes:\n";
				if (isMhGVtkbfLyExXoVEywLMZgASNr != null)
				{
					for (int i = 0; i < isMhGVtkbfLyExXoVEywLMZgASNr.Count; i++)
					{
						text = text + isMhGVtkbfLyExXoVEywLMZgASNr[i].ToString() + "\n";
					}
				}
				return text;
			}
		}

		[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
		[CustomObfuscation(rename = false)]
		public struct Enumerator : IEnumerator<Field>, IEnumerator, IDisposable
		{
			private IndexedDictionary<string, ysQjoUnlrACbUCKuNmYYPurbTSdW> fcjFnRGowamLpABnFaXkWytOeXZhb;

			private Field ohLwLzUEiIrWdiWlCMVoaxoxKevj;

			private IEnumerator<KeyValuePair<string, ysQjoUnlrACbUCKuNmYYPurbTSdW>> UtLOzqvcxtkcpCLYEFzkCaxbFcjGA;

			Field IEnumerator<Field>.Current => ohLwLzUEiIrWdiWlCMVoaxoxKevj;

			object IEnumerator.Current => ohLwLzUEiIrWdiWlCMVoaxoxKevj;

			internal Enumerator(object P_0)
			{
				fcjFnRGowamLpABnFaXkWytOeXZhb = (IndexedDictionary<string, ysQjoUnlrACbUCKuNmYYPurbTSdW>)P_0;
				ohLwLzUEiIrWdiWlCMVoaxoxKevj = default(Field);
				UtLOzqvcxtkcpCLYEFzkCaxbFcjGA = fcjFnRGowamLpABnFaXkWytOeXZhb.GetEnumerator();
			}

			public bool MoveNext()
			{
				if (!UtLOzqvcxtkcpCLYEFzkCaxbFcjGA.MoveNext())
				{
					return false;
				}
				KeyValuePair<string, ysQjoUnlrACbUCKuNmYYPurbTSdW> current = UtLOzqvcxtkcpCLYEFzkCaxbFcjGA.Current;
				ohLwLzUEiIrWdiWlCMVoaxoxKevj = new Field(current.Key, current.Value.ykDdLPMhayTettlvvLMxtApaKiCX, current.Value.VvTjeMCwzNLIMnXeumTjjfaYHKUO, current.Value.niLiZtIBeVjKsAgtlTaXIlYWtTqh);
				return true;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			public void Dispose()
			{
			}

			void IDisposable.Dispose()
			{
				//ILSpy generated this explicit interface implementation from .override directive in Dispose
				this.Dispose();
			}

			void IEnumerator.Reset()
			{
				ohLwLzUEiIrWdiWlCMVoaxoxKevj = default(Field);
				UtLOzqvcxtkcpCLYEFzkCaxbFcjGA = fcjFnRGowamLpABnFaXkWytOeXZhb.GetEnumerator();
			}
		}

		private class LGUdKudrZaaUOAfIKJsudqPyvcfiB
		{
			public class aIfarARnGAuzhtkgfJktofioRbWQ
			{
				public readonly string MLXRsaVtOMjzVqlRdnpZWwQpCGTM;

				public readonly aIfarARnGAuzhtkgfJktofioRbWQ ZmhhoVpCjcvTCzAtCnYuUOJXsdLA;

				public string WugilENpLeggaKXbdDsCsloPkZzyA;

				public Dictionary<string, string> wiJgbvKNXeTAWpeOsdUAgwhCBfUBc;

				public List<aIfarARnGAuzhtkgfJktofioRbWQ> KatkNYKWUyiJzeOtrpEYmUXKExkQ;

				public int eVXBGTyrZsjtmqDQzPDkvLkNvBCI
				{
					get
					{
						if (KatkNYKWUyiJzeOtrpEYmUXKExkQ == null)
						{
							return 0;
						}
						return KatkNYKWUyiJzeOtrpEYmUXKExkQ.Count;
					}
				}

				public int pFsnwuLpNjiuBgxojbTGrwpjUYYH
				{
					get
					{
						if (wiJgbvKNXeTAWpeOsdUAgwhCBfUBc == null)
						{
							return 0;
						}
						return wiJgbvKNXeTAWpeOsdUAgwhCBfUBc.Count;
					}
				}

				public aIfarARnGAuzhtkgfJktofioRbWQ(string P_0, aIfarARnGAuzhtkgfJktofioRbWQ P_1)
				{
					MLXRsaVtOMjzVqlRdnpZWwQpCGTM = P_0;
					ZmhhoVpCjcvTCzAtCnYuUOJXsdLA = P_1;
					P_1?.REMPDuBLFajPUAFtZWiKOXbvscLw(this);
				}

				public void REMPDuBLFajPUAFtZWiKOXbvscLw(aIfarARnGAuzhtkgfJktofioRbWQ P_0)
				{
					if (P_0 != null)
					{
						if (KatkNYKWUyiJzeOtrpEYmUXKExkQ == null)
						{
							KatkNYKWUyiJzeOtrpEYmUXKExkQ = new List<aIfarARnGAuzhtkgfJktofioRbWQ>();
						}
						KatkNYKWUyiJzeOtrpEYmUXKExkQ.Add(P_0);
					}
				}

				public void yTHqQZoWUoAxIBuLDlInszSOJIiI(string P_0, string P_1)
				{
					if (!string.IsNullOrEmpty(P_0))
					{
						if (wiJgbvKNXeTAWpeOsdUAgwhCBfUBc == null)
						{
							wiJgbvKNXeTAWpeOsdUAgwhCBfUBc = new Dictionary<string, string>();
						}
						if (wiJgbvKNXeTAWpeOsdUAgwhCBfUBc.ContainsKey(P_0))
						{
							wiJgbvKNXeTAWpeOsdUAgwhCBfUBc[P_0] = P_1;
						}
						else
						{
							wiJgbvKNXeTAWpeOsdUAgwhCBfUBc.Add(P_0, P_1);
						}
					}
				}

				public bool QTXRuCqywjntsECjHEeWeYxIXfAJA(string P_0)
				{
					return ZEoYAjkdGNWFwycyFngRwtMwVthb(P_0) != null;
				}

				public aIfarARnGAuzhtkgfJktofioRbWQ ZEoYAjkdGNWFwycyFngRwtMwVthb(string P_0)
				{
					if (eVXBGTyrZsjtmqDQzPDkvLkNvBCI == 0)
					{
						return null;
					}
					for (int i = 0; i < KatkNYKWUyiJzeOtrpEYmUXKExkQ.Count; i++)
					{
						if (string.Equals(KatkNYKWUyiJzeOtrpEYmUXKExkQ[i].MLXRsaVtOMjzVqlRdnpZWwQpCGTM, P_0, StringComparison.Ordinal))
						{
							return KatkNYKWUyiJzeOtrpEYmUXKExkQ[i];
						}
					}
					return null;
				}

				public object mEsgfHzKwLFhAyeDYZofrDaBsVFD()
				{
					if (eVXBGTyrZsjtmqDQzPDkvLkNvBCI == 0)
					{
						return WugilENpLeggaKXbdDsCsloPkZzyA;
					}
					SerializedObject serializedObject = new SerializedObject(null, ObjectType.List);
					for (int i = 0; i < eVXBGTyrZsjtmqDQzPDkvLkNvBCI; i++)
					{
						aIfarARnGAuzhtkgfJktofioRbWQ aIfarARnGAuzhtkgfJktofioRbWQ2 = KatkNYKWUyiJzeOtrpEYmUXKExkQ[i];
						if (aIfarARnGAuzhtkgfJktofioRbWQ2 != null)
						{
							serializedObject.Add(aIfarARnGAuzhtkgfJktofioRbWQ2.MLXRsaVtOMjzVqlRdnpZWwQpCGTM, aIfarARnGAuzhtkgfJktofioRbWQ2.mEsgfHzKwLFhAyeDYZofrDaBsVFD());
						}
					}
					return serializedObject;
				}

				public virtual string fPgHDRJRCNyGZbgMeUwzVGUiUDwH()
				{
					return OnvbjEWrcCDjTAmvUNFwcyPrGveOA("", 0);
				}

				private string OnvbjEWrcCDjTAmvUNFwcyPrGveOA(string P_0, int P_1)
				{
					string text = "";
					for (int i = 0; i < P_1; i++)
					{
						text += "    ";
					}
					P_0 = P_0 + text + "Name = " + MLXRsaVtOMjzVqlRdnpZWwQpCGTM + "\n";
					P_0 = P_0 + text + "Content = " + ((WugilENpLeggaKXbdDsCsloPkZzyA == null) ? "NULL" : WugilENpLeggaKXbdDsCsloPkZzyA.ToString()) + "\n";
					P_0 = P_0 + text + "Attribute Count = " + pFsnwuLpNjiuBgxojbTGrwpjUYYH + "\n";
					if (wiJgbvKNXeTAWpeOsdUAgwhCBfUBc != null)
					{
						foreach (KeyValuePair<string, string> item in wiJgbvKNXeTAWpeOsdUAgwhCBfUBc)
						{
							P_0 = P_0 + text + "Attribute " + item.Key + ": = " + item.Value + "\n";
						}
					}
					P_0 = P_0 + text + "Child Count = " + eVXBGTyrZsjtmqDQzPDkvLkNvBCI + "\n";
					if (KatkNYKWUyiJzeOtrpEYmUXKExkQ != null)
					{
						string text2 = "";
						foreach (aIfarARnGAuzhtkgfJktofioRbWQ item2 in KatkNYKWUyiJzeOtrpEYmUXKExkQ)
						{
							text2 += "\n";
							text2 = item2.OnvbjEWrcCDjTAmvUNFwcyPrGveOA(text2, P_1 + 1);
						}
						P_0 += text2;
					}
					return P_0;
				}
			}

			private readonly aIfarARnGAuzhtkgfJktofioRbWQ xDxxJIQJItUwyjTizWtNpCOCaOzF;

			public aIfarARnGAuzhtkgfJktofioRbWQ MxerPDocZYbtGHAGnmbXXUmyFEFPA => xDxxJIQJItUwyjTizWtNpCOCaOzF;

			public bool rhWsuCZJeodzRQuQsdBHfAqTPwWp => xDxxJIQJItUwyjTizWtNpCOCaOzF != null;

			public LGUdKudrZaaUOAfIKJsudqPyvcfiB(string P_0)
			{
				if (string.IsNullOrEmpty(P_0))
				{
					throw new ArgumentNullException("xml");
				}
				try
				{
					using StringReader input = new StringReader(P_0);
					XmlReader xmlReader = XmlReader.Create(input);
					if (xmlReader == null)
					{
						throw new ArgumentNullException("reader");
					}
					xDxxJIQJItUwyjTizWtNpCOCaOzF = new aIfarARnGAuzhtkgfJktofioRbWQ("Root", null);
					wUEfBVOhbWsZUEddKYeclZpsWgml(xmlReader);
				}
				catch
				{
					xDxxJIQJItUwyjTizWtNpCOCaOzF = null;
				}
			}

			private void wUEfBVOhbWsZUEddKYeclZpsWgml(XmlReader P_0)
			{
				aIfarARnGAuzhtkgfJktofioRbWQ aIfarARnGAuzhtkgfJktofioRbWQ2 = xDxxJIQJItUwyjTizWtNpCOCaOzF;
				int num = 0;
				while (P_0.Read())
				{
					XmlNodeType nodeType = P_0.NodeType;
					if (nodeType == XmlNodeType.Comment || nodeType == XmlNodeType.XmlDeclaration)
					{
						num++;
						continue;
					}
					bool flag = false;
					if (P_0.NodeType == XmlNodeType.Element)
					{
						if (P_0.IsStartElement())
						{
							_ = P_0.IsEmptyElement;
							aIfarARnGAuzhtkgfJktofioRbWQ2 = new aIfarARnGAuzhtkgfJktofioRbWQ(P_0.LocalName, aIfarARnGAuzhtkgfJktofioRbWQ2);
							for (int i = 0; i < P_0.AttributeCount; i++)
							{
								P_0.MoveToNextAttribute();
								aIfarARnGAuzhtkgfJktofioRbWQ2.yTHqQZoWUoAxIBuLDlInszSOJIiI(P_0.Name, P_0.Value);
							}
							if (P_0.IsEmptyElement)
							{
								flag = true;
							}
						}
					}
					else if (P_0.NodeType == XmlNodeType.Text)
					{
						if (!P_0.IsEmptyElement && P_0.HasValue)
						{
							aIfarARnGAuzhtkgfJktofioRbWQ2.WugilENpLeggaKXbdDsCsloPkZzyA = P_0.ReadContentAsString();
						}
						else
						{
							flag = true;
						}
					}
					else
					{
						_ = P_0.NodeType;
						_ = 15;
					}
					if ((flag || P_0.NodeType == XmlNodeType.EndElement) && aIfarARnGAuzhtkgfJktofioRbWQ2 != null && aIfarARnGAuzhtkgfJktofioRbWQ2 != xDxxJIQJItUwyjTizWtNpCOCaOzF && P_0.Name == aIfarARnGAuzhtkgfJktofioRbWQ2.MLXRsaVtOMjzVqlRdnpZWwQpCGTM)
					{
						aIfarARnGAuzhtkgfJktofioRbWQ2 = aIfarARnGAuzhtkgfJktofioRbWQ2.ZmhhoVpCjcvTCzAtCnYuUOJXsdLA;
					}
					num++;
				}
			}

			public virtual string hSdaOZLEYHaNWcfcGihNZIHMqzPtA()
			{
				if (xDxxJIQJItUwyjTizWtNpCOCaOzF == null || xDxxJIQJItUwyjTizWtNpCOCaOzF.eVXBGTyrZsjtmqDQzPDkvLkNvBCI == 0)
				{
					return "Document is empty.";
				}
				return xDxxJIQJItUwyjTizWtNpCOCaOzF.ToString();
			}
		}

		[Serializable]
		private sealed class BmGGkbBcLqUElAphdsQXwbbipJNgb
		{
			public static readonly BmGGkbBcLqUElAphdsQXwbbipJNgb _003C_003E9 = new BmGGkbBcLqUElAphdsQXwbbipJNgb();

			public static Func<FieldInfo, bool> _003C_003E9__63_0;

			public static Func<FieldInfo, string> _003C_003E9__63_1;

			public static Func<PropertyInfo, bool> _003C_003E9__63_2;

			public static Func<PropertyInfo, string> _003C_003E9__63_3;

			internal bool NKPHUxKOJjJxMXVLnUCusBSGHmkc(FieldInfo P_0)
			{
				if ((P_0.IsPublic || P_0.IsDefined(typeof(SerializeAttribute), inherit: true) || P_0.IsDefined(typeof(SerializeField), inherit: true)) && !P_0.IsDefined(typeof(NonSerializedAttribute), inherit: true))
				{
					return !P_0.IsDefined(typeof(DoNotSerializeAttribute), inherit: true);
				}
				return false;
			}

			internal string mnXxMMWhXVQEGuhlBeplOFhbfpniA(FieldInfo P_0)
			{
				string name;
				if (P_0.IsDefined(typeof(SerializeAttribute), inherit: true) && !string.IsNullOrEmpty(name = (CollectionTools.GetValue(P_0.GetCustomAttributes(typeof(SerializeAttribute), inherit: true), 0) as SerializeAttribute).Name))
				{
					return name;
				}
				return P_0.Name;
			}

			internal bool OclmGCxXtgdXmnhxZiyQRLesHzae(PropertyInfo P_0)
			{
				if (P_0.CanWrite && P_0.IsDefined(typeof(SerializeAttribute), inherit: true))
				{
					return !P_0.IsDefined(typeof(DoNotSerializeAttribute), inherit: true);
				}
				return false;
			}

			internal string sTlHyqXdTGePEfeSXrmNgKdzWDOnA(PropertyInfo P_0)
			{
				string name;
				if (P_0.IsDefined(typeof(SerializeAttribute), inherit: true) && !string.IsNullOrEmpty(name = (CollectionTools.GetValue(P_0.GetCustomAttributes(typeof(SerializeAttribute), inherit: true), 0) as SerializeAttribute).Name))
				{
					return name;
				}
				return P_0.Name;
			}
		}

		private readonly IndexedDictionary<string, ysQjoUnlrACbUCKuNmYYPurbTSdW> CDokqMCqaGTJEoPERdEykkZcZPfBA;

		private XmlInfo GsSLQlNbeVToweRedxplITmKRIcQ;

		private Type YieyxdycBgfNnahNWLUFpurASECJA;

		private ObjectType VCgSRfQflkVfZVNxGhubJWmtivDm;

		private static readonly Dictionary<Type, Dictionary<string, FieldInfo>> FTCfxRaTJSciknApGvpHlntYwwQc = new Dictionary<Type, Dictionary<string, FieldInfo>>();

		private static readonly Dictionary<Type, Dictionary<string, PropertyInfo>> StPeTproUCsBQNmAYnnkEROWtyHl = new Dictionary<Type, Dictionary<string, PropertyInfo>>();

		private bool allowDuplicateKeys => VCgSRfQflkVfZVNxGhubJWmtivDm == ObjectType.List;

		public ObjectType objectType
		{
			get
			{
				return VCgSRfQflkVfZVNxGhubJWmtivDm;
			}
			set
			{
				if (value != VCgSRfQflkVfZVNxGhubJWmtivDm)
				{
					VCgSRfQflkVfZVNxGhubJWmtivDm = value;
					CDokqMCqaGTJEoPERdEykkZcZPfBA.AllowDuplicateKeys = allowDuplicateKeys;
				}
			}
		}

		public Type type => YieyxdycBgfNnahNWLUFpurASECJA;

		public XmlInfo xmlInfo
		{
			get
			{
				return GsSLQlNbeVToweRedxplITmKRIcQ;
			}
			set
			{
				GsSLQlNbeVToweRedxplITmKRIcQ = value;
			}
		}

		public int count => CDokqMCqaGTJEoPERdEykkZcZPfBA.Count;

		public Field this[int index]
		{
			get
			{
				ysQjoUnlrACbUCKuNmYYPurbTSdW ysQjoUnlrACbUCKuNmYYPurbTSdW2 = CDokqMCqaGTJEoPERdEykkZcZPfBA[index];
				return new Field(CDokqMCqaGTJEoPERdEykkZcZPfBA.GetKeyAt(index), ysQjoUnlrACbUCKuNmYYPurbTSdW2.ykDdLPMhayTettlvvLMxtApaKiCX, ysQjoUnlrACbUCKuNmYYPurbTSdW2.VvTjeMCwzNLIMnXeumTjjfaYHKUO, ysQjoUnlrACbUCKuNmYYPurbTSdW2.niLiZtIBeVjKsAgtlTaXIlYWtTqh);
			}
		}

		bool IExportToXml.writesOwnElementTag => true;

		[CustomObfuscation(rename = false)]
		private SerializedObject()
			: this(0)
		{
		}

		private SerializedObject(int P_0)
		{
			VCgSRfQflkVfZVNxGhubJWmtivDm = ObjectType.List;
			CDokqMCqaGTJEoPERdEykkZcZPfBA = new IndexedDictionary<string, ysQjoUnlrACbUCKuNmYYPurbTSdW>(P_0, true);
		}

		public SerializedObject(Type P_0, ObjectType P_1)
			: this(P_0, P_1, 0)
		{
		}

		public SerializedObject(Type P_0, ObjectType P_1, int P_2)
			: this(P_2)
		{
			YieyxdycBgfNnahNWLUFpurASECJA = P_0;
			objectType = P_1;
		}

		public SerializedObject(Type P_0, IDictionary<string, object> P_1, ObjectType P_2)
			: this(P_0, P_2, P_1?.Count ?? 0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			foreach (KeyValuePair<string, object> item in P_1)
			{
				CDokqMCqaGTJEoPERdEykkZcZPfBA.Add(item.Key, new ysQjoUnlrACbUCKuNmYYPurbTSdW((item.Value != null) ? item.Value.GetType() : null, item.Value, FieldOptions.None));
			}
		}

		public void Add<T>(string fieldName, T value, FieldOptions options = FieldOptions.None)
		{
			Add(typeof(T), fieldName, value, options);
		}

		public void Add(Type type, string fieldName, object value, FieldOptions options = FieldOptions.None)
		{
			if (type != null && value != null && (object)type != value.GetType())
			{
				throw new Exception("Type does not match value type.");
			}
			if (string.IsNullOrEmpty(fieldName))
			{
				if (VCgSRfQflkVfZVNxGhubJWmtivDm != ObjectType.List)
				{
					throw new ArgumentNullException("fieldName");
				}
				fieldName = "value";
			}
			if (allowDuplicateKeys)
			{
				CDokqMCqaGTJEoPERdEykkZcZPfBA.Add(fieldName, new ysQjoUnlrACbUCKuNmYYPurbTSdW(type, value, options));
			}
			else if (!CDokqMCqaGTJEoPERdEykkZcZPfBA.ContainsKey(fieldName))
			{
				CDokqMCqaGTJEoPERdEykkZcZPfBA.Add(fieldName, new ysQjoUnlrACbUCKuNmYYPurbTSdW(type, value, options));
			}
			else
			{
				CDokqMCqaGTJEoPERdEykkZcZPfBA.SetValue(fieldName, new ysQjoUnlrACbUCKuNmYYPurbTSdW(type, value, options));
			}
		}

		public void Add(string fieldName, object value)
		{
			Add(value?.GetType(), fieldName, value);
		}

		public bool Remove(string fieldName)
		{
			if (string.IsNullOrEmpty(fieldName))
			{
				return false;
			}
			return CDokqMCqaGTJEoPERdEykkZcZPfBA.Remove(fieldName);
		}

		public bool Contains(string fieldName)
		{
			if (string.IsNullOrEmpty(fieldName))
			{
				return false;
			}
			return CDokqMCqaGTJEoPERdEykkZcZPfBA.ContainsKey(fieldName);
		}

		public Type GetDataType(string fieldName)
		{
			if (string.IsNullOrEmpty(fieldName))
			{
				return null;
			}
			if (!CDokqMCqaGTJEoPERdEykkZcZPfBA.TryGetValue(fieldName, out var value))
			{
				return null;
			}
			return value.VvTjeMCwzNLIMnXeumTjjfaYHKUO;
		}

		public bool TryGetOriginalValue(string fieldName, out object value)
		{
			value = null;
			if (string.IsNullOrEmpty(fieldName))
			{
				return false;
			}
			if (!CDokqMCqaGTJEoPERdEykkZcZPfBA.TryGetValue(fieldName, out var value2))
			{
				return false;
			}
			value = value2.ykDdLPMhayTettlvvLMxtApaKiCX;
			return true;
		}

		public Field GetEntry(string fieldName)
		{
			KeyValuePair<string, ysQjoUnlrACbUCKuNmYYPurbTSdW> entry = CDokqMCqaGTJEoPERdEykkZcZPfBA.GetEntry(fieldName);
			return new Field(entry.Key, entry.Value.ykDdLPMhayTettlvvLMxtApaKiCX, entry.Value.VvTjeMCwzNLIMnXeumTjjfaYHKUO, entry.Value.niLiZtIBeVjKsAgtlTaXIlYWtTqh);
		}

		public object GetOriginalValue(string fieldName)
		{
			return CDokqMCqaGTJEoPERdEykkZcZPfBA.GetEntry(fieldName).Value.ykDdLPMhayTettlvvLMxtApaKiCX;
		}

		public object GetOriginalValue(int index)
		{
			return CDokqMCqaGTJEoPERdEykkZcZPfBA[index].ykDdLPMhayTettlvvLMxtApaKiCX;
		}

		public T GetOriginalValue<T>(string fieldName)
		{
			return (T)GetOriginalValue(fieldName);
		}

		public T GetOriginalValue<T>(int index)
		{
			return (T)GetOriginalValue(index);
		}

		public bool TryGetDeserializedValue<T>(string fieldName, out T value)
		{
			if (string.IsNullOrEmpty(fieldName))
			{
				value = default(T);
				return false;
			}
			if (!CDokqMCqaGTJEoPERdEykkZcZPfBA.TryGetValue(fieldName, out var value2))
			{
				value = default(T);
				return false;
			}
			return zsrmoHPKxJTxQdVloeXERebCfEND<T>(value2.ykDdLPMhayTettlvvLMxtApaKiCX, out value, NumberStyles.Any, CultureInfo.InvariantCulture);
		}

		public bool TryGetDeserializedValue<T>(int index, out T value)
		{
			if ((uint)index > (uint)CDokqMCqaGTJEoPERdEykkZcZPfBA.Count)
			{
				value = default(T);
				return false;
			}
			return zsrmoHPKxJTxQdVloeXERebCfEND<T>(CDokqMCqaGTJEoPERdEykkZcZPfBA.GetEntryAt(index).Value.ykDdLPMhayTettlvvLMxtApaKiCX, out value, NumberStyles.Any, CultureInfo.InvariantCulture);
		}

		public bool TryGetDeserializedValueByRef<T>(string fieldName, ref T value)
		{
			if (string.IsNullOrEmpty(fieldName))
			{
				return false;
			}
			if (!TryGetDeserializedValue<T>(fieldName, out var value2))
			{
				return false;
			}
			value = value2;
			return true;
		}

		public bool TryGetDeserializedValueByRef<T>(int index, ref T value)
		{
			if ((uint)index > (uint)CDokqMCqaGTJEoPERdEykkZcZPfBA.Count)
			{
				return false;
			}
			if (!TryGetDeserializedValue<T>(index, out var value2))
			{
				return false;
			}
			value = value2;
			return true;
		}

		public string ToXmlString(bool writeDocumentTag)
		{
			if (GsSLQlNbeVToweRedxplITmKRIcQ == null)
			{
				throw new Exception("XmlInfo is null. Cannot write Xml without XmlInfo.");
			}
			string empty = string.Empty;
			using StringWriter stringWriter = new StringWriter();
			using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter))
			{
				if (writeDocumentTag)
				{
					xmlWriter.WriteStartDocument();
				}
				ZvegkhDQfrYRNmhdtBDwwfVjbYaAA(xmlWriter);
				xmlWriter.Flush();
			}
			return stringWriter.ToString();
		}

		public string ToJsonString()
		{
			return JsonWriter.ToJson(this);
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("count = ");
			stringBuilder.Append(count.ToString());
			stringBuilder.Append("\n");
			stringBuilder.Append("type = ");
			stringBuilder.Append((YieyxdycBgfNnahNWLUFpurASECJA != null) ? YieyxdycBgfNnahNWLUFpurASECJA.Name : "NULL\n");
			stringBuilder.Append("objectType = ");
			stringBuilder.Append(VCgSRfQflkVfZVNxGhubJWmtivDm.ToString());
			stringBuilder.Append("\n");
			stringBuilder.Append("xmlInfo = ");
			stringBuilder.Append((GsSLQlNbeVToweRedxplITmKRIcQ != null) ? GsSLQlNbeVToweRedxplITmKRIcQ.ToString() : "NULL\n");
			stringBuilder.Append("\n");
			for (int i = 0; i < CDokqMCqaGTJEoPERdEykkZcZPfBA.Count; i++)
			{
				string keyAt = CDokqMCqaGTJEoPERdEykkZcZPfBA.GetKeyAt(i);
				stringBuilder.Append("key = ");
				stringBuilder.Append((keyAt != null) ? keyAt : "NULL");
				stringBuilder.Append(", value = ");
				stringBuilder.Append(CDokqMCqaGTJEoPERdEykkZcZPfBA[i].ToString());
				stringBuilder.Append("\n");
			}
			return stringBuilder.ToString();
		}

		private void ZvegkhDQfrYRNmhdtBDwwfVjbYaAA(XmlWriter P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("writer");
			}
			P_0.WriteStartElement(type.Name, "http://guavaman.com/rewired");
			VEsKyMMkaEyNXDmbhBkJSOQFiSml(P_0);
			P_0.WriteEndElement();
		}

		private void VEsKyMMkaEyNXDmbhBkJSOQFiSml(XmlWriter P_0)
		{
			int num = ((xmlInfo != null) ? xmlInfo.attributes.Count : 0);
			for (int i = 0; i < num; i++)
			{
				XmlInfo.yNlNhbgspAXUCpVrAdsXgrpzrKKV yNlNhbgspAXUCpVrAdsXgrpzrKKV = xmlInfo.attributes[i];
				if (yNlNhbgspAXUCpVrAdsXgrpzrKKV is XmlInfo.JISccXeaCjmCeBJbWJlLPGJisilo)
				{
					XmlInfo.JISccXeaCjmCeBJbWJlLPGJisilo jISccXeaCjmCeBJbWJlLPGJisilo = yNlNhbgspAXUCpVrAdsXgrpzrKKV as XmlInfo.JISccXeaCjmCeBJbWJlLPGJisilo;
					if (!string.IsNullOrEmpty(jISccXeaCjmCeBJbWJlLPGJisilo.XCTmYYtBCCzTuZmlJPszaypSgJdS))
					{
						P_0.WriteAttributeString(jISccXeaCjmCeBJbWJlLPGJisilo.XCTmYYtBCCzTuZmlJPszaypSgJdS, jISccXeaCjmCeBJbWJlLPGJisilo.qItGqcAiFsVuEXTeGzrduYLlUPFM, jISccXeaCjmCeBJbWJlLPGJisilo.GpDRMyFZBJdjlvWjrvVYAzZnhbYW, jISccXeaCjmCeBJbWJlLPGJisilo.vLOpmXQkMsPmsBAJNcXkcKfWoznZ);
					}
					else if (!string.IsNullOrEmpty(jISccXeaCjmCeBJbWJlLPGJisilo.GpDRMyFZBJdjlvWjrvVYAzZnhbYW))
					{
						P_0.WriteAttributeString(jISccXeaCjmCeBJbWJlLPGJisilo.qItGqcAiFsVuEXTeGzrduYLlUPFM, jISccXeaCjmCeBJbWJlLPGJisilo.GpDRMyFZBJdjlvWjrvVYAzZnhbYW, jISccXeaCjmCeBJbWJlLPGJisilo.vLOpmXQkMsPmsBAJNcXkcKfWoznZ);
					}
					else
					{
						P_0.WriteAttributeString(jISccXeaCjmCeBJbWJlLPGJisilo.qItGqcAiFsVuEXTeGzrduYLlUPFM, jISccXeaCjmCeBJbWJlLPGJisilo.vLOpmXQkMsPmsBAJNcXkcKfWoznZ);
					}
					continue;
				}
				throw new NotImplementedException();
			}
			for (int j = 0; j < count; j++)
			{
				ysQjoUnlrACbUCKuNmYYPurbTSdW ysQjoUnlrACbUCKuNmYYPurbTSdW2 = CDokqMCqaGTJEoPERdEykkZcZPfBA[j];
				string text = CDokqMCqaGTJEoPERdEykkZcZPfBA.GetKeyAt(j);
				if ((ysQjoUnlrACbUCKuNmYYPurbTSdW2.niLiZtIBeVjKsAgtlTaXIlYWtTqh & FieldOptions.ExculdeFromXml) == 0)
				{
					if (string.IsNullOrEmpty(text))
					{
						text = ((ysQjoUnlrACbUCKuNmYYPurbTSdW2.VvTjeMCwzNLIMnXeumTjjfaYHKUO != null) ? ysQjoUnlrACbUCKuNmYYPurbTSdW2.GetType().Name : ((ysQjoUnlrACbUCKuNmYYPurbTSdW2.ykDdLPMhayTettlvvLMxtApaKiCX == null) ? "value" : ysQjoUnlrACbUCKuNmYYPurbTSdW2.ykDdLPMhayTettlvvLMxtApaKiCX.GetType().Name));
					}
					SerializationTools.WriteXmlElement(P_0, text, ysQjoUnlrACbUCKuNmYYPurbTSdW2.ykDdLPMhayTettlvvLMxtApaKiCX);
				}
			}
		}

		private void wylzMWxyHCQaWttTGvUztUOTvtQj(XmlWriter P_0)
		{
			ZvegkhDQfrYRNmhdtBDwwfVjbYaAA(P_0);
		}

		void IExportToXml.WriteXml(XmlWriter P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in wylzMWxyHCQaWttTGvUztUOTvtQj
			this.wylzMWxyHCQaWttTGvUztUOTvtQj(P_0);
		}

		private void emIidjBUOvmKmlEoNpFkhPkFIqLp(StringBuilder P_0, Action<StringBuilder, object> P_1)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("stringBuilder");
			}
			if (P_1 == null)
			{
				throw new ArgumentNullException("appendValueDelegate");
			}
			int num = CDokqMCqaGTJEoPERdEykkZcZPfBA.Count;
			if (CDokqMCqaGTJEoPERdEykkZcZPfBA.ContainsDuplicateKeys)
			{
				P_0.Append('[');
				bool flag = true;
				for (int i = 0; i < num; i++)
				{
					if (flag)
					{
						flag = false;
					}
					else
					{
						P_0.Append(',');
					}
					P_1(P_0, CDokqMCqaGTJEoPERdEykkZcZPfBA[i].ykDdLPMhayTettlvvLMxtApaKiCX);
				}
				P_0.Append(']');
				return;
			}
			P_0.Append('{');
			bool flag2 = true;
			for (int j = 0; j < num; j++)
			{
				if (flag2)
				{
					flag2 = false;
				}
				else
				{
					P_0.Append(',');
				}
				ysQjoUnlrACbUCKuNmYYPurbTSdW ysQjoUnlrACbUCKuNmYYPurbTSdW2 = CDokqMCqaGTJEoPERdEykkZcZPfBA[j];
				string value = CDokqMCqaGTJEoPERdEykkZcZPfBA.GetKeyAt(j);
				if (string.IsNullOrEmpty(value))
				{
					value = j.ToString();
				}
				P_0.Append('"');
				P_0.Append(value);
				P_0.Append("\":");
				P_1(P_0, ysQjoUnlrACbUCKuNmYYPurbTSdW2.ykDdLPMhayTettlvvLMxtApaKiCX);
			}
			P_0.Append('}');
		}

		void IExportToJson.WriteJson(StringBuilder P_0, Action<StringBuilder, object> P_1)
		{
			//ILSpy generated this explicit interface implementation from .override directive in emIidjBUOvmKmlEoNpFkhPkFIqLp
			this.emIidjBUOvmKmlEoNpFkhPkFIqLp(P_0, P_1);
		}

		private void HuafuPSroABqNZQkOiaaEDRoDuWvA(object P_0)
		{
			Add(null, P_0);
		}

		void IAddValue<object>.Add(object P_0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in HuafuPSroABqNZQkOiaaEDRoDuWvA
			this.HuafuPSroABqNZQkOiaaEDRoDuWvA(P_0);
		}

		private void oJtumJIrjEePbQjPFNBjuggwghPdA(string P_0, object P_1)
		{
			Add(P_0, P_1);
		}

		void IAddKeyValue<string, object>.Add(string P_0, object P_1)
		{
			//ILSpy generated this explicit interface implementation from .override directive in oJtumJIrjEePbQjPFNBjuggwghPdA
			this.oJtumJIrjEePbQjPFNBjuggwghPdA(P_0, P_1);
		}

		IEnumerator<Field> IEnumerable<Field>.GetEnumerator()
		{
			return new Enumerator(CDokqMCqaGTJEoPERdEykkZcZPfBA);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Enumerator(CDokqMCqaGTJEoPERdEykkZcZPfBA);
		}

		private static bool zsrmoHPKxJTxQdVloeXERebCfEND<_0001>(object P_0, out _0001 P_1, NumberStyles P_2 = NumberStyles.Any, CultureInfo P_3 = null)
		{
			if (!UNxZBfhwgZzXDJfZvESaixZzOOhE(typeof(_0001), P_0, out var obj, P_2, P_3))
			{
				P_1 = default(_0001);
				return false;
			}
			P_1 = (_0001)obj;
			return true;
		}

		private static bool UNxZBfhwgZzXDJfZvESaixZzOOhE(Type P_0, object P_1, out object P_2, NumberStyles P_3 = NumberStyles.Any, CultureInfo P_4 = null)
		{
			P_2 = null;
			if (P_1 == null)
			{
				if ((object)P_0 == typeof(string))
				{
					P_2 = string.Empty;
					return true;
				}
				if (!ReflectionTools.IsValueType(P_0))
				{
					return true;
				}
				if (Nullable.GetUnderlyingType(P_0) != null)
				{
					return true;
				}
				return false;
			}
			Type type = P_1.GetType();
			if ((object)P_0 == type)
			{
				P_2 = P_1;
				return true;
			}
			try
			{
				if ((object)P_0 == typeof(string))
				{
					P_2 = P_1.ToString();
					return true;
				}
				if ((object)P_0 == typeof(int))
				{
					if ((object)type == typeof(float))
					{
						P_2 = (int)(float)P_1;
					}
					else if ((object)type == typeof(uint))
					{
						P_2 = (int)(uint)P_1;
					}
					else if ((object)type == typeof(long))
					{
						P_2 = (int)(long)P_1;
					}
					else if ((object)type == typeof(ulong))
					{
						P_2 = (int)(ulong)P_1;
					}
					else if ((object)type == typeof(double))
					{
						P_2 = (int)(double)P_1;
					}
					else if ((object)type == typeof(decimal))
					{
						P_2 = (int)(decimal)P_1;
					}
					else if ((object)type == typeof(short))
					{
						P_2 = (int)(short)P_1;
					}
					else if ((object)type == typeof(ushort))
					{
						P_2 = (int)(ushort)P_1;
					}
					else if ((object)type == typeof(byte))
					{
						P_2 = (int)(byte)P_1;
					}
					else if ((object)type == typeof(sbyte))
					{
						P_2 = (int)(sbyte)P_1;
					}
					else
					{
						if ((object)type != typeof(string))
						{
							return false;
						}
						int result;
						if (P_4 != null)
						{
							if (!int.TryParse(P_1.ToString(), P_3, P_4, out result))
							{
								return false;
							}
						}
						else if (!int.TryParse(P_1.ToString(), out result))
						{
							return false;
						}
						P_2 = result;
					}
					return true;
				}
				if ((object)P_0 == typeof(float))
				{
					if ((object)type == typeof(int))
					{
						P_2 = (float)(int)P_1;
					}
					else if ((object)type == typeof(uint))
					{
						P_2 = (float)(uint)P_1;
					}
					else if ((object)type == typeof(long))
					{
						P_2 = (float)(long)P_1;
					}
					else if ((object)type == typeof(ulong))
					{
						P_2 = (float)(ulong)P_1;
					}
					else if ((object)type == typeof(double))
					{
						P_2 = (float)(double)P_1;
					}
					else if ((object)type == typeof(decimal))
					{
						P_2 = (float)(decimal)P_1;
					}
					else if ((object)type == typeof(short))
					{
						P_2 = (float)(short)P_1;
					}
					else if ((object)type == typeof(ushort))
					{
						P_2 = (float)(int)(ushort)P_1;
					}
					else if ((object)type == typeof(byte))
					{
						P_2 = (float)(int)(byte)P_1;
					}
					else if ((object)type == typeof(sbyte))
					{
						P_2 = (float)(sbyte)P_1;
					}
					else
					{
						if ((object)type != typeof(string))
						{
							return false;
						}
						float result2;
						if (P_4 != null)
						{
							if (!float.TryParse(P_1.ToString(), P_3, P_4, out result2))
							{
								return false;
							}
						}
						else if (!float.TryParse(P_1.ToString(), out result2))
						{
							return false;
						}
						P_2 = result2;
					}
					return true;
				}
				if (ReflectionTools.IsEnum(P_0))
				{
					if (UNxZBfhwgZzXDJfZvESaixZzOOhE(ReflectionTools.GetUnderlyingEnumType(P_0), P_1, out var value, P_3, P_4))
					{
						P_2 = Enum.ToObject(P_0, value);
						return true;
					}
					if ((object)type == typeof(string))
					{
						try
						{
							P_2 = Enum.Parse(P_0, (string)P_1, ignoreCase: true);
							return true;
						}
						catch
						{
							P_2 = null;
							return false;
						}
					}
				}
				else
				{
					if ((object)P_0 == typeof(uint))
					{
						if ((object)type == typeof(int))
						{
							P_2 = (uint)(int)P_1;
						}
						else if ((object)type == typeof(float))
						{
							P_2 = (uint)(float)P_1;
						}
						else if ((object)type == typeof(long))
						{
							P_2 = (uint)(long)P_1;
						}
						else if ((object)type == typeof(ulong))
						{
							P_2 = (uint)(ulong)P_1;
						}
						else if ((object)type == typeof(double))
						{
							P_2 = (uint)(double)P_1;
						}
						else if ((object)type == typeof(decimal))
						{
							P_2 = (uint)(decimal)P_1;
						}
						else if ((object)type == typeof(short))
						{
							P_2 = (uint)(short)P_1;
						}
						else if ((object)type == typeof(ushort))
						{
							P_2 = (uint)(ushort)P_1;
						}
						else if ((object)type == typeof(byte))
						{
							P_2 = (uint)(byte)P_1;
						}
						else if ((object)type == typeof(sbyte))
						{
							P_2 = (uint)(sbyte)P_1;
						}
						else
						{
							if ((object)type != typeof(string))
							{
								return false;
							}
							uint result3;
							if (P_4 != null)
							{
								if (!uint.TryParse(P_1.ToString(), P_3, P_4, out result3))
								{
									return false;
								}
							}
							else if (!uint.TryParse(P_1.ToString(), out result3))
							{
								return false;
							}
							P_2 = result3;
						}
						return true;
					}
					if ((object)P_0 == typeof(double))
					{
						if ((object)type == typeof(float))
						{
							P_2 = (double)(float)P_1;
						}
						else if ((object)type == typeof(int))
						{
							P_2 = (double)(int)P_1;
						}
						else if ((object)type == typeof(uint))
						{
							P_2 = (double)(uint)P_1;
						}
						else if ((object)type == typeof(long))
						{
							P_2 = (double)(long)P_1;
						}
						else if ((object)type == typeof(ulong))
						{
							P_2 = (double)(ulong)P_1;
						}
						else if ((object)type == typeof(short))
						{
							P_2 = (double)(short)P_1;
						}
						else if ((object)type == typeof(ushort))
						{
							P_2 = (double)(int)(ushort)P_1;
						}
						else if ((object)type == typeof(byte))
						{
							P_2 = (double)(int)(byte)P_1;
						}
						else if ((object)type == typeof(sbyte))
						{
							P_2 = (double)(sbyte)P_1;
						}
						else if ((object)type == typeof(decimal))
						{
							P_2 = (double)(decimal)P_1;
						}
						else
						{
							if ((object)type != typeof(string))
							{
								return false;
							}
							double result4;
							if (P_4 != null)
							{
								if (!double.TryParse(P_1.ToString(), P_3, P_4, out result4))
								{
									return false;
								}
							}
							else if (!double.TryParse(P_1.ToString(), out result4))
							{
								return false;
							}
							P_2 = result4;
						}
						return true;
					}
					if ((object)P_0 == typeof(bool))
					{
						if ((object)type == typeof(int))
						{
							P_2 = (int)P_1 > 0;
						}
						else if ((object)type == typeof(float))
						{
							P_2 = (float)P_1 > 0f;
						}
						else if ((object)type == typeof(uint))
						{
							P_2 = (uint)P_1 != 0;
						}
						else if ((object)type == typeof(long))
						{
							P_2 = (long)P_1 > 0;
						}
						else if ((object)type == typeof(ulong))
						{
							P_2 = (ulong)P_1 != 0;
						}
						else if ((object)type == typeof(double))
						{
							P_2 = (double)P_1 > 0.0;
						}
						else if ((object)type == typeof(decimal))
						{
							P_2 = (decimal)P_1 > 0m;
						}
						else if ((object)type == typeof(short))
						{
							P_2 = (short)P_1 > 0;
						}
						else if ((object)type == typeof(ushort))
						{
							P_2 = (ushort)P_1 > 0;
						}
						else if ((object)type == typeof(byte))
						{
							P_2 = (byte)P_1 > 0;
						}
						else if ((object)type == typeof(sbyte))
						{
							P_2 = (sbyte)P_1 > 0;
						}
						else
						{
							if ((object)type != typeof(string))
							{
								return false;
							}
							if (string.Equals((string)P_1, "true", StringComparison.OrdinalIgnoreCase))
							{
								P_2 = true;
							}
							else
							{
								if (!string.Equals((string)P_1, "false", StringComparison.OrdinalIgnoreCase))
								{
									return false;
								}
								P_2 = false;
							}
						}
						return true;
					}
					if ((object)P_0 == typeof(long))
					{
						if ((object)type == typeof(int))
						{
							P_2 = (long)(int)P_1;
						}
						else if ((object)type == typeof(ulong))
						{
							P_2 = (long)(ulong)P_1;
						}
						else if ((object)type == typeof(float))
						{
							P_2 = (long)(float)P_1;
						}
						else if ((object)type == typeof(uint))
						{
							P_2 = (long)(uint)P_1;
						}
						else if ((object)type == typeof(double))
						{
							P_2 = (long)(double)P_1;
						}
						else if ((object)type == typeof(decimal))
						{
							P_2 = (long)(decimal)P_1;
						}
						else if ((object)type == typeof(short))
						{
							P_2 = (long)(short)P_1;
						}
						else if ((object)type == typeof(ushort))
						{
							P_2 = (long)(ushort)P_1;
						}
						else if ((object)type == typeof(byte))
						{
							P_2 = (long)(byte)P_1;
						}
						else if ((object)type == typeof(sbyte))
						{
							P_2 = (long)(sbyte)P_1;
						}
						else
						{
							if ((object)type != typeof(string))
							{
								return false;
							}
							long result5;
							if (P_4 != null)
							{
								if (!long.TryParse(P_1.ToString(), P_3, P_4, out result5))
								{
									return false;
								}
							}
							else if (!long.TryParse(P_1.ToString(), out result5))
							{
								return false;
							}
							P_2 = result5;
						}
						return true;
					}
					if ((object)P_0 == typeof(ulong))
					{
						if ((object)type == typeof(long))
						{
							P_2 = (ulong)(long)P_1;
						}
						else if ((object)type == typeof(int))
						{
							P_2 = (ulong)(int)P_1;
						}
						else if ((object)type == typeof(float))
						{
							P_2 = (ulong)(float)P_1;
						}
						else if ((object)type == typeof(uint))
						{
							P_2 = (ulong)(uint)P_1;
						}
						else if ((object)type == typeof(double))
						{
							P_2 = (ulong)(double)P_1;
						}
						else if ((object)type == typeof(decimal))
						{
							P_2 = (ulong)(decimal)P_1;
						}
						else if ((object)type == typeof(short))
						{
							P_2 = (ulong)(short)P_1;
						}
						else if ((object)type == typeof(ushort))
						{
							P_2 = (ulong)(ushort)P_1;
						}
						else if ((object)type == typeof(byte))
						{
							P_2 = (ulong)(byte)P_1;
						}
						else if ((object)type == typeof(sbyte))
						{
							P_2 = (ulong)(sbyte)P_1;
						}
						else
						{
							if ((object)type != typeof(string))
							{
								return false;
							}
							ulong result6;
							if (P_4 != null)
							{
								if (!ulong.TryParse(P_1.ToString(), P_3, P_4, out result6))
								{
									return false;
								}
							}
							else if (!ulong.TryParse(P_1.ToString(), out result6))
							{
								return false;
							}
							P_2 = result6;
						}
						return true;
					}
					if ((object)P_0 == typeof(short))
					{
						if ((object)type == typeof(ushort))
						{
							P_2 = (short)(ushort)P_1;
						}
						else if ((object)type == typeof(int))
						{
							P_2 = (short)(int)P_1;
						}
						else if ((object)type == typeof(uint))
						{
							P_2 = (short)(uint)P_1;
						}
						else if ((object)type == typeof(long))
						{
							P_2 = (short)(long)P_1;
						}
						else if ((object)type == typeof(ulong))
						{
							P_2 = (short)(ulong)P_1;
						}
						else if ((object)type == typeof(float))
						{
							P_2 = (short)(float)P_1;
						}
						else if ((object)type == typeof(double))
						{
							P_2 = (short)(double)P_1;
						}
						else if ((object)type == typeof(decimal))
						{
							P_2 = (short)(decimal)P_1;
						}
						else if ((object)type == typeof(byte))
						{
							P_2 = (short)(byte)P_1;
						}
						else if ((object)type == typeof(sbyte))
						{
							P_2 = (short)(sbyte)P_1;
						}
						else
						{
							if ((object)type != typeof(string))
							{
								return false;
							}
							short result7;
							if (P_4 != null)
							{
								if (!short.TryParse(P_1.ToString(), P_3, P_4, out result7))
								{
									return false;
								}
							}
							else if (!short.TryParse(P_1.ToString(), out result7))
							{
								return false;
							}
							P_2 = result7;
						}
						return true;
					}
					if ((object)P_0 == typeof(ushort))
					{
						if ((object)type == typeof(short))
						{
							P_2 = (ushort)(short)P_1;
						}
						else if ((object)type == typeof(int))
						{
							P_2 = (ushort)(int)P_1;
						}
						else if ((object)type == typeof(uint))
						{
							P_2 = (ushort)(uint)P_1;
						}
						else if ((object)type == typeof(long))
						{
							P_2 = (ushort)(long)P_1;
						}
						else if ((object)type == typeof(ulong))
						{
							P_2 = (ushort)(ulong)P_1;
						}
						else if ((object)type == typeof(float))
						{
							P_2 = (ushort)(float)P_1;
						}
						else if ((object)type == typeof(double))
						{
							P_2 = (ushort)(double)P_1;
						}
						else if ((object)type == typeof(decimal))
						{
							P_2 = (ushort)(decimal)P_1;
						}
						else if ((object)type == typeof(byte))
						{
							P_2 = (ushort)(byte)P_1;
						}
						else if ((object)type == typeof(sbyte))
						{
							P_2 = (ushort)(sbyte)P_1;
						}
						else
						{
							if ((object)type != typeof(string))
							{
								return false;
							}
							ushort result8;
							if (P_4 != null)
							{
								if (!ushort.TryParse(P_1.ToString(), P_3, P_4, out result8))
								{
									return false;
								}
							}
							else if (!ushort.TryParse(P_1.ToString(), out result8))
							{
								return false;
							}
							P_2 = result8;
						}
						return true;
					}
					if ((object)P_0 == typeof(byte))
					{
						if ((object)type == typeof(sbyte))
						{
							P_2 = (byte)(sbyte)P_1;
						}
						else if ((object)type == typeof(int))
						{
							P_2 = (byte)(int)P_1;
						}
						else if ((object)type == typeof(uint))
						{
							P_2 = (byte)(uint)P_1;
						}
						else if ((object)type == typeof(long))
						{
							P_2 = (byte)(long)P_1;
						}
						else if ((object)type == typeof(ulong))
						{
							P_2 = (byte)(ulong)P_1;
						}
						else if ((object)type == typeof(float))
						{
							P_2 = (byte)(float)P_1;
						}
						else if ((object)type == typeof(double))
						{
							P_2 = (byte)(double)P_1;
						}
						else if ((object)type == typeof(decimal))
						{
							P_2 = (byte)(decimal)P_1;
						}
						else if ((object)type == typeof(short))
						{
							P_2 = (byte)(short)P_1;
						}
						else if ((object)type == typeof(ushort))
						{
							P_2 = (byte)(ushort)P_1;
						}
						else
						{
							if ((object)type != typeof(string))
							{
								return false;
							}
							byte result9;
							if (P_4 != null)
							{
								if (!byte.TryParse(P_1.ToString(), P_3, P_4, out result9))
								{
									return false;
								}
							}
							else if (!byte.TryParse(P_1.ToString(), out result9))
							{
								return false;
							}
							P_2 = result9;
						}
						return true;
					}
					if ((object)P_0 == typeof(sbyte))
					{
						if ((object)type == typeof(byte))
						{
							P_2 = (sbyte)(byte)P_1;
						}
						else if ((object)type == typeof(int))
						{
							P_2 = (sbyte)(int)P_1;
						}
						else if ((object)type == typeof(uint))
						{
							P_2 = (sbyte)(uint)P_1;
						}
						else if ((object)type == typeof(long))
						{
							P_2 = (sbyte)(long)P_1;
						}
						else if ((object)type == typeof(ulong))
						{
							P_2 = (sbyte)(ulong)P_1;
						}
						else if ((object)type == typeof(float))
						{
							P_2 = (sbyte)(float)P_1;
						}
						else if ((object)type == typeof(double))
						{
							P_2 = (sbyte)(double)P_1;
						}
						else if ((object)type == typeof(decimal))
						{
							P_2 = (sbyte)(decimal)P_1;
						}
						else if ((object)type == typeof(short))
						{
							P_2 = (sbyte)(short)P_1;
						}
						else if ((object)type == typeof(ushort))
						{
							P_2 = (sbyte)(ushort)P_1;
						}
						else
						{
							if ((object)type != typeof(string))
							{
								return false;
							}
							sbyte result10;
							if (P_4 != null)
							{
								if (!sbyte.TryParse(P_1.ToString(), P_3, P_4, out result10))
								{
									return false;
								}
							}
							else if (!sbyte.TryParse(P_1.ToString(), out result10))
							{
								return false;
							}
							P_2 = result10;
						}
						return true;
					}
					if ((object)P_0 == typeof(decimal))
					{
						if ((object)type == typeof(float))
						{
							P_2 = (decimal)(float)P_1;
						}
						else if ((object)type == typeof(double))
						{
							P_2 = (decimal)(double)P_1;
						}
						else if ((object)type == typeof(int))
						{
							P_2 = (decimal)(int)P_1;
						}
						else if ((object)type == typeof(long))
						{
							P_2 = (decimal)(long)P_1;
						}
						else if ((object)type == typeof(uint))
						{
							P_2 = (decimal)(uint)P_1;
						}
						else if ((object)type == typeof(ulong))
						{
							P_2 = (decimal)(ulong)P_1;
						}
						else if ((object)type == typeof(short))
						{
							P_2 = (decimal)(short)P_1;
						}
						else if ((object)type == typeof(ushort))
						{
							P_2 = (decimal)(ushort)P_1;
						}
						else if ((object)type == typeof(byte))
						{
							P_2 = (decimal)(byte)P_1;
						}
						else if ((object)type == typeof(sbyte))
						{
							P_2 = (decimal)(sbyte)P_1;
						}
						else
						{
							if ((object)type != typeof(string))
							{
								return false;
							}
							decimal result11;
							if (P_4 != null)
							{
								if (!decimal.TryParse(P_1.ToString(), P_3, P_4, out result11))
								{
									return false;
								}
							}
							else if (!decimal.TryParse(P_1.ToString(), out result11))
							{
								return false;
							}
							P_2 = result11;
						}
						return true;
					}
					if ((object)P_0 == typeof(char))
					{
						P_2 = P_1.ToString();
						return true;
					}
					if ((object)P_0 == typeof(Guid))
					{
						if ((object)type == typeof(string))
						{
							P_2 = StringTools.ToGuid((string)P_1);
							return true;
						}
						return false;
					}
					if ((object)P_0 == typeof(AnimationCurve))
					{
						if (!(P_1 is SerializedObject serializedObject))
						{
							P_2 = null;
							return false;
						}
						AnimationCurve animationCurve = (AnimationCurve)(P_2 = new AnimationCurve());
						if (!serializedObject.TryGetDeserializedValue<Keyframe[]>("keys", out var value2))
						{
							return true;
						}
						animationCurve.keys = value2;
						return true;
					}
					if ((object)P_0 == typeof(Keyframe))
					{
						if (!(P_1 is SerializedObject serializedObject2))
						{
							P_2 = default(Keyframe);
							return false;
						}
						Keyframe keyframe = default(Keyframe);
						if (serializedObject2.TryGetDeserializedValue<float>("time", out var value3))
						{
							keyframe.time = value3;
						}
						if (serializedObject2.TryGetDeserializedValue<float>("value", out value3))
						{
							keyframe.value = value3;
						}
						if (serializedObject2.TryGetDeserializedValue<float>("inTangent", out value3))
						{
							keyframe.inTangent = value3;
						}
						if (serializedObject2.TryGetDeserializedValue<float>("outTangent", out value3))
						{
							keyframe.outTangent = value3;
						}
						if (serializedObject2.TryGetDeserializedValue<float>("inWeight", out value3))
						{
							keyframe.inWeight = value3;
						}
						if (serializedObject2.TryGetDeserializedValue<float>("outWeight", out value3))
						{
							keyframe.outWeight = value3;
						}
						if (serializedObject2.TryGetDeserializedValue<WeightedMode>("weightedMode", out var value4))
						{
							keyframe.weightedMode = value4;
						}
						P_2 = keyframe;
						return true;
					}
					if (ReflectionTools.IsArray(P_0))
					{
						Type elementType = P_0.GetElementType();
						if (ReflectionTools.DoesTypeImplement(type, typeof(SerializedObject)))
						{
							if (!(P_1 is SerializedObject serializedObject3))
							{
								return false;
							}
							Array array = Array.CreateInstance(elementType, serializedObject3.count);
							for (int i = 0; i < serializedObject3.count; i++)
							{
								if (UNxZBfhwgZzXDJfZvESaixZzOOhE(elementType, serializedObject3[i].value, out var value5, P_3, P_4))
								{
									array.SetValue(value5, i);
								}
							}
							P_2 = array;
							return true;
						}
						if (ReflectionTools.DoesTypeImplement(type, typeof(IReadOnlyList)))
						{
							if (!(P_1 is IReadOnlyList readOnlyList))
							{
								return false;
							}
							Array array2 = Array.CreateInstance(elementType, readOnlyList.Count);
							for (int j = 0; j < readOnlyList.Count; j++)
							{
								if (UNxZBfhwgZzXDJfZvESaixZzOOhE(elementType, readOnlyList[j], out var value6, P_3, P_4))
								{
									array2.SetValue(value6, j);
								}
							}
							P_2 = array2;
							return true;
						}
						if (ReflectionTools.DoesTypeImplement(type, typeof(IList)))
						{
							if (!(P_1 is IList list))
							{
								return false;
							}
							Array array3 = Array.CreateInstance(elementType, list.Count);
							for (int k = 0; k < list.Count; k++)
							{
								if (UNxZBfhwgZzXDJfZvESaixZzOOhE(elementType, list[k], out var value7, P_3, P_4))
								{
									array3.SetValue(value7, k);
								}
							}
							P_2 = array3;
							return true;
						}
						if (ReflectionTools.DoesTypeImplement(type, typeof(Array)))
						{
							Array array4 = P_1 as Array;
							Array array5 = Array.CreateInstance(elementType, array4.Length);
							for (int l = 0; l < array4.Length; l++)
							{
								if (UNxZBfhwgZzXDJfZvESaixZzOOhE(elementType, array4.GetValue(l), out var value8, P_3, P_4))
								{
									array5.SetValue(value8, l);
								}
							}
							P_2 = array5;
							return true;
						}
						if (ReflectionTools.DoesTypeImplement(type, typeof(IDictionary)))
						{
							Type type2 = ReflectionTools.GetGenericArguments(P_0)[1];
							IDictionary dictionary = P_1 as IDictionary;
							Array array6 = Array.CreateInstance(elementType, dictionary.Count);
							int num = 0;
							foreach (object value18 in dictionary.Values)
							{
								if (UNxZBfhwgZzXDJfZvESaixZzOOhE(type2, value18, out var value9, P_3, P_4))
								{
									array6.SetValue(value9, num);
									num++;
								}
							}
							P_2 = array6;
							return true;
						}
						if (ReflectionTools.DoesTypeImplement(type, typeof(ICollection)))
						{
							ICollection collection = P_1 as ICollection;
							Array array7 = Array.CreateInstance(elementType, collection.Count);
							int num2 = 0;
							foreach (object item in collection)
							{
								if (UNxZBfhwgZzXDJfZvESaixZzOOhE(elementType, item, out var value10, P_3, P_4))
								{
									array7.SetValue(value10, num2);
									num2++;
								}
							}
							P_2 = array7;
							return true;
						}
						if (ReflectionTools.DoesTypeImplement(type, typeof(IEnumerable)))
						{
							IEnumerable enumerable = P_1 as IEnumerable;
							int num3 = 0;
							foreach (object item2 in enumerable)
							{
								_ = item2;
								num3++;
							}
							Array array8 = Array.CreateInstance(elementType, num3);
							int num4 = 0;
							foreach (object item3 in enumerable)
							{
								if (UNxZBfhwgZzXDJfZvESaixZzOOhE(elementType, item3, out var value11, P_3, P_4))
								{
									array8.SetValue(value11, num4);
									num4++;
								}
							}
							P_2 = array8;
							return true;
						}
						return false;
					}
					if (ReflectionTools.IsGenericType(P_0))
					{
						Type genericTypeDefinition = P_0.GetGenericTypeDefinition();
						if (ReflectionTools.DoesTypeImplement(P_0, typeof(IList)))
						{
							Type type3 = ReflectionTools.GetGenericArguments(P_0)[0];
							if (ReflectionTools.DoesTypeImplement(type, typeof(SerializedObject)))
							{
								SerializedObject serializedObject4 = P_1 as SerializedObject;
								IList list2 = (IList)Factory.CreateInstance(genericTypeDefinition.MakeGenericType(type3));
								for (int m = 0; m < serializedObject4.count; m++)
								{
									if (UNxZBfhwgZzXDJfZvESaixZzOOhE(type3, serializedObject4[m].value, out var value12, P_3, P_4))
									{
										list2.Add(value12);
									}
								}
								P_2 = list2;
								return true;
							}
							if (ReflectionTools.DoesTypeImplement(type, typeof(IReadOnlyList)))
							{
								IReadOnlyList readOnlyList2 = P_1 as IReadOnlyList;
								IList list3 = (IList)Factory.CreateInstance(genericTypeDefinition.MakeGenericType(type3));
								for (int n = 0; n < readOnlyList2.Count; n++)
								{
									if (UNxZBfhwgZzXDJfZvESaixZzOOhE(type3, readOnlyList2[n], out var value13, P_3, P_4))
									{
										list3.Add(value13);
									}
								}
								P_2 = list3;
								return true;
							}
							if (ReflectionTools.DoesTypeImplement(type, typeof(IList)))
							{
								IList list4 = P_1 as IList;
								IList list5 = (IList)Factory.CreateInstance(genericTypeDefinition.MakeGenericType(type3));
								for (int num5 = 0; num5 < list4.Count; num5++)
								{
									if (UNxZBfhwgZzXDJfZvESaixZzOOhE(type3, list4[num5], out var value14, P_3, P_4))
									{
										list5.Add(value14);
									}
								}
								P_2 = list5;
								return true;
							}
							if (ReflectionTools.DoesTypeImplement(type, typeof(Array)))
							{
								Array array9 = P_1 as Array;
								IList list6 = (IList)Factory.CreateInstance(genericTypeDefinition.MakeGenericType(type3));
								for (int num6 = 0; num6 < array9.Length; num6++)
								{
									if (UNxZBfhwgZzXDJfZvESaixZzOOhE(type3, array9.GetValue(num6), out var value15, P_3, P_4))
									{
										list6.Add(value15);
									}
								}
								P_2 = list6;
								return true;
							}
							if (ReflectionTools.DoesTypeImplement(type, typeof(IEnumerable)))
							{
								IEnumerable enumerable2 = P_1 as IEnumerable;
								IList list7 = (IList)Factory.CreateInstance(genericTypeDefinition.MakeGenericType(type3));
								foreach (object item4 in enumerable2)
								{
									if (UNxZBfhwgZzXDJfZvESaixZzOOhE(type3, item4, out var value16, P_3, P_4))
									{
										list7.Add(value16);
									}
								}
								P_2 = list7;
								return true;
							}
						}
						else if (ReflectionTools.DoesTypeImplement(genericTypeDefinition, typeof(IDictionary)))
						{
							Type[] genericArguments = ReflectionTools.GetGenericArguments(P_0);
							Type type4 = genericArguments[0];
							Type type5 = genericArguments[1];
							if (!(P_1 is IDictionary dictionary2))
							{
								return false;
							}
							IDictionary dictionary3 = (IDictionary)Factory.CreateInstance(genericTypeDefinition.MakeGenericType(type4, type5));
							foreach (object key2 in dictionary2.Keys)
							{
								if (UNxZBfhwgZzXDJfZvESaixZzOOhE(type4, key2, out var key, P_3, P_4) && UNxZBfhwgZzXDJfZvESaixZzOOhE(type5, dictionary2[key2], out var value17, P_3, P_4))
								{
									dictionary3.Add(key, value17);
								}
							}
							P_2 = dictionary3;
							return true;
						}
					}
				}
				if ((object)P_0 == typeof(object))
				{
					P_2 = P_1;
					return true;
				}
				if (ReflectionTools.DoesTypeImplement(type, typeof(SerializedObject)))
				{
					if (!eIjeZELpFejYtLNGFAvyeZskAPScb(P_0, P_1 as SerializedObject, out P_1))
					{
						return false;
					}
					P_2 = P_1;
					return true;
				}
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
			return false;
		}

		private static bool eIjeZELpFejYtLNGFAvyeZskAPScb(Type P_0, SerializedObject P_1, out object P_2, NumberStyles P_3 = NumberStyles.Any, CultureInfo P_4 = null)
		{
			if (P_1 == null || P_0 == null)
			{
				P_2 = null;
				return false;
			}
			P_2 = Factory.CreateInstance(P_0);
			if (!FTCfxRaTJSciknApGvpHlntYwwQc.TryGetValue(P_0, out var value))
			{
				value = ReflectionTools.GetFields(P_0, ReflectionTools.BindingFlags.Instance | ReflectionTools.BindingFlags.Public | ReflectionTools.BindingFlags.NonPublic).Where(BmGGkbBcLqUElAphdsQXwbbipJNgb._003C_003E9.NKPHUxKOJjJxMXVLnUCusBSGHmkc).ToDictionary(BmGGkbBcLqUElAphdsQXwbbipJNgb._003C_003E9.mnXxMMWhXVQEGuhlBeplOFhbfpniA);
				FTCfxRaTJSciknApGvpHlntYwwQc.Add(P_0, value);
			}
			if (!StPeTproUCsBQNmAYnnkEROWtyHl.TryGetValue(P_0, out var value2))
			{
				value2 = ReflectionTools.GetProperties(P_0, ReflectionTools.BindingFlags.Instance | ReflectionTools.BindingFlags.Public | ReflectionTools.BindingFlags.NonPublic).Where(BmGGkbBcLqUElAphdsQXwbbipJNgb._003C_003E9.OclmGCxXtgdXmnhxZiyQRLesHzae).ToDictionary(BmGGkbBcLqUElAphdsQXwbbipJNgb._003C_003E9.sTlHyqXdTGePEfeSXrmNgKdzWDOnA);
				StPeTproUCsBQNmAYnnkEROWtyHl.Add(P_0, value2);
			}
			foreach (Field item in (IEnumerable<Field>)P_1)
			{
				string name = item.name;
				object value3 = item.value;
				object value5;
				PropertyInfo value6;
				if (value.TryGetValue(name, out var value4))
				{
					if (UNxZBfhwgZzXDJfZvESaixZzOOhE(value4.FieldType, value3, out value5, P_3, P_4))
					{
						value4.SetValue(P_2, value5);
					}
				}
				else if (value2.TryGetValue(name, out value6) && value6.CanWrite && UNxZBfhwgZzXDJfZvESaixZzOOhE(value6.PropertyType, value3, out value5, P_3, P_4))
				{
					value6.SetValue(P_2, value5, null);
				}
			}
			if (P_2 is ISerializationCallbackReceiver serializationCallbackReceiver)
			{
				try
				{
					serializationCallbackReceiver.OnAfterDeserialize();
				}
				catch (Exception ex)
				{
					Logger.LogError(ex.ToString(), requiredThreadSafety: true);
				}
			}
			return true;
		}

		public static SerializedObject FromJson(Type type, string jsonString)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (string.IsNullOrEmpty(jsonString))
			{
				throw new ArgumentNullException("jsonString");
			}
			SerializedObject serializedObject = JsonParser.FromJson<SerializedObject>(jsonString, typeof(SerializedObject));
			if (serializedObject == null || serializedObject.count == 0)
			{
				throw new Exception("No data found in Json string.");
			}
			return serializedObject;
		}

		public static SerializedObject FromXml(Type type, string xmlString)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (string.IsNullOrEmpty(xmlString))
			{
				throw new ArgumentNullException("xmlString");
			}
			LGUdKudrZaaUOAfIKJsudqPyvcfiB lGUdKudrZaaUOAfIKJsudqPyvcfiB = new LGUdKudrZaaUOAfIKJsudqPyvcfiB(xmlString);
			if (!lGUdKudrZaaUOAfIKJsudqPyvcfiB.rhWsuCZJeodzRQuQsdBHfAqTPwWp)
			{
				throw new Exception("Failed to parse XML string.");
			}
			if (lGUdKudrZaaUOAfIKJsudqPyvcfiB.MxerPDocZYbtGHAGnmbXXUmyFEFPA.eVXBGTyrZsjtmqDQzPDkvLkNvBCI == 0)
			{
				throw new Exception("No data found in XML string.");
			}
			if (!((lGUdKudrZaaUOAfIKJsudqPyvcfiB.MxerPDocZYbtGHAGnmbXXUmyFEFPA.ZEoYAjkdGNWFwycyFngRwtMwVthb(type.Name) ?? throw new Exception("Main element not found in XML string.")).mEsgfHzKwLFhAyeDYZofrDaBsVFD() is SerializedObject { count: not 0 } serializedObject))
			{
				throw new Exception("No data found in XML string.");
			}
			return serializedObject;
		}
	}
}
