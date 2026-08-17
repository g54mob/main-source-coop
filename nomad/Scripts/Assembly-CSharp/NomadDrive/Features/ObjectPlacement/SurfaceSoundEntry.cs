using System;
using Ami.BroAudio;
using NomadDrive.Features.Player;

namespace NomadDrive.Features.ObjectPlacement
{
	[Serializable]
	public class SurfaceSoundEntry
	{
		public SurfaceType surfaceType;

		public SoundID soundId;
	}
}
