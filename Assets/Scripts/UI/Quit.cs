using UnityEditor;
using UnityEngine;

public class Quit : MonoBehaviour
{
    public void OnQuitClick()
    {
#if UNITY_EDITOR 
        EditorApplication.isPlaying = false;
#elif (UNITY_STANDALONE) 
        Application.Quit();
#elif (UNITY_WEBGL)
        Application.OpenURL("about:blank");
#endif
    }
}
