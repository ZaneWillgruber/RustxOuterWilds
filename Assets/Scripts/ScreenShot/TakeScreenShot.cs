using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TakeScreenShot : MonoBehaviour
{
    Camera cam;
    [SerializeField] string path;
    [SerializeField] string fileName;

    
    [ContextMenu("ScreenShot")]
    void ScreenShot()
    {
        TakeScreenshots(path);
    }
    void TakeScreenshots(string fullPath)
    {
        Debug.Log("taking screenshot");
        if (cam == null)
            cam = GetComponent<Camera>();


        RenderTexture rt = new RenderTexture(256, 256, 24);
        cam.targetTexture = rt;
        Texture2D screenShot = new Texture2D(256, 256, TextureFormat.RGBA32, false);
        cam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
        cam.targetTexture = null;
        RenderTexture.active = null;

        if (Application.isEditor)
        {
            DestroyImmediate(rt);
        }
        else
        {
            Destroy(rt);
        }

        byte[] bytes = screenShot.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.dataPath + fullPath + "/" + fileName + ".png", bytes);
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif

    }

}
