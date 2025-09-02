using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioLibrary", menuName = "Scriptable Objects/Audio/Audio Library")]
public class AudioLibrary : ScriptableObject
{
	[SerializeField] private List<SoundToPlay> soundsToPlay = new List<SoundToPlay>();
	public List<SoundToPlay> SoundsToPlay => soundsToPlay;
}
