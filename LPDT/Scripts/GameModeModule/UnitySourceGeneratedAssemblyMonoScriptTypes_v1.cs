using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Runtime.CompilerServices;

[EditorBrowsable(EditorBrowsableState.Never)]
[GeneratedCode("Unity.MonoScriptGenerator.MonoScriptInfoGenerator", null)]
internal class UnitySourceGeneratedAssemblyMonoScriptTypes_v1
{
	private struct MonoScriptData
	{
		public byte[] FilePathsData;

		public byte[] TypesData;

		public int TotalTypes;

		public int TotalFiles;

		public bool IsEditorOnly;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static MonoScriptData Get()
	{
		return new MonoScriptData
		{
			FilePathsData = new byte[64]
			{
				0, 0, 0, 1, 0, 0, 0, 56, 92, 65,
				115, 115, 101, 116, 115, 92, 70, 101, 97, 116,
				117, 114, 101, 115, 92, 71, 97, 109, 101, 77,
				111, 100, 101, 77, 111, 100, 117, 108, 101, 92,
				83, 99, 114, 105, 112, 116, 115, 92, 71, 97,
				109, 101, 77, 111, 100, 101, 77, 111, 100, 101,
				108, 46, 99, 115
			},
			TypesData = new byte[50]
			{
				0, 0, 0, 0, 45, 70, 101, 97, 116, 117,
				114, 101, 115, 46, 71, 97, 109, 101, 77, 111,
				100, 101, 77, 111, 100, 117, 108, 101, 46, 83,
				99, 114, 105, 112, 116, 115, 124, 71, 97, 109,
				101, 77, 111, 100, 101, 77, 111, 100, 101, 108
			},
			TotalFiles = 1,
			TotalTypes = 1,
			IsEditorOnly = false
		};
	}
}
