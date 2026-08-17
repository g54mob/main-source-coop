using System;
using Ami.BroAudio;
using NomadDrive.Features.Player;

namespace NomadDrive.Features.ObjectPlacement
{
	[Serializable]
	public class MaterialSurfaceSoundEntry
	{
		public ItemMaterial material;

		public SurfaceType surfaceType;

		public SoundID soundId;
	}
}
