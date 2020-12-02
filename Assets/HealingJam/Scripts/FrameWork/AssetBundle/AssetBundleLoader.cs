using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssetBundles;
using UnityEngine.Networking;
using Utility = AssetBundles.Utility;

public class AssetBundleLoader : MonoBehaviour
{
    public Text text = null;

    IEnumerator Start()
    {
        AssetBundleManager.SetDevelopmentAssetBundleServer();

        var operation = AssetBundleManager.Initialize();
        yield return operation;

        var mapDataOperation = AssetBundleManager.LoadAllAssetAsync("mapdata");

        //yield return mapDataOperation;

        //List<TextAsset> textAssets = new List<TextAsset>();
        //var c = AssetBundleManager.GetLoadedAssetBundle("mapdata", out string error);

        //if (string.IsNullOrEmpty(error))
        //{
        //    foreach (var f in c.m_AssetBundle.GetAllAssetNames())
        //    {
        //        textAssets.Add(c.m_AssetBundle.LoadAsset<TextAsset>(f));
        //    }

        //    text.text = textAssets.Count + "개\n" + textAssets[textAssets.Count - 1].text;
        //}
        //else
        //{
        //    text.text = error;
        //}
    }
}

