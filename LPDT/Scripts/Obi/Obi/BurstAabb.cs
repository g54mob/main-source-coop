using Unity.Mathematics;

namespace Obi
{
	public struct BurstAabb
	{
		public float4 min;

		public float4 max;

		public float4 size => max - min;

		public float4 center => min + (max - min) * 0.5f;

		public BurstAabb(float4 min, float4 max)
		{
			this.min = min;
			this.max = max;
		}

		public BurstAabb(float4 v1, float4 v2, float4 v3, float4 margin)
		{
			min = math.min(math.min(v1, v2), v3) - margin;
			max = math.max(math.max(v1, v2), v3) + margin;
		}

		public BurstAabb(float4 v1, float4 v2, float4 margin)
		{
			min = math.min(v1, v2) - margin;
			max = math.max(v1, v2) + margin;
		}

		public BurstAabb(float4 previousPosition, float4 position, float radius)
		{
			min = math.min(position - radius, previousPosition - radius);
			max = math.max(position + radius, previousPosition + radius);
		}

		public float AverageAxisLength()
		{
			float4 float5 = max - min;
			return (float5.x + float5.y + float5.z) * 0.33f;
		}

		public float MaxAxisLength()
		{
			return math.cmax((max - min).xyz);
		}

		public void EncapsulateParticle(float4 position, float radius)
		{
			min = math.min(min, position - radius);
			max = math.max(max, position + radius);
		}

		public void EncapsulateParticle(float4 previousPosition, float4 position, float radius)
		{
			min = math.min(math.min(min, position - radius), previousPosition - radius);
			max = math.max(math.max(max, position + radius), previousPosition + radius);
		}

		public void EncapsulateBounds(in BurstAabb bounds)
		{
			min = math.min(min, bounds.min);
			max = math.max(max, bounds.max);
		}

		public void Expand(float4 amount)
		{
			min -= amount;
			max += amount;
		}

		public void Sweep(float4 velocity)
		{
			min = math.min(min, min + velocity);
			max = math.max(max, max + velocity);
		}

		public void Transform(in BurstAffineTransform transform)
		{
			Transform(float4x4.TRS(transform.translation.xyz, transform.rotation, transform.scale.xyz));
		}

		public void Transform(in float4x4 transform)
		{
			float3 x = transform.c0.xyz * min.x;
			float3 y = transform.c0.xyz * max.x;
			float3 x2 = transform.c1.xyz * min.y;
			float3 y2 = transform.c1.xyz * max.y;
			float3 x3 = transform.c2.xyz * min.z;
			float3 y3 = transform.c2.xyz * max.z;
			min = new float4(math.min(x, y) + math.min(x2, y2) + math.min(x3, y3) + transform.c3.xyz, 0f);
			max = new float4(math.max(x, y) + math.max(x2, y2) + math.max(x3, y3) + transform.c3.xyz, 0f);
		}

		public BurstAabb Transformed(in BurstAffineTransform transform)
		{
			BurstAabb result = this;
			result.Transform(in transform);
			return result;
		}

		public BurstAabb Transformed(in float4x4 transform)
		{
			BurstAabb result = this;
			result.Transform(in transform);
			return result;
		}

		public bool IntersectsAabb(in BurstAabb bounds, bool in2D = false)
		{
			if (in2D)
			{
				if (min[0] <= bounds.max[0] && max[0] >= bounds.min[0])
				{
					if (min[1] <= bounds.max[1])
					{
						return max[1] >= bounds.min[1];
					}
					return false;
				}
				return false;
			}
			if (min[0] <= bounds.max[0] && max[0] >= bounds.min[0] && min[1] <= bounds.max[1] && max[1] >= bounds.min[1])
			{
				if (min[2] <= bounds.max[2])
				{
					return max[2] >= bounds.min[2];
				}
				return false;
			}
			return false;
		}

		public bool IntersectsRay(float4 origin, float4 inv_dir, bool in2D = false)
		{
			float4 x = (min - origin) * inv_dir;
			float4 y = (max - origin) * inv_dir;
			float4 float5 = math.min(x, y);
			float4 float6 = math.max(x, y);
			float num;
			float num2;
			if (in2D)
			{
				num = math.cmax(float5.xy);
				num2 = math.cmin(float6.xy);
			}
			else
			{
				num = math.cmax(float5.xyz);
				num2 = math.cmin(float6.xyz);
			}
			if (num2 >= math.max(0f, num))
			{
				return num <= 1f;
			}
			return false;
		}
	}
}
