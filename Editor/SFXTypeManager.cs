#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public class SFXTypeManager : EditorWindow
{
	private string newTypeName = "NewSFXType";
	private Color newTypeColor = Color.white;

	[MenuItem("Tools/Audio/SFX Type Manager")]
	public static void ShowWindow()
	{
		GetWindow<SFXTypeManager>("SFX Type Manager");
	}

	private void OnGUI()
	{
		GUILayout.Label("Create New SFX Type", EditorStyles.boldLabel);

		newTypeName = EditorGUILayout.TextField("Type Name", newTypeName);
		newTypeColor = EditorGUILayout.ColorField("Editor Color", newTypeColor);

		if (GUILayout.Button("Create New Type"))
		{
			CreateNewSFXType();
		}

		EditorGUILayout.Space();
		GUILayout.Label("Existing Types", EditorStyles.boldLabel);

		string[] typeAssets = Directory.GetFiles("Assets/", "*.asset", SearchOption.AllDirectories);
		foreach (string assetPath in typeAssets)
		{
			SFXTypeDefinition typeDef = AssetDatabase.LoadAssetAtPath<SFXTypeDefinition>(assetPath);
			if (typeDef != null)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(typeDef.name, EditorStyles.boldLabel);
				if (GUILayout.Button("Delete", GUILayout.Width(60)))
				{
					AssetDatabase.DeleteAsset(assetPath);
					AssetDatabase.Refresh();
				}
				EditorGUILayout.EndHorizontal();
			}
		}
	}

	private void CreateNewSFXType()
	{
		SFXTypeDefinition newType = CreateInstance<SFXTypeDefinition>();
		newType.typeName = newTypeName;

		string path = "Assets/Audio/SFXTypes/";
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}

		string assetPath = path + newTypeName + ".asset";
		AssetDatabase.CreateAsset(newType, assetPath);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		EditorUtility.FocusProjectWindow();
		Selection.activeObject = newType;
	}
}
#endif