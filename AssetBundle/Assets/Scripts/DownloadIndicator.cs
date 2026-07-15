using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownloadIndicator : MonoBehaviour
{
    [SerializeField]
    GameObject _PanelDownloadProgress;

    RectTransform _RectTransform;
    float _Progress = 0F;

    void Awake()
    {
        // 1. Ensure we have the reference even if it wasn't assigned manually
        if (_PanelDownloadProgress == null) _PanelDownloadProgress = this.gameObject;
        
        // 2. Cache the component immediately
        _RectTransform = _PanelDownloadProgress.GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _RectTransform = _PanelDownloadProgress.transform.GetComponent<RectTransform>();
        Debug.Log(_RectTransform);
    }

    public float progress
    {
        get => _Progress;
        set
        {
            _Progress = value;
            _RectTransform.localScale = new Vector3(progress, 1F, 1F);
        }
    }
}