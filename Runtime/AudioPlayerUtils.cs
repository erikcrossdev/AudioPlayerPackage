using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public static class AudioPlayerUtils 
{
	private static SoundToPlay GetSFX(AudioLibrary audioLibrary, SFXTypeDefinition typeDefinition)
	{
		SoundToPlay sfx = audioLibrary.SoundsToPlay.Find(x => x.TypeDefinition == typeDefinition);
		return sfx;
	}

	public static void OverrideAudioSource(AudioSource audioSource, AudioLibrary audioLibrary, SFXTypeDefinition typeDefinition) {
		SoundToPlay sfx = GetSFX(audioLibrary, typeDefinition);
		if (sfx != null || !sfx.ShouldOverrideSourceSettings)
		{
			return;
		}
		audioSource.priority = sfx.Priority;
		audioSource.spread = sfx.Spread;
		audioSource.rolloffMode = sfx.VolumeRolloff;
		audioSource.reverbZoneMix = sfx.ReverbZoneMix;
		audioSource.dopplerLevel = sfx.DopplerLevel;
		audioSource.spatialBlend = sfx.SpatialBlend;
		audioSource.minDistance = sfx.MinDistance;
		audioSource.maxDistance = sfx.MaxDistance;
	}

	public static void PlaySound(AudioSource audioSource, AudioLibrary audioLibrary, SFXTypeDefinition typeDefinition)
	{
		float defaultPitch = audioSource.pitch;
		if (audioLibrary == null)
		{
			Debug.LogError("AudioLibrary not set");
			return;
		}

		SoundToPlay sfx = GetSFX(audioLibrary, typeDefinition);
		if (sfx != null)
		{
			audioSource.pitch = (sfx.ShouldRandomizePitch) ? sfx.GetPitch() : defaultPitch;

			audioSource.volume = sfx.GetVolume();
			if (sfx.ShouldUseAudioMixer && sfx.Mixer != null) {
				audioSource.outputAudioMixerGroup = sfx.Mixer;
			}
			OverrideAudioSource(audioSource, audioLibrary, typeDefinition);
			if (!sfx.ShouldLoop)
			{
				audioSource.PlayOneShot(sfx.GetClip());
			}
			else
			{
				audioSource.loop = true;
				audioSource.clip = sfx.GetClip();
				audioSource.Play();
			}
		}
		else
		{
			Debug.LogError($"SFX from type {typeDefinition.name} not found in audio library");
		}
	}

	public static void PlaySoundWithDelay(AudioSource audioSource, AudioLibrary audioLibrary, SFXTypeDefinition soundData, float delay)
	{
		audioSource.GetComponent<MonoBehaviour>().StartCoroutine(PlayDelayed(audioSource, audioLibrary, soundData, delay));
	}

	private static IEnumerator PlayDelayed(AudioSource audioSource, AudioLibrary audioLibrary, SFXTypeDefinition soundData, float delay)
	{
		if (soundData == null) yield break;

		delay = Mathf.Max(0f, delay);
		yield return new WaitForSeconds(delay);

		PlaySound(audioSource, audioLibrary, soundData);
	}

	public static void PlaySoundWithFade(AudioSource audioSource, AudioLibrary audioLibrary, SFXTypeDefinition typeDefinition, float fadeInDuration, float fadeOutDuration)
	{
		SoundToPlay sfx = GetSFX(audioLibrary,typeDefinition);
		if (sfx == null || sfx.GetClip() == null)
		{
			Debug.LogWarning("Invalid sound or clip.");
			return;
		}

		AudioSource source = audioSource;
		source.clip = sfx.GetClip();
		source.pitch = sfx.GetPitch();
		source.loop = sfx.ShouldLoop;
		source.volume = 0f;
		OverrideAudioSource(audioSource, audioLibrary, typeDefinition);
		source.Play();

		audioSource.GetComponent<MonoBehaviour>().StartCoroutine(FadeInFadeOutRoutine(audioSource, audioLibrary, sfx.GetVolume(), fadeInDuration, fadeOutDuration, sfx.ShouldLoop, source));
	}

	private static IEnumerator FadeInFadeOutRoutine(AudioSource audioSource, AudioLibrary audioLibrary, float targetVolume, float fadeInDuration, float fadeOutDuration, bool isLooping, AudioSource source = null)
	{
		if (source == null) { source = audioSource; }
		float timer = 0f;
		while (timer < fadeInDuration)
		{
			timer += Time.deltaTime;
			source.volume = Mathf.Lerp(0f, targetVolume, timer / fadeInDuration);
			yield return null;
		}
		source.volume = targetVolume;

		if (!isLooping)
		{
			float remainingTime = source.clip.length - fadeInDuration - fadeOutDuration;
			if (remainingTime > 0)
				yield return new WaitForSeconds(remainingTime);

			timer = 0f;
			while (timer < fadeOutDuration)
			{
				timer += Time.deltaTime;
				source.volume = Mathf.Lerp(targetVolume, 0f, timer / fadeOutDuration);
				yield return null;
			}
			source.Stop();
		}
	}

	public static void StopSFX(AudioSource audioSource)
	{
		if (audioSource.isPlaying)
		{
			audioSource.Stop();
		}
	}
}
