namespace Fusion
{
	internal interface ISerializationSurrogate<T>
	{
		void CopyFrom(T source);

		T CopyTo();
	}
}
