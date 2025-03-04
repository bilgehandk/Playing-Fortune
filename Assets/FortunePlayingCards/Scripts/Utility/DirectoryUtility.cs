using UnityEngine;
using System.IO;

static public class DirectoryUtility 
{
    static public string ExternalAssets()
    {
        string externalPath = string.Empty;
        
        switch (Application.platform)
        {
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.WindowsEditor:    
                externalPath = Path.Combine(Application.dataPath, "../ExternalAssets/");
                break;
            
            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.IPhonePlayer:        
            case RuntimePlatform.LinuxPlayer:
                externalPath = Path.Combine(Application.dataPath, "ExternalAssets/");
                break;

            case RuntimePlatform.Android:
                // For Android, ExternalAssets should be handled differently (e.g., persistentDataPath or streamingAssetsPath)
                externalPath = Path.Combine(Application.persistentDataPath, "ExternalAssets/");
                break;
            
            case RuntimePlatform.WebGLPlayer:
                // WebGL cannot directly access files in the same way; may need to handle asset loading differently
                externalPath = Path.Combine(Application.streamingAssetsPath, "ExternalAssets/");
                break;

            default:
                Debug.LogError("Unsupported platform for ExternalAssets path.");
                break;
        }

        return externalPath;
    }
}