namespace DigitalOpus.MB.Core
{
	public static class MeshBakerSettingsUtility
	{
		public static MB_MeshVertexChannelFlags GetMeshChannelsAsFlags(MB_IMeshBakerSettings settings, bool doVerts, bool uvsSliceIdx_w)
		{
			return (MB_MeshVertexChannelFlags)((doVerts ? 1 : 0) | (settings.doNorm ? 2 : 0) | (settings.doTan ? 4 : 0) | (settings.doCol ? 8 : 0) | (settings.doUV ? 16 : 0) | (uvsSliceIdx_w ? 32 : 0) | (DoUV2getDataFromSourceMeshes(ref settings) ? 64 : 0) | (settings.doUV3 ? 128 : 0) | (settings.doUV4 ? 256 : 0) | (settings.doUV5 ? 512 : 0) | (settings.doUV6 ? 1024 : 0) | (settings.doUV7 ? 2048 : 0) | (settings.doUV8 ? 4096 : 0) | ((settings.renderType == MB_RenderType.skinnedMeshRenderer) ? 8192 : 0) | ((settings.renderType == MB_RenderType.skinnedMeshRenderer) ? 16384 : 0));
		}

		public static bool DoUV2getDataFromSourceMeshes(ref MB_IMeshBakerSettings settings)
		{
			if (settings.lightmapOption != MB2_LightmapOptions.copy_UV2_unchanged && settings.lightmapOption != MB2_LightmapOptions.preserve_current_lightmapping)
			{
				return settings.lightmapOption == MB2_LightmapOptions.copy_UV2_unchanged_to_separate_rects;
			}
			return true;
		}
	}
}
