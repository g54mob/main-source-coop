using System;
using EvilCore.Particles;
using NomadDrive.Features.Player;

namespace NomadDrive.Features.ObjectPlacement
{
	[Serializable]
	public class SurfaceEffectEntry
	{
		public SurfaceType surfaceType;

		public ParticleKey particleKey;
	}
}
