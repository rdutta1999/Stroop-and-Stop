using TMPro;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StopSignal0 : MonoBehaviour
{
    public GameObject signalBox, levelInfoPanel, sceneFaderPanel;
    public TextMeshProUGUI responseText, trialsText;
    public Button[] respondButtons;
    public Sprite[] signals;

    private int signalIndex, moveCount = 0, totalTrials = 3, trialsRemaining;
    private float randomTime, signalChangeTime, respondClickTime, responseTime;
    private string stopSignalLevel0FileName = "StopSignalLevel0.txt";
    List<float> allResponseTimes = new List<float>();

    IEnumerator ChangeSignalOfBox()
    {
        UpdateTrials();
        ResetBoxSignal();
        RespondButtonsInteractable(false);
        randomTime = Random.Range(2.0f, 4.0f);                  // Randomly Generating Time Gap
        signalIndex = Random.Range(0, 2);                        // Randomly Generating Color Index
        yield return new WaitForSeconds(randomTime);
        signalChangeTime = Time.time;
        signalBox.GetComponent<Image>().sprite = signals[signalIndex];
        RespondButtonsInteractable(true);
    }

    private void UpdateTrials()
    {
        moveCount++;
        trialsRemaining = totalTrials - moveCount;
        if (trialsRemaining < 0)
        {
            WriteResponseTimes();
            sceneFaderPanel.GetComponent<LoadNextScene>().FadeOutAndLoadNextScene("Credits");
        }
        else
        {
            trialsText.text = "Trials Remaining: " + trialsRemaining.ToString();
        }
    }

    private void RespondButtonsInteractable(bool toggle)
    {
        for (int i = 0; i < respondButtons.Length; i++)
            respondButtons[i].interactable = toggle;
    }

    private void ResetBoxSignal()
    {
        signalBox.GetComponent<Image>().sprite = null;     
    }

    private void CalculateResponseTime()
    {
        responseTime = respondClickTime - signalChangeTime;
        responseText.text = "Response Time: " + responseTime.ToString();
        allResponseTimes.Add(responseTime);
    }

    private void WriteResponseTimes()
    {
        string stopSignalLevel0Path = Path.Combine(Application.persistentDataPath, stopSignalLevel0FileName);
        using (StreamWriter stopSignalLevel0Writer = new StreamWriter(stopSignalLevel0Path))
        {
            for (int i = 0; i < allResponseTimes.Count; i++)
            {
                stopSignalLevel0Writer.WriteLine(allResponseTimes[i]);
            }
        }
    }

    public void OnStartClick()
    {
        levelInfoPanel.GetComponent<Animator>().SetTrigger("startLevelTrigger");
        StartCoroutine(ChangeSignalOfBox());
    }

    public void OnRespondButtonClick(Button button)
    {
        if ((button.name == "RightRespondButton" && signalIndex == 0) || (button.name == "LeftRespondButton" && signalIndex == 1))
        {
            respondClickTime = Time.time;
            CalculateResponseTime();
            StartCoroutine(ChangeSignalOfBox());
        }
    }
}
