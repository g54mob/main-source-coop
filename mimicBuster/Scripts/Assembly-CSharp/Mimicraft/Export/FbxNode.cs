using System.Collections.Generic;

namespace Mimicraft.Export
{
	public sealed class FbxNode
	{
		public readonly string Name;

		public readonly List<object> Properties = new List<object>();

		public readonly List<FbxNode> Children = new List<FbxNode>();

		public FbxNode(string name, params object[] properties)
		{
			Name = name;
			Properties.AddRange(properties);
		}

		public FbxNode Add(string name, params object[] properties)
		{
			FbxNode fbxNode = new FbxNode(name, properties);
			Children.Add(fbxNode);
			return fbxNode;
		}

		public FbxNode P(string name, string type, string label, string flags, params object[] values)
		{
			FbxNode fbxNode = new FbxNode("P", name, type, label, flags);
			fbxNode.Properties.AddRange(values);
			Children.Add(fbxNode);
			return fbxNode;
		}
	}
}
