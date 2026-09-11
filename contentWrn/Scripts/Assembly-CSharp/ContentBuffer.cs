using System;
using System.Collections.Generic;
using UnityEngine;
using Zorro.Core;
using Zorro.Core.Serizalization;

public class ContentBuffer
{
	public class BufferedContent
	{
		public ContentEventFrame frame;

		public float score;

		public void Serialize(BinarySerializer serializer)
		{
			serializer.WriteFloat(score);
			frame.Serialize(serializer);
		}

		public void Deserialize(BinaryDeserializer deserializer)
		{
			score = deserializer.ReadFloat();
			frame = new ContentEventFrame(deserializer);
		}
	}

	public class Streak
	{
		public ushort contentID;

		private List<BufferedContent> content = new List<BufferedContent>();

		public void EndStreak(List<BufferedContent> buffer)
		{
			float num = SingletonAsset<BigNumbers>.Instance.streakCurve.Evaluate(content.Count);
			BufferedContent bufferedContent = null;
			float num2 = float.MinValue;
			foreach (BufferedContent item in content)
			{
				item.score *= num;
				if (item.score > num2)
				{
					num2 = item.score;
					bufferedContent = item;
				}
			}
			if (bufferedContent != null)
			{
				buffer.Add(bufferedContent);
			}
		}

		public void PushFrame(ContentEventFrame frame)
		{
			content.Add(new BufferedContent
			{
				frame = frame,
				score = frame.GetScore()
			});
		}
	}

	public struct UniqueContent
	{
		public ushort contentID;

		public int instanceID;
	}

	public List<BufferedContent> buffer = new List<BufferedContent>();

	public Dictionary<UniqueContent, Streak> currentStreaks = new Dictionary<UniqueContent, Streak>();

	public ContentBuffer()
	{
	}

	public void PushFrame(List<ContentEventFrame> frames)
	{
		foreach (ContentEventFrame frame in frames)
		{
			ushort iD = frame.contentEvent.GetID();
			int uniqueID = frame.contentEvent.GetUniqueID();
			UniqueContent key = new UniqueContent
			{
				contentID = iD,
				instanceID = uniqueID
			};
			if (!currentStreaks.ContainsKey(key))
			{
				currentStreaks.Add(key, new Streak
				{
					contentID = iD
				});
			}
			currentStreaks[key].PushFrame(frame);
		}
		List<UniqueContent> list = new List<UniqueContent>();
		foreach (KeyValuePair<UniqueContent, Streak> currentStreak in currentStreaks)
		{
			if (!frames.Exists(delegate(ContentEventFrame frame)
			{
				UniqueContent uniqueContent = new UniqueContent
				{
					contentID = frame.contentEvent.GetID(),
					instanceID = frame.contentEvent.GetUniqueID()
				};
				return uniqueContent.Equals(currentStreak.Key);
			}))
			{
				currentStreak.Value.EndStreak(buffer);
				list.Add(currentStreak.Key);
			}
		}
		foreach (UniqueContent item in list)
		{
			currentStreaks.Remove(item);
		}
	}

	public void PickBest()
	{
		foreach (KeyValuePair<UniqueContent, Streak> currentStreak in currentStreaks)
		{
			currentStreak.Value.EndStreak(buffer);
		}
		currentStreaks.Clear();
		Dictionary<UniqueContent, int> dictionary = new Dictionary<UniqueContent, int>();
		List<BufferedContent> list = new List<BufferedContent>();
		buffer.Sort((BufferedContent c1, BufferedContent c2) => c2.score.CompareTo(c1.score));
		foreach (BufferedContent item in buffer)
		{
			UniqueContent key = new UniqueContent
			{
				contentID = item.frame.contentEvent.GetID(),
				instanceID = item.frame.contentEvent.GetUniqueID()
			};
			if (dictionary.ContainsKey(key))
			{
				int num = dictionary[key];
				if (num >= SingletonAsset<BigNumbers>.Instance.MaxSameEventType)
				{
					continue;
				}
				num++;
				dictionary[key] = num;
			}
			else
			{
				dictionary.Add(key, 1);
			}
			list.Add(item);
		}
		buffer = list;
		if (buffer.Count > 0)
		{
			VerboseDebug.Log("Highest score: " + buffer[0].score);
		}
	}

	public void AddBuffer(ContentBuffer contentBuffer)
	{
		buffer.AddRange(contentBuffer.buffer);
	}

	public List<Comment> GenerateComments()
	{
		new HashSet<string>();
		List<Comment> list = new List<Comment>();
		foreach (BufferedContent item in buffer)
		{
			Comment comment = item.frame.contentEvent.GenerateComment();
			if (comment != null)
			{
				comment.EventID = item.frame.contentEvent.GetID();
				comment.Likes = BigNumbers.GetScoreToViews(Mathf.RoundToInt(item.score), GameAPI.CurrentDay);
				comment.Time = item.frame.time;
				comment.Face = FaceDatabase.GetRandomFaceIndex();
				comment.FaceColor = FaceDatabase.GetRandomColorIndex();
				list.Add(comment);
			}
		}
		list.Sort((Comment c1, Comment c2) => c1.Time.CompareTo(c2.Time));
		return list;
	}

	public void Serialize(BinarySerializer serializer)
	{
		int count = buffer.Count;
		serializer.WriteInt(count);
		foreach (BufferedContent item in buffer)
		{
			item.Serialize(serializer);
		}
	}

	public ContentBuffer(BinaryDeserializer deserializer)
	{
		try
		{
			int num = deserializer.ReadInt();
			List<BufferedContent> list = new List<BufferedContent>();
			for (int i = 0; i < num; i++)
			{
				BufferedContent bufferedContent = new BufferedContent();
				bufferedContent.Deserialize(deserializer);
				list.Add(bufferedContent);
			}
			buffer = list;
		}
		catch (Exception ex)
		{
			Modal.Show("Please share on discord", ex.Message, new ModalOption[1]
			{
				new ModalOption("Okay")
			});
			throw;
		}
	}

	public float GetScore()
	{
		float num = 0f;
		foreach (BufferedContent item in buffer)
		{
			num += item.score;
		}
		return num;
	}
}
