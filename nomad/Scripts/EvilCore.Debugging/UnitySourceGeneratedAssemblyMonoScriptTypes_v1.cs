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
			FilePathsData = new byte[60]
			{
				0, 0, 0, 1, 0, 0, 0, 52, 92, 65,
				115, 115, 101, 116, 115, 92, 95, 80, 114, 111,
				106, 101, 99, 116, 92, 95, 67, 111, 114, 101,
				92, 68, 101, 98, 117, 103, 103, 105, 110, 103,
				92, 66, 117, 105, 108, 100, 69, 114, 114, 111,
				114, 76, 111, 103, 103, 101, 114, 46, 99, 115
			},
			TypesData = new byte[40]
			{
				0, 0, 0, 0, 35, 69, 118, 105, 108, 67,
				111, 114, 101, 46, 68, 101, 98, 117, 103, 103,
				105, 110, 103, 124, 66, 117, 105, 108, 100, 69,
				114, 114, 111, 114, 76, 111, 103, 103, 101, 114
			},
			TotalFiles = 1,
			TotalTypes = 1,
			IsEditorOnly = false
		};
	}
}
