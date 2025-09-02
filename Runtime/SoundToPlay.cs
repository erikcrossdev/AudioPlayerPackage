using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class SoundToPlay
{
	[SerializeField] private SFXTypeDefinition typeDefinition;
	[SerializeField] private bool shouldRandomizePitch = false;
	[SerializeField] private bool shouldRandomizeClip = false;
	[SerializeField] private bool shouldRandomizeVolume = false;
	[SerializeField] private bool shouldLoop = false;
	[SerializeField] private bool shouldUseAudioMixer = false;
	[SerializeField] private bool shouldOverrideSourceSettings = false;

	[SerializeField] private AudioClip _clip;
	[SerializeField] private List<AudioClip> _clips;
	[SerializeField] private float _minPitch = 1f;
	[SerializeField] private float _maxPitch = 1f;

	[SerializeField] private float _minVolume = 1f;
	[SerializeField] private float _maxVolume = 1f;

	[SerializeField] private float _defaultVolume = 1f;

	[SerializeField] private AudioMixerGroup _mixer;
	public AudioMixerGroup Mixer => _mixer;

	[SerializeField] private int _priority = 128;
	public int Priority => _priority;

	[SerializeField] private float _stereoPan = 1.0f;
	public float StereoPan => _stereoPan;

	[SerializeField] private float _spatialBlend = 1.0f;
	public float SpatialBlend => _spatialBlend;

	[SerializeField] private float _reverbZoneMix = 0.0f;
	public float ReverbZoneMix => _reverbZoneMix;

	[SerializeField] private float _dopplerLevel = 1.0f;
	public float DopplerLevel => _dopplerLevel;

	[SerializeField] private float _spread = 0.0f;
	public float Spread => _spread;

	[SerializeField] private int _minDistance = 1;
	public int MinDistance => _minDistance;

	[SerializeField] private int _maxDistance = 500;
	public int MaxDistance => _maxDistance;

	[SerializeField] private AudioRolloffMode _volumeRolloff;
	public AudioRolloffMode VolumeRolloff => _volumeRolloff;


	public SFXTypeDefinition TypeDefinition => typeDefinition;
	public bool ShouldRandomizePitch => shouldRandomizePitch;
	public bool ShouldRandomizeClip => shouldRandomizeClip;
	public bool ShouldRandomizeVolume => shouldRandomizeVolume;
	public bool ShouldLoop => shouldLoop;
	public bool ShouldUseAudioMixer => shouldUseAudioMixer;

	public bool ShouldOverrideSourceSettings => shouldOverrideSourceSettings;

	public AudioClip GetClip()
	{
		if (shouldRandomizeClip && _clips != null && _clips.Count > 0)
		{
			return _clips[UnityEngine.Random.Range(0, _clips.Count)];
		}
		return _clip;
	}

	public float GetPitch()
	{
		return shouldRandomizePitch ? UnityEngine.Random.Range(_minPitch, _maxPitch) : 1f;
	}

	public float GetVolume()
	{
		return shouldRandomizeVolume ? UnityEngine.Random.Range(_minVolume, _maxVolume) : _defaultVolume;
	}
}