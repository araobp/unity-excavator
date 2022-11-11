using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class RestClient : MonoBehaviour
{
    enum Method
    {
        GET, POST, PUT, DELETE
    }

    class API
    {
        public EndPoint endPoint;
        public string path;

        public API(EndPoint endPoint, string path)
        {
            this.endPoint = endPoint;
            this.path = path;
        }
    }

    const string CONTENT_TYPE = "application/json";

    /*** REST APIs ***/

    public delegate void CallbackGet(bool err, string text);
    public delegate void CallbackPost(bool err);
    public delegate void CallbackPut(bool err, string text = null);
    public delegate void CallbackDelete(bool err);

    static char[] TRIM_CHARS = { '[', ']' };

    string[] ToStringArray(string text)
    {
        return text.Replace("\"", "").Trim(TRIM_CHARS).Split(',');
    }

    public void Get(EndPoint endPoint, string path, Hashtable headers, CallbackGet callback)
    {
        _Get(new API(endPoint, path), (err, res) =>
        {
            if (err)
            {
                Debug.Log("Get failed");
                callback(true, null);
            }
            else
            {
                callback(false, res.text);
            }
        }, headers);
    }

    public void Post(EndPoint endPoint, string path, Hashtable headers, string jsonBody, CallbackPost callback)
    {
        byte[] body = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        _Post(new API(endPoint, path), null, body, (err, res) =>
        {
            if (err)
            {
                Debug.Log("Post failed");
                callback(true);
            }
            else
            {
                callback(false);
            }
        }, headers);
    }

    public void Put(EndPoint endPoint, string path, string jsonBody, CallbackPut callback)
    {
        byte[] body = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        _Put(new API(endPoint, path), body, (err, res) =>
        {
            if (err)
            {
                Debug.Log("Put failed");
                callback(true, null);
            }
            else
            {
                callback(false, res.text);
            }
        });
    }

    public void Delete(EndPoint endPoint, string path, CallbackDelete callback)
    {
        _Delete(new API(endPoint, path), null, (err, res) =>
        {
            if (err)
            {
                Debug.Log("Delete failed");
                callback(true);
            }
            else
            {
                callback(false);
            }
        });
    }

    /*** REST client common part ***/

    delegate void Callback(bool err, DownloadHandler downloadHandler);

    void _Post(API api, WWWForm postData, byte[] body, Callback callback, Hashtable headers)
    {
        StartCoroutine(Request(api, Method.POST, postData, body, callback, headers));
    }

    void _Put(API api, byte[] body, Callback callback)
    {
        StartCoroutine(Request(api, Method.PUT, null, body, callback));
    }

    void _Get(API api, Callback callback, Hashtable headers)
    {
        StartCoroutine(Request(api, Method.GET, null, null, callback, headers));
    }

    void _Delete(API api, byte[] body, Callback callback)
    {
        StartCoroutine(Request(api, Method.DELETE, null, body, callback));
    }

    void SetHeaders(UnityWebRequest webRequest, Hashtable headers)
    {
        webRequest.SetRequestHeader("Accept", CONTENT_TYPE);
        if (headers != null)
        {
            foreach (DictionaryEntry h in headers)
            {
                Debug.Log($"{h.Key}: {h.Value}");
                webRequest.SetRequestHeader(h.Key.ToString(), h.Value.ToString());
            }
        }
    }

    IEnumerator Request(API api, Method m, WWWForm form, byte[] body, Callback callback, Hashtable headers = null)
    {
        UnityWebRequest webRequest;
        string uri = api.endPoint.baseUrl + api.path;
        Debug.Log(uri);

        switch (m)
        {
            case Method.GET:
                webRequest = UnityWebRequest.Get(uri);
                SetHeaders(webRequest, headers);
                break;
            case Method.PUT:
                webRequest = UnityWebRequest.Put(uri, body);
                SetHeaders(webRequest, headers);
                webRequest.uploadHandler.contentType = CONTENT_TYPE;
                break;
            case Method.POST:
                webRequest = UnityWebRequest.Post(uri, form);
                SetHeaders(webRequest, headers);
                UploadHandlerRaw uh2 = new UploadHandlerRaw(body);
                uh2.contentType = CONTENT_TYPE;
                webRequest.uploadHandler = uh2;
                break;
            case Method.DELETE:
                webRequest = UnityWebRequest.Delete(uri);
                SetHeaders(webRequest, headers);
                UploadHandlerRaw uh3 = new UploadHandlerRaw(body);
                uh3.contentType = CONTENT_TYPE;
                webRequest.uploadHandler = uh3;
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                break;
            default:
                webRequest = UnityWebRequest.Get(uri);
                SetHeaders(webRequest, headers);
                break;
        }

        using (webRequest)
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log("Error: " + webRequest.error);
                    callback(true, null);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log("HTTP Error: " + webRequest.error);
                    callback(true, null);
                    break;
                case UnityWebRequest.Result.Success:
                    if (webRequest.downloadHandler.text.Length <= 1024)
                    {
                        Debug.Log("Received: " + webRequest.downloadHandler.text);
                    }
                    else
                    {
                        Debug.Log("Received: <text data larger than 1024 characters...>");
                    }
                    callback(false, webRequest.downloadHandler);
                    break;
            }
        }
    }

}