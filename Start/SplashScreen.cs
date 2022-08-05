using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.Networking;
using EasyUI.Toast;
public class SplashScreen : MonoBehaviour
{
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait; 
        SetOrganCache();
    }

    async void SetOrganCache()
    {
        StartCoroutine(LoadMainScene());
    }

    IEnumerator LoadMainScene ()
    {
        string nextSceneName = string.IsNullOrEmpty(PlayerPrefs.GetString(PlayerPrefConfig.userToken)) ? SceneConfig.home_nosignin : SceneConfig.home_user;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
