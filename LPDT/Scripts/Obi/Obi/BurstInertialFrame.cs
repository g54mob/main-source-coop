using Unity.Mathematics;

namespace Obi
{
	public struct BurstInertialFrame
	{
		public BurstAffineTransform frame;

		public BurstAffineTransform prevFrame;

		public float4 velocity;

		public float4 angularVelocity;

		public float4 acceleration;

		public float4 angularAcceleration;

		public BurstInertialFrame(float4 position, float4 scale, quaternion rotation)
		{
			frame = new BurstAffineTransform(position, rotation, scale);
			prevFrame = frame;
			velocity = float4.zero;
			angularVelocity = float4.zero;
			acceleration = float4.zero;
			angularAcceleration = float4.zero;
		}

		public BurstInertialFrame(BurstAffineTransform frame)
		{
			this.frame = frame;
			prevFrame = frame;
			velocity = float4.zero;
			angularVelocity = float4.zero;
			acceleration = float4.zero;
			angularAcceleration = float4.zero;
		}

		public float4 VelocityAtPoint(float4 point)
		{
			return velocity + new float4(math.cross(angularVelocity.xyz, (point - prevFrame.translation).xyz), 0f);
		}

		public void Update(float4 position, float4 scale, quaternion rotation, float dt)
		{
			prevFrame = frame;
			float4 prevPosition = velocity;
			float4 prevPosition2 = angularVelocity;
			frame.translation = position;
			frame.rotation = rotation;
			frame.scale = scale;
			velocity = BurstIntegration.DifferentiateLinear(frame.translation, prevFrame.translation, dt);
			angularVelocity = BurstIntegration.DifferentiateAngular(frame.rotation, prevFrame.rotation, dt);
			acceleration = BurstIntegration.DifferentiateLinear(velocity, prevPosition, dt);
			angularAcceleration = BurstIntegration.DifferentiateLinear(angularVelocity, prevPosition2, dt);
		}
	}
}
