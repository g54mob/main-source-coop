using System;
using UnityEngine;

namespace Den.Tools.Splines
{
	[Serializable]
	public struct Segment
	{
		public Node start;

		public Node end;

		public StructArray lengthToPercentLut;

		public float length;

		private static double[][] abscissaeLut = new double[18][]
		{
			new double[0],
			new double[0],
			new double[2] { -0.5773502691896257, 0.5773502691896257 },
			new double[3] { 0.0, -0.7745966692414834, 0.7745966692414834 },
			new double[4] { -0.33998104358485626, 0.33998104358485626, -0.8611363115940526, 0.8611363115940526 },
			new double[5] { 0.0, -0.5384693101056831, 0.5384693101056831, -0.906179845938664, 0.906179845938664 },
			new double[6] { 0.6612093864662645, -0.6612093864662645, -0.2386191860831969, 0.2386191860831969, -0.932469514203152, 0.932469514203152 },
			new double[7] { 0.0, 0.4058451513773972, -0.4058451513773972, -0.7415311855993945, 0.7415311855993945, -0.9491079123427585, 0.9491079123427585 },
			new double[8] { -0.1834346424956498, 0.1834346424956498, -0.525532409916329, 0.525532409916329, -0.7966664774136267, 0.7966664774136267, -0.9602898564975363, 0.9602898564975363 },
			new double[9] { 0.0, -0.8360311073266358, 0.8360311073266358, -0.9681602395076261, 0.9681602395076261, -0.3242534234038089, 0.3242534234038089, -0.6133714327005904, 0.6133714327005904 },
			new double[10] { -0.14887433898163122, 0.14887433898163122, -0.4333953941292472, 0.4333953941292472, -0.6794095682990244, 0.6794095682990244, -0.8650633666889845, 0.8650633666889845, -0.9739065285171717, 0.9739065285171717 },
			new double[11]
			{
				0.0, -0.26954315595234496, 0.26954315595234496, -0.5190961292068118, 0.5190961292068118, -0.7301520055740494, 0.7301520055740494, -0.8870625997680953, 0.8870625997680953, -0.978228658146057,
				0.978228658146057
			},
			new double[12]
			{
				-0.1252334085114689, 0.1252334085114689, -0.3678314989981802, 0.3678314989981802, -0.5873179542866175, 0.5873179542866175, -0.7699026741943047, 0.7699026741943047, -0.9041172563704749, 0.9041172563704749,
				-0.9815606342467192, 0.9815606342467192
			},
			new double[13]
			{
				0.0, -0.2304583159551348, 0.2304583159551348, -0.44849275103644687, 0.44849275103644687, -0.6423493394403402, 0.6423493394403402, -0.8015780907333099, 0.8015780907333099, -0.9175983992229779,
				0.9175983992229779, -0.9841830547185881, 0.9841830547185881
			},
			new double[14]
			{
				-0.10805494870734367, 0.10805494870734367, -0.31911236892788974, 0.31911236892788974, -0.5152486363581541, 0.5152486363581541, -0.6872929048116855, 0.6872929048116855, -0.827201315069765, 0.827201315069765,
				-0.9284348836635735, 0.9284348836635735, -0.9862838086968123, 0.9862838086968123
			},
			new double[15]
			{
				0.0, -0.20119409399743451, 0.20119409399743451, -0.3941513470775634, 0.3941513470775634, -0.5709721726085388, 0.5709721726085388, -0.7244177313601701, 0.7244177313601701, -0.8482065834104272,
				0.8482065834104272, -0.937273392400706, 0.937273392400706, -0.9879925180204854, 0.9879925180204854
			},
			new double[16]
			{
				-0.09501250983763744, 0.09501250983763744, -0.2816035507792589, 0.2816035507792589, -0.45801677765722737, 0.45801677765722737, -0.6178762444026438, 0.6178762444026438, -0.755404408355003, 0.755404408355003,
				-0.8656312023878318, 0.8656312023878318, -0.9445750230732326, 0.9445750230732326, -0.9894009349916499, 0.9894009349916499
			},
			new double[17]
			{
				0.0, -0.17848418149584785, 0.17848418149584785, -0.3512317634538763, 0.3512317634538763, -0.5126905370864769, 0.5126905370864769, -0.6576711592166907, 0.6576711592166907, -0.7815140038968014,
				0.7815140038968014, -0.8802391537269859, 0.8802391537269859, -0.9506755217687678, 0.9506755217687678, -0.9905754753144174, 0.9905754753144174
			}
		};

