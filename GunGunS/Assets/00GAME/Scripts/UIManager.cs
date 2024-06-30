using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    void Start()
    {
        
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeUI(string UIName)
    {
        var lastSceneIndex = SceneManager.sceneCount - 1;

        // Get last Scene by index in all loaded Scenes
        Scene lastLoadedScene = SceneManager.GetSceneAt(lastSceneIndex);

        if (lastLoadedScene.name == CONSTANTS.SCENE_MAIN)
        {
            SceneManager.LoadSceneAsync(UIName, LoadSceneMode.Additive);
            return;
        }
        // Unload Scene
        SceneManager.UnloadSceneAsync(lastLoadedScene);
        SceneManager.LoadSceneAsync(UIName, LoadSceneMode.Additive);

    }
}
