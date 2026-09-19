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
			FilePathsData = new byte[70]
			{
				0, 0, 0, 1, 0, 0, 0, 62, 92, 65,
				115, 115, 101, 116, 115, 92, 70, 101, 97, 116,
				117, 114, 101, 115, 92, 68, 111, 111, 114, 76,
				111, 99, 107, 77, 111, 100, 117, 108, 101, 92,
				83, 99, 114, 105, 112, 116, 115, 92, 68, 111,
				111, 114, 76, 111, 99, 107, 73, 110, 105, 116,
				105, 97, 108, 105, 122, 101, 114, 46, 99, 115
			},
			TypesData = new byte[56]
			{
				0, 0, 0, 0, 51, 70, 101, 97, 116, 117,
				114, 101, 115, 46, 68, 111, 111, 114, 76, 111,
				99, 107, 77, 111, 100, 117, 108, 101, 46, 83,
				99, 114, 105, 112, 116, 115, 124, 68, 111, 111,
				114, 76, 111, 99, 107, 73, 110, 105, 116, 105,
				97, 108, 105, 122, 101, 114
			},
			TotalFiles = 1,
			TotalTypes = 1,
			IsEditorOnly = false
		};
	}
}
