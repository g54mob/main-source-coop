using System.Collections.Generic;
using FishNet.Managing;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FishNet.Component.Prediction
{
	public class RigidbodyPauser
	{
		private struct RigidbodyData
		{
			public Rigidbody Rigidbody;

			public Vector3 Velocity;

			public Vector3 AngularVelocity;

			public Scene SimulatedScene;

			public bool IsKinematic;

			public Transform Parent;

			public bool HasParent;

			public RigidbodyData(Rigidbody rb)
			{
				Rigidbody = rb;
				Rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
				Velocity = Vector3.zero;
				AngularVelocity = Vector3.zero;
				SimulatedScene = rb.gameObject.scene;
				IsKinematic = rb.isKinematic;
				Parent = rb.transform.parent;
				HasParent = Parent != null;
			}

			public void Update(Rigidbody rb)
			{
				Velocity = rb.velocity;
				AngularVelocity = rb.angularVelocity;
				SimulatedScene = rb.gameObject.scene;
				IsKinematic = rb.isKinematic;
				Parent = rb.transform.parent;
				HasParent = Parent != null;
			}
		}

		private struct Rigidbody2DData
		{
			public Rigidbody2D Rigidbody2d;

			public Vector2 Velocity;

			public float AngularVelocity;

			public Scene SimulatedScene;

			public bool Simulated;

			public bool IsKinematic;

			public Transform Parent;

			public bool HasParent;

			public Rigidbody2DData(Rigidbody2D rb)
			{
				Rigidbody2d = rb;
				Rigidbody2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
				Velocity = Vector2.zero;
				AngularVelocity = 0f;
				SimulatedScene = rb.gameObject.scene;
				Simulated = rb.simulated;
				IsKinematic = rb.isKinematic;
				Parent = rb.transform.parent;
				HasParent = Parent != null;
			}

			public void Update(Rigidbody2D rb)
			{
				Velocity = rb.velocity;
				AngularVelocity = rb.angularVelocity;
				SimulatedScene = rb.gameObject.scene;
				Simulated = rb.simulated;
				IsKinematic = rb.isKinematic;
				Parent = rb.transform.parent;
				HasParent = Parent != null;
			}
		}

		private List<RigidbodyData> _rigidbodyDatas = new List<RigidbodyData>();

		private List<Rigidbody2DData> _rigidbody2dDatas = new List<Rigidbody2DData>();

		private static Scene _kinematicSceneCache;

		private Transform _graphicalParent;

		private Transform _graphicalObject;

		private bool _getInChildren;

		private Transform _transform;

		private RigidbodyType _rigidbodyType;

		private bool _initialized;

		public bool Paused { get; private set; }

		private static Scene _kinematicScene
		{
			get
			{
				if (!_kinematicSceneCache.IsValid())
				{
					_kinematicSceneCache = SceneManager.CreateScene("RigidbodyPauser_Kinematic", new CreateSceneParameters(LocalPhysicsMode.Physics2D | LocalPhysicsMode.Physics3D));
				}
				return _kinematicSceneCache;
			}
		}

		public void UpdateRigidbodies()
		{
			if (!_initialized)
			{
				InstanceFinder.NetworkManager.LogError("T" + GetType().Name + " has not been initialized yet. This method cannot be used.");
			}
			else
			{
				UpdateRigidbodies(_transform, _rigidbodyType, _getInChildren, _graphicalObject);
			}
		}

		public void UpdateRigidbodies(Transform t, RigidbodyType rbType, bool getInChildren, Transform graphicalObject)
		{
			_rigidbodyType = rbType;
			_getInChildren = getInChildren;
			_rigidbodyDatas.Clear();
			_rigidbody2dDatas.Clear();
			if (rbType == RigidbodyType.Rigidbody)
			{
				if (getInChildren)
				{
					Rigidbody[] componentsInChildren = t.GetComponentsInChildren<Rigidbody>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						_rigidbodyDatas.Add(new RigidbodyData(componentsInChildren[i]));
					}
				}
				else
				{
					Rigidbody component = t.GetComponent<Rigidbody>();
					if (component != null)
					{
						_rigidbodyDatas.Add(new RigidbodyData(component));
					}
				}
				for (int j = 0; j < _rigidbodyDatas.Count; j++)
				{
					if (_rigidbodyDatas[j].Rigidbody.transform == graphicalObject)
					{
						NetworkManager.StaticLogError("GameObject " + t.name + " has it's GraphicalObject as a child or on the same object as a Rigidbody object. The GraphicalObject must be a child of root, and not sit beneath or on any rigidbodies.");
						graphicalObject = null;
					}
				}
			}
			else
			{
				if (getInChildren)
				{
					Rigidbody2D[] componentsInChildren2 = t.GetComponentsInChildren<Rigidbody2D>();
					for (int k = 0; k < componentsInChildren2.Length; k++)
					{
						_rigidbody2dDatas.Add(new Rigidbody2DData(componentsInChildren2[k]));
					}
				}
				else
				{
					Rigidbody2D component2 = t.GetComponent<Rigidbody2D>();
					if (component2 != null)
					{
						_rigidbody2dDatas.Add(new Rigidbody2DData(component2));
					}
				}
				for (int l = 0; l < _rigidbody2dDatas.Count; l++)
				{
					if (_rigidbody2dDatas[l].Rigidbody2d.transform == graphicalObject)
					{
						NetworkManager.StaticLogError("GameObject " + t.name + " has it's GraphicalObject as a child or on the same object as a Rigidbody object. The GraphicalObject must be a child of root, and not sit beneath or on any rigidbodies.");
						graphicalObject = null;
					}
				}
			}
			if (graphicalObject != null)
			{
				_graphicalObject = graphicalObject;
				_graphicalParent = graphicalObject.parent;
			}
			_initialized = true;
		}

		public void Pause()
		{
			if (Paused)
			{
				return;
			}
			Paused = true;
			_graphicalObject?.SetParent(null);
			Scene kinematicScene = _kinematicScene;
			if (_rigidbodyType == RigidbodyType.Rigidbody)
			{
				for (int i = 0; i < _rigidbodyDatas.Count; i++)
				{
					if (!PauseRigidbody(i))
					{
						_rigidbodyDatas.RemoveAt(i);
						i--;
					}
				}
				return;
			}
			for (int j = 0; j < _rigidbody2dDatas.Count; j++)
			{
				if (!PauseRigidbody2(j))
				{
					_rigidbody2dDatas.RemoveAt(j);
					j--;
				}
			}
			bool PauseRigidbody(int index)
			{
				RigidbodyData value = _rigidbodyDatas[index];
				Rigidbody rigidbody = value.Rigidbody;
				if (rigidbody == null)
				{
					return false;
				}
				value.Update(rigidbody);
				_rigidbodyDatas[index] = value;
				SceneManager.MoveGameObjectToScene(rigidbody.transform.gameObject, kinematicScene);
				return true;
			}
			bool PauseRigidbody2(int index)
			{
				Rigidbody2DData value = _rigidbody2dDatas[index];
				Rigidbody2D rigidbody2d = value.Rigidbody2d;
				if (rigidbody2d == null)
				{
					return false;
				}
				value.Update(rigidbody2d);
				_rigidbody2dDatas[index] = value;
				SceneManager.MoveGameObjectToScene(rigidbody2d.transform.gameObject, kinematicScene);
				return true;
			}
		}

		public void Unpause()
		{
			if (!Paused)
			{
				return;
			}
			Paused = false;
			if (_rigidbodyType == RigidbodyType.Rigidbody)
			{
				for (int i = 0; i < _rigidbodyDatas.Count; i++)
				{
					if (!UnpauseRigidbody(i))
					{
						_rigidbodyDatas.RemoveAt(i);
						i--;
					}
				}
			}
			else
			{
				for (int j = 0; j < _rigidbody2dDatas.Count; j++)
				{
					if (!UnpauseRigidbody2(j))
					{
						_rigidbody2dDatas.RemoveAt(j);
						j--;
					}
				}
			}
			if (_graphicalParent == null && _graphicalObject != null)
			{
				UnityEngine.Object.Destroy(_graphicalObject.gameObject);
			}
			else
			{
				_graphicalObject?.SetParent(_graphicalParent);
			}
			bool UnpauseRigidbody(int index)
			{
				RigidbodyData rigidbodyData = _rigidbodyDatas[index];
				Rigidbody rigidbody = rigidbodyData.Rigidbody;
				if (rigidbody == null)
				{
					return false;
				}
				SceneManager.MoveGameObjectToScene(rigidbody.transform.gameObject, rigidbodyData.SimulatedScene);
				rigidbody.velocity = rigidbodyData.Velocity;
				rigidbody.angularVelocity = rigidbodyData.AngularVelocity;
				rigidbody.isKinematic = rigidbodyData.IsKinematic;
				return true;
			}
			bool UnpauseRigidbody2(int index)
			{
				Rigidbody2DData rigidbody2DData = _rigidbody2dDatas[index];
				Rigidbody2D rigidbody2d = rigidbody2DData.Rigidbody2d;
				if (rigidbody2d == null)
				{
					return false;
				}
				SceneManager.MoveGameObjectToScene(rigidbody2d.transform.gameObject, rigidbody2DData.SimulatedScene);
				rigidbody2d.velocity = rigidbody2DData.Velocity;
				rigidbody2d.angularVelocity = rigidbody2DData.AngularVelocity;
				rigidbody2d.simulated = rigidbody2DData.Simulated;
				rigidbody2d.isKinematic = rigidbody2DData.IsKinematic;
				return true;
			}
		}
	}
}
