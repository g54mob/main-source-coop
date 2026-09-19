using System;
using System.Collections.Generic;
using Mirror.BouncyCastle.Asn1;
using Mirror.BouncyCastle.Asn1.CryptoPro;
using Mirror.BouncyCastle.Asn1.Kisa;
using Mirror.BouncyCastle.Asn1.Misc;
using Mirror.BouncyCastle.Asn1.Nist;
using Mirror.BouncyCastle.Asn1.Nsri;
using Mirror.BouncyCastle.Asn1.Ntt;
using Mirror.BouncyCastle.Asn1.Oiw;
using Mirror.BouncyCastle.Asn1.Pkcs;
using Mirror.BouncyCastle.Crypto;
using Mirror.BouncyCastle.Crypto.Agreement;
using Mirror.BouncyCastle.Crypto.Digests;
using Mirror.BouncyCastle.Crypto.Encodings;
using Mirror.BouncyCastle.Crypto.Engines;
using Mirror.BouncyCastle.Crypto.Generators;
using Mirror.BouncyCastle.Crypto.Macs;
using Mirror.BouncyCastle.Crypto.Modes;
using Mirror.BouncyCastle.Crypto.Paddings;
using Mirror.BouncyCastle.Utilities;
using Mirror.BouncyCastle.Utilities.Collections;

namespace Mirror.BouncyCastle.Security
{
	public static class CipherUtilities
	{
		private enum CipherAlgorithm
		{
			AES = 0,
			ARC4 = 1,
			ARIA = 2,
			BLOWFISH = 3,
			CAMELLIA = 4,
			CAST5 = 5,
			CAST6 = 6,
			CHACHA = 7,
			CHACHA20_POLY1305 = 8,
			CHACHA7539 = 9,
			DES = 10,
			DESEDE = 11,
			ELGAMAL = 12,
			GOST28147 = 13,
			HC128 = 14,
			HC256 = 15,
			IDEA = 16,
			NOEKEON = 17,
			PBEWITHSHAAND128BITRC4 = 18,
			PBEWITHSHAAND40BITRC4 = 19,
			RC2 = 20,
			RC5 = 21,
			RC5_64 = 22,
			RC6 = 23,
			RIJNDAEL = 24,
			RSA = 25,
			SALSA20 = 26,
			SEED = 27,
			SERPENT = 28,
			SKIPJACK = 29,
			SM4 = 30,
			TEA = 31,
			THREEFISH_256 = 32,
			THREEFISH_512 = 33,
			THREEFISH_1024 = 34,
			TNEPRES = 35,
			TWOFISH = 36,
			VMPC = 37,
			VMPC_KSA3 = 38,
			XTEA = 39
		}

		private enum CipherMode
		{
			ECB = 0,
			NONE = 1,
			CBC = 2,
			CCM = 3,
			CFB = 4,
			CTR = 5,
			CTS = 6,
			EAX = 7,
			GCM = 8,
			GOFB = 9,
			OCB = 10,
			OFB = 11,
			OPENPGPCFB = 12,
			SIC = 13
		}

		private enum CipherPadding
		{
			NOPADDING = 0,
			RAW = 1,
			ISO10126PADDING = 2,
			ISO10126D2PADDING = 3,
			ISO10126_2PADDING = 4,
			ISO7816_4PADDING = 5,
			ISO9797_1PADDING = 6,
			ISO9796_1 = 7,
			ISO9796_1PADDING = 8,
			OAEP = 9,
			OAEPPADDING = 10,
			OAEPWITHMD5ANDMGF1PADDING = 11,
			OAEPWITHSHA1ANDMGF1PADDING = 12,
			OAEPWITHSHA_1ANDMGF1PADDING = 13,
			OAEPWITHSHA224ANDMGF1PADDING = 14,
			OAEPWITHSHA_224ANDMGF1PADDING = 15,
			OAEPWITHSHA256ANDMGF1PADDING = 16,
			OAEPWITHSHA_256ANDMGF1PADDING = 17,
			OAEPWITHSHA256ANDMGF1WITHSHA256PADDING = 18,
			OAEPWITHSHA_256ANDMGF1WITHSHA_256PADDING = 19,
			OAEPWITHSHA256ANDMGF1WITHSHA1PADDING = 20,
			OAEPWITHSHA_256ANDMGF1WITHSHA_1PADDING = 21,
			OAEPWITHSHA384ANDMGF1PADDING = 22,
			OAEPWITHSHA_384ANDMGF1PADDING = 23,
			OAEPWITHSHA512ANDMGF1PADDING = 24,
			OAEPWITHSHA_512ANDMGF1PADDING = 25,
			PKCS1 = 26,
			PKCS1PADDING = 27,
			PKCS5 = 28,
			PKCS5PADDING = 29,
			PKCS7 = 30,
			PKCS7PADDING = 31,
			TBCPADDING = 32,
			WITHCTS = 33,
			X923PADDING = 34,
			ZEROBYTEPADDING = 35
		}

