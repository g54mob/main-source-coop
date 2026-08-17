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
				115, 115, 101, 116, 115, 92, 80, 108, 117, 103,
				105, 110, 115, 92, 65, 100, 118, 97, 110, 99,
				101, 100, 70, 80, 83, 67, 111, 117, 110, 116,
				101, 114, 92, 69, 120, 97, 109, 112, 108, 101,
				115, 92, 65, 80, 73, 84, 101, 115, 116, 101,
				114, 46, 99, 115
			},
			TypesData = new byte[43]
			{
				0, 0, 0, 0, 38, 67, 111, 100, 101, 83,
				116, 97, 103, 101, 46, 65, 100, 118, 97, 110,
				99, 101, 100, 70, 80, 83, 67, 111, 117, 110,
				116, 101, 114, 124, 65, 80, 73, 84, 101, 115,
				116, 101, 114
			},
			TotalFiles = 1,
			TotalTypes = 1,
			IsEditorOnly = false
		};
	}
}
