using System;
using System.Collections.Generic;
using CW.Common;
using UnityEngine;

namespace CW.ShaderBundle
{
	public class CwShaderBundle : ScriptableObject
	{
		public enum Pipeline
		{
			Invalid = -1,
			Standard = 0,
			URP2019 = 1,
			URP2020 = 2,
			URP2021 = 3,
			URP2022 = 4,
			URP2023 = 5,
			HDRP2019 = 6,
			HDRP2020 = 7,
			HDRP2021 = 8,
			HDRP2022 = 9,
			HDRP2023 = 10,
			COUNT = 11
		}

		[Serializable]
		public class ShaderVariant
		{
			public Pipeline Pipe;

			public string Code;

			public int Hash;

			public bool Dirty;

			public string HashString => "//<HASH>" + Hash + "</HASH>";
		}

		[SerializeField]
		private string title;

		[SerializeField]
		private Shader target;

		[SerializeField]
		private int variantHash;

		[SerializeField]
		private int projectHash;

		[SerializeField]
		private List<ShaderVariant> variants;

		public string Title
		{
			get
			{
				return title;
			}
			set
			{
				title = value;
			}
		}

		public Shader Target
		{
			get
			{
				return target;
			}
			set
			{
				target = value;
			}
		}

		public int VariantHash
		{
			get
			{
				return variantHash;
			}
			set
			{
				variantHash = value;
			}
		}

		public int ProjectHash
		{
			get
			{
				return projectHash;
			}
			set
			{
				projectHash = value;
			}
		}

		public List<ShaderVariant> Variants
		{
			get
			{
				if (variants == null)
				{
					variants = new List<ShaderVariant>();
				}
				return variants;
			}
		}

		public bool Dirty
		{
			get
			{
				if (variants != null)
				{
					foreach (ShaderVariant variant in variants)
					{
						if (variant.Dirty)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		public static int GetProjectHash()
		{
			return Application.productName.GetHashCode();
		}

		public static Pipeline DetectProjectPipeline()
		{
			if (CwHelper.IsSRP)
			{
				if (CwHelper.IsHDRP)
				{
					return Pipeline.HDRP2023;
				}
				if (CwHelper.IsURP)
				{
					return Pipeline.URP2023;
				}
				return Pipeline.Invalid;
			}
			return Pipeline.Standard;
		}

		public static bool IsStandard(Pipeline pipe)
		{
			return pipe == Pipeline.Standard;
		}

		public static bool IsScriptable(Pipeline pipe)
		{
			if (!IsURP(pipe))
			{
				return IsHDRP(pipe);
			}
			return true;
		}

		public static bool IsURP(Pipeline pipe)
		{
			if (pipe != Pipeline.URP2019 && pipe != Pipeline.URP2020 && pipe != Pipeline.URP2021)
			{
				return pipe == Pipeline.URP2022;
			}
			return true;
		}

		public static bool IsHDRP(Pipeline pipe)
		{
			if (pipe != Pipeline.HDRP2019 && pipe != Pipeline.HDRP2020 && pipe != Pipeline.HDRP2021)
			{
				return pipe == Pipeline.HDRP2022;
			}
			return true;
		}
	}
}
