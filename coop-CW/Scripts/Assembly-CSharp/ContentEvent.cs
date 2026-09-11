using Zorro.Core.Serizalization;

public abstract class ContentEvent
{
	public abstract float GetContentValue();

	public abstract ushort GetID();

	public abstract string GetName();

	public abstract string[] GetAllComments();

	public abstract Comment GenerateComment();

	public abstract void Serialize(BinarySerializer serializer);

	public abstract void Deserialize(BinaryDeserializer deserializer);

	public virtual int GetUniqueID()
	{
		return 0;
	}
}
