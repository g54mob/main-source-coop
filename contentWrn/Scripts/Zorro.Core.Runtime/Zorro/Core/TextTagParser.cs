using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;

namespace Zorro.Core
{
	public class TextTagParser<T> where T : TextTag
	{
		public class RegisteredTag
		{
			public Type type;
		}

		private static char tagStart = '<';

		private static char tagEnd = '>';

		private static char tagEndIndicator = '/';

		private Dictionary<string, RegisteredTag> m_registeredTags = new Dictionary<string, RegisteredTag>();

		private List<(string, T)> m_openTags = new List<(string, T)>();

		public TextTagParser()
		{
			(Type, TagAttribute)[] classesWithAttribute = ReflectionUtility.GetClassesWithAttribute<TagAttribute>(ReflectionUtility.GetClassesThatDeriveFrom(typeof(T)));
			for (int i = 0; i < classesWithAttribute.Length; i++)
			{
				(Type, TagAttribute) tuple = classesWithAttribute[i];
				Dictionary<string, RegisteredTag> registeredTags = m_registeredTags;
				string tag = tuple.Item2.Tag;
				RegisteredTag registeredTag = new RegisteredTag();
				(registeredTag.type, _) = tuple;
				registeredTags.Add(tag, registeredTag);
			}
		}

		public string ParseText(string text, out List<T> tags)
		{
			string text2 = "";
			List<(int2, string)> list = new List<(int2, string)>();
			int? num = null;
			string text3 = "";
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (c == tagStart)
				{
					if (num.HasValue)
					{
						throw new Exception("Tag start without ending last tag");
					}
					num = i;
				}
				else if (c == tagEnd)
				{
					if (!num.HasValue)
					{
						throw new Exception("Tag end without start");
					}
					list.Add((new int2(num.Value, i), text3));
					text3 = "";
					num = null;
				}
				else if (!num.HasValue)
				{
					text2 += c;
					text3 += c;
				}
			}
			if (num.HasValue)
			{
				throw new Exception("Tag start without ending last tag");
			}
			m_openTags = new List<(string, T)>();
			List<T> list2 = new List<T>();
			foreach (var item3 in list)
			{
				int2 item = item3.Item1;
				string item2 = item3.Item2;
				string text4 = text.Substring(item.x, item.y - item.x + 1);
				bool num2 = text4[1] == tagEndIndicator;
				string text5 = "";
				text5 = (num2 ? text4.Substring(2, text4.Length - 3) : text4.Substring(1, text4.Length - 2));
				string param = "";
				if (text5.Contains('='))
				{
					string[] array = text5.Split('=');
					text5 = array[0];
					param = array[1];
				}
				if (!m_registeredTags.ContainsKey(text5))
				{
					throw new Exception("Tag " + text5 + " is not registered");
				}
				RegisteredTag registeredTag = m_registeredTags[text5];
				if (!num2)
				{
					T val = (T)Activator.CreateInstance(registeredTag.type);
					m_openTags.Add((text5, val));
					list2.Add(val);
					val.ParseParameter(param);
					val.Setup(item2);
				}
				else
				{
					list2.Add(CloseTag(text5, item2));
				}
			}
			List<string> list3 = new List<string>();
			foreach (var openTag in m_openTags)
			{
				list3.Add(openTag.Item1);
			}
			int num3 = 0;
			foreach (string item4 in list3)
			{
				string text6 = "";
				if (num3 == 0)
				{
					text6 = text.Substring(list.Last().Item1.y + 1);
				}
				list2.Add(CloseTag(item4, text6));
				list.Add((new int2(text.Length - 1, text.Length + item4.Length + 2), text6));
				text = text + "</" + item4 + ">";
				num3++;
			}
			if (m_openTags.Count > 0)
			{
				throw new Exception("Tag " + m_openTags.First().Item1 + " was not closed but requires to it");
			}
			if (list2.Count > 0)
			{
				string lastText = text.Substring(list.Last().Item1.y + 1);
				list2.Last().SetLastText(lastText);
			}
			tags = list2;
			return text2;
		}

		private T CloseTag(string tagName, string textBefore)
		{
			if (m_openTags.Count == 0)
			{
				throw new Exception("Closing tag " + tagName + " without opening tag");
			}
			RegisteredTag registeredTag = m_registeredTags[tagName];
			if (!TryGetOpenTag(out var openTag))
			{
				throw new Exception("Closing tag " + tagName + " without opening tag");
			}
			T obj = (T)Activator.CreateInstance(registeredTag.type);
			obj.Setup(textBefore, openTag.Item2);
			m_openTags.Remove(openTag);
			return obj;
			bool TryGetOpenTag(out (string, T) reference)
			{
				reference = default((string, T));
				foreach (var openTag2 in m_openTags)
				{
					if (openTag2.Item2.GetType() == registeredTag.type)
					{
						reference = openTag2;
						return true;
					}
				}
				return false;
			}
		}
	}
}