		private static readonly double[][] weightsLut = new double[18][]
		{
			new double[0],
			new double[0],
			new double[2] { 1.0, 1.0 },
			new double[3]
			{
				8.0 / 9.0,
				5.0 / 9.0,
				5.0 / 9.0
			},
			new double[4] { 0.6521451548625461, 0.6521451548625461, 0.34785484513745385, 0.34785484513745385 },
			new double[5]
			{
				128.0 / 225.0,
				0.47862867049936647,
				0.47862867049936647,
				0.23692688505618908,
				0.23692688505618908
			},
			new double[6] { 0.3607615730481386, 0.3607615730481386, 0.46791393457269104, 0.46791393457269104, 0.17132449237917036, 0.17132449237917036 },
			new double[7] { 0.4179591836734694, 0.3818300505051189, 0.3818300505051189, 0.27970539148927664, 0.27970539148927664, 0.1294849661688697, 0.1294849661688697 },
			new double[8] { 0.362683783378362, 0.362683783378362, 0.31370664587788727, 0.31370664587788727, 0.22238103445337448, 0.22238103445337448, 0.10122853629037626, 0.10122853629037626 },
			new double[9] { 0.3302393550012598, 0.1806481606948574, 0.1806481606948574, 0.08127438836157441, 0.08127438836157441, 0.31234707704000286, 0.31234707704000286, 0.26061069640293544, 0.26061069640293544 },
			new double[10] { 0.29552422471475287, 0.29552422471475287, 0.26926671930999635, 0.26926671930999635, 0.21908636251598204, 0.21908636251598204, 0.1494513491505806, 0.1494513491505806, 0.06667134430868814, 0.06667134430868814 },
			new double[11]
			{
				0.2729250867779006, 0.26280454451024665, 0.26280454451024665, 0.23319376459199048, 0.23319376459199048, 0.18629021092773426, 0.18629021092773426, 0.1255803694649046, 0.1255803694649046, 0.05566856711617366,
				0.05566856711617366
			},
			new double[12]
			{
				0.24914704581340277, 0.24914704581340277, 0.2334925365383548, 0.2334925365383548, 0.20316742672306592, 0.20316742672306592, 0.16007832854334622, 0.16007832854334622, 0.10693932599531843, 0.10693932599531843,
				0.04717533638651183, 0.04717533638651183
			},
			new double[13]
			{
				0.2325515532308739, 0.22628318026289723, 0.22628318026289723, 0.2078160475368885, 0.2078160475368885, 0.17814598076194574, 0.17814598076194574, 0.13887351021978725, 0.13887351021978725, 0.09212149983772845,
				0.09212149983772845, 0.04048400476531588, 0.04048400476531588
			},
			new double[14]
			{
				0.2152638534631578, 0.2152638534631578, 0.2051984637212956, 0.2051984637212956, 0.18553839747793782, 0.18553839747793782, 0.15720316715819355, 0.15720316715819355, 0.12151857068790319, 0.12151857068790319,
				0.08015808715976021, 0.08015808715976021, 0.03511946033175186, 0.03511946033175186
			},
			new double[15]
			{
				0.2025782419255613, 0.19843148532711158, 0.19843148532711158, 0.1861610000155622, 0.1861610000155622, 0.16626920581699392, 0.16626920581699392, 0.13957067792615432, 0.13957067792615432, 0.10715922046717194,
				0.10715922046717194, 0.07036604748810812, 0.07036604748810812, 0.03075324199611727, 0.03075324199611727
			},
			new double[16]
			{
				0.1894506104550685, 0.1894506104550685, 0.18260341504492358, 0.18260341504492358, 0.16915651939500254, 0.16915651939500254, 0.14959598881657674, 0.14959598881657674, 0.12462897125553388, 0.12462897125553388,
				0.09515851168249279, 0.09515851168249279, 0.062253523938647894, 0.062253523938647894, 0.027152459411754096, 0.027152459411754096
			},
			new double[17]
			{
				0.17944647035620653, 0.17656270536699264, 0.17656270536699264, 0.16800410215645004, 0.16800410215645004, 0.15404576107681028, 0.15404576107681028, 0.13513636846852548, 0.13513636846852548, 0.11188384719340397,
				0.11188384719340397, 0.08503614831717918, 0.08503614831717918, 0.0554595293739872, 0.0554595293739872, 0.02414830286854793, 0.02414830286854793
			}
		};

		public float StartEndLength => (start.pos - end.pos).magnitude;

		public Segment Inverted => new Segment(end, start);

