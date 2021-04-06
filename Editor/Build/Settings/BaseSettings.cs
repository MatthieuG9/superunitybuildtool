﻿using System.IO;
using UnityEditor;
using UnityEngine;

namespace SuperSystems.UnityBuild
{

public class BaseSettings : ScriptableObject
{
    protected const string SettingsFolderName = "UnityBuildSettings";
    protected const string SettingsPath = "Assets/{0}";
    protected const string SettingsPrefsKey = "SuperSystems.UnityBuildSettings";

    protected static T CreateAsset<T>(string assetName) where T : BaseSettings
    {
        // Try to load an existing settings asset at the path specified in EditorPrefs, or fallback to a default path
        string settingsRoot = string.Format(SettingsPath, SettingsFolderName);
        string defaultAssetPath = settingsRoot + "/" + string.Format("{0}.asset", assetName);
        string prefsAssetPath = EditorPrefs.HasKey(SettingsPrefsKey) ?
            EditorPrefs.GetString(SettingsPrefsKey, defaultAssetPath) :
            defaultAssetPath;
        string assetPath = File.Exists(prefsAssetPath) ? prefsAssetPath : defaultAssetPath;

        T instance = AssetDatabase.LoadAssetAtPath<T>(assetPath) as T;

        if (instance == null)
        {
            Debug.Log("UnityBuild: Creating settings file - " + defaultAssetPath);
            instance = CreateInstance<T>();
            instance.name = assetName;

            if (!Directory.Exists(settingsRoot))
                AssetDatabase.CreateFolder("Assets", SettingsFolderName);

            AssetDatabase.CreateAsset(instance, defaultAssetPath);
        }

        return instance;
    }
}

}