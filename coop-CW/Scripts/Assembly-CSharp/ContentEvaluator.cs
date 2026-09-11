using System;
using Unity.Collections;
using UnityEngine;
using Zorro.Core.Serizalization;

public static class ContentEvaluator
{
	public static bool EvaluateRecording(CameraRecording recording, out ContentBuffer buffer)
	{
		buffer = new ContentBuffer();
		for (int i = 0; i < recording.ClipCount; i++)
		{
			Clip clip = recording.GetClip(i);
			if (clip.Valid)
			{
				if (!clip.TryGetContentBuffer(out var contentBuffer))
				{
					Debug.LogError("No content buffer found for clip: " + clip.clipID.ToMiniString());
					return false;
				}
				BinarySerializer binarySerializer = new BinarySerializer(512, Allocator.Temp);
				contentBuffer.Serialize(binarySerializer);
				buffer.AddBuffer(contentBuffer);
				BinaryDeserializer binaryDeserializer = new BinaryDeserializer(binarySerializer.buffer);
				float score = new ContentBuffer(binaryDeserializer).GetScore();
				float score2 = contentBuffer.GetScore();
				Debug.Log($"Original score: {score2}, Copied score: {score}, Buffer Size: {binaryDeserializer.buffer.Length} bytes");
				binarySerializer.Dispose();
			}
		}
		buffer.PickBest();
		return true;
	}

	public static Comment[] GenerateLostDiscComments()
	{
		return Array.Empty<Comment>();
	}
}
