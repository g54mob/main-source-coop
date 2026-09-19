namespace Obi
{
	public struct BurstPathSmootherData
	{
		public uint smoothing;

		public float decimation;

		public float twist;

		public float restLength;

		public float smoothLength;

		public uint usesOrientedParticles;

		public BurstPathSmootherData(ObiRopeBase rope, ObiPathSmoother smoother)
		{
			smoothing = smoother.smoothing;
			decimation = smoother.decimation;
			twist = smoother.twist;
			usesOrientedParticles = (rope.usesOrientedParticles ? 1u : 0u);
			restLength = rope.restLength;
			smoothLength = 0f;
		}
	}
}