		private static readonly Dictionary<string, string> AlgorithmMap;

		private static readonly Dictionary<DerObjectIdentifier, string> AlgorithmOidMap;

		static CipherUtilities()
		{
			AlgorithmMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			AlgorithmOidMap = new Dictionary<DerObjectIdentifier, string>();
			Enums.GetArbitraryValue<CipherAlgorithm>().ToString();
			Enums.GetArbitraryValue<CipherMode>().ToString();
			Enums.GetArbitraryValue<CipherPadding>().ToString();
			AlgorithmOidMap[NistObjectIdentifiers.IdAes128Cbc] = "AES/CBC/PKCS7PADDING";
			AlgorithmOidMap[NistObjectIdentifiers.IdAes192Cbc] = "AES/CBC/PKCS7PADDING";
			AlgorithmOidMap[NistObjectIdentifiers.IdAes256Cbc] = "AES/CBC/PKCS7PADDING";
			AlgorithmOidMap[NistObjectIdentifiers.IdAes128Ccm] = "AES/CCM/NOPADDING";
			AlgorithmOidMap[NistObjectIdentifiers.IdAes192Ccm] = "AES/CCM/NOPADDING";
			AlgorithmOidMap[NistObjectIdentifiers.IdAes256Ccm] = "AES/CCM/NOPADDING";
			AlgorithmOidMap[NistObjectIdentifiers.IdAes128Cfb] = "AES/CFB/NOPADDING";
			AlgorithmOidMap[NistObjectIdentifiers.IdAes192Cfb] = "AES/CFB/NOPADDING";
			AlgorithmOidMap[NistObjectIdentifiers.IdAes256Cfb] = "AES/CFB/NOPADDING";
			AlgorithmOidMap[NistObjectIdentifiers.IdAes128Ecb] = "AES/ECB/PKCS7PADDING";
			AlgorithmOidMap[NistObjectIdentifiers.IdAes192Ecb] = "AES/ECB/PKCS7PADDING";
			AlgorithmOidMap[NistObjectIdentifiers.IdAes256Ecb] = "AES/ECB/PKCS7PADDING";
			AlgorithmMap["AES//PKCS7"] = "AES/ECB/PKCS7PADDING";
			AlgorithmMap["AES//PKCS7PADDING"] = "AES/ECB/PKCS7PADDING";
			AlgorithmMap["AES//PKCS5"] = "AES/ECB/PKCS7PADDING";
			AlgorithmMap["AES//PKCS5PADDING"] = "AES/ECB/PKCS7PADDING";
			AlgorithmOidMap[NistObjectIdentifiers.IdAes128Gcm] = "AES/GCM/NOPADDING";
			AlgorithmOidMap[NistObjectIdentifiers.IdAes192Gcm] = "AES/GCM/NOPADDING";
			AlgorithmOidMap[NistObjectIdentifiers.IdAes256Gcm] = "AES/GCM/NOPADDING";
			AlgorithmOidMap[NistObjectIdentifiers.IdAes128Ofb] = "AES/OFB/NOPADDING";
			AlgorithmOidMap[NistObjectIdentifiers.IdAes192Ofb] = "AES/OFB/NOPADDING";
			AlgorithmOidMap[NistObjectIdentifiers.IdAes256Ofb] = "AES/OFB/NOPADDING";
			AlgorithmOidMap[NsriObjectIdentifiers.id_aria128_cbc] = "ARIA/CBC/PKCS7PADDING";
			AlgorithmOidMap[NsriObjectIdentifiers.id_aria192_cbc] = "ARIA/CBC/PKCS7PADDING";
			AlgorithmOidMap[NsriObjectIdentifiers.id_aria256_cbc] = "ARIA/CBC/PKCS7PADDING";
			AlgorithmOidMap[NsriObjectIdentifiers.id_aria128_ccm] = "ARIA/CCM/NOPADDING";
			AlgorithmOidMap[NsriObjectIdentifiers.id_aria192_ccm] = "ARIA/CCM/NOPADDING";
			AlgorithmOidMap[NsriObjectIdentifiers.id_aria256_ccm] = "ARIA/CCM/NOPADDING";
			AlgorithmOidMap[NsriObjectIdentifiers.id_aria128_cfb] = "ARIA/CFB/NOPADDING";
			AlgorithmOidMap[NsriObjectIdentifiers.id_aria192_cfb] = "ARIA/CFB/NOPADDING";
			AlgorithmOidMap[NsriObjectIdentifiers.id_aria256_cfb] = "ARIA/CFB/NOPADDING";
			AlgorithmOidMap[NsriObjectIdentifiers.id_aria128_ctr] = "ARIA/CTR/NOPADDING";
			AlgorithmOidMap[NsriObjectIdentifiers.id_aria192_ctr] = "ARIA/CTR/NOPADDING";
			AlgorithmOidMap[NsriObjectIdentifiers.id_aria256_ctr] = "ARIA/CTR/NOPADDING";
			AlgorithmOidMap[NsriObjectIdentifiers.id_aria128_ecb] = "ARIA/ECB/PKCS7PADDING";
			AlgorithmOidMap[NsriObjectIdentifiers.id_aria192_ecb] = "ARIA/ECB/PKCS7PADDING";
			AlgorithmOidMap[NsriObjectIdentifiers.id_aria256_ecb] = "ARIA/ECB/PKCS7PADDING";
			AlgorithmMap["ARIA//PKCS7"] = "ARIA/ECB/PKCS7PADDING";
			AlgorithmMap["ARIA//PKCS7PADDING"] = "ARIA/ECB/PKCS7PADDING";
			AlgorithmMap["ARIA//PKCS5"] = "ARIA/ECB/PKCS7PADDING";
			AlgorithmMap["ARIA//PKCS5PADDING"] = "ARIA/ECB/PKCS7PADDING";
			AlgorithmOidMap[NsriObjectIdentifiers.id_aria128_gcm] = "ARIA/GCM/NOPADDING";
			AlgorithmOidMap[NsriObjectIdentifiers.id_aria192_gcm] = "ARIA/GCM/NOPADDING";
			AlgorithmOidMap[NsriObjectIdentifiers.id_aria256_gcm] = "ARIA/GCM/NOPADDING";
			AlgorithmOidMap[NsriObjectIdentifiers.id_aria128_ofb] = "ARIA/OFB/NOPADDING";
			AlgorithmOidMap[NsriObjectIdentifiers.id_aria192_ofb] = "ARIA/OFB/NOPADDING";
			AlgorithmOidMap[NsriObjectIdentifiers.id_aria256_ofb] = "ARIA/OFB/NOPADDING";
			AlgorithmMap["RSA/ECB/PKCS1"] = "RSA//PKCS1PADDING";
			AlgorithmMap["RSA/ECB/PKCS1PADDING"] = "RSA//PKCS1PADDING";
			AlgorithmOidMap[PkcsObjectIdentifiers.RsaEncryption] = "RSA//PKCS1PADDING";
			AlgorithmOidMap[PkcsObjectIdentifiers.IdRsaesOaep] = "RSA//OAEPPADDING";
			AlgorithmOidMap[OiwObjectIdentifiers.DesCbc] = "DES/CBC";
			AlgorithmOidMap[OiwObjectIdentifiers.DesCfb] = "DES/CFB";
			AlgorithmOidMap[OiwObjectIdentifiers.DesEcb] = "DES/ECB";
			AlgorithmOidMap[OiwObjectIdentifiers.DesOfb] = "DES/OFB";
			AlgorithmOidMap[OiwObjectIdentifiers.DesEde] = "DESEDE";
			AlgorithmMap["TDEA"] = "DESEDE";
			AlgorithmOidMap[PkcsObjectIdentifiers.DesEde3Cbc] = "DESEDE/CBC";
			AlgorithmOidMap[PkcsObjectIdentifiers.RC2Cbc] = "RC2/CBC";
			AlgorithmOidMap[MiscObjectIdentifiers.as_sys_sec_alg_ideaCBC] = "IDEA/CBC";
			AlgorithmOidMap[MiscObjectIdentifiers.cast5CBC] = "CAST5/CBC";
			AlgorithmMap["RC4"] = "ARC4";
			AlgorithmMap["ARCFOUR"] = "ARC4";
			AlgorithmOidMap[PkcsObjectIdentifiers.rc4] = "ARC4";
			AlgorithmMap["PBEWITHSHA1AND128BITRC4"] = "PBEWITHSHAAND128BITRC4";
			AlgorithmOidMap[PkcsObjectIdentifiers.PbeWithShaAnd128BitRC4] = "PBEWITHSHAAND128BITRC4";
			AlgorithmMap["PBEWITHSHA1AND40BITRC4"] = "PBEWITHSHAAND40BITRC4";
			AlgorithmOidMap[PkcsObjectIdentifiers.PbeWithShaAnd40BitRC4] = "PBEWITHSHAAND40BITRC4";
			AlgorithmMap["PBEWITHSHA1ANDDES"] = "PBEWITHSHA1ANDDES-CBC";
			AlgorithmOidMap[PkcsObjectIdentifiers.PbeWithSha1AndDesCbc] = "PBEWITHSHA1ANDDES-CBC";
			AlgorithmMap["PBEWITHSHA1ANDRC2"] = "PBEWITHSHA1ANDRC2-CBC";
			AlgorithmOidMap[PkcsObjectIdentifiers.PbeWithSha1AndRC2Cbc] = "PBEWITHSHA1ANDRC2-CBC";
			AlgorithmMap["PBEWITHSHA1AND3-KEYTRIPLEDES-CBC"] = "PBEWITHSHAAND3-KEYTRIPLEDES-CBC";
			AlgorithmMap["PBEWITHSHAAND3KEYTRIPLEDES"] = "PBEWITHSHAAND3-KEYTRIPLEDES-CBC";
			AlgorithmOidMap[PkcsObjectIdentifiers.PbeWithShaAnd3KeyTripleDesCbc] = "PBEWITHSHAAND3-KEYTRIPLEDES-CBC";
			AlgorithmMap["PBEWITHSHA1ANDDESEDE"] = "PBEWITHSHAAND3-KEYTRIPLEDES-CBC";
			AlgorithmMap["PBEWITHSHA1AND2-KEYTRIPLEDES-CBC"] = "PBEWITHSHAAND2-KEYTRIPLEDES-CBC";
			AlgorithmOidMap[PkcsObjectIdentifiers.PbeWithShaAnd2KeyTripleDesCbc] = "PBEWITHSHAAND2-KEYTRIPLEDES-CBC";
			AlgorithmMap["PBEWITHSHA1AND128BITRC2-CBC"] = "PBEWITHSHAAND128BITRC2-CBC";
			AlgorithmOidMap[PkcsObjectIdentifiers.PbeWithShaAnd128BitRC2Cbc] = "PBEWITHSHAAND128BITRC2-CBC";
			AlgorithmMap["PBEWITHSHA1AND40BITRC2-CBC"] = "PBEWITHSHAAND40BITRC2-CBC";
			AlgorithmOidMap[PkcsObjectIdentifiers.PbewithShaAnd40BitRC2Cbc] = "PBEWITHSHAAND40BITRC2-CBC";
			AlgorithmMap["PBEWITHSHA1AND128BITAES-CBC-BC"] = "PBEWITHSHAAND128BITAES-CBC-BC";
			AlgorithmMap["PBEWITHSHA-1AND128BITAES-CBC-BC"] = "PBEWITHSHAAND128BITAES-CBC-BC";
			AlgorithmMap["PBEWITHSHA1AND192BITAES-CBC-BC"] = "PBEWITHSHAAND192BITAES-CBC-BC";
			AlgorithmMap["PBEWITHSHA-1AND192BITAES-CBC-BC"] = "PBEWITHSHAAND192BITAES-CBC-BC";
			AlgorithmMap["PBEWITHSHA1AND256BITAES-CBC-BC"] = "PBEWITHSHAAND256BITAES-CBC-BC";
			AlgorithmMap["PBEWITHSHA-1AND256BITAES-CBC-BC"] = "PBEWITHSHAAND256BITAES-CBC-BC";
			AlgorithmMap["PBEWITHSHA-256AND128BITAES-CBC-BC"] = "PBEWITHSHA256AND128BITAES-CBC-BC";
			AlgorithmMap["PBEWITHSHA-256AND192BITAES-CBC-BC"] = "PBEWITHSHA256AND192BITAES-CBC-BC";
			AlgorithmMap["PBEWITHSHA-256AND256BITAES-CBC-BC"] = "PBEWITHSHA256AND256BITAES-CBC-BC";
			AlgorithmMap["GOST"] = "GOST28147";
			AlgorithmMap["GOST-28147"] = "GOST28147";
			AlgorithmOidMap[CryptoProObjectIdentifiers.GostR28147Gcfb] = "GOST28147/CBC/PKCS7PADDING";
			AlgorithmMap["RC5-32"] = "RC5";
			AlgorithmOidMap[NttObjectIdentifiers.IdCamellia128Cbc] = "CAMELLIA/CBC/PKCS7PADDING";
			AlgorithmOidMap[NttObjectIdentifiers.IdCamellia192Cbc] = "CAMELLIA/CBC/PKCS7PADDING";
			AlgorithmOidMap[NttObjectIdentifiers.IdCamellia256Cbc] = "CAMELLIA/CBC/PKCS7PADDING";
			AlgorithmOidMap[KisaObjectIdentifiers.IdSeedCbc] = "SEED/CBC/PKCS7PADDING";
			AlgorithmOidMap[new DerObjectIdentifier("1.3.6.1.4.1.3029.1.2")] = "BLOWFISH/CBC";
			AlgorithmOidMap[MiscObjectIdentifiers.cryptlib_algorithm_blowfish_CBC] = "BLOWFISH/CBC";
			AlgorithmMap["CHACHA20"] = "CHACHA7539";
			AlgorithmOidMap[PkcsObjectIdentifiers.IdAlgAeadChaCha20Poly1305] = "CHACHA20-POLY1305";
		}