		public Segment(Node start, Node end)
		{
			this.start = start;
			this.end = end;
			lengthToPercentLut = default(StructArray);
			length = 0f;
		}

		public Segment(Vector3 start, Vector3 end)
		{
			this.start = new Node
			{
				pos = start
			};
			this.end = new Node
			{
				pos = end
			};
			lengthToPercentLut = default(StructArray);
			length = 0f;
		}

		public override string ToString()
		{
			return $"Segment ({start.pos.x},{start.pos.y},{start.pos.z})-({end.pos.x},{end.pos.y},{end.pos.z}) {StartEndLength}";
		}

		public Vector3 GetPoint(float p)
		{
			float num = 1f - p;
			return num * num * num * start.pos + 3f * p * num * num * (start.pos + start.dir) + 3f * p * p * num * (end.pos + end.dir) + p * p * p * end.pos;
		}

		public Vector3 GetDerivative(float p)
		{
			float num = 1f - p;
			return num * num * 3f * (start.pos + start.dir - start.pos) + 2f * num * p * 3f * (end.pos + end.dir - (start.pos + start.dir)) + p * p * 3f * (end.pos - (end.pos + end.dir));
		}

		public (Vector3 inDir, Vector3 outDir) GetSplitTangents(float p, Vector3 point)
		{
			float num = 1f - p;
			Vector3 vector = start.pos + start.dir * p;
			Vector3 vector2 = (start.pos + start.dir) * num + (end.pos + end.dir) * p;
			return new ValueTuple<Vector3, Vector3>(item2: (end.pos + end.dir * num) * p + vector2 * num - point, item1: vector * num + vector2 * p - point);
		}

		public (Segment, Segment) GetSplitted(float p)
		{
			float num = 1f - p;
			Vector3 vector = start.pos + start.dir * p;
			Vector3 vector2 = (start.pos + start.dir) * num + (end.pos + end.dir) * p;
			Vector3 vector3 = end.pos + end.dir * num;
			Vector3 point = GetPoint(p);
			Segment item = new Segment
			{
				start = new Node
				{
					pos = start.pos,
					dir = start.dir * p,
					type = start.type
				},
				end = new Node
				{
					pos = point,
					dir = vector * (1f - p) + vector2 * p - point,
					type = start.type
				}
			};
			Segment item2 = new Segment
			{
				start = new Node
				{
					pos = point,
					dir = vector3 * p + vector2 * num - point,
					type = start.type
				},
				end = new Node
				{
					pos = end.pos,
					dir = end.dir * num,
					type = end.type
				}
			};
			return (item, item2);
		}

		public static Segment Join(Segment prev, Segment next)
		{
			float magnitude = (prev.start.pos - prev.end.pos).magnitude;
			float magnitude2 = (next.start.pos - next.end.pos).magnitude;
			float num = magnitude / (magnitude + magnitude2);
			return new Segment
			{
				start = new Node
				{
					pos = prev.start.pos,
					dir = prev.start.dir / num
				},
				end = new Node
				{
					pos = next.end.pos,
					dir = prev.end.dir / (1f - num)
				}
			};
		}

		public Vector3 GetPerpendicular(float p, Vector3 cross)
		{
			return Vector3.Cross(GetDerivative(p), cross);
		}

		public Vector2D GetPerpendicular2D(float p)
		{
			Vector3 derivative = GetDerivative(p);
			return new Vector2D(0f - derivative.z, derivative.x);
		}

		public Vector3 ApproxMin()
		{
			Vector3 result = new Vector3(3.4028235E+38f, 3.4028235E+38f, 3.4028235E+38f);
			if (start.pos.x < result.x)
			{
				result.x = start.pos.x;
			}
			if (start.pos.y < result.y)
			{
				result.y = start.pos.y;
			}
			if (start.pos.z < result.z)
			{
				result.z = start.pos.z;
			}
			if (end.pos.x < result.x)
			{
				result.x = end.pos.x;
			}
			if (end.pos.y < result.y)
			{
				result.y = end.pos.y;
			}
			if (end.pos.z < result.z)
			{
				result.z = end.pos.z;
			}
			if (start.pos.x + start.dir.x < result.x)
			{
				result.x = start.pos.x + start.dir.x;
			}
			if (start.pos.y + start.dir.y < result.y)
			{
				result.y = start.pos.y + start.dir.y;
			}
			if (start.pos.z + start.dir.z < result.z)
			{
				result.z = start.pos.z + start.dir.z;
			}
			if (end.pos.x + end.dir.x < result.x)
			{
				result.x = end.pos.x + end.dir.x;
			}
			if (end.pos.y + end.dir.y < result.y)
			{
				result.y = end.pos.y + end.dir.y;
			}
			if (end.pos.z + end.dir.z < result.z)
			{
				result.z = end.pos.z + end.dir.z;
			}
			return result;
		}

		public Vector3 ApproxMax()
		{
			Vector3 result = new Vector3(-3.4028235E+38f, -3.4028235E+38f, -3.4028235E+38f);
			if (start.pos.x > result.x)
			{
				result.x = start.pos.x;
			}
			if (start.pos.y > result.y)
			{
				result.y = start.pos.y;
			}
			if (start.pos.z > result.z)
			{
				result.z = start.pos.z;
			}
			if (end.pos.x > result.x)
			{
				result.x = end.pos.x;
			}
			if (end.pos.y > result.y)
			{
				result.y = end.pos.y;
			}
			if (end.pos.z > result.z)
			{
				result.z = end.pos.z;
			}
			if (start.pos.x + start.dir.x > result.x)
			{
				result.x = start.pos.x + start.dir.x;
			}
			if (start.pos.y + start.dir.y > result.y)
			{
				result.y = start.pos.y + start.dir.y;
			}
			if (start.pos.z + start.dir.z > result.z)
			{
				result.z = start.pos.z + start.dir.z;
			}
			if (end.pos.x + end.dir.x > result.x)
			{
				result.x = end.pos.x + end.dir.x;
			}
			if (end.pos.y + end.dir.y > result.y)
			{
				result.y = end.pos.y + end.dir.y;
			}
			if (end.pos.z + end.dir.z > result.z)
			{
				result.z = end.pos.z + end.dir.z;
			}
			return result;
		}

		public Vector3 Min()
		{
			throw new NotImplementedException();
		}

		public Vector3 Max()
		{
			throw new NotImplementedException();
		}

		public float IntersectRect(Vector3 pos, Vector3 size)
		{
			Vector3 vector = ApproxMin();
			Vector3 vector2 = ApproxMax();
			float num = 3.4028235E+38f;
			if (vector.x < pos.x && vector2.x > pos.x)
			{
				float intersectPercent = GetIntersectPercent(IntersectFn);
				if (intersectPercent < num)
				{
					num = intersectPercent;
				}
			}
			if (vector.x < pos.x + size.x && vector2.x > pos.x + size.x)
			{
				float intersectPercent2 = GetIntersectPercent(IntersectFn2);
				if (intersectPercent2 < num)
				{
					num = intersectPercent2;
				}
			}
			if (vector.z < pos.z && vector2.z > pos.z)
			{
				float intersectPercent3 = GetIntersectPercent(IntersectFn3);
				if (intersectPercent3 < num)
				{
					num = intersectPercent3;
				}
			}
			if (vector.z < pos.z + size.z && vector2.z > pos.z + size.z)
			{
				float intersectPercent4 = GetIntersectPercent(IntersectFn4);
				if (intersectPercent4 < num)
				{
					num = intersectPercent4;
				}
			}
			return num;
			bool IntersectFn(Vector3 point)
			{
				return point.x > pos.x;
			}
			bool IntersectFn2(Vector3 point)
			{
				return point.x < pos.x + size.x;
			}
			bool IntersectFn3(Vector3 point)
			{
				return point.z > pos.z;
			}
			bool IntersectFn4(Vector3 point)
			{
				return point.z < pos.z + size.z;
			}
		}

		public void ClampByRect(Vector3 pos, Vector3 size)
		{
		}

		public void PushPoints(Vector3[] points, float[] ranges, bool horizontalOnly = true, float distFactor = 1f)
		{
			Vector3 vector = ApproxMin();
			Vector3 vector2 = ApproxMax();
			Func<Vector3, Vector3, float> distanceFn = ((!horizontalOnly) ? ((Func<Vector3, Vector3, float>)((Vector3 vector3, Vector3 vector4) => (vector3.x - vector4.x) * (vector3.x - vector4.x) + (vector3.z - vector4.z) * (vector3.z - vector4.z) + (vector3.y - vector4.y) * (vector3.y - vector4.y))) : ((Func<Vector3, Vector3, float>)((Vector3 vector3, Vector3 vector4) => (vector3.x - vector4.x) * (vector3.x - vector4.x) + (vector3.z - vector4.z) * (vector3.z - vector4.z))));
			for (int num = 0; num < points.Length; num++)
			{
				Vector3 point = points[num];
				float num2 = ranges[num];
				if (!(vector.x > point.x + num2) && !(vector2.x < point.x - num2) && !(vector.z > point.z + num2) && !(vector2.z < point.z - num2) && (horizontalOnly || (!(vector.y > point.y + num2) && !(vector2.y < point.y - num2))))
				{
					(float percent, float distSq) closest = GetClosest(distanceFn, point, 5, 5);
					var (p, _) = closest;
					if (!(closest.distSq > num2 * num2))
					{
						Vector3 point2 = GetPoint(p);
						points[num] = PushPoint(point, point2, num2, horizontalOnly, distFactor);
					}
				}
			}
		}

