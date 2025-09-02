using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(AudioLibrary))]
public class AudioLibraryEditor : Editor
{
	private SerializedProperty soundsProp;
	private bool initialized = false;

	private void OnEnable()
	{
		try
		{			
			if (target == null || serializedObject == null)
			{
				Debug.LogWarning("AudioLibraryEditor: Target or serializedObject is null");
				initialized = false;
				return;
			}

			soundsProp = serializedObject.FindProperty("soundsToPlay");
			initialized = true;
		}
		catch (System.Exception e)
		{
			Debug.LogError($"AudioLibraryEditor initialization failed: {e.Message}");
			initialized = false;
		}
	}

	public override void OnInspectorGUI()
	{
		if (!initialized || target == null)
		{
			EditorGUILayout.HelpBox("Editor could not be initilized. Try select it again.", MessageType.Error);
			return;
		}

		serializedObject.Update();

		try
		{
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Sound Settings", EditorStyles.boldLabel);

			if (soundsProp != null)
			{
				EditorGUILayout.PropertyField(soundsProp, new GUIContent("Sound List"), true);
				EditorGUILayout.Space(10);

				VerifyContent();
			}
			else
			{
				EditorGUILayout.HelpBox("Property 'soundsToPlay' not found!", MessageType.Error);
			}
		}
		finally
		{
			if (serializedObject != null && serializedObject.targetObject != null)
			{
				serializedObject.ApplyModifiedProperties();
			}
		}
	}

	private void VerifyContent()
	{
		// verify duplicated types
		var duplicates = new HashSet<string>();
		var types = new HashSet<string>();

		for (int i = 0; i < soundsProp.arraySize; i++)
		{
			var element = soundsProp.GetArrayElementAtIndex(i);
			if (element == null) continue;

			var typeDef = element.FindPropertyRelative("typeDefinition");
			if (typeDef == null || typeDef.objectReferenceValue == null) continue;

			string typeName = typeDef.objectReferenceValue.name;
			if (!types.Add(typeName))
			{
				duplicates.Add(typeName);
			}
		}

		//show error messages
		if (duplicates.Count > 0)
		{
			EditorGUILayout.HelpBox($"Duplicated Types: {string.Join(", ", duplicates)}", MessageType.Error);

			if (GUILayout.Button("Remove Duplicated Sounds", GUILayout.Height(30)))
			{
				RemoveDuplicates();
			}
			EditorGUILayout.Space(10);
		}

		//empty clips
		bool hasEmptyClips = false;
		string typeClipName = string.Empty;
		for (int i = 0; i < soundsProp.arraySize; i++)
		{
			var element = soundsProp.GetArrayElementAtIndex(i);
			if (element == null) continue;

			var randomClip = element.FindPropertyRelative("shouldRandomizeClip");
			if (randomClip == null) continue;

			if (randomClip.boolValue)
			{
				var clips = element.FindPropertyRelative("_clips");
				if (clips == null || clips.arraySize == 0)
				{
					hasEmptyClips = true;
					typeClipName = element.FindPropertyRelative("typeDefinition")?.objectReferenceValue?.name;
				}
				for (int j = 0; j < clips.arraySize; j++)
				{
					var clipElement = clips.GetArrayElementAtIndex(j);
					if (clipElement == null || clipElement.objectReferenceValue == null)
					{
						hasEmptyClips = true;
						typeClipName = element.FindPropertyRelative("typeDefinition")?.objectReferenceValue?.name;
						break;
					}
				}
			}
			else
			{
				var clip = element.FindPropertyRelative("_clip");
				if (clip == null || clip.objectReferenceValue == null)
				{
					hasEmptyClips = true;
					typeClipName = element.FindPropertyRelative("typeDefinition")?.objectReferenceValue?.name;
				}
			}
		}

		bool hasEmptyMixer = false;
		for (int i = 0; i < soundsProp.arraySize; i++)
		{
			var element = soundsProp.GetArrayElementAtIndex(i);
			if (element == null) continue;

			var mixerProp = element.FindPropertyRelative("shouldUseAudioMixer");
			if (mixerProp == null) continue;

			if (mixerProp.boolValue)
			{
				var mixer = element.FindPropertyRelative("_mixer");
				if (mixer == null || mixer.objectReferenceValue == null) { 
					hasEmptyMixer = true;
					typeClipName = element.FindPropertyRelative("typeDefinition")?.objectReferenceValue?.name;
				}
			}
		}

		if (hasEmptyMixer)
		{
			EditorGUILayout.HelpBox($"Property 'mixer' is null on Sound Type: {typeClipName}", MessageType.Error);
		}

		if (hasEmptyClips)
		{
			EditorGUILayout.HelpBox($"Warning: Some sounds does not have a valid AudioClip. Sound type: {typeClipName}", MessageType.Warning);
		}
	}

	private void RemoveDuplicates()
	{
		if (soundsProp == null) return;

		var uniqueTypes = new HashSet<string>();
		int removedCount = 0;

		for (int i = soundsProp.arraySize - 1; i >= 0; i--)
		{
			var element = soundsProp.GetArrayElementAtIndex(i);
			if (element == null) continue;

			var typeDef = element.FindPropertyRelative("typeDefinition");
			if (typeDef == null || typeDef.objectReferenceValue == null) continue;

			string typeName = typeDef.objectReferenceValue.name;
			if (!uniqueTypes.Add(typeName))
			{
				soundsProp.DeleteArrayElementAtIndex(i);
				removedCount++;
			}
		}

		Debug.Log($"Removed {removedCount} duplicated sounds");
	}

}