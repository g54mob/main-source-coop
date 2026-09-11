using System;
using Unity.Collections;
using UnityEngine;
using Zorro.Core.Serizalization;

public class VideoChunk
{
	private byte[][] m_chunks;

	public ContentBuffer contentBuffer;

	public ClipID ClipID { get; }

	public VideoHandle VideoID { get; }

	public byte[] ChunkData { get; private set; }

	public bool Completed { get; private set; }

	private bool IsReadyForMakeVideo
	{
		get
		{
			byte[][] chunks = m_chunks;
			for (int i = 0; i < chunks.Length; i++)
			{
				if (chunks[i] == null)
				{
					return false;
				}
			}
			return contentBuffer != null;
		}
	}

	public VideoChunk(ushort chunkCount, ClipID clipID, VideoHandle videoID)
	{
		VideoID = videoID;
		ClipID = clipID;
		m_chunks = new byte[chunkCount][];
		Debug.Log($"Received first chunk: clipID: {ClipID.id} Video ID: {VideoID} Expecting Number Of Chunks: {chunkCount}");
	}

	public void AddChunk(byte[] chunkData, ushort index, NativeArray<byte> contentEventData)
	{
		Debug.Log($"Adding Video ChunkIndex: {index} Data Length: {chunkData.Length} Video ID: {VideoID} Clip ID: {ClipID}");
		m_chunks[index] = chunkData;
		if (index == 0)
		{
			BinaryDeserializer binaryDeserializer = new BinaryDeserializer(contentEventData);
			contentBuffer = new ContentBuffer(binaryDeserializer);
			binaryDeserializer.Dispose();
		}
		if (IsReadyForMakeVideo)
		{
			MakeVideo();
			Completed = true;
		}
	}

	private void MakeVideo()
	{
		int num = 0;
		byte[][] chunks = m_chunks;
		foreach (byte[] array in chunks)
		{
			num += array.Length;
		}
		Debug.Log("Chunks Complete: TotalBytes " + num);
		int num2 = 0;
		byte[] array2 = new byte[num];
		chunks = m_chunks;
		foreach (byte[] array3 in chunks)
		{
			Array.Copy(array3, 0, array2, num2, array3.Length);
			num2 += array3.Length;
		}
		ChunkData = array2;
	}
}