		public bool PushStartEnd(Vector3[] points, float[] ranges, bool horizontalOnly = true, float distFactor = 1f)
		{
			bool result = false;
			for (int i = 0; i < points.Length; i++)
			{
				Vector3 vector = points[i];
				float num = ranges[i];
				if (start.pos.x < vector.x + num && start.pos.x > vector.x - num && start.pos.z < vector.z + num && start.pos.z > vector.z - num && (horizontalOnly || (start.pos.y < vector.y + num && start.pos.y > vector.y - num)) && (vector - start.pos).sqrMagnitude < num * num)
				{
					start.pos = PushPoint(start.pos, vector, num, horizontalOnly, distFactor);
					result = true;
				}
				if (end.pos.x < end.pos.x + num && end.pos.x > vector.x - num && end.pos.z < end.pos.z + num && end.pos.z > vector.z - num && (horizontalOnly || (end.pos.y < vector.y + num && end.pos.y > vector.y - num)) && (vector - end.pos).sqrMagnitude < num * num)
				{
					end.pos = PushPoint(end.pos, vector, num, horizontalOnly, distFactor);
					result = true;
				}
			}
			return result;
		}

		private Vector3 PushPoint(Vector3 point, Vector3 otherPoint, float otherRange, bool horizontalOnly = true, float distFactor = 1f)
		{
			Vector3 vector = otherPoint - point;
			if (horizontalOnly)
			{
				vector.y = 0f;
			}
			Vector3 normalized = vector.normalized;
			float magnitude = vector.magnitude;
			float num = otherRange - magnitude;
			return point - normalized * num * distFactor;
		}

		public (float percent, float distSq) ApproximateClosest(Vector3 point, int iterations)
		{
			return ApproximateClosest(null, point, iterations);
		}

		private (float percent, float distSq) ApproximateClosest(Func<Vector3, Vector3, float> distanceFn, Vector3 point, int iterations)
		{
			float num = 3.4028235E+38f;
			float num2 = 0f;
			Vector3 pos = start.pos;
			float num3 = 1f / (float)(iterations - 1);
			for (int i = 0; i < iterations; i++)
			{
				float num4 = (float)i * num3;
				Vector3 point2 = GetPoint(num4);
				float num5 = distanceFn?.Invoke(point2, point) ?? ((point2.x - point.x) * (point2.x - point.x) + (point2.z - point.z) * (point2.z - point.z) + (point2.y - point.y) * (point2.y - point.y));
				if (num5 < num)
				{
					num = num5;
					num2 = num4;
					pos.x = point2.x;
					pos.y = point2.y;
					pos.z = point2.z;
				}
			}
			if (num2 > 1f)
			{
				num2 = 1f;
			}
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			return (percent: num2, distSq: num);
		}

		public (float percent, float distSq) RefineClosest(Vector3 point, float percent, float range, int iterations)
		{
			return RefineClosest(null, point, percent, range, iterations);
		}

		private (float percent, float distSq) RefineClosest(Func<Vector3, Vector3, float> distanceFn, Vector3 point, float percent, float range, int iterations)
		{
			float item = 3.4028235E+38f;
			for (int i = 0; i < iterations; i++)
			{
				range /= 2f;
				float num = percent + range;
				if (num > 1f)
				{
					num = 1f;
				}
				Vector3 point2 = GetPoint(num);
				float num2 = distanceFn?.Invoke(point2, point) ?? ((point2.x - point.x) * (point2.x - point.x) + (point2.z - point.z) * (point2.z - point.z) + (point2.y - point.y) * (point2.y - point.y));
				float num3 = percent - range;
				if (num3 < 0f)
				{
					num3 = 0f;
				}
				Vector3 point3 = GetPoint(num3);
				float num4 = distanceFn?.Invoke(point3, point) ?? ((point3.x - point.x) * (point3.x - point.x) + (point3.z - point.z) * (point3.z - point.z) + (point3.y - point.y) * (point3.y - point.y));
				if (num2 < num4)
				{
					percent = num;
					item = num2;
				}
				else
				{
					percent = num3;
					item = num4;
				}
			}
			return (percent: percent, distSq: item);
		}

