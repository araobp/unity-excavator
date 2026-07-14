using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ManagerAddressable : MonoBehaviour
{
    // REMOVED URL CONSTANT: The URL is handled globally by your Addressables Profile settings, not here!

    [SerializeField]
    TMP_Dropdown _Dropdown;

    [SerializeField]
    GameObject _PanelDownloadProgress;

    GameObject _Instance;
    AsyncOperationHandle<GameObject> _CurrentLoadHandle; // Added to track progress and safe releases
    DownloadIndicator _DownloadIndicator;

    void Start()
    {
        _DownloadIndicator = _PanelDownloadProgress.GetComponent<DownloadIndicator>();
        _DownloadIndicator.gameObject.SetActive(false);
    }

    public void OnDownloadButtonPressed()
    {
        // 1. SAFE RELEASE: Avoid memory leaks by releasing the old asset through Addressables
        if (_Instance != null)
        {
            Addressables.ReleaseInstance(_Instance);
            _Instance = null;
        }

        _DownloadIndicator.gameObject.SetActive(true);

        // 2. CORRECT KEY SELECTION: Fetch the internal Addressable key string (e.g., "ExcavatorPrefab")
        string assetAddressName = _Dropdown.options[_Dropdown.value].text;

        // 3. TRACK PROGRESS: Start a coroutine to monitor the download bar percentage
        StartCoroutine(TrackDownloadProgress(assetAddressName));
    }

    private IEnumerator TrackDownloadProgress(string assetAddressName)
    {
        // Start the instantiation async call and store its handle
        _CurrentLoadHandle = Addressables.InstantiateAsync(assetAddressName);

        // Loop until the network download and spawning finishes
        while (!_CurrentLoadHandle.IsDone)
        {
            float progress = _CurrentLoadHandle.PercentComplete;
            
            // OPTIONAL: If your DownloadIndicator script has a custom update method, call it here:
            // _DownloadIndicator.UpdateSlider(progress); 

            yield return null;
        }

        // Handle the final result once out of the loop
        OnAssetInstantiated(_CurrentLoadHandle);
    }

    private void OnAssetInstantiated(AsyncOperationHandle<GameObject> handle)
    {
        // Hide the progress panel now that the network operation is complete
        _DownloadIndicator.gameObject.SetActive(false);

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            _Instance = handle.Result;
            Debug.Log("[CDN] Asset successfully pulled from GitHub Pages and spawned!");
        }
        else
        {
            Debug.LogError($"[CDN] Failed to load remote asset. Error: {handle.OperationException}");
            
            // Clean up the invalid handle safely if it fails
            if (handle.IsValid())
            {
                Addressables.Release(handle);
            }
        }
    }

    private void OnDestroy()
    {
        // Double check cleanup if this manager object is suddenly destroyed
        if (_Instance != null)
        {
            Addressables.ReleaseInstance(_Instance);
        }
    }
}
