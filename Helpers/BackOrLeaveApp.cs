using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackOrLeaveApp : MonoBehaviour
{
    private bool clickedBefore = false;
    const float timerTime = 1f;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !clickedBefore)
        { 
            clickedBefore = true;
            StartCoroutine(QuitingTimer());
        }
    }

    IEnumerator QuitingTimer()
    {
        yield return null;
        float counter = 0;
        while (counter < timerTime)
        {
            counter += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Quit();
            }
            yield return null;
        }
        clickedBefore = false;
        StopAllCoroutines();
        StartCoroutine(Helper.LoadAsynchronously(SceneNameManager.prevScene));
    }

    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
