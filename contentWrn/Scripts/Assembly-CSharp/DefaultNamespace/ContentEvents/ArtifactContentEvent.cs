using Zorro.Core;
using Zorro.Core.Serizalization;

namespace DefaultNamespace.ContentEvents
{
	public class ArtifactContentEvent : PropContentEvent
	{
		public Item artifact;

		public bool isHeld;

		public bool isActive;

		public override float GetContentValue()
		{
			return content.contentValue * (isHeld ? 1f : 0.5f) * (isActive ? 1f : 0.5f);
		}

		public override ushort GetID()
		{
			return content.id;
		}

		public override string GetName()
		{
			if (!(artifact != null))
			{
				return string.Empty;
			}
			return artifact.name;
		}

		public override Comment GenerateComment()
		{
			return new Comment(content.comments.GetRandom());
		}

		public override void Serialize(BinarySerializer serializer)
		{
			serializer.WriteBool(isHeld);
			serializer.WriteBool(isActive);
			serializer.WriteByte(artifact.id);
		}

		public override void Deserialize(BinaryDeserializer deserializer)
		{
			isHeld = deserializer.ReadBool();
			isActive = deserializer.ReadBool();
			ItemDatabase.TryGetItemFromID(deserializer.ReadByte(), out artifact);
		}
	}
}
