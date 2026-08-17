using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using Microsoft.Win32.SafeHandles;

internal static class Interop
{
	internal static class Crypt32
	{
		internal struct CRYPT_OID_INFO
		{
			public int cbSize;

			public IntPtr pszOID;

			public IntPtr pwszName;

			public OidGroup dwGroupId;

			public int AlgId;

			public int cbData;

			public IntPtr pbData;

			public string OID => Marshal.PtrToStringAnsi(pszOID);

			public string Name => Marshal.PtrToStringUni(pwszName);
		}

		internal enum CryptOidInfoKeyType
		{
			CRYPT_OID_INFO_OID_KEY = 1,
			CRYPT_OID_INFO_NAME_KEY = 2,
			CRYPT_OID_INFO_ALGID_KEY = 3,
			CRYPT_OID_INFO_SIGN_KEY = 4,
			CRYPT_OID_INFO_CNG_ALGID_KEY = 5,
			CRYPT_OID_INFO_CNG_SIGN_KEY = 6
		}

		internal static CRYPT_OID_INFO FindOidInfo(CryptOidInfoKeyType keyType, string key, OidGroup group, bool fallBackToAllGroups)
		{
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				intPtr = keyType switch
				{
					CryptOidInfoKeyType.CRYPT_OID_INFO_OID_KEY => Marshal.StringToCoTaskMemAnsi(key), 
					CryptOidInfoKeyType.CRYPT_OID_INFO_NAME_KEY => Marshal.StringToCoTaskMemUni(key), 
					_ => throw new NotSupportedException(), 
				};
				if (!OidGroupWillNotUseActiveDirectory(group))
				{
					OidGroup oidGroup = group | (OidGroup)(-2147483648);
					IntPtr intPtr2 = CryptFindOIDInfo(keyType, intPtr, oidGroup);
					if (intPtr2 != IntPtr.Zero)
					{
						return Marshal.PtrToStructure<CRYPT_OID_INFO>(intPtr2);
					}
				}
				IntPtr intPtr3 = CryptFindOIDInfo(keyType, intPtr, group);
				if (intPtr3 != IntPtr.Zero)
				{
					return Marshal.PtrToStructure<CRYPT_OID_INFO>(intPtr3);
				}
				if (fallBackToAllGroups && group != OidGroup.All)
				{
					IntPtr intPtr4 = CryptFindOIDInfo(keyType, intPtr, OidGroup.All);
					if (intPtr4 != IntPtr.Zero)
					{
						return Marshal.PtrToStructure<CRYPT_OID_INFO>(intPtr4);
					}
				}
				return new CRYPT_OID_INFO
				{
					AlgId = -1
				};
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(intPtr);
				}
			}
		}

		private static bool OidGroupWillNotUseActiveDirectory(OidGroup group)
		{
			if (group != OidGroup.HashAlgorithm && group != OidGroup.EncryptionAlgorithm && group != OidGroup.PublicKeyAlgorithm && group != OidGroup.SignatureAlgorithm && group != OidGroup.Attribute && group != OidGroup.ExtensionOrAttribute)
			{
				return group == OidGroup.KeyDerivationFunction;
			}
			return true;
		}

		[DllImport("crypt32.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr CryptFindOIDInfo(CryptOidInfoKeyType dwKeyType, IntPtr pvKey, OidGroup group);
	}

	internal enum BOOL
	{
		FALSE = 0,
		TRUE = 1
	}

	internal class Kernel32
	{
		internal struct SECURITY_ATTRIBUTES
		{
			internal uint nLength;

			internal IntPtr lpSecurityDescriptor;

			internal BOOL bInheritHandle;
		}

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CloseHandle(IntPtr handle);

		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Unicode, EntryPoint = "CreateFileW", ExactSpelling = true, SetLastError = true)]
		private unsafe static extern IntPtr CreateFilePrivate(string lpFileName, int dwDesiredAccess, FileShare dwShareMode, SECURITY_ATTRIBUTES* securityAttrs, FileMode dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

		internal static SafeFileHandle CreateFile(string lpFileName, int dwDesiredAccess, FileShare dwShareMode, FileMode dwCreationDisposition, int dwFlagsAndAttributes)
		{
			IntPtr intPtr = CreateFile_IntPtr(lpFileName, dwDesiredAccess, dwShareMode, dwCreationDisposition, dwFlagsAndAttributes);
			try
			{
				return new SafeFileHandle(intPtr, ownsHandle: true);
			}
			catch
			{
				CloseHandle(intPtr);
				throw;
			}
		}

		internal unsafe static IntPtr CreateFile_IntPtr(string lpFileName, int dwDesiredAccess, FileShare dwShareMode, FileMode dwCreationDisposition, int dwFlagsAndAttributes)
		{
			lpFileName = PathInternal.EnsureExtendedPrefixIfNeeded(lpFileName);
			return CreateFilePrivate(lpFileName, dwDesiredAccess, dwShareMode, null, dwCreationDisposition, dwFlagsAndAttributes, IntPtr.Zero);
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal unsafe static extern bool ReadDirectoryChangesW(SafeFileHandle hDirectory, byte[] lpBuffer, uint nBufferLength, [MarshalAs(UnmanagedType.Bool)] bool bWatchSubtree, int dwNotifyFilter, out int lpBytesReturned, NativeOverlapped* lpOverlapped, IntPtr lpCompletionRoutine);
	}
}
