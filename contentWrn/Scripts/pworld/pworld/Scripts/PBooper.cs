using System;
using System.Collections;
using UnityEngine;
using pworld.Scripts.PPhys;

namespace pworld.Scripts
{
	public class PBooper : MonoBehaviour
	{
		private class Booping
		{
			public PPhysSpringBase booper;

			public GameObject go;
		}

		public static PBooper me;

		public float spring = 15f;

		public float damp = 40f;

		private void Awake()
		{
			me = this;
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		private void OnDestroy()
		{
		}

		public void ScaleBoopSmall(GameObject go, float force = 5f)
		{
			Booping booping = new Booping();
			booping.booper = go.GetComponent<PPhysScale>();
			if (!booping.booper)
			{
				booping.booper = go.AddComponent<PPhysScale>();
			}
			booping.booper.damp = spring;
			booping.booper.spring = damp;
			booping.go = go;
			booping.booper.Velocity += Vector3.one * force;
			StartCoroutine(RemoveWhenStill(booping));
		}

		public void PositionBoopSmall(GameObject go, Vector3 force)
		{
			Booping booping = new Booping();
			booping.booper = go.GetComponent<PPhysPositional>();
			if (!booping.booper)
			{
				booping.booper = go.AddComponent<PPhysPositional>();
			}
			booping.booper.damp = spring;
			booping.booper.spring = damp;
			booping.go = go;
			booping.booper.Velocity += force;
			StartCoroutine(RemoveWhenStill(booping));
		}

		private void PutItOn<T>(GameObject go, Vector3 force, float spring, float damp) where T : PPhysSpringBase
		{
			Booping booping = new Booping();
			booping.booper = go.GetComponent<T>();
			if (!booping.booper)
			{
				booping.booper = go.AddComponent<T>();
			}
			booping.booper.damp = damp;
			booping.booper.spring = spring;
			booping.go = go;
			booping.booper.Velocity += force;
			StartCoroutine(RemoveWhenStill(booping));
		}

		private IEnumerator RemoveWhenStill(Booping go)
		{
			Vector3 lastVel;
			do
			{
				lastVel = go.booper.Velocity;
				yield return null;
			}
			while ((double)Math.Abs((lastVel - go.booper.Velocity).magnitude) > 1E-05);
			UnityEngine.Object.Destroy(go.booper);
		}
	}
}