		public (float percent, float distSq) GetClosest(Vector3 point, int initialApprox = 10, int recursiveApprox = 10)
		{
			float item = ApproximateClosest(point, initialApprox).percent;
			return RefineClosest(point, item, 1f / (float)(initialApprox - 1), recursiveApprox);
		}

		public (float percent, float distSq) GetClosest(Func<Vector3, Vector3, float> distanceFn, Vector3 point, int initialApprox = 10, int recursiveApprox = 10)
		{
			float item = ApproximateClosest(distanceFn, point, initialApprox).percent;
			return RefineClosest(distanceFn, point, item, 1f / (float)(initialApprox - 1), recursiveApprox);
		}

		public bool IsWithinRange(Vector3 point, float range)
		{
			Vector3 vector = ApproxMin();
			if (point.x + range < vector.x || point.y + range < vector.y || point.z + range < vector.z)
			{
				return false;
			}
			Vector3 vector2 = ApproxMax();
			if (point.x - range > vector2.x || point.y - range > vector2.y || point.z - range > vector2.z)
			{
				return false;
			}
			return true;
		}

		public static (float, float) ClosestDistance(Segment s1, Segment s2, int initialApprox = 6, int recursiveApprox = 3)
		{
			float num = 1f / (float)(initialApprox - 1);
			float num2 = 3.4028235E+38f;
			float num3 = 0f;
			float num4 = 0f;
			for (int i = 0; i < initialApprox; i++)
			{
				float num5 = (float)i * num;
				Vector3 point = s1.GetPoint(num5);
				var (num6, num7) = s2.ApproximateClosest(point, initialApprox);
				if (num7 < num2)
				{
					num2 = num7;
					num3 = num5;
					num4 = num6;
				}
			}
			float num8 = num;
			for (int j = 0; j < recursiveApprox; j++)
			{
				num8 /= 2f;
				float num9 = num3 + num8;
				if (num9 > 1f)
				{
					num9 = 1f;
				}
				Vector3 point2 = s1.GetPoint(num9);
				(float percent, float distSq) tuple2 = s2.RefineClosest(point2, num4, num, recursiveApprox);
				float item = tuple2.percent;
				float item2 = tuple2.distSq;
				float num10 = num3 - num8;
				if (num10 < 0f)
				{
					num10 = 0f;
				}
				Vector3 point3 = s1.GetPoint(num10);
				var (num11, num12) = s2.RefineClosest(point3, num4, num, recursiveApprox);
				if (item2 < num12)
				{
					num3 = num9;
					num4 = item;
				}
				else
				{
					num3 = num10;
					num4 = num11;
				}
			}
			return (num3, num4);
		}

		private float GetIntersectPercent(Predicate<Vector3> intersectFn, int initialApprox = 10, int recursiveApprox = 10)
		{
			bool flag = intersectFn(start.pos);
			float num = 0f;
			float num2 = 1f / (float)(initialApprox - 1);
			for (int i = 0; i < initialApprox; i++)
			{
				float num3 = (float)i * num2;
				Vector3 point = GetPoint(num3);
				if (intersectFn(point) != flag)
				{
					break;
				}
				num = num3;
			}
			if (num > 1f)
			{
				num = 1f;
			}
			if (num < 0f)
			{
				num = 0f;
			}
			for (int j = 0; j < recursiveApprox; j++)
			{
				num2 /= 2f;
				float num4 = num + num2;
				if (num4 > 1f)
				{
					num4 = 1f;
				}
				Vector3 point2 = GetPoint(num4);
				bool flag2 = intersectFn(point2);
				if (flag == flag2)
				{
					num = num4;
				}
			}
			return num + num2;
		}

		public bool IsNearPoints(Vector3[] points, float[] ranges, bool horizontalOnly = true, float startEndProximityFactor = 0f)
		{
			float nearPercent;
			int nearPointNum;
			return IsNearPoints(points, ranges, out nearPercent, out nearPointNum, horizontalOnly, startEndProximityFactor);
		}

