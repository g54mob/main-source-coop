using UnityEngine;

namespace ShadedTechnology.WindshieldRainAsset
{
	[CreateAssetMenu(fileName = "RainPostProcessProfile", menuName = "WindshieldRain/RainPostProcessProfile")]
	public class RainPostProcessProfile : ScriptableObject
	{
		[SerializeField]
		public RainPostProcess[] postProcesses;

		private void InitRenderTexture(Vector2Int resolution, ref RenderTexture renderTexture)
		{
			renderTexture = new RenderTexture(resolution.x, resolution.y, 0, RenderTextureFormat.ARGBFloat);
			renderTexture.enableRandomWrite = true;
			renderTexture.Create();
			renderTexture.filterMode = FilterMode.Point;
		}

		public void InitPostProcesses(Vector2Int resolution)
		{
			RainPostProcess[] array = postProcesses;
			foreach (RainPostProcess rainPostProcess in array)
			{
				InitRenderTexture(resolution, ref rainPostProcess.renderTexture);
				if (!(rainPostProcess.material == null))
				{
					rainPostProcess.material.SetVector("_Resolution", new Vector4(resolution.x, resolution.y));
					MaterialTexture[] texturesToSet = rainPostProcess.texturesToSet;
					foreach (MaterialTexture materialTexture in texturesToSet)
					{
						materialTexture.material.SetTexture(materialTexture.textureName, rainPostProcess.renderTexture);
					}
				}
			}
		}

		public Texture UpdatePostProcesses(Texture currentTexture)
		{
			for (int i = 0; i < postProcesses.Length; i++)
			{
				if (postProcesses[i].material == null)
				{
					MaterialTexture[] texturesToSet = postProcesses[i].texturesToSet;
					foreach (MaterialTexture materialTexture in texturesToSet)
					{
						materialTexture.material.SetTexture(materialTexture.textureName, currentTexture);
					}
				}
				else if (i == 0)
				{
					postProcesses[i].material.SetTexture("_MainTex", currentTexture);
					Graphics.Blit(currentTexture, postProcesses[i].renderTexture, postProcesses[i].material);
				}
				else
				{
					postProcesses[i].material.SetTexture("_MainTex", postProcesses[i - 1].renderTexture);
					Graphics.Blit(postProcesses[i - 1].renderTexture, postProcesses[i].renderTexture, postProcesses[i].material);
				}
			}
			if (postProcesses.Length == 0)
			{
				return currentTexture;
			}
			return postProcesses[postProcesses.Length - 1].renderTexture;
		}
	}
}
