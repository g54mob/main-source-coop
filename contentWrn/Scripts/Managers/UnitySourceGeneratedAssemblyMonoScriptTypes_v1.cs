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
			FilePathsData = new byte[59]
			{
				0, 0, 0, 1, 0, 0, 0, 51, 92, 65,
				115, 115, 101, 116, 115, 92, 80, 111, 114, 116,
				110, 105, 110, 103, 115, 98, 111, 108, 97, 103,
				101, 116, 92, 77, 97, 110, 97, 103, 101, 114,
				92, 80, 108, 97, 116, 102, 111, 114, 109, 77,
				97, 110, 97, 103, 101, 114, 46, 99, 115
			},
			TypesData = new byte[47]
			{
				0, 0, 0, 0, 42, 80, 111, 114, 116, 110,
				105, 110, 103, 115, 98, 111, 108, 97, 103, 101,
				116, 46, 80, 108, 97, 116, 102, 111, 114, 109,
				115, 124, 80, 108, 97, 116, 102, 111, 114, 109,
				77, 97, 110, 97, 103, 101, 114
			},
			TotalFiles = 1,
			TotalTypes = 1,
			IsEditorOnly = false
		};
	}
}