		public static string GetAlgorithmName(DerObjectIdentifier oid)
		{
			return CollectionUtilities.GetValueOrNull(AlgorithmOidMap, oid);
		}

		public static IBufferedCipher GetCipher(DerObjectIdentifier oid)
		{
			if (oid == null)
			{
				throw new ArgumentNullException("oid");
			}
			if (AlgorithmOidMap.TryGetValue(oid, out var value))
			{
				IBufferedCipher cipherForMechanism = GetCipherForMechanism(value);
				if (cipherForMechanism != null)
				{
					return cipherForMechanism;
				}
			}
			throw new SecurityUtilityException("Cipher OID not recognised.");
		}

		public static IBufferedCipher GetCipher(string algorithm)
		{
			if (algorithm == null)
			{
				throw new ArgumentNullException("algorithm");
			}
			IBufferedCipher cipherForMechanism = GetCipherForMechanism(GetMechanism(algorithm) ?? algorithm.ToUpperInvariant());
			if (cipherForMechanism != null)
			{
				return cipherForMechanism;
			}
			throw new SecurityUtilityException("Cipher " + algorithm + " not recognised.");
		}

		private static IBufferedCipher GetCipherForMechanism(string mechanism)
		{
			IBasicAgreement basicAgreement = null;
			if (mechanism == "IES")
			{
				basicAgreement = new DHBasicAgreement();
			}
			else if (mechanism == "ECIES")
			{
				basicAgreement = new ECDHBasicAgreement();
			}
			if (basicAgreement != null)
			{
				return new BufferedIesCipher(new IesEngine(basicAgreement, new Kdf2BytesGenerator(new Sha1Digest()), new HMac(new Sha1Digest())));
			}
			if (Platform.StartsWith(mechanism, "PBE"))
			{
				if (Platform.EndsWith(mechanism, "-CBC"))
				{
					if (mechanism == "PBEWITHSHA1ANDDES-CBC")
					{
						return new PaddedBufferedBlockCipher(new CbcBlockCipher(new DesEngine()));
					}
					if (mechanism == "PBEWITHSHA1ANDRC2-CBC")
					{
						return new PaddedBufferedBlockCipher(new CbcBlockCipher(new RC2Engine()));
					}
					if (Strings.IsOneOf(mechanism, "PBEWITHSHAAND2-KEYTRIPLEDES-CBC", "PBEWITHSHAAND3-KEYTRIPLEDES-CBC"))
					{
						return new PaddedBufferedBlockCipher(new CbcBlockCipher(new DesEdeEngine()));
					}
					if (Strings.IsOneOf(mechanism, "PBEWITHSHAAND128BITRC2-CBC", "PBEWITHSHAAND40BITRC2-CBC"))
					{
						return new PaddedBufferedBlockCipher(new CbcBlockCipher(new RC2Engine()));
					}
				}
				else if ((Platform.EndsWith(mechanism, "-BC") || Platform.EndsWith(mechanism, "-OPENSSL")) && Strings.IsOneOf(mechanism, "PBEWITHSHAAND128BITAES-CBC-BC", "PBEWITHSHAAND192BITAES-CBC-BC", "PBEWITHSHAAND256BITAES-CBC-BC", "PBEWITHSHA256AND128BITAES-CBC-BC", "PBEWITHSHA256AND192BITAES-CBC-BC", "PBEWITHSHA256AND256BITAES-CBC-BC", "PBEWITHMD5AND128BITAES-CBC-OPENSSL", "PBEWITHMD5AND192BITAES-CBC-OPENSSL", "PBEWITHMD5AND256BITAES-CBC-OPENSSL"))
				{
					return new PaddedBufferedBlockCipher(new CbcBlockCipher(AesUtilities.CreateEngine()));
				}
			}
			string[] array = mechanism.Split(new char[1] { '/' });
			IAeadCipher aeadCipher = null;
			IBlockCipher blockCipher = null;
			IAsymmetricBlockCipher asymmetricBlockCipher = null;
			IStreamCipher streamCipher = null;
			if (!Enums.TryGetEnumValue<CipherAlgorithm>(CollectionUtilities.GetValueOrKey(AlgorithmMap, array[0]).ToUpperInvariant(), out var result))
			{
				return null;
			}
			switch (result)
			{
			case CipherAlgorithm.AES:
				blockCipher = AesUtilities.CreateEngine();
				break;
			case CipherAlgorithm.ARC4:
				streamCipher = new RC4Engine();
				break;
			case CipherAlgorithm.ARIA:
				blockCipher = new AriaEngine();
				break;
			case CipherAlgorithm.BLOWFISH:
				blockCipher = new BlowfishEngine();
				break;
			case CipherAlgorithm.CAMELLIA:
				blockCipher = new CamelliaEngine();
				break;
			case CipherAlgorithm.CAST5:
				blockCipher = new Cast5Engine();
				break;
			case CipherAlgorithm.CAST6:
				blockCipher = new Cast6Engine();
				break;
			case CipherAlgorithm.CHACHA:
				streamCipher = new ChaChaEngine();
				break;
			case CipherAlgorithm.CHACHA20_POLY1305:
				aeadCipher = new ChaCha20Poly1305();
				break;
			case CipherAlgorithm.CHACHA7539:
				streamCipher = new ChaCha7539Engine();
				break;
			case CipherAlgorithm.DES:
				blockCipher = new DesEngine();
				break;
			case CipherAlgorithm.DESEDE:
				blockCipher = new DesEdeEngine();
				break;
			case CipherAlgorithm.ELGAMAL:
				asymmetricBlockCipher = new ElGamalEngine();
				break;
			case CipherAlgorithm.GOST28147:
				blockCipher = new Gost28147Engine();
				break;
			case CipherAlgorithm.HC128:
				streamCipher = new HC128Engine();
				break;
			case CipherAlgorithm.HC256:
				streamCipher = new HC256Engine();
				break;
			case CipherAlgorithm.IDEA:
				blockCipher = new IdeaEngine();
				break;
			case CipherAlgorithm.NOEKEON:
				blockCipher = new NoekeonEngine();
				break;
			case CipherAlgorithm.PBEWITHSHAAND128BITRC4:
			case CipherAlgorithm.PBEWITHSHAAND40BITRC4:
				streamCipher = new RC4Engine();
				break;
			case CipherAlgorithm.RC2:
				blockCipher = new RC2Engine();
				break;
			case CipherAlgorithm.RC5:
				blockCipher = new RC532Engine();
				break;
			case CipherAlgorithm.RC5_64:
				blockCipher = new RC564Engine();
				break;
			case CipherAlgorithm.RC6:
				blockCipher = new RC6Engine();
				break;
			case CipherAlgorithm.RIJNDAEL:
				blockCipher = new RijndaelEngine();
				break;
			case CipherAlgorithm.RSA:
				asymmetricBlockCipher = new RsaBlindedEngine();
				break;
			case CipherAlgorithm.SALSA20:
				streamCipher = new Salsa20Engine();
				break;
			case CipherAlgorithm.SEED:
				blockCipher = new SeedEngine();
				break;
			case CipherAlgorithm.SERPENT:
				blockCipher = new SerpentEngine();
				break;
			case CipherAlgorithm.SKIPJACK:
				blockCipher = new SkipjackEngine();
				break;
			case CipherAlgorithm.SM4:
				blockCipher = new SM4Engine();
				break;
			case CipherAlgorithm.TEA:
				blockCipher = new TeaEngine();
				break;
			case CipherAlgorithm.THREEFISH_256:
				blockCipher = new ThreefishEngine(256);
				break;
			case CipherAlgorithm.THREEFISH_512:
				blockCipher = new ThreefishEngine(512);
				break;
			case CipherAlgorithm.THREEFISH_1024:
				blockCipher = new ThreefishEngine(1024);
				break;
			case CipherAlgorithm.TNEPRES:
				blockCipher = new TnepresEngine();
				break;
			case CipherAlgorithm.TWOFISH:
				blockCipher = new TwofishEngine();
				break;
			case CipherAlgorithm.VMPC:
				streamCipher = new VmpcEngine();
				break;
			case CipherAlgorithm.VMPC_KSA3:
				streamCipher = new VmpcKsa3Engine();
				break;
			case CipherAlgorithm.XTEA:
				blockCipher = new XteaEngine();
				break;
			default:
				return null;
			}
			if (aeadCipher != null)
			{
				if (array.Length > 1)
				{
					throw new ArgumentException("Modes and paddings cannot be applied to AEAD ciphers");
				}
				return new BufferedAeadCipher(aeadCipher);
			}
			if (streamCipher != null)
			{
				if (array.Length > 1)
				{
					throw new ArgumentException("Modes and paddings not used for stream ciphers");
				}
				return new BufferedStreamCipher(streamCipher);
			}
			bool flag = false;
			bool flag2 = true;
			IBlockCipherPadding blockCipherPadding = null;
			IAeadBlockCipher aeadBlockCipher = null;
			if (array.Length > 2)
			{
				string text = array[2];
				CipherPadding result2;
				if (text == "")
				{
					result2 = CipherPadding.RAW;
				}
				else if (text == "X9.23PADDING")
				{
					result2 = CipherPadding.X923PADDING;
				}
				else if (!Enums.TryGetEnumValue<CipherPadding>(text, out result2))
				{
					return null;
				}
				switch (result2)
				{
				case CipherPadding.NOPADDING:
					flag2 = false;
					break;
				case CipherPadding.ISO10126PADDING:
				case CipherPadding.ISO10126D2PADDING:
				case CipherPadding.ISO10126_2PADDING:
					blockCipherPadding = new ISO10126d2Padding();
					break;
				case CipherPadding.ISO7816_4PADDING:
				case CipherPadding.ISO9797_1PADDING:
					blockCipherPadding = new ISO7816d4Padding();
					break;
				case CipherPadding.ISO9796_1:
				case CipherPadding.ISO9796_1PADDING:
					asymmetricBlockCipher = new ISO9796d1Encoding(asymmetricBlockCipher);
					break;
				case CipherPadding.OAEP:
				case CipherPadding.OAEPPADDING:
					asymmetricBlockCipher = new OaepEncoding(asymmetricBlockCipher);
					break;
				case CipherPadding.OAEPWITHMD5ANDMGF1PADDING:
					asymmetricBlockCipher = new OaepEncoding(asymmetricBlockCipher, new MD5Digest());
					break;
				case CipherPadding.OAEPWITHSHA1ANDMGF1PADDING:
				case CipherPadding.OAEPWITHSHA_1ANDMGF1PADDING:
					asymmetricBlockCipher = new OaepEncoding(asymmetricBlockCipher, new Sha1Digest());
					break;
				case CipherPadding.OAEPWITHSHA224ANDMGF1PADDING:
				case CipherPadding.OAEPWITHSHA_224ANDMGF1PADDING:
					asymmetricBlockCipher = new OaepEncoding(asymmetricBlockCipher, new Sha224Digest());
					break;
				case CipherPadding.OAEPWITHSHA256ANDMGF1PADDING:
				case CipherPadding.OAEPWITHSHA_256ANDMGF1PADDING:
				case CipherPadding.OAEPWITHSHA256ANDMGF1WITHSHA256PADDING:
				case CipherPadding.OAEPWITHSHA_256ANDMGF1WITHSHA_256PADDING:
					asymmetricBlockCipher = new OaepEncoding(asymmetricBlockCipher, new Sha256Digest());
					break;
				case CipherPadding.OAEPWITHSHA256ANDMGF1WITHSHA1PADDING:
				case CipherPadding.OAEPWITHSHA_256ANDMGF1WITHSHA_1PADDING:
					asymmetricBlockCipher = new OaepEncoding(asymmetricBlockCipher, new Sha256Digest(), new Sha1Digest(), null);
					break;
				case CipherPadding.OAEPWITHSHA384ANDMGF1PADDING:
				case CipherPadding.OAEPWITHSHA_384ANDMGF1PADDING:
					asymmetricBlockCipher = new OaepEncoding(asymmetricBlockCipher, new Sha384Digest());
					break;
				case CipherPadding.OAEPWITHSHA512ANDMGF1PADDING:
				case CipherPadding.OAEPWITHSHA_512ANDMGF1PADDING:
					asymmetricBlockCipher = new OaepEncoding(asymmetricBlockCipher, new Sha512Digest());
					break;
				case CipherPadding.PKCS1:
				case CipherPadding.PKCS1PADDING:
					asymmetricBlockCipher = new Pkcs1Encoding(asymmetricBlockCipher);
					break;
				case CipherPadding.PKCS5:
				case CipherPadding.PKCS5PADDING:
				case CipherPadding.PKCS7:
				case CipherPadding.PKCS7PADDING:
					blockCipherPadding = new Pkcs7Padding();
					break;
				case CipherPadding.TBCPADDING:
					blockCipherPadding = new TbcPadding();
					break;
				case CipherPadding.WITHCTS:
					flag = true;
					break;
				case CipherPadding.X923PADDING:
					blockCipherPadding = new X923Padding();
					break;
				case CipherPadding.ZEROBYTEPADDING:
					blockCipherPadding = new ZeroBytePadding();
					break;
				default:
					return null;
				case CipherPadding.RAW:
					break;
				}
			}
			string text2 = "";
			IBlockCipherMode blockCipherMode = null;
			if (array.Length > 1)
			{
				text2 = array[1];
				int digitIndex = GetDigitIndex(text2);
				string text3 = ((digitIndex >= 0) ? text2.Substring(0, digitIndex) : text2);
				CipherMode result3;
				if (text3 == "")
				{
					result3 = CipherMode.NONE;
				}
				else if (!Enums.TryGetEnumValue<CipherMode>(text3, out result3))
				{
					return null;
				}
				switch (result3)
				{
				case CipherMode.CBC:
					blockCipherMode = new CbcBlockCipher(blockCipher);
					break;
				case CipherMode.CCM:
					aeadBlockCipher = new CcmBlockCipher(blockCipher);
					break;
				case CipherMode.CFB:
				{
					int bitBlockSize = ((digitIndex < 0) ? (8 * blockCipher.GetBlockSize()) : int.Parse(text2.Substring(digitIndex)));
					blockCipherMode = new CfbBlockCipher(blockCipher, bitBlockSize);
					break;
				}
				case CipherMode.CTR:
					blockCipherMode = new SicBlockCipher(blockCipher);
					break;
				case CipherMode.CTS:
					flag = true;
					blockCipherMode = new CbcBlockCipher(blockCipher);
					break;
				case CipherMode.EAX:
					aeadBlockCipher = new EaxBlockCipher(blockCipher);
					break;
				case CipherMode.GCM:
					aeadBlockCipher = new GcmBlockCipher(blockCipher);
					break;
				case CipherMode.GOFB:
					blockCipherMode = new GOfbBlockCipher(blockCipher);
					break;
				case CipherMode.OCB:
					aeadBlockCipher = new OcbBlockCipher(blockCipher, CreateBlockCipher(result));
					break;
				case CipherMode.OFB:
				{
					int blockSize = ((digitIndex < 0) ? (8 * blockCipher.GetBlockSize()) : int.Parse(text2.Substring(digitIndex)));
					blockCipherMode = new OfbBlockCipher(blockCipher, blockSize);
					break;
				}
				case CipherMode.OPENPGPCFB:
					blockCipherMode = new OpenPgpCfbBlockCipher(blockCipher);
					break;
				case CipherMode.SIC:
					if (blockCipher.GetBlockSize() < 16)
					{
						return null;
					}
					blockCipherMode = new SicBlockCipher(blockCipher);
					break;
				default:
					return null;
				case CipherMode.ECB:
				case CipherMode.NONE:
					break;
				}
			}
			if (aeadBlockCipher != null)
			{
				if (flag)
				{
					throw new SecurityUtilityException("CTS mode not valid for AEAD ciphers.");
				}
				if (flag2 && array.Length > 2 && array[2] != "")
				{
					throw new SecurityUtilityException("Bad padding specified for AEAD cipher.");
				}
				return new BufferedAeadBlockCipher(aeadBlockCipher);
			}
			if (blockCipher != null)
			{
				if (blockCipherMode == null)
				{
					blockCipherMode = EcbBlockCipher.GetBlockCipherMode(blockCipher);
				}
				if (flag)
				{
					return new CtsBlockCipher(blockCipherMode);
				}
				if (blockCipherPadding != null)
				{
					return new PaddedBufferedBlockCipher(blockCipherMode, blockCipherPadding);
				}
				if (!flag2 || blockCipherMode.IsPartialBlockOkay)
				{
					return new BufferedBlockCipher(blockCipherMode);
				}
				return new PaddedBufferedBlockCipher(blockCipherMode);
			}
			if (asymmetricBlockCipher != null)
			{
				return new BufferedAsymmetricBlockCipher(asymmetricBlockCipher);
			}
			return null;
		}

		private static int GetDigitIndex(string s)
		{
			for (int i = 0; i < s.Length; i++)
			{
				if (char.IsDigit(s[i]))
				{
					return i;
				}
			}
			return -1;
		}

		private static string GetMechanism(string algorithm)
		{
			if (AlgorithmMap.TryGetValue(algorithm, out var value))
			{
				return value;
			}
			if (DerObjectIdentifier.TryFromID(algorithm, out var oid) && AlgorithmOidMap.TryGetValue(oid, out var value2))
			{
				return value2;
			}
			return null;
		}

		private static IBlockCipher CreateBlockCipher(CipherAlgorithm cipherAlgorithm)
		{
			return cipherAlgorithm switch
			{
				CipherAlgorithm.AES => AesUtilities.CreateEngine(), 
				CipherAlgorithm.ARIA => new AriaEngine(), 
				CipherAlgorithm.BLOWFISH => new BlowfishEngine(), 
				CipherAlgorithm.CAMELLIA => new CamelliaEngine(), 
				CipherAlgorithm.CAST5 => new Cast5Engine(), 
				CipherAlgorithm.CAST6 => new Cast6Engine(), 
				CipherAlgorithm.DES => new DesEngine(), 
				CipherAlgorithm.DESEDE => new DesEdeEngine(), 
				CipherAlgorithm.GOST28147 => new Gost28147Engine(), 
				CipherAlgorithm.IDEA => new IdeaEngine(), 
				CipherAlgorithm.NOEKEON => new NoekeonEngine(), 
				CipherAlgorithm.RC2 => new RC2Engine(), 
				CipherAlgorithm.RC5 => new RC532Engine(), 
				CipherAlgorithm.RC5_64 => new RC564Engine(), 
				CipherAlgorithm.RC6 => new RC6Engine(), 
				CipherAlgorithm.RIJNDAEL => new RijndaelEngine(), 
				CipherAlgorithm.SEED => new SeedEngine(), 
				CipherAlgorithm.SERPENT => new SerpentEngine(), 
				CipherAlgorithm.SKIPJACK => new SkipjackEngine(), 
				CipherAlgorithm.SM4 => new SM4Engine(), 
				CipherAlgorithm.TEA => new TeaEngine(), 
				CipherAlgorithm.THREEFISH_256 => new ThreefishEngine(256), 
				CipherAlgorithm.THREEFISH_512 => new ThreefishEngine(512), 
				CipherAlgorithm.THREEFISH_1024 => new ThreefishEngine(1024), 
				CipherAlgorithm.TNEPRES => new TnepresEngine(), 
				CipherAlgorithm.TWOFISH => new TwofishEngine(), 
				CipherAlgorithm.XTEA => new XteaEngine(), 
				_ => throw new SecurityUtilityException("Cipher " + cipherAlgorithm.ToString() + " not recognised or not a block cipher"), 
			};
		}
	}
}
