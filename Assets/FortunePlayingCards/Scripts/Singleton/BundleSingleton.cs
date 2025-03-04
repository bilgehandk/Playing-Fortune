using UnityEngine;
using System.Collections.Generic;

public class BundleSingleton : Singleton<BundleSingleton>
{
    private readonly List<AssetBundle> assetBundleList = new List<AssetBundle>();
    private AssetBundle currentLevelAssetBundle;

    private void Awake()
    {
        UnloadCurrentLevelAssetBundle();
    }

    private void UnloadCurrentLevelAssetBundle()
    {
        if (currentLevelAssetBundle != null)
        {
            Debug.Log("Unloading current level AssetBundle.");
            currentLevelAssetBundle.Unload(false);
            currentLevelAssetBundle = null;
        }
    }

    private AssetBundle GetBundle(string name)
    {
        return assetBundleList.Find(bundle => bundle.name == name);
    }

    public AssetBundle LoadBundle(string path)
    {
        string name = System.IO.Path.GetFileNameWithoutExtension(path);
        AssetBundle assetBundle = GetBundle(name);
        
        if (assetBundle == null)
        {
            assetBundle = AssetBundle.LoadFromFile(path);
            if (assetBundle != null)
            {
                assetBundle.name = name;
                assetBundleList.Add(assetBundle);
                Debug.Log($"AssetBundle loaded successfully: {name}");
            }
            else
            {
                Debug.LogError($"Failed to load AssetBundle from path: {path}");
            }
        }
        else
        {
            Debug.Log($"AssetBundle already loaded: {name}");
        }

        return assetBundle;
    }

    public void UnloadAllBundles()
    {
        Debug.Log("Unloading all AssetBundles.");
        foreach (var bundle in assetBundleList)
        {
            bundle.Unload(true);
        }
        assetBundleList.Clear();
    }

    public void LoadLevelAssetBundle(string level)
    {
        string path = DirectoryUtility.ExternalAssets() + level + ".assetBundle";
        Debug.Log("LoadLevelAssetBundle: " + path);

        UnloadCurrentLevelAssetBundle();
        currentLevelAssetBundle = AssetBundle.LoadFromFile(path);

        if (currentLevelAssetBundle != null)
        {
            Debug.Log("AssetBundle loaded for level: " + level);

            if (Application.CanStreamedLevelBeLoaded(level))
            {
                Application.LoadLevel(level); // Deprecated, use SceneManager.LoadScene instead
            }
            else
            {
                Debug.LogError($"Level {level} cannot be streamed or is not available.");
            }
        }
        else
        {
            Debug.LogError("AssetBundle Not Found or Failed to Load: " + path);
        }
    }

    public void OnDestroy()
    {
        UnloadAllBundles();
        Destroy(gameObject);
    }
}
