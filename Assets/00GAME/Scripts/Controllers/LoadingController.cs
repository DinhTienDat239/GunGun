using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour
{
    [SerializeField] Slider _loadingProgressBar;
	List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();   

    void Start()
    {
        scenesToLoad.Add(SceneManager.LoadSceneAsync("Main"));
        StartCoroutine(LoadingScreen());
    }

    void Update()
    {
        
    }

    IEnumerator LoadingScreen()
    {
        float totalProgress = 0f;
        for (int i = 0; i < scenesToLoad.Count; i++)
        {
            while (!scenesToLoad[i].isDone)
            {
				totalProgress += scenesToLoad[i].progress;
				_loadingProgressBar.value = totalProgress / scenesToLoad.Count;
				yield return null;
			}           
        }
    }
}
