using UnityEngine;

namespace EvilCore.EvilSave.Tests
{
	public class EvilSaveUnityTypeTests : EvilSaveTestBase
	{
		public override string CategoryName => "Unity Types";

		public void RunAll()
		{
			ClearResults();
			EvilSave.Clear();
			TestVector2();
			TestVector3();
			TestVector4();
			TestVector2Int();
			TestVector3Int();
			TestQuaternion();
			TestColor();
			TestColor32();
			TestRect();
			TestRectInt();
			TestBounds();
			TestBoundsInt();
			TestMatrix4x4();
			TestLayerMask();
			TestAnimationCurve();
			TestGradient();
			LogSummary("Unity Types");
		}

		private void TestVector2()
		{
			Vector2 value = new Vector2(1.5f, -2.7f);
			EvilSave.Save("test.v2", value);
			Vector2 vector = EvilSave.Load<Vector2>("test.v2");
			AssertFloatEqual("Vector2.x", value.x, vector.x);
			AssertFloatEqual("Vector2.y", value.y, vector.y);
		}

		private void TestVector3()
		{
			Vector3 value = new Vector3(10.1f, -20.2f, 30.3f);
			EvilSave.Save("test.v3", value);
			Vector3 vector = EvilSave.Load<Vector3>("test.v3");
			AssertFloatEqual("Vector3.x", value.x, vector.x);
			AssertFloatEqual("Vector3.y", value.y, vector.y);
			AssertFloatEqual("Vector3.z", value.z, vector.z);
		}

		private void TestVector4()
		{
			Vector4 value = new Vector4(1f, 2f, 3f, 4f);
			EvilSave.Save("test.v4", value);
			Vector4 vector = EvilSave.Load<Vector4>("test.v4");
			AssertFloatEqual("Vector4.x", value.x, vector.x);
			AssertFloatEqual("Vector4.y", value.y, vector.y);
			AssertFloatEqual("Vector4.z", value.z, vector.z);
			AssertFloatEqual("Vector4.w", value.w, vector.w);
		}

		private void TestVector2Int()
		{
			Vector2Int value = new Vector2Int(-5, 10);
			EvilSave.Save("test.v2i", value);
			Vector2Int vector2Int = EvilSave.Load<Vector2Int>("test.v2i");
			Assert("Vector2Int.x", value.x, vector2Int.x);
			Assert("Vector2Int.y", value.y, vector2Int.y);
		}

		private void TestVector3Int()
		{
			Vector3Int value = new Vector3Int(100, -200, 300);
			EvilSave.Save("test.v3i", value);
			Vector3Int vector3Int = EvilSave.Load<Vector3Int>("test.v3i");
			Assert("Vector3Int.x", value.x, vector3Int.x);
			Assert("Vector3Int.y", value.y, vector3Int.y);
			Assert("Vector3Int.z", value.z, vector3Int.z);
		}

		private void TestQuaternion()
		{
			Quaternion value = Quaternion.Euler(45f, 90f, 180f);
			EvilSave.Save("test.quat", value);
			Quaternion quaternion = EvilSave.Load<Quaternion>("test.quat");
			AssertFloatEqual("Quaternion.x", value.x, quaternion.x);
			AssertFloatEqual("Quaternion.y", value.y, quaternion.y);
			AssertFloatEqual("Quaternion.z", value.z, quaternion.z);
			AssertFloatEqual("Quaternion.w", value.w, quaternion.w);
		}

		private void TestColor()
		{
			Color value = new Color(0.1f, 0.5f, 0.9f, 0.75f);
			EvilSave.Save("test.color", value);
			Color color = EvilSave.Load<Color>("test.color");
			AssertFloatEqual("Color.r", value.r, color.r);
			AssertFloatEqual("Color.g", value.g, color.g);
			AssertFloatEqual("Color.b", value.b, color.b);
			AssertFloatEqual("Color.a", value.a, color.a);
		}

		private void TestColor32()
		{
			Color32 value = new Color32(255, 128, 0, 200);
			EvilSave.Save("test.color32", value);
			Color32 color = EvilSave.Load<Color32>("test.color32");
			Assert("Color32.r", value.r, color.r);
			Assert("Color32.g", value.g, color.g);
			Assert("Color32.b", value.b, color.b);
			Assert("Color32.a", value.a, color.a);
		}

		private void TestRect()
		{
			Rect value = new Rect(10f, 20f, 300f, 400f);
			EvilSave.Save("test.rect", value);
			Rect rect = EvilSave.Load<Rect>("test.rect");
			AssertFloatEqual("Rect.x", value.x, rect.x);
			AssertFloatEqual("Rect.y", value.y, rect.y);
			AssertFloatEqual("Rect.width", value.width, rect.width);
			AssertFloatEqual("Rect.height", value.height, rect.height);
		}

		private void TestRectInt()
		{
			RectInt value = new RectInt(5, 10, 100, 200);
			EvilSave.Save("test.rectint", value);
			RectInt rectInt = EvilSave.Load<RectInt>("test.rectint");
			Assert("RectInt.x", value.x, rectInt.x);
			Assert("RectInt.y", value.y, rectInt.y);
			Assert("RectInt.width", value.width, rectInt.width);
			Assert("RectInt.height", value.height, rectInt.height);
		}

