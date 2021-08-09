using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    //Credit to Brackeys' Tower Defence tutorial

    /// <summary>
    /// Black screen
    /// </summary>
    public Image img;

    /// <summary>
    /// Called anytime a scene is loaded
    /// </summary>
    void Start()
    {
        StartCoroutine(FadeIn());
    }

    /// <summary>
    /// Calls fade out on current scene, and moves to the next one. 
    /// Replace any reference to SceneManager.LoadScene with this function
    /// </summary>
    /// <param name="scene">name of the scene being transitioned to</param>
    public void FadeTo(string scene)
    {
        StartCoroutine(FadeOut(scene));
    }

    /// <summary>
    /// Increments black screen to transparent
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeIn()
    {
        float t = 1f;

        while (t > 0f)
        {
            t -= Time.unscaledDeltaTime;
            img.color = new Color(0f, 0f, 0f, t);
            yield return 0;
        }

    }
    
    /// <summary>
    /// Increments black screen to opaque, then transitions to next scene
    /// </summary>
    /// <param name="scene">name of next scene</param>
    /// <returns></returns>
    IEnumerator FadeOut(string scene)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime;
            img.color = new Color(0f, 0f, 0f, t);
            yield return 0;
        }

        SceneManager.LoadScene(scene);
    }

}
