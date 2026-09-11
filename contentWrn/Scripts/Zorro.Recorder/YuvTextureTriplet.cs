using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class YuvTextureTriplet
{
	public RenderTexture Y;

	public RenderTexture U;

	public RenderTexture V;

	public YuvTextureTriplet(int textureWidth, int textureHeight)
	{
		Y = new RenderTexture(textureWidth, textureHeight, 0, GraphicsFormat.R8_UNorm);
		Y.enableRandomWrite = true;
		Y.antiAliasing = 1;
		Y.useMipMap = false;
		Y.autoGenerateMips = false;
		Y.Create();
		U = new RenderTexture(210, 210, 0, GraphicsFormat.R8_UNorm);
		U.enableRandomWrite = true;
		U.antiAliasing = 1;
		U.useMipMap = false;
		U.autoGenerateMips = false;
		U.Create();
		V = new RenderTexture(210, 210, 0, GraphicsFormat.R8_UNorm);
		V.enableRandomWrite = true;
		V.antiAliasing = 1;
		V.useMipMap = false;
		V.autoGenerateMips = false;
		V.Create();
	}
}