		private void TestBounds()
		{
			Bounds value = new Bounds(new Vector3(1f, 2f, 3f), new Vector3(10f, 20f, 30f));
			EvilSave.Save("test.bounds", value);
			Bounds bounds = EvilSave.Load<Bounds>("test.bounds");
			AssertFloatEqual("Bounds.center.x", value.center.x, bounds.center.x);
			AssertFloatEqual("Bounds.center.y", value.center.y, bounds.center.y);
			AssertFloatEqual("Bounds.center.z", value.center.z, bounds.center.z);
			AssertFloatEqual("Bounds.size.x", value.size.x, bounds.size.x);
			AssertFloatEqual("Bounds.size.y", value.size.y, bounds.size.y);
			AssertFloatEqual("Bounds.size.z", value.size.z, bounds.size.z);
		}

		private void TestBoundsInt()
		{
			BoundsInt value = new BoundsInt(new Vector3Int(1, 2, 3), new Vector3Int(10, 20, 30));
			EvilSave.Save("test.boundsint", value);
			BoundsInt boundsInt = EvilSave.Load<BoundsInt>("test.boundsint");
			Assert("BoundsInt.pos.x", value.position.x, boundsInt.position.x);
			Assert("BoundsInt.pos.y", value.position.y, boundsInt.position.y);
			Assert("BoundsInt.pos.z", value.position.z, boundsInt.position.z);
			Assert("BoundsInt.size.x", value.size.x, boundsInt.size.x);
			Assert("BoundsInt.size.y", value.size.y, boundsInt.size.y);
			Assert("BoundsInt.size.z", value.size.z, boundsInt.size.z);
		}

		private void TestMatrix4x4()
		{
			Matrix4x4 value = Matrix4x4.TRS(new Vector3(1f, 2f, 3f), Quaternion.Euler(45f, 90f, 0f), new Vector3(2f, 2f, 2f));
			EvilSave.Save("test.matrix", value);
			Matrix4x4 matrix4x = EvilSave.Load<Matrix4x4>("test.matrix");
			bool condition = true;
			for (int i = 0; i < 16; i++)
			{
				if (Mathf.Abs(value[i] - matrix4x[i]) > 0.0001f)
				{
					condition = false;
					break;
				}
			}
			AssertTrue("Matrix4x4 (16 elements)", condition);
		}

		private void TestLayerMask()
		{
			LayerMask value = 4232;
			EvilSave.Save("test.layermask", value);
			LayerMask layerMask = EvilSave.Load<LayerMask>("test.layermask");
			Assert("LayerMask", value.value, layerMask.value);
		}

		private void TestAnimationCurve()
		{
			AnimationCurve animationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(0.5f, 1f, 0f, 0f), new Keyframe(1f, 0f, -1f, 0f));
			animationCurve.preWrapMode = WrapMode.Loop;
			animationCurve.postWrapMode = WrapMode.PingPong;
			EvilSave.Save("test.curve", animationCurve);
			AnimationCurve animationCurve2 = EvilSave.Load<AnimationCurve>("test.curve");
			AssertTrue("AnimationCurve not null", animationCurve2 != null);
			if (animationCurve2 != null)
			{
				Assert("AnimationCurve.keys.Length", animationCurve.keys.Length, animationCurve2.keys.Length);
				Assert("AnimationCurve.preWrapMode", animationCurve.preWrapMode, animationCurve2.preWrapMode);
				Assert("AnimationCurve.postWrapMode", animationCurve.postWrapMode, animationCurve2.postWrapMode);
				for (int i = 0; i < animationCurve.keys.Length && i < animationCurve2.keys.Length; i++)
				{
					AssertFloatEqual($"AnimationCurve.key[{i}].time", animationCurve.keys[i].time, animationCurve2.keys[i].time);
					AssertFloatEqual($"AnimationCurve.key[{i}].value", animationCurve.keys[i].value, animationCurve2.keys[i].value);
				}
			}
		}

		private void TestGradient()
		{
			Gradient gradient = new Gradient();
			gradient.SetKeys(new GradientColorKey[3]
			{
				new GradientColorKey(Color.red, 0f),
				new GradientColorKey(Color.blue, 0.5f),
				new GradientColorKey(Color.green, 1f)
			}, new GradientAlphaKey[2]
			{
				new GradientAlphaKey(1f, 0f),
				new GradientAlphaKey(0.5f, 1f)
			});
			gradient.mode = GradientMode.Blend;
			EvilSave.Save("test.gradient", gradient);
			Gradient gradient2 = EvilSave.Load<Gradient>("test.gradient");
			AssertTrue("Gradient not null", gradient2 != null);
			if (gradient2 != null)
			{
				Assert("Gradient.colorKeys.Length", gradient.colorKeys.Length, gradient2.colorKeys.Length);
				Assert("Gradient.alphaKeys.Length", gradient.alphaKeys.Length, gradient2.alphaKeys.Length);
				Assert("Gradient.mode", gradient.mode, gradient2.mode);
				for (int i = 0; i < gradient.colorKeys.Length && i < gradient2.colorKeys.Length; i++)
				{
					AssertFloatEqual($"Gradient.colorKey[{i}].time", gradient.colorKeys[i].time, gradient2.colorKeys[i].time);
				}
			}
		}
	}
}
