using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class InteractionUIForgotPass : MonoBehaviour
{
    public Button backBtn; 
    public Button signBtn;
    void Start()
    {
        SetActions(); 
    }
 
    void SetActions()
    {
        backBtn.onClick.AddListener(BackToSignIn); 
        signBtn.onClick.AddListener(BackToSignIn); 
    }
    void BackToSignIn()
    {
        SceneManager.LoadScene(SceneConfig.signIn); 
    }
}
