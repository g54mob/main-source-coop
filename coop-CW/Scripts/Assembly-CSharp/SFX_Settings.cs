using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

[Serializable]
public class SFX_Settings
{
	public bool spatialize = true;

	public bool nonSpatializedForLocalPlayer;

	public bool occlusion = true;

	[FormerlySerializedAs("reflection")]
	public bool reflections = true;

	public bool transmission;

	[Range(0f, 1f)]
	public float volume = 0.5f;

	[Range(0f, 1f)]
	[Tooltip("0.2 variation means random between 80% of specified volume and 100% of specified volume")]
	public float volume_Variation = 0.2f;

	public float pitch = 1f;

	[Range(0f, 1f)]
	[Tooltip("0.1 variation means random between 95% of specified volume and 105% of specified volume")]
	public float pitch_Variation = 0.1f;

	[Range(0f, 1f)]
	public float spatialBlend = 1f;

	[Range(0f, 1f)]
	public float obstructability = 0.7f;

	[Range(0f, 1f)]
	public float dopplerLevel = 1f;

	public float range = 150f;

	public float minRange = 1f;

	public int noiseDistance;

	public float cooldown = 0.02f;

	public int maxInstances = 5;

	public AudioMixerGroup mixerGroup;
}