		public bool IsNearPoints(Vector3[] points, float[] ranges, out float nearPercent, out int nearPointNum, bool horizontalOnly = true, float startEndProximityFactor = 0f)
		{
			Vector3 vector = ApproxMin();
			Vector3 vector2 = ApproxMax();
			Func<Vector3, Vector3, float> distanceFn = ((!horizontalOnly) ? ((Func<Vector3, Vector3, float>)((Vector3 p1, Vector3 p2) => (p1.x - p2.x) * (p1.x - p2.x) + (p1.z - p2.z) * (p1.z - p2.z) + (p1.y - p2.y) * (p1.y - p2.y))) : ((Func<Vector3, Vector3, float>)((Vector3 p1, Vector3 p2) => (p1.x - p2.x) * (p1.x - p2.x) + (p1.z - p2.z) * (p1.z - p2.z))));
			for (int num = 0; num < points.Length; num++)
			{
				Vector3 vector3 = points[num];
				float num2 = ranges[num];
				if (vector.x > vector3.x + num2 || vector2.x < vector3.x - num2 || vector.z > vector3.z + num2 || vector2.z < vector3.z - num2 || (!horizontalOnly && (vector.y > vector3.y + num2 || vector2.y < vector3.y - num2)))
				{
					continue;
				}
				if (startEndProximityFactor > 1E-05f)
				{
					float num3 = num2 * startEndProximityFactor * num2 * startEndProximityFactor;
					if ((start.pos - vector3).sqrMagnitude < num3 || (end.pos - vector3).sqrMagnitude < num3)
					{
						continue;
					}
				}
				var (num4, num5) = GetClosest(distanceFn, vector3, 5, 5);
				if (!(num4 > 0.999f) && !(num4 < 0.001f) && num5 < num2 * num2)
				{
					nearPercent = num4;
					nearPointNum = num;
					return true;
				}
			}
			nearPercent = -1f;
			nearPointNum = -1;
			return false;
		}

		public float WorldLengthToPercent(float worldLength)
		{
			return NormLengthToPercent(worldLength / length);
		}

		public float NormLengthToPercent(float normLength)
		{
			int num = (int)(normLength * 9f);
			float num2 = (normLength - (float)num / 9f) * 9f;
			float num3;
			float num4;
			switch (num)
			{
			case 0:
				num3 = 0f;
				num4 = (float)(int)lengthToPercentLut.b0 / 255f;
				break;
			case 1:
				num3 = (float)(int)lengthToPercentLut.b0 / 255f;
				num4 = (float)(int)lengthToPercentLut.b1 / 255f;
				break;
			case 2:
				num3 = (float)(int)lengthToPercentLut.b1 / 255f;
				num4 = (float)(int)lengthToPercentLut.b2 / 255f;
				break;
			case 3:
				num3 = (float)(int)lengthToPercentLut.b2 / 255f;
				num4 = (float)(int)lengthToPercentLut.b3 / 255f;
				break;
			case 4:
				num3 = (float)(int)lengthToPercentLut.b3 / 255f;
				num4 = (float)(int)lengthToPercentLut.b4 / 255f;
				break;
			case 5:
				num3 = (float)(int)lengthToPercentLut.b4 / 255f;
				num4 = (float)(int)lengthToPercentLut.b5 / 255f;
				break;
			case 6:
				num3 = (float)(int)lengthToPercentLut.b5 / 255f;
				num4 = (float)(int)lengthToPercentLut.b6 / 255f;
				break;
			case 7:
				num3 = (float)(int)lengthToPercentLut.b6 / 255f;
				num4 = (float)(int)lengthToPercentLut.b7 / 255f;
				break;
			case 8:
				num3 = (float)(int)lengthToPercentLut.b7 / 255f;
				num4 = 1f;
				break;
			default:
				num3 = 0f;
				num4 = 0f;
				break;
			}
			return num3 * (1f - num2) + num4 * num2;
		}

		public float GetLinearDistance(float a, float b)
		{
			float num = 1f - a;
			float num2 = 1f - b;
			return ((3f * a * num * num - 3f * b * num2 * num2) * start.dir + (3f * a * a * num - 3f * b * b * num2) * (end.pos + end.dir - start.pos) + (a * a * a - b * b * b) * (end.pos - start.pos)).magnitude;
		}

		public float ApproxLength(float pStart = 0f, float pEnd = 1f, int iterations = 32)
		{
			float num = 0f;
			float num2 = (pEnd - pStart) / (float)(iterations - 1);
			float a = 0f;
			for (int i = 1; i < iterations; i++)
			{
				float num3 = pStart + num2 * (float)i;
				num += GetLinearDistance(a, num3);
				a = num3;
			}
			return num;
		}

