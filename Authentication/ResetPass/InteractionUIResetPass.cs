using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class InteractionUIResetPass : MonoBehaviour
{
    public Button backtoForgotBtn; 
    public Button signInBtn;
    void Start()
    {
        SetActions(); 
    }
    void SetActions()
    {
        backtoForgotBtn.onClick.AddListener(BackToForgot); 
        signInBtn.onClick.AddListener(BackToSignIn); 
    }
    void BackToForgot()
    {
        SceneManager.LoadScene(SceneConfig.forgotPass); 
    }

    void BackToSignIn()
    {
        SceneManager.LoadScene(SceneConfig.signIn); 
    }
}
