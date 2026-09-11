using UnityEngine;
using Zorro.Core;
using Zorro.Core.Serizalization;

public struct ContentEventFrame
{
	public ContentEvent contentEvent;

	public float seenAmount;

	public float time;

	public ContentEventFrame(ContentEvent contentEvent, float seenAmount, float time)
	{
		this.contentEvent = contentEvent;
		this.seenAmount = seenAmount;
		this.time = time;
	}

	public ContentEventFrame(BinaryDeserializer deserializer)
	{
		seenAmount = deserializer.ReadFloat();
		time = deserializer.ReadFloat();
		ushort id = deserializer.ReadUShort();
		contentEvent = ContentEventIDMapper.GetContentEvent(id);
		int position = deserializer.position;
		contentEvent.Deserialize(deserializer);
		int num = deserializer.position - position;
		int num2 = deserializer.ReadInt();
		if (num != num2)
		{
			Modal.Show("Content Event Size Mismatch", $"Size mismatch on content event: {contentEvent.GetType().Name}. Expected: {num2}, Read: {num}", new ModalOption[1]
			{
				new ModalOption("Okay")
			});
			Debug.LogError("Size mismatch on content event: " + contentEvent.GetType().Name + ". Expected: " + num2 + ", Read: " + num);
		}
	}

	public float GetScore()
	{
		return SingletonAsset<BigNumbers>.Instance.percentageToScreenToFactorCurve.Evaluate(seenAmount) * contentEvent.GetContentValue();
	}

	public void Serialize(BinarySerializer serializer)
	{
		serializer.WriteFloat(seenAmount);
		serializer.WriteFloat(time);
		serializer.WriteUshort(contentEvent.GetID());
		int position = serializer.Position;
		contentEvent.Serialize(serializer);
		int value = serializer.Position - position;
		serializer.WriteInt(value);
	}
}