		public void UpdateLength(int iterations = 32, float[] tmp = null)
		{
			float magnitude = (start.pos - end.pos).magnitude;
			float num = start.dir.magnitude + (start.dir + start.pos - (end.dir + end.pos)).magnitude + end.dir.magnitude;
			if (magnitude > num - num / 100f && magnitude < num + num / 100f)
			{
				FillLinearLengthLut();
			}
			else
			{
				FillApproxLengthLut(iterations, tmp);
			}
			if (length < magnitude)
			{
				length = magnitude;
			}
		}

		public void FillApproxLengthLut(int iterations = 32, float[] tmp = null)
		{
			float num = 0f;
			float[] array = ((tmp == null) ? new float[iterations] : tmp);
			for (int i = 0; i < array.Length; i++)
			{
				float a = 1f * (float)i / (float)iterations;
				float b = 1f * (float)(i + 1) / (float)iterations;
				float linearDistance = GetLinearDistance(a, b);
				array[i] = num + linearDistance;
				num += linearDistance;
			}
			length = num;
			if (num >= 0f)
			{
				for (int j = 0; j < array.Length; j++)
				{
					array[j] /= num;
				}
			}
			for (int k = 0; k < 8; k++)
			{
				float num2 = (float)(k + 1) / 9f;
				int num3 = 0;
				for (int l = 0; l < array.Length; l++)
				{
					if (array[l] > num2)
					{
						num3 = l - 1;
						break;
					}
				}
				int num4 = num3 + 1;
				float num5 = (num2 - array[num3]) / (array[num4] - array[num3]);
				float f = ((float)num3 * (1f - num5) + (float)num4 * num5) / (float)array.Length;
				lengthToPercentLut.SetFloat(k, f);
			}
		}

		public void FillLinearLengthLut()
		{
			for (int i = 0; i < 8; i++)
			{
				float f = (float)(i + 1) / 9f;
				lengthToPercentLut.SetFloat(i, f);
			}
		}

		public float IntegralLength(float t = 1f, int iterations = 8)
		{
			float num = t / 2f;
			float num2 = 0f;
			double[] array = abscissaeLut[iterations];
			double[] array2 = weightsLut[iterations];
			for (int i = 0; i < iterations; i++)
			{
				float p = num * (float)array[i] + num;
				Vector3 derivative = GetDerivative(p);
				float num3 = Mathf.Sqrt(derivative.x * derivative.x + derivative.z * derivative.z);
				num2 += (float)array2[i] * num3;
			}
			return num * num2;
		}

		public void FillIntegralLengthLut(float fullLength = -1f, int iterations = 32)
		{
			if (fullLength < 0f)
			{
				fullLength = IntegralLength();
			}
			float num = LengthToPercent(fullLength * 0.5f, 0f, 1f, iterations);
			float num2 = LengthToPercent(fullLength * 0.25f, 0f, num, iterations / 2);
			float num3 = LengthToPercent(fullLength * 0.75f, num, 1f, iterations / 2);
			float num4 = LengthToPercent(fullLength * 0.125f, 0f, num2, iterations / 4);
			float num5 = LengthToPercent(fullLength * 0.375f, num2, num, iterations / 4);
			float num6 = LengthToPercent(fullLength * 0.625f, num, num3, iterations / 4);
			float num7 = LengthToPercent(fullLength * 0.875f, num3, 1f, iterations / 4);
			lengthToPercentLut.b0 = (byte)(num4 * 255f + 0.5f);
			lengthToPercentLut.b1 = (byte)(num2 * 255f + 0.5f);
			lengthToPercentLut.b2 = (byte)(num5 * 255f + 0.5f);
			lengthToPercentLut.b3 = (byte)(num * 255f + 0.5f);
			lengthToPercentLut.b4 = (byte)(num6 * 255f + 0.5f);
			lengthToPercentLut.b5 = (byte)(num3 * 255f + 0.5f);
			lengthToPercentLut.b6 = (byte)(num7 * 255f + 0.5f);
		}

		public float LengthToPercent(float length, float pFrom = 0f, float pTo = 1f, int iterations = 8)
		{
			float num = (pFrom + pTo) / 2f;
			float num2 = IntegralLength(num);
			if (length < num2)
			{
				if (iterations <= 1)
				{
					return (pFrom + num) / 2f;
				}
				return LengthToPercent(length, pFrom, num, iterations - 1);
			}
			if (iterations <= 1)
			{
				return (num + pTo) / 2f;
			}
			return LengthToPercent(length, num, pTo, iterations - 1);
		}
	}
}
