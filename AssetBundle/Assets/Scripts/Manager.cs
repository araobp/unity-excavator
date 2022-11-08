using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manager : WebClient
{
    const string URL_LOCAL = "http://localhost:8080/asset_bundles";
    const string URL = "http://localhost:8080/asset_bundles";

    [SerializeField]
    TMP_Dropdown m_Dropdown;

    GameObject instance;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnDownloadButtonPressed()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        string name = m_Dropdown.options[m_Dropdown.value].text;
        string url = $"{URL_LOCAL}/{name}";

        GetAsset(url,

    (err, bundle) =>
    {
        if (!err)
        {
            
            GameObject prefab = bundle.LoadAsset<GameObject>(name);
            instance = Instantiate(prefab);
        }
    },
    (progress, downloadBytes) =>
    {
        Debug.Log(progress);
        Debug.Log(downloadBytes);   
    });
    }
}
