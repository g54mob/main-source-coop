using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MessagePack.Resolvers;

namespace MessagePack
{
	public class MessagePackSerializerOptions
	{
		private static class MessagePackSerializerOptionsDefaultSettingsLazyInitializationHelper
		{
			public static readonly MessagePackSerializerOptions Standard = new MessagePackSerializerOptions(StandardResolver.Instance);
		}

		internal static readonly Regex AssemblyNameVersionSelectorRegex = new Regex(", Version=\\d+.\\d+.\\d+.\\d+, Culture=[\\w-]+, PublicKeyToken=(?:null|[a-f0-9]{16})", RegexOptions.Compiled);

		private static readonly HashSet<string> DisallowedTypes = new HashSet<string> { "System.CodeDom.Compiler.TempFileCollection", "System.Management.IWbemClassObjectFreeThreaded" };

		public static MessagePackSerializerOptions Standard => MessagePackSerializerOptionsDefaultSettingsLazyInitializationHelper.Standard;

		public IFormatterResolver Resolver { get; private set; }

		public MessagePackCompression Compression { get; private set; }

		public int CompressionMinLength { get; private set; } = 64;

		public int SuggestedContiguousMemorySize { get; private set; } = 1048576;

		public bool? OldSpec { get; private set; }

		public bool OmitAssemblyVersion { get; private set; }

		public bool AllowAssemblyVersionMismatch { get; private set; }

		public MessagePackSecurity Security { get; private set; } = MessagePackSecurity.TrustedData;

		public SequencePool SequencePool { get; private set; } = MessagePack.SequencePool.Shared;

		public MessagePackSerializerOptions(IFormatterResolver resolver)
		{
			Resolver = resolver ?? throw new ArgumentNullException("resolver");
		}

		protected MessagePackSerializerOptions(MessagePackSerializerOptions copyFrom)
		{
			if (copyFrom == null)
			{
				throw new ArgumentNullException("copyFrom");
			}
			Resolver = copyFrom.Resolver;
			Compression = copyFrom.Compression;
			CompressionMinLength = copyFrom.CompressionMinLength;
			SuggestedContiguousMemorySize = copyFrom.SuggestedContiguousMemorySize;
			OldSpec = copyFrom.OldSpec;
			OmitAssemblyVersion = copyFrom.OmitAssemblyVersion;
			AllowAssemblyVersionMismatch = copyFrom.AllowAssemblyVersionMismatch;
			Security = copyFrom.Security;
			SequencePool = copyFrom.SequencePool;
		}

		public virtual Type? LoadType(string typeName)
		{
			Type type = Type.GetType(typeName, throwOnError: false);
			if (type == null && AllowAssemblyVersionMismatch)
			{
				string text = AssemblyNameVersionSelectorRegex.Replace(typeName, string.Empty);
				if (text != typeName)
				{
					type = Type.GetType(text, throwOnError: false);
				}
			}
			return type;
		}

		public virtual void ThrowIfDeserializingTypeIsDisallowed(Type type)
		{
			string fullName = type.FullName;
			if (fullName != null && DisallowedTypes.Contains(fullName))
			{
				throw new MessagePackSerializationException("Deserialization attempted to create the type " + fullName + " which is not allowed.");
			}
		}

		public MessagePackSerializerOptions WithResolver(IFormatterResolver resolver)
		{
			if (Resolver == resolver)
			{
				return this;
			}
			MessagePackSerializerOptions messagePackSerializerOptions = Clone();
			messagePackSerializerOptions.Resolver = resolver;
			return messagePackSerializerOptions;
		}

		public MessagePackSerializerOptions WithCompression(MessagePackCompression compression)
		{
			if (Compression == compression)
			{
				return this;
			}
			MessagePackSerializerOptions messagePackSerializerOptions = Clone();
			messagePackSerializerOptions.Compression = compression;
			return messagePackSerializerOptions;
		}

		public MessagePackSerializerOptions WithCompressionMinLength(int compressionMinLength)
		{
			if (CompressionMinLength == compressionMinLength)
			{
				return this;
			}
			if (compressionMinLength <= 0)
			{
				throw new ArgumentOutOfRangeException("compressionMinLength");
			}
			MessagePackSerializerOptions messagePackSerializerOptions = Clone();
			messagePackSerializerOptions.CompressionMinLength = compressionMinLength;
			return messagePackSerializerOptions;
		}

		public MessagePackSerializerOptions WithSuggestedContiguousMemorySize(int suggestedContiguousMemorySize)
		{
			if (SuggestedContiguousMemorySize == suggestedContiguousMemorySize)
			{
				return this;
			}
			if (suggestedContiguousMemorySize < 256)
			{
				throw new ArgumentOutOfRangeException("suggestedContiguousMemorySize", "This should be at least 256");
			}
			MessagePackSerializerOptions messagePackSerializerOptions = Clone();
			messagePackSerializerOptions.SuggestedContiguousMemorySize = suggestedContiguousMemorySize;
			return messagePackSerializerOptions;
		}

		public MessagePackSerializerOptions WithOldSpec(bool? oldSpec = true)
		{
			if (OldSpec == oldSpec)
			{
				return this;
			}
			MessagePackSerializerOptions messagePackSerializerOptions = Clone();
			messagePackSerializerOptions.OldSpec = oldSpec;
			return messagePackSerializerOptions;
		}

		public MessagePackSerializerOptions WithOmitAssemblyVersion(bool omitAssemblyVersion)
		{
			if (OmitAssemblyVersion == omitAssemblyVersion)
			{
				return this;
			}
			MessagePackSerializerOptions messagePackSerializerOptions = Clone();
			messagePackSerializerOptions.OmitAssemblyVersion = omitAssemblyVersion;
			return messagePackSerializerOptions;
		}

		public MessagePackSerializerOptions WithAllowAssemblyVersionMismatch(bool allowAssemblyVersionMismatch)
		{
			if (AllowAssemblyVersionMismatch == allowAssemblyVersionMismatch)
			{
				return this;
			}
			MessagePackSerializerOptions messagePackSerializerOptions = Clone();
			messagePackSerializerOptions.AllowAssemblyVersionMismatch = allowAssemblyVersionMismatch;
			return messagePackSerializerOptions;
		}

		public MessagePackSerializerOptions WithSecurity(MessagePackSecurity security)
		{
			if (security == null)
			{
				throw new ArgumentNullException("security");
			}
			if (Security == security)
			{
				return this;
			}
			MessagePackSerializerOptions messagePackSerializerOptions = Clone();
			messagePackSerializerOptions.Security = security;
			return messagePackSerializerOptions;
		}

		public MessagePackSerializerOptions WithPool(SequencePool pool)
		{
			if (pool == null)
			{
				throw new ArgumentNullException("pool");
			}
			if (SequencePool == pool)
			{
				return this;
			}
			MessagePackSerializerOptions messagePackSerializerOptions = Clone();
			messagePackSerializerOptions.SequencePool = pool;
			return messagePackSerializerOptions;
		}

		protected virtual MessagePackSerializerOptions Clone()
		{
			if (GetType() != typeof(MessagePackSerializerOptions))
			{
				throw new NotSupportedException("The derived type " + GetType().FullName + " did not override the Clone method as required.");
			}
			return new MessagePackSerializerOptions(this);
		}
	}
}
