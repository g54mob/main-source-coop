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
			FilePathsData = new byte[56]
			{
				0, 0, 0, 1, 0, 0, 0, 48, 92, 65,
				115, 115, 101, 116, 115, 92, 49, 48, 46, 32,
				83, 67, 82, 73, 80, 84, 83, 92, 86, 101,
				114, 98, 111, 115, 101, 68, 101, 98, 117, 103,
				92, 86, 101, 114, 98, 111, 115, 101, 68, 101,
				98, 117, 103, 46, 99, 115
			},
			TypesData = new byte[18]
			{
				0, 0, 0, 0, 13, 124, 86, 101, 114, 98,
				111, 115, 101, 68, 101, 98, 117, 103
			},
			TotalFiles = 1,
			TotalTypes = 1,
			IsEditorOnly = false
		};
	}
}
