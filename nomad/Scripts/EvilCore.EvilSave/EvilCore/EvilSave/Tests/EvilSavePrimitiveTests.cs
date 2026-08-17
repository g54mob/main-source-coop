using System;

namespace EvilCore.EvilSave.Tests
{
	public class EvilSavePrimitiveTests : EvilSaveTestBase
	{
		public override string CategoryName => "Primitives";

		public void RunAll()
		{
			ClearResults();
			EvilSave.Clear();
			TestBool();
			TestByte();
			TestSByte();
			TestShort();
			TestUShort();
			TestInt();
			TestUInt();
			TestLong();
			TestULong();
			TestFloat();
			TestDouble();
			TestDecimal();
			TestChar();
			TestString();
			TestStringEmpty();
			TestStringUnicode();
			TestDateTime();
			TestByteArray();
			TestByteArrayEmpty();
			TestDefaultValues();
			TestOverwrite();
			TestDeleteKey();
			TestHasKey();
			LogSummary("Primitives");
		}

		private void TestBool()
		{
			EvilSave.Save("test.bool.true", value: true);
			EvilSave.Save("test.bool.false", value: false);
			Assert("Bool (true)", expected: true, EvilSave.Load("test.bool.true", defaultValue: false));
			Assert("Bool (false)", expected: false, EvilSave.Load("test.bool.false", defaultValue: false));
		}

		private void TestByte()
		{
			EvilSave.Save("test.byte", (byte)255);
			Assert("Byte", (byte)255, EvilSave.Load("test.byte", (byte)0));
			EvilSave.Save("test.byte.zero", (byte)0);
			Assert("Byte (zero)", (byte)0, EvilSave.Load("test.byte.zero", (byte)0));
		}

		private void TestSByte()
		{
			EvilSave.Save("test.sbyte.pos", (sbyte)127);
			EvilSave.Save("test.sbyte.neg", (sbyte)(-128));
			Assert("SByte (+127)", (sbyte)127, EvilSave.Load("test.sbyte.pos", (sbyte)0));
			Assert("SByte (-128)", (sbyte)(-128), EvilSave.Load("test.sbyte.neg", (sbyte)0));
		}

		private void TestShort()
		{
			EvilSave.Save("test.short", (short)(-12345));
			Assert("Short", (short)(-12345), EvilSave.Load("test.short", (short)0));
		}

		private void TestUShort()
		{
			EvilSave.Save("test.ushort", (ushort)65535);
			Assert("UShort", (ushort)65535, EvilSave.Load("test.ushort", (ushort)0));
		}

		private void TestInt()
		{
			EvilSave.Save("test.int.pos", 42);
			EvilSave.Save("test.int.neg", -999999);
			EvilSave.Save("test.int.max", 2147483647);
			EvilSave.Save("test.int.min", -2147483648);
			Assert("Int (42)", 42, EvilSave.Load("test.int.pos", 0));
			Assert("Int (-999999)", -999999, EvilSave.Load("test.int.neg", 0));
			Assert("Int (MaxValue)", 2147483647, EvilSave.Load("test.int.max", 0));
			Assert("Int (MinValue)", -2147483648, EvilSave.Load("test.int.min", 0));
		}

		private void TestUInt()
		{
			EvilSave.Save("test.uint", 4294967295u);
			Assert("UInt", 4294967295u, EvilSave.Load("test.uint", 0u));
		}

		private void TestLong()
		{
			EvilSave.Save("test.long", 9223372036854775807L);
			Assert("Long (MaxValue)", 9223372036854775807L, EvilSave.Load("test.long", 0L));
		}

		private void TestULong()
		{
			EvilSave.Save("test.ulong", 18446744073709551615uL);
			Assert("ULong (MaxValue)", 18446744073709551615uL, EvilSave.Load("test.ulong", 0uL));
		}

		private void TestFloat()
		{
			EvilSave.Save("test.float", 3.14159f);
			EvilSave.Save("test.float.neg", -0.001f);
			AssertFloatEqual("Float (pi)", 3.14159f, EvilSave.Load("test.float", 0f));
			AssertFloatEqual("Float (-0.001)", -0.001f, EvilSave.Load("test.float.neg", 0f));
		}

		private void TestDouble()
		{
			EvilSave.Save("test.double", Math.PI);
			double num = EvilSave.Load("test.double", 0.0);
			AssertTrue("Double", Math.Abs(Math.PI - num) < 1E-08, $"Expected: 3.141592653589793 Got: {num}");
		}

		private void TestDecimal()
		{
			EvilSave.Save("test.decimal", 123456.789012m);
			Assert("Decimal", 123456.789012m, EvilSave.Load("test.decimal", 0m));
		}

		private void TestChar()
		{
			EvilSave.Save("test.char", 'Z');
			Assert("Char", 'Z', EvilSave.Load("test.char", '\0'));
		}

		private void TestString()
		{
			EvilSave.Save("test.string", "Hello EvilSave!");
			Assert("String", "Hello EvilSave!", EvilSave.Load<string>("test.string"));
		}

		private void TestStringEmpty()
		{
			EvilSave.Save("test.string.empty", "");
			Assert("String (empty)", "", EvilSave.Load<string>("test.string.empty"));
		}

		private void TestStringUnicode()
		{
			string text = "Merhaba Dunya! çşğüöı ☃ \ud83d\ude80";
			EvilSave.Save("test.string.unicode", text);
			Assert("String (unicode)", text, EvilSave.Load<string>("test.string.unicode"));
		}

		private void TestDateTime()
		{
			DateTime dateTime = new DateTime(2026, 2, 18, 14, 30, 0, DateTimeKind.Utc);
			EvilSave.Save("test.datetime", dateTime);
			Assert("DateTime", dateTime, EvilSave.Load<DateTime>("test.datetime"));
		}

		private void TestByteArray()
		{
			byte[] array = new byte[5] { 0, 1, 127, 128, 255 };
			EvilSave.Save("test.bytes", array);
			byte[] array2 = EvilSave.Load<byte[]>("test.bytes");
			AssertTrue("byte[] length", array2 != null && array2.Length == 5);
			if (array2 != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					Assert($"byte[{i}]", array[i], array2[i]);
				}
			}
		}

		private void TestByteArrayEmpty()
		{
			byte[] value = Array.Empty<byte>();
			EvilSave.Save("test.bytes.empty", value);
			byte[] array = EvilSave.Load<byte[]>("test.bytes.empty");
			AssertTrue("byte[] (empty)", array != null && array.Length == 0);
		}

		private void TestDefaultValues()
		{
			Assert("Default int", 42, EvilSave.Load("nonexistent.int", 42));
			Assert("Default string", "fallback", EvilSave.Load("nonexistent.string", "fallback"));
			Assert("Default bool", expected: true, EvilSave.Load("nonexistent.bool", defaultValue: true));
			AssertFloatEqual("Default float", 1.5f, EvilSave.Load("nonexistent.float", 1.5f));
		}

		private void TestOverwrite()
		{
			EvilSave.Save("test.overwrite", 100);
			EvilSave.Save("test.overwrite", 200);
			Assert("Overwrite", 200, EvilSave.Load("test.overwrite", 0));
		}

		private void TestDeleteKey()
		{
			EvilSave.Save("test.delete", 999);
			AssertTrue("DeleteKey - exists before", EvilSave.HasKey("test.delete"));
			EvilSave.DeleteKey("test.delete");
			AssertTrue("DeleteKey - gone after", !EvilSave.HasKey("test.delete"));
			Assert("DeleteKey - returns default", 0, EvilSave.Load("test.delete", 0));
		}

		private void TestHasKey()
		{
			EvilSave.Save("test.haskey", 1);
			AssertTrue("HasKey (existing)", EvilSave.HasKey("test.haskey"));
			AssertTrue("HasKey (missing)", !EvilSave.HasKey("test.haskey.nope"));
		}
	}
}
