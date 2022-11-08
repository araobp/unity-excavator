using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownloadIndicator : MonoBehaviour
{
    [SerializeField]
    GameObject m_PanelDownloadProgress;

    RectTransform m_RectTransform;
    float m_Progress = 0F;

    // Start is called before the first frame update
    void Start()
    {
        m_RectTransform = m_PanelDownloadProgress.transform.GetComponent<RectTransform>();
    }

    public float progress
    {
        get => m_Progress;
        set
        {
            m_Progress = value;
            m_RectTransform.localScale = new Vector3(progress, 1F, 1F);
        }
    }
}