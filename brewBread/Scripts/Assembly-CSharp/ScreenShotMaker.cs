using System.IO;
using UnityEngine;

public class ScreenShotMaker : MonoBehaviour
{
	public GameObject target;

	private RenderTexture renderTexture;

	private Camera renderCamera;

	private Vector4 bounds;

	private int resolution = 160;

	private float cameraDistance = -2f;

	private void Start()
	{
		Debug.Log("Initializing camera and stuff...");
		base.gameObject.AddComponent(typeof(Camera));
		renderCamera = GetComponent<Camera>();
		renderCamera.enabled = true;
		renderCamera.cameraType = CameraType.Game;
		renderCamera.forceIntoRenderTexture = true;
		renderCamera.orthographic = true;
		renderCamera.orthographicSize = 5f;
		renderCamera.aspect = 1f;
		renderCamera.targetDisplay = 2;
		renderTexture = new RenderTexture(resolution, resolution, 24);
		renderCamera.targetTexture = renderTexture;
		this.bounds = default(Vector4);
		Debug.Log("Initialized successfully!");
		Debug.Log("Computing level boundaries...");
		if (target != null)
		{
			Bounds bounds;
			if (target.GetComponentInChildren<Renderer>() != null)
			{
				bounds = target.GetComponentInChildren<Renderer>().bounds;
			}
			else
			{
				if (!(target.GetComponentInChildren<Collider2D>() != null))
				{
					Debug.Log("Unfortunately no boundaries could be found :/");
					return;
				}
				bounds = target.GetComponentInChildren<Collider2D>().bounds;
			}
			this.bounds.w = bounds.min.x;
			this.bounds.x = bounds.max.x;
			this.bounds.y = bounds.min.y;
			this.bounds.z = bounds.max.y;
		}
		else
		{
			object[] array = Object.FindObjectsOfType(typeof(GameObject));
			array = array;
			for (int i = 0; i < array.Length; i++)
			{
				GameObject gameObject = (GameObject)array[i];
				Bounds bounds2 = default(Bounds);
				if (gameObject.GetComponentInChildren<Renderer>() != null)
				{
					bounds2 = gameObject.GetComponentInChildren<Renderer>().bounds;
				}
				else
				{
					if (!(gameObject.GetComponentInChildren<Collider2D>() != null))
					{
						continue;
					}
					bounds2 = gameObject.GetComponentInChildren<Collider2D>().bounds;
				}
				this.bounds.w = Mathf.Min(this.bounds.w, bounds2.min.x);
				this.bounds.x = Mathf.Max(this.bounds.x, bounds2.max.x);
				this.bounds.y = Mathf.Min(this.bounds.y, bounds2.min.y);
				this.bounds.z = Mathf.Max(this.bounds.z, bounds2.max.y);
			}
		}
		Vector4 vector = this.bounds;
		Debug.Log("Boundaries computed successfuly! The computed boundaries are " + vector.ToString());
		Debug.Log("Computing target image resolution and final setup...");
		int width = Mathf.RoundToInt((float)resolution * ((this.bounds.x - this.bounds.w) / (renderCamera.aspect * renderCamera.orthographicSize * 2f * renderCamera.aspect)));
		int height = Mathf.RoundToInt((float)resolution * ((this.bounds.z - this.bounds.y) / (renderCamera.aspect * renderCamera.orthographicSize * 2f / renderCamera.aspect)));
		Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGB24, mipChain: false);
		RenderTexture.active = renderTexture;
		Debug.Log("Success! Everything seems ready to render!");
		float num = this.bounds.w;
		float num2 = 0f;
		while (num < this.bounds.x)
		{
			float num3 = this.bounds.y;
			float num4 = 0f;
			while (num3 < this.bounds.z)
			{
				base.gameObject.transform.position = new Vector3(num + renderCamera.aspect * renderCamera.orthographicSize, num3 + renderCamera.aspect * renderCamera.orthographicSize, cameraDistance);
				renderCamera.Render();
				texture2D.ReadPixels(new Rect(0f, 0f, resolution, resolution), (int)num2 * resolution, (int)num4 * resolution);
				Debug.Log("Rendered and copied chunk " + (num2 + 1f) + ":" + (num4 + 1f));
				num3 += renderCamera.aspect * renderCamera.orthographicSize * 2f;
				num4 += 1f;
			}
			num += renderCamera.aspect * renderCamera.orthographicSize * 2f;
			num2 += 1f;
		}
		Debug.Log("All chunks rendered! Some final adjustments and picture should be saved!");
		RenderTexture.active = null;
		renderCamera.targetTexture = null;
		byte[] bytes = texture2D.EncodeToPNG();
		File.WriteAllBytes(Application.persistentDataPath + "Screenshot.png", bytes);
		Debug.Log("All done! Always happy to help you :)");
	}
}
