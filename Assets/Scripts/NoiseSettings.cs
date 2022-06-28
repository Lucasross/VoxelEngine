using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "noiseSettings", menuName = "Data/Noise Settings")]
public class NoiseSettings : ScriptableObject
{
	public float noiseZoom;
	public int octaves;
	public Vector2Int offset;
	public Vector2Int worldOffset;
	public float persistance;
	public float redistributionModifier;
	public float exponent;
}