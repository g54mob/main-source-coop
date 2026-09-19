using System;
using System.IO;

namespace Google.Apis
{
	public interface ISerializer
	{
		string Format { get; }

		void Serialize(object obj, Stream target);

		string Serialize(object obj);

		T Deserialize<T>(string input);

		object Deserialize(string input, Type type);

		T Deserialize<T>(Stream input);
	}
}
