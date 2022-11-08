using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manager : WebClient
{
    //const string URL_LOCAL = "http://localhost:8080/asset_bundles";
    const string URL = "https://araobp.github.io/unity-excavator/www/asset_bundles";

    [SerializeField]
    TMP_Dropdown m_Dropdown;

    [SerializeField]
    GameObject m_PanelDownloadProgress;

    GameObject m_Instance;
    DownloadIndicator m_DownloadIndicator;

    // Start is called before the first frame update
    void Start()
    {
        m_DownloadIndicator = m_PanelDownloadProgress.GetComponent<DownloadIndicator>();
        m_DownloadIndicator.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnDownloadButtonPressed()
    {
        if (m_Instance != null)
        {
            Destroy(m_Instance);
        }

        m_DownloadIndicator.gameObject.SetActive(true);

        string name = m_Dropdown.options[m_Dropdown.value].text;
        //string url = $"{URL_LOCAL}/{name}";
        string url = $"{URL}/{name}";

        GetAsset(url,

    (err, bundle) =>
    {
        if (!err)
        {
            
            GameObject prefab = bundle.LoadAsset<GameObject>(name);
            m_Instance = Instantiate(prefab);
        }
    },
    (progress, downloadBytes) =>
    {
        //Debug.Log(progress);
        //Debug.Log(downloadBytes);
        m_DownloadIndicator.progress = progress;
        if (progress >= 1F)
        {
            m_DownloadIndicator.gameObject.SetActive(false);
        }
    });
    }
}
