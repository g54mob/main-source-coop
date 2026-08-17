using System;
using System.Collections.Generic;
using Den.Tools;
using UnityEngine;

namespace MapMagic.Expose
{
	public class Calculator
	{
		public enum DataType
		{
			Null = 0,
			Operator = 1,
			Reference = 2,
			Number = 3
		}

		public struct Vector
		{
			public float x;

			public float y;

			public float z;

			public float w;

			public UnityEngine.Object uobj;

			public float this[int i]
			{
				get
				{
					return i switch
					{
						0 => x, 
						1 => y, 
						2 => z, 
						3 => w, 
						_ => 0f, 
					};
				}
				set
				{
					switch (i)
					{
					case 0:
						x = value;
						break;
					case 1:
						y = value;
						break;
					case 2:
						z = value;
						break;
					case 3:
						w = value;
						break;
					}
				}
			}

			private Vector(float x, float y, float z, float w)
			{
				this.x = x;
				this.y = y;
				this.z = z;
				this.w = w;
				uobj = null;
			}

			public static explicit operator Vector(float f)
			{
				return new Vector(f, f, f, f);
			}

			public static explicit operator Vector(int i)
			{
				return new Vector(i, i, i, i);
			}

			public static explicit operator Vector(double d)
			{
				return new Vector((float)d, (float)d, (float)d, (float)d);
			}

			public static explicit operator Vector(Vector2 v)
			{
				return new Vector(v.x, v.y, v.y, 0f);
			}

			public static explicit operator Vector(Vector2D v)
			{
				return new Vector(v.x, v.z, v.z, 0f);
			}

			public static explicit operator Vector(Coord c)
			{
				return new Vector(c.x, c.z, c.z, 0f);
			}

			public static explicit operator Vector(Vector3 v)
			{
				return new Vector(v.x, v.y, v.z, 0f);
			}

			public static explicit operator Vector(Vector4 v)
			{
				return new Vector(v.x, v.y, v.z, v.w);
			}

			public static explicit operator Vector(Color c)
			{
				return new Vector(c.r, c.g, c.b, c.a);
			}

			public static explicit operator Vector(bool b)
			{
				if (!b)
				{
					return new Vector(0f, 0f, 0f, 0f);
				}
				return new Vector(1f, 1f, 1f, 1f);
			}

			public static explicit operator Vector(UnityEngine.Object o)
			{
				Vector result = ((o != null) ? new Vector(1f, 1f, 1f, 1f) : new Vector(0f, 0f, 0f, 0f));
				result.uobj = o;
				return result;
			}

			public static explicit operator float(Vector v)
			{
				return v.x;
			}

			public static explicit operator int(Vector v)
			{
				return Mathf.RoundToInt(v.x);
			}

			public static explicit operator double(Vector v)
			{
				return v.x;
			}

			public static explicit operator Vector2(Vector v)
			{
				return new Vector2(v.x, v.y);
			}

			public static explicit operator Vector2D(Vector v)
			{
				return new Vector2D(v.x, v.z);
			}

			public static explicit operator Coord(Vector v)
			{
				return new Coord(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.z));
			}

			public static explicit operator Vector3(Vector v)
			{
				return new Vector3(v.x, v.y, v.z);
			}

			public static explicit operator Vector4(Vector v)
			{
				return new Vector4(v.x, v.y, v.z, v.w);
			}

			public static explicit operator Color(Vector v)
			{
				return new Color(v.x, v.y, v.z, v.w);
			}

			public static explicit operator bool(Vector v)
			{
				return v.x > 1E-05f;
			}

			public static explicit operator UnityEngine.Object(Vector v)
			{
				if (Mathf.Abs(v.x) < 1E-05f && Mathf.Abs(v.y) < 1E-05f && Mathf.Abs(v.z) < 1E-05f && Mathf.Abs(v.w) < 1E-05f)
				{
					return null;
				}
				return v.uobj;
			}

			public Vector(object obj)
			{
				if (!(obj is float num))
				{
					if (!(obj is int num2))
					{
						if (!(obj is double num3))
						{
							if (!(obj is Vector2 vector))
							{
								if (!(obj is Vector2D vector2D))
								{
									if (!(obj is Coord coord))
									{
										if (!(obj is Vector3 vector2))
										{
											if (!(obj is Vector4 vector3))
											{
												if (!(obj is Color color))
												{
													if (!(obj is bool flag))
													{
														if (obj is UnityEngine.Object obj2)
														{
															this = (Vector)obj2;
														}
														else
														{
															this = default(Vector);
														}
													}
													else
													{
														this = (Vector)flag;
													}
												}
												else
												{
													this = (Vector)color;
												}
											}
											else
											{
												this = (Vector)vector3;
											}
										}
										else
										{
											this = (Vector)vector2;
										}
									}
									else
									{
										this = (Vector)coord;
									}
								}
								else
								{
									this = (Vector)vector2D;
								}
							}
							else
							{
								this = (Vector)vector;
							}
						}
						else
						{
							this = (Vector)num3;
						}
					}
					else
					{
						this = (Vector)num2;
					}
				}
				else
				{
					this = (Vector)num;
				}
			}

			public object Convert(Type type)
			{
				if (type == typeof(float))
				{
					return (float)this;
				}
				if (type == typeof(int))
				{
					return (int)this;
				}
				if (type == typeof(double))
				{
					return (double)this;
				}
				if (type == typeof(Vector2))
				{
					return (Vector2)this;
				}
				if (type == typeof(Vector2D))
				{
					return (Vector2D)this;
				}
				if (type == typeof(Coord))
				{
					return (Coord)this;
				}
				if (type == typeof(Vector3))
				{
					return (Vector3)this;
				}
				if (type == typeof(Vector4))
				{
					return (Vector4)this;
				}
				if (type == typeof(Color))
				{
					return (Color)this;
				}
				if (type == typeof(bool))
				{
					return (bool)this;
				}
				if (typeof(UnityEngine.Object).IsAssignableFrom(type))
				{
					return (UnityEngine.Object)this;
				}
				if (typeof(Enum).IsAssignableFrom(type))
				{
					return (int)this;
				}
				return null;
			}

			public object ConvertToChannel(object wholeVal, int channel, Type type)
			{
				Vector vector = new Vector(wholeVal);
				vector[channel] = (float)this;
				return vector.Convert(type);
			}

			public void Unify(int channel)
			{
				w = (z = (y = (x = this[channel])));
			}

			public static Vector operator +(Vector c1, Vector c2)
			{
				c1.x += c2.x;
				c1.y += c2.y;
				c1.z += c2.z;
				c1.w += c2.w;
				return c1;
			}

			public static Vector operator -(Vector c1, Vector c2)
			{
				c1.x -= c2.x;
				c1.y -= c2.y;
				c1.z -= c2.z;
				c1.w -= c2.w;
				return c1;
			}

			public static Vector operator *(Vector c1, Vector c2)
			{
				c1.x *= c2.x;
				c1.y *= c2.y;
				c1.z *= c2.z;
				c1.w *= c2.w;
				return c1;
			}

			public static Vector operator /(Vector c1, Vector c2)
			{
				if (c2.x != 0f)
				{
					c1.x /= c2.x;
				}
				if (c2.y != 0f)
				{
					c1.y /= c2.y;
				}
				if (c2.z != 0f)
				{
					c1.z /= c2.z;
				}
				if (c2.w != 0f)
				{
					c1.w /= c2.w;
				}
				return c1;
			}

			public static Vector operator ^(Vector c1, Vector c2)
			{
				c1.x = (float)Math.Pow(c1.x, c2.x);
				c1.y = (float)Math.Pow(c1.y, c2.y);
				c1.z = (float)Math.Pow(c1.z, c2.z);
				c1.w = (float)Math.Pow(c1.w, c2.w);
				return c1;
			}
		}

		public DataType dataType;

		public char action;

		public Calculator left;

		public Calculator right;

		public string reference;

		public float number;

		public string error;

		private static readonly char[] actionChars = new char[5] { '+', '-', '*', '/', '^' };

		public Vector Calculate(Dictionary<string, object> refVals = null)
		{
			if (dataType == DataType.Operator)
			{
				Vector vector = left.Calculate(refVals);
				Vector vector2 = right.Calculate(refVals);
				return Action(vector, vector2, action);
			}
			if (dataType == DataType.Reference)
			{
				if (refVals.TryGetValue(reference, out var value))
				{
					return new Vector(value);
				}
				return (Vector)0;
			}
			if (dataType == DataType.Number)
			{
				return (Vector)number;
			}
			throw new Exception("Could not perform calculation: operator type is Null");
		}

		public Vector Calculate(Override ovd)
		{
			if (dataType == DataType.Operator)
			{
				Vector vector = left.Calculate(ovd);
				Vector vector2 = right.Calculate(ovd);
				return Action(vector, vector2, action);
			}
			if (dataType == DataType.Reference)
			{
				if (ovd.TryGetValue(reference, out var _, out var obj))
				{
					return new Vector(obj);
				}
				return (Vector)0;
			}
			if (dataType == DataType.Number)
			{
				return (Vector)number;
			}
			throw new Exception("Could not perform calculation: operator type is Null");
		}

		public static Vector Action(Vector left, Vector right, char action)
		{
			return action switch
			{
				'+' => left + right, 
				'-' => left - right, 
				'*' => left * right, 
				'/' => left / right, 
				'^' => left ^ right, 
				'.' => (Vector)left[(int)right], 
				_ => default(Vector), 
			};
		}

		public static Calculator Parse(string str, bool prevActionIsDot = false)
		{
			Calculator calculator = new Calculator();
			if (ContainsAction(str))
			{
				char c = ' ';
				int num = -1;
				int num2 = 2147483647;
				foreach (var (c2, num3, num4) in StringActions(str))
				{
					if (num4 < num2)
					{
						c = c2;
						num = num3;
						num2 = num4;
					}
				}
				if (num == -1 || c == ' ')
				{
					return null;
				}
				string str2 = str.Substring(0, num);
				string str3 = str.Substring(num + 1);
				calculator.dataType = DataType.Operator;
				calculator.action = c;
				calculator.left = Parse(str2);
				calculator.right = Parse(str3, calculator.action == '.');
				return calculator;
			}
			if (prevActionIsDot)
			{
				int num5 = StringToChannel(str);
				if (num5 >= 0)
				{
					calculator.dataType = DataType.Number;
					calculator.number = num5;
					return calculator;
				}
			}
			string text = StringToReference(str);
			if (text != null)
			{
				calculator.dataType = DataType.Reference;
				calculator.reference = text;
				return calculator;
			}
			if (float.TryParse(str.Replace('(', ' ').Replace(')', ' '), out var result))
			{
				calculator.dataType = DataType.Number;
				calculator.number = result;
				return calculator;
			}
			calculator.dataType = DataType.Null;
			if (str.Length == 0 || str == " ")
			{
				calculator.error = "Empty expression member";
			}
			else
			{
				calculator.error = "Could not parse '" + str + "'";
			}
			return calculator;
		}

		internal static bool ContainsAction(string str)
		{
			if (str.IndexOfAny(actionChars) >= 0)
			{
				return true;
			}
			int num = str.IndexOf('.');
			if (num >= 0 && num < str.Length - 1 && !char.IsDigit(str[num + 1]))
			{
				return true;
			}
			return false;
		}

		internal static IEnumerable<(char chr, int pos, int priority)> StringActions(string str)
		{
			int order = 0;
			int bracket = 0;
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				if (c == '(')
				{
					bracket++;
				}
				if (c == ')')
				{
					bracket--;
				}
				if (c == '+' || c == '-')
				{
					flag = true;
				}
				if (c == '*' || c == '/')
				{
					flag = true;
					flag2 = true;
				}
				if (c == '^')
				{
					flag = true;
					flag3 = true;
				}
				if (c == '.' && i < str.Length - 1 && !char.IsDigit(str[i + 1]))
				{
					flag = true;
					flag4 = true;
				}
				if (flag)
				{
					int item = bracket * 10000000 + (flag4 ? 1000000 : 0) + (flag3 ? 100000 : 0) + (flag2 ? 10000 : 0) + order;
					yield return (chr: c, pos: i, priority: item);
					order--;
				}
			}
		}

		internal static string StringToReference(string str)
		{
			string text = null;
			bool flag = false;
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				if (text == null)
				{
					if (c != ' ' && c != '(')
					{
						if (char.IsDigit(c))
						{
							return null;
						}
						if (char.IsLetter(c))
						{
							text = c.ToString() ?? "";
						}
					}
				}
				else if (char.IsLetterOrDigit(c))
				{
					if (flag)
					{
						return null;
					}
					text += c;
				}
				else
				{
					if (c != ' ' && c != ')')
					{
						return null;
					}
					flag = true;
				}
			}
			return text;
		}

		internal static int StringToChannel(string str)
		{
			int num = -1;
			foreach (char c in str)
			{
				if (c != ' ')
				{
					if (num >= 0)
					{
						return -1;
					}
					switch (c)
					{
					case 'r':
					case 'x':
						num = 0;
						break;
					case 'g':
					case 'y':
						num = 1;
						break;
					case 'b':
					case 'z':
						num = 2;
						break;
					case 'a':
					case 'w':
						num = 3;
						break;
					default:
						return -1;
					}
				}
			}
			return num;
		}

		public bool CheckValidity(out string error)
		{
			error = null;
			if (dataType == DataType.Null)
			{
				error = this.error;
				return false;
			}
			if (dataType == DataType.Operator)
			{
				if (action != '+' && action != '-' && action != '*' && action != '/' && action != '^' && action != '.')
				{
					error = "Unknown operator type " + action;
					return false;
				}
				if (left == null)
				{
					error = "Left operator not defined";
					return false;
				}
				if (right == null)
				{
					error = "Right operator not defined";
					return false;
				}
				if (!left.CheckValidity(out var text))
				{
					error = text;
					return false;
				}
				if (!right.CheckValidity(out var text2))
				{
					error = text2;
					return false;
				}
			}
			if (dataType == DataType.Reference && reference == null)
			{
				error = "Reference not defined";
				return false;
			}
			return true;
		}

		public string CheckOverrideAssign(Override ovd)
		{
			if (dataType == DataType.Reference && (!ovd.Contains(reference) || !ovd.Contains(reference)))
			{
				return reference;
			}
			if (dataType == DataType.Operator)
			{
				string text = left.CheckOverrideAssign(ovd);
				if (text != null)
				{
					return text;
				}
				string text2 = right.CheckOverrideAssign(ovd);
				if (text2 != null)
				{
					return text2;
				}
			}
			return null;
		}

		public IEnumerable<string> AllRefenrences()
		{
			if (dataType == DataType.Reference)
			{
				yield return reference;
			}
			if (dataType != DataType.Operator)
			{
				yield break;
			}
			foreach (string item in left.AllRefenrences())
			{
				yield return item;
			}
			foreach (string item2 in right.AllRefenrences())
			{
				yield return item2;
			}
		}

		public bool ContainsReference(string reference)
		{
			if (dataType == DataType.Reference)
			{
				return this.reference == reference;
			}
			if (dataType == DataType.Operator)
			{
				if (left.ContainsReference(reference))
				{
					return true;
				}
				if (right.ContainsReference(reference))
				{
					return true;
				}
			}
			return false;
		}

		public override string ToString()
		{
			if (dataType == DataType.Operator)
			{
				bool flag = left.dataType == DataType.Operator;
				bool flag2 = right.dataType == DataType.Operator;
				if (!flag && !flag2)
				{
					return $"{left.ToString()} {action} {right.ToString()}";
				}
				if (flag && !flag2)
				{
					return $"({left.ToString()}) {action} {right.ToString()}";
				}
				if (!flag && flag2)
				{
					return $"{left.ToString()} {action} ({right.ToString()})";
				}
				return $"({left.ToString()}) {action} ({right.ToString()})";
			}
			if (dataType == DataType.Reference)
			{
				return reference;
			}
			return number.ToString();
		}
	}
}
