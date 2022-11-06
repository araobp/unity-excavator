using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebClient : MonoBehaviour
{
    /*** HTTPS client for AssetBundle ***/

    const string CONTENT_TYPE_FILE = "application/octet-stream";

    public delegate void CallbackGetAsset(bool err, AssetBundle bundle);
    public delegate void CallbackGetAssetProgress(float progress, ulong downloadBytes);

    public void GetAsset(string url, CallbackGetAsset callback, CallbackGetAssetProgress progress)
    {
        StartCoroutine(_GetAsset(url, callback, progress));
    }

    IEnumerator _GetAsset(string url, CallbackGetAsset callback, CallbackGetAssetProgress progress)
    {
        using (UnityWebRequest webRequest = UnityWebRequestAssetBundle.GetAssetBundle(url))
        {
            webRequest.SetRequestHeader("Accept", CONTENT_TYPE_FILE);

            UnityWebRequestAsyncOperation op = webRequest.SendWebRequest();

            while (true)
            {
                if (op.isDone)
                {
                    if (webRequest.result != UnityWebRequest.Result.Success)
                    {
                        Debug.Log("Get asset failed");
                        callback(true, null);
                    }
                    else
                    {
                        // Get downloaded asset bundle
                        Debug.Log("Get asset succeeded");
                        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(webRequest);
                        callback(false, bundle);
                    }
                    yield break;
                }

                yield return null;

                progress(op.progress, webRequest.downloadedBytes);
            }
        }
    }

}
