using System.Collections.Generic;
using UnityEngine;

namespace Features.BeachPresetModule.Scripts.Core.Interfaces
{
	public class BeachMaterialSwapHandle
	{
		internal Dictionary<Renderer, Material[]> RendererSnapshots { get; } = new Dictionary<Renderer, Material[]>();
	}
}
