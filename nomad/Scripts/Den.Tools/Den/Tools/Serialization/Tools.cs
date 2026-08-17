using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Den.Tools.Serialization
{
	public static class Tools
	{
		public static string ReadBlock(string str, int start, char blockOpen, char blockClose, bool skipStrings = true)
		{
			int num = 0;
			bool flag = false;
			bool flag2 = false;
			List<char> list = new List<char>();
			for (int i = start; i < str.Length; i++)
			{
				char c = str[i];
				if (skipStrings && c == '"')
				{
					if (flag2 && i != 0 && str[i - 1] == '\\')
					{
						list.Add('"');
						continue;
					}
					flag2 = !flag2;
				}
				if (!flag2)
				{
					if (c == blockOpen)
					{
						num++;
						flag = true;
					}
					if (flag)
					{
						list.Add(c);
						if (c == blockClose)
						{
							num--;
						}
						if (num == 0)
						{
							break;
						}
					}
				}
				else
				{
					list.Add(c);
				}
			}
			return new string(list.ToArray());
		}

		public static object SplitBlock(string str, char splitSymbol, char blockOpen, char blockClose, bool skipStrings = true, bool trim = true)
		{
			bool flag = false;
			List<object> list = new List<object>();
			List<char> list2 = new List<char>();
			Stack<List<object>> stack = new Stack<List<object>>();
			Stack<List<char>> stack2 = new Stack<List<char>>();
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				if (skipStrings && c == '"')
				{
					if (flag && i != 0 && str[i - 1] == '\\')
					{
						list2.Add('"');
						continue;
					}
					flag = !flag;
				}
				if (flag)
				{
					list2.Add(c);
				}
				else if (c == splitSymbol)
				{
					AddString(list, list2, trim);
					list2.Clear();
				}
				else if (c == blockOpen)
				{
					stack.Push(list);
					list = new List<object>();
					stack2.Push(list2);
					list2 = new List<char>();
				}
				else if (c == blockClose)
				{
					AddString(list, list2, trim);
					object[] item = list.ToArray();
					list = stack.Pop();
					list2 = stack2.Pop();
					list.Add(item);
				}
				else
				{
					list2.Add(c);
				}
			}
			AddString(list, list2, trim);
			return list.ToArray();
		}

		private static void AddString(List<object> strList, List<char> charArr, bool trim = true)
		{
			string text = new string(charArr.ToArray());
			if (trim)
			{
				text = text.Trim();
			}
			if (text.Length != 0)
			{
				strList.Add(text);
			}
		}

		public static int CheckEquality(object srcObj, object dstObj, HashSet<object> used = null)
		{
			if (used == null)
			{
				used = new HashSet<object>();
			}
			if (used.Contains(srcObj))
			{
				return 0;
			}
			used.Add(srcObj);
			int num = 1;
			if (srcObj == null && dstObj == null)
			{
				return 1;
			}
			if (srcObj == null && dstObj != null)
			{
				throw new Exception("CheckEquality: Src is null while dst is " + dstObj);
			}
			if (srcObj != null && dstObj == null)
			{
				throw new Exception("CheckEquality: Dst is null while src is " + srcObj);
			}
			Type type = srcObj.GetType();
			if (type.IsSubclassOf(typeof(UnityEngine.Object)))
			{
				return 0;
			}
			if (type.IsSubclassOf(typeof(MemberInfo)))
			{
				return 0;
			}
			if (typeof(IDictionary).IsAssignableFrom(type))
			{
				return 0;
			}
			if (type != dstObj.GetType())
			{
				throw new Exception("CheckEquality: Types differ " + type?.ToString() + " and " + dstObj.GetType());
			}
			if (!type.IsValueType && type != typeof(string) && srcObj == dstObj)
			{
				throw new Exception("CheckEquality: Are same " + srcObj);
			}
			if (type.IsArray)
			{
				Array array = (Array)srcObj;
				Array array2 = (Array)dstObj;
				if (type.GetElementType().IsValueType)
				{
					for (int i = 0; i < array.Length; i++)
					{
						if (!array.GetValue(i).Equals(array2.GetValue(i)))
						{
							throw new Exception("CheckEquality: arrays not equal " + srcObj?.ToString() + " and " + array2);
						}
					}
				}
				else
				{
					for (int j = 0; j < array.Length; j++)
					{
						num += CheckEquality(array.GetValue(j), array2.GetValue(j), used);
					}
				}
			}
			else
			{
				FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (FieldInfo fieldInfo in fields)
				{
					if (fieldInfo.IsLiteral || fieldInfo.FieldType.IsPointer)
					{
						continue;
					}
					if (fieldInfo.FieldType.IsValueType)
					{
						if (!fieldInfo.GetValue(srcObj).Equals(fieldInfo.GetValue(dstObj)))
						{
							throw new Exception("CheckEquality: not equal " + fieldInfo.Name + " " + fieldInfo.GetValue(srcObj)?.ToString() + " and " + fieldInfo.GetValue(dstObj));
						}
					}
					else
					{
						num += CheckEquality(fieldInfo.GetValue(srcObj), fieldInfo.GetValue(dstObj), used);
					}
				}
			}
			return num;
		}
	}
}
