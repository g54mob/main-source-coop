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
			FilePathsData = new byte[65]
			{
				0, 0, 0, 1, 0, 0, 0, 57, 92, 65,
				115, 115, 101, 116, 115, 92, 70, 101, 97, 116,
				117, 114, 101, 115, 92, 85, 115, 101, 114, 82,
				101, 112, 111, 114, 116, 92, 65, 100, 97, 112,
				116, 101, 114, 92, 73, 85, 115, 101, 114, 82,
				101, 112, 111, 114, 116, 83, 101, 114, 118, 105,
				99, 101, 46, 99, 115
			},
			TypesData = new byte[51]
			{
				0, 0, 0, 0, 46, 70, 101, 97, 116, 117,
				114, 101, 115, 46, 85, 115, 101, 114, 82, 101,
				112, 111, 114, 116, 46, 65, 100, 97, 112, 116,
				101, 114, 124, 73, 85, 115, 101, 114, 82, 101,
				112, 111, 114, 116, 83, 101, 114, 118, 105, 99,
				101
			},
			TotalFiles = 1,
			TotalTypes = 1,
			IsEditorOnly = false
		};
	}
}
