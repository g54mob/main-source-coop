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
			FilePathsData = new byte[55]
			{
				0, 0, 0, 1, 0, 0, 0, 47, 92, 65,
				115, 115, 101, 116, 115, 92, 80, 111, 114, 116,
				110, 105, 110, 103, 115, 98, 111, 108, 97, 103,
				101, 116, 92, 85, 116, 105, 108, 105, 116, 105,
				101, 115, 92, 85, 116, 105, 108, 105, 116, 105,
				101, 115, 46, 99, 115
			},
			TypesData = new byte[41]
			{
				0, 0, 0, 0, 36, 80, 111, 114, 116, 110,
				105, 110, 103, 115, 98, 111, 108, 97, 103, 101,
				116, 46, 85, 116, 105, 108, 105, 116, 105, 101,
				115, 124, 85, 116, 105, 108, 105, 116, 105, 101,
				115
			},
			TotalFiles = 1,
			TotalTypes = 1,
			IsEditorOnly = false
		};
	}
}
