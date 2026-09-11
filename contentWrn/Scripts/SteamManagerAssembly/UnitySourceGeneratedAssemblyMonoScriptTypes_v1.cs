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
			FilePathsData = new byte[54]
			{
				0, 0, 0, 1, 0, 0, 0, 46, 92, 65,
				115, 115, 101, 116, 115, 92, 83, 99, 114, 105,
				112, 116, 115, 92, 83, 116, 101, 97, 109, 119,
				111, 114, 107, 115, 46, 78, 69, 84, 92, 83,
				116, 101, 97, 109, 77, 97, 110, 97, 103, 101,
				114, 46, 99, 115
			},
			TypesData = new byte[18]
			{
				0, 0, 0, 0, 13, 124, 83, 116, 101, 97,
				109, 77, 97, 110, 97, 103, 101, 114
			},
			TotalFiles = 1,
			TotalTypes = 1,
			IsEditorOnly = false
		};
	}
}
