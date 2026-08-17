using Cysharp.Threading.Tasks;
using UnityEngine;

namespace EvilCore.EvilSave.Tests
{
	public class EvilSaveTestRunner : MonoBehaviour
	{
		[Header("Test Components (auto-detected)")]
		[SerializeField]
		private EvilSavePrimitiveTests primitiveTests;

		[SerializeField]
		private EvilSaveUnityTypeTests unityTypeTests;

		[SerializeField]
		private EvilSaveCollectionTests collectionTests;

		[SerializeField]
		private EvilSaveComplexTypeTests complexTypeTests;

		[SerializeField]
		private EvilSaveSerializationTests serializationTests;

		[SerializeField]
		private EvilSavePlayerPrefsTests playerPrefsTests;

		[SerializeField]
		private EvilSaveSlotTests slotTests;

		[SerializeField]
		private EvilSaveDiskIOTests diskIOTests;

		[Header("Results")]
		[SerializeField]
		private int totalPassed;

		[SerializeField]
		private int totalFailed;

		private void OnValidate()
		{
			AutoDetectComponents();
		}

		private void AutoDetectComponents()
		{
			primitiveTests = GetComponentInChildren<EvilSavePrimitiveTests>(includeInactive: true);
			unityTypeTests = GetComponentInChildren<EvilSaveUnityTypeTests>(includeInactive: true);
			collectionTests = GetComponentInChildren<EvilSaveCollectionTests>(includeInactive: true);
			complexTypeTests = GetComponentInChildren<EvilSaveComplexTypeTests>(includeInactive: true);
			serializationTests = GetComponentInChildren<EvilSaveSerializationTests>(includeInactive: true);
			playerPrefsTests = GetComponentInChildren<EvilSavePlayerPrefsTests>(includeInactive: true);
			slotTests = GetComponentInChildren<EvilSaveSlotTests>(includeInactive: true);
			diskIOTests = GetComponentInChildren<EvilSaveDiskIOTests>(includeInactive: true);
		}

		public void RunAllTests()
		{
			RunAllTestsAsync().Forget();
		}

		private async UniTaskVoid RunAllTestsAsync()
		{
			totalPassed = 0;
			totalFailed = 0;
			string originalSlot = EvilSave.ActiveSlot;
			EvilSaveSettings instance = EvilSaveSettings.Instance;
			bool enableCompression = instance.EnableCompression;
			bool enableEncryption = instance.EnableEncryption;
			SaveFormat format = instance.Format;
			instance.EnableCompression = false;
			instance.EnableEncryption = false;
			instance.Format = SaveFormat.Binary;
			if (primitiveTests != null)
			{
				primitiveTests.RunAll();
				Accumulate(primitiveTests);
			}
			if (unityTypeTests != null)
			{
				unityTypeTests.RunAll();
				Accumulate(unityTypeTests);
			}
			if (collectionTests != null)
			{
				collectionTests.RunAll();
				Accumulate(collectionTests);
			}
			if (complexTypeTests != null)
			{
				complexTypeTests.RunAll();
				Accumulate(complexTypeTests);
			}
			if (serializationTests != null)
			{
				serializationTests.RunAll();
				Accumulate(serializationTests);
			}
			if (playerPrefsTests != null)
			{
				playerPrefsTests.RunAll();
				Accumulate(playerPrefsTests);
			}
			if (slotTests != null)
			{
				slotTests.RunAll();
				Accumulate(slotTests);
			}
			instance.EnableCompression = enableCompression;
			instance.EnableEncryption = enableEncryption;
			instance.Format = format;
			if (diskIOTests != null)
			{
				diskIOTests.RunAll();
				await UniTask.WaitUntil(() => diskIOTests.PassedCount + diskIOTests.FailedCount > 0);
				await UniTask.Delay(500);
				Accumulate(diskIOTests);
			}
			EvilSave.ActiveSlot = originalSlot;
			_ = totalPassed;
			_ = totalFailed;
			_ = totalFailed;
		}

		private void Accumulate(EvilSaveTestBase test)
		{
			totalPassed += test.PassedCount;
			totalFailed += test.FailedCount;
		}
	}
}
