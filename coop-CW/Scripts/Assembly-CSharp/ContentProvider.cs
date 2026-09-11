using System.Collections.Generic;
using UnityEngine;

public abstract class ContentProvider : MonoBehaviour
{
	public abstract void GetContent(List<ContentEventFrame> contentEvents, float seenAmount, Camera camera, float time);
}
