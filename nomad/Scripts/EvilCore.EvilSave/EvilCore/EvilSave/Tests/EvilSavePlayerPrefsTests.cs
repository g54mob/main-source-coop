using System.Linq;

namespace EvilCore.EvilSave.Tests
{
	public class EvilSavePlayerPrefsTests : EvilSaveTestBase
	{
		public override string CategoryName => "PlayerPrefs";

		public void RunAll()
		{
			ClearResults();
			TestSetGetString();
			TestSetGetInt();
			TestSetGetFloat();
			TestSetGetBool();
			TestSetGetObject();
			TestHasKey();
			TestDeleteKey();
			TestDefaultValues();
			TestGetAllKeys();
			TestDeleteAll();
			TestStaticFacade();
			LogSummary("PlayerPrefs");
		}

		private void TestSetGetString()
		{
			PlayerPrefsBackend.SetString("test.str", "Hello");
			Assert("Prefs: SetGet string", "Hello", PlayerPrefsBackend.GetString("test.str"));
			PlayerPrefsBackend.DeleteKey("test.str");
		}

		private void TestSetGetInt()
		{
			PlayerPrefsBackend.SetInt("test.int", 42);
			Assert("Prefs: SetGet int", 42, PlayerPrefsBackend.GetInt("test.int"));
			PlayerPrefsBackend.SetInt("test.int.neg", -999);
			Assert("Prefs: SetGet int (neg)", -999, PlayerPrefsBackend.GetInt("test.int.neg"));
			PlayerPrefsBackend.DeleteKey("test.int");
			PlayerPrefsBackend.DeleteKey("test.int.neg");
		}

		private void TestSetGetFloat()
		{
			PlayerPrefsBackend.SetFloat("test.float", 3.14f);
			AssertFloatEqual("Prefs: SetGet float", 3.14f, PlayerPrefsBackend.GetFloat("test.float"));
			PlayerPrefsBackend.DeleteKey("test.float");
		}

		private void TestSetGetBool()
		{
			PlayerPrefsBackend.SetBool("test.bool.t", value: true);
			PlayerPrefsBackend.SetBool("test.bool.f", value: false);
			Assert("Prefs: SetGet bool (true)", expected: true, PlayerPrefsBackend.GetBool("test.bool.t"));
			Assert("Prefs: SetGet bool (false)", expected: false, PlayerPrefsBackend.GetBool("test.bool.f"));
			PlayerPrefsBackend.DeleteKey("test.bool.t");
			PlayerPrefsBackend.DeleteKey("test.bool.f");
		}

		private void TestSetGetObject()
		{
			PrefsTestObject value = new PrefsTestObject
			{
				name = "TestObj",
				value = 123
			};
			PlayerPrefsBackend.SetObject("test.obj", value);
			PrefsTestObject prefsTestObject = PlayerPrefsBackend.GetObject<PrefsTestObject>("test.obj");
			AssertTrue("Prefs: Object not null", prefsTestObject != null);
			if (prefsTestObject != null)
			{
				Assert("Prefs: Object.name", "TestObj", prefsTestObject.name);
				Assert("Prefs: Object.value", 123, prefsTestObject.value);
			}
			PlayerPrefsBackend.DeleteKey("test.obj");
		}

		private void TestHasKey()
		{
			PlayerPrefsBackend.SetInt("test.exists", 1);
			AssertTrue("Prefs: HasKey (existing)", PlayerPrefsBackend.HasKey("test.exists"));
			AssertTrue("Prefs: HasKey (missing)", !PlayerPrefsBackend.HasKey("test.nope_not_here"));
			PlayerPrefsBackend.DeleteKey("test.exists");
		}

		private void TestDeleteKey()
		{
			PlayerPrefsBackend.SetInt("test.del", 999);
			AssertTrue("Prefs: Delete before", PlayerPrefsBackend.HasKey("test.del"));
			PlayerPrefsBackend.DeleteKey("test.del");
			AssertTrue("Prefs: Delete after", !PlayerPrefsBackend.HasKey("test.del"));
		}

		private void TestDefaultValues()
		{
			Assert("Prefs: Default string", "fallback", PlayerPrefsBackend.GetString("no_key_str", "fallback"));
			Assert("Prefs: Default int", 77, PlayerPrefsBackend.GetInt("no_key_int", 77));
			AssertFloatEqual("Prefs: Default float", 1.5f, PlayerPrefsBackend.GetFloat("no_key_float", 1.5f));
			Assert("Prefs: Default bool", expected: true, PlayerPrefsBackend.GetBool("no_key_bool", defaultValue: true));
		}

		private void TestGetAllKeys()
		{
			PlayerPrefsBackend.SetInt("test.keys.a", 1);
			PlayerPrefsBackend.SetInt("test.keys.b", 2);
			PlayerPrefsBackend.SetInt("test.keys.c", 3);
			PlayerPrefsBackend.Save();
			string[] allKeys = PlayerPrefsBackend.GetAllKeys();
			AssertTrue("Prefs: GetAllKeys contains A", allKeys.Contains("test.keys.a"));
			AssertTrue("Prefs: GetAllKeys contains B", allKeys.Contains("test.keys.b"));
			AssertTrue("Prefs: GetAllKeys contains C", allKeys.Contains("test.keys.c"));
			PlayerPrefsBackend.DeleteKey("test.keys.a");
			PlayerPrefsBackend.DeleteKey("test.keys.b");
			PlayerPrefsBackend.DeleteKey("test.keys.c");
		}

		private void TestDeleteAll()
		{
			PlayerPrefsBackend.SetInt("test.da.x", 1);
			PlayerPrefsBackend.SetInt("test.da.y", 2);
			PlayerPrefsBackend.Save();
			PlayerPrefsBackend.DeleteAll();
			AssertTrue("Prefs: DeleteAll - x gone", !PlayerPrefsBackend.HasKey("test.da.x"));
			AssertTrue("Prefs: DeleteAll - y gone", !PlayerPrefsBackend.HasKey("test.da.y"));
		}

		private void TestStaticFacade()
		{
			EvilSave.Prefs.SetInt("facade.int", 55);
			Assert("Facade: SetGet int", 55, EvilSave.Prefs.GetInt("facade.int"));
			EvilSave.Prefs.SetString("facade.str", "test");
			Assert("Facade: SetGet string", "test", EvilSave.Prefs.GetString("facade.str"));
			EvilSave.Prefs.SetBool("facade.bool", value: true);
			Assert("Facade: SetGet bool", expected: true, EvilSave.Prefs.GetBool("facade.bool"));
			EvilSave.Prefs.SetFloat("facade.float", 2.5f);
			AssertFloatEqual("Facade: SetGet float", 2.5f, EvilSave.Prefs.GetFloat("facade.float"));
			AssertTrue("Facade: HasKey", EvilSave.Prefs.HasKey("facade.int"));
			EvilSave.Prefs.DeleteKey("facade.int");
			AssertTrue("Facade: DeleteKey", !EvilSave.Prefs.HasKey("facade.int"));
			EvilSave.Prefs.DeleteKey("facade.str");
			EvilSave.Prefs.DeleteKey("facade.bool");
			EvilSave.Prefs.DeleteKey("facade.float");
		}
	}
}
