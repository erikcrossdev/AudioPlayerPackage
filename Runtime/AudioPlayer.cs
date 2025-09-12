using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
	protected AudioSource audioSource;

	[SerializeField] protected AudioLibrary _audioLibrary;

	void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}
	public void PlaySound(SFXTypeDefinition typeDefinition)
	{
		AudioPlayerUtils.PlaySound(audioSource, _audioLibrary, typeDefinition);
	}
	public void PlaySoundWithDelay(SFXTypeDefinition soundData, float delay) {
		AudioPlayerUtils.PlaySoundWithDelay(audioSource, _audioLibrary, soundData, delay);
	}
	public void PlaySoundWithFade(SFXTypeDefinition typeDefinition, float fadeInDuration, float fadeOutDuration)
	{
		AudioPlayerUtils.PlaySoundWithFade(audioSource, _audioLibrary, typeDefinition, fadeInDuration, fadeOutDuration);
	}
	public void StopSFX()
	{
		AudioPlayerUtils.StopSFX(audioSource);
	}
}