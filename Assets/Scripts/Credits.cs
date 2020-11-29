using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public Button tryAgain, quit;
    public GameObject sceneFaderPanel;

    public void OnClickTryAgainButton()
    {
        sceneFaderPanel.GetComponent<LoadNextScene>().FadeOutAndLoadNextScene("ChooseTest");
    }

    public void OnClickQuitButton()
    {
        Application.Quit();
    }

}
