using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    public Animator anim;

    private string nextSceneName;

    public void FadeOutAndLoadNextScene(string sceneName)
    {
        nextSceneName = sceneName;
        anim.SetTrigger("fadeOutTrigger");
    }

    public void LoadingNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}

