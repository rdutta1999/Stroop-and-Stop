using UnityEngine;
using UnityEngine.UI;


public class ChooseTestController : MonoBehaviour
{
    public Button stroop, stopSignal;
    public GameObject sceneFaderPanel;

    public void OnClickStroopButton()
    {
        sceneFaderPanel.GetComponent<LoadNextScene>().FadeOutAndLoadNextScene("Stroop0");
    }

    public void OnClickStopSignalButton()
    {
        sceneFaderPanel.GetComponent<LoadNextScene>().FadeOutAndLoadNextScene("StopSignal0");
    }

}
