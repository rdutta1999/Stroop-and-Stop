using TMPro;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StroopController1 : MonoBehaviour
{
    public GameObject levelInfoPanel, sceneFaderPanel;
    public TextMeshProUGUI responseText, trialsText, wordText;
    public Button[] respondButtons;

    private string[] words = { "RED", "GREEN", "BLUE", "YELLOW" };
    private float wordChangeTime, respondClickTime, responseTime, randomTime;
    private int wordIndex, moveCount = 0, totalTrials = 3, trialsRemaining;
    private string stroopLevel1FileName = "StroopLevel1.txt";
    List<float> allResponseTimes = new List<float>(); 

    IEnumerator ChangeWordOfBox()
    {
        UpdateTrials();
        ResetBoxWord();
        RespondButtonsInteractable(false);
        randomTime = Random.Range(2.0f, 4.0f);                  // Randomly Generating Time Gap
        wordIndex = Random.Range(0, 4);                        // Randomly Generating Color Index
        yield return new WaitForSeconds(randomTime);
        wordChangeTime = Time.time;
        wordText.text = words[wordIndex];
        RespondButtonsInteractable(true);
    }

    private void UpdateTrials()
    {
        moveCount++;
        trialsRemaining = totalTrials - moveCount;
        if (trialsRemaining < 0)
        {
            WriteResponseTimes();
            sceneFaderPanel.GetComponent<LoadNextScene>().FadeOutAndLoadNextScene("Stroop2");
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

    private void ResetBoxWord()
    {
        wordText.text = "";
    }

    private void CalculateResponseTime()
    {
        responseTime = respondClickTime - wordChangeTime;
        responseText.text = "Response Time: " + responseTime.ToString();
        allResponseTimes.Add(responseTime);
    }

    private void WriteResponseTimes()
    {
        string stroopLevel1Path = Path.Combine(Application.persistentDataPath, stroopLevel1FileName);
        using (StreamWriter stroopLevel1Writer = new StreamWriter(stroopLevel1Path))
        {
            for (int i=0; i<allResponseTimes.Count; i++)
            {
                stroopLevel1Writer.WriteLine(allResponseTimes[i]);
            }
        }
    }

    public void OnStartClick()
    {
        levelInfoPanel.GetComponent<Animator>().SetTrigger("startLevelTrigger");
        StartCoroutine(ChangeWordOfBox());
    }

    public void OnRespondButtonClick(Button button)
    {
        if ((button.name == "RedRespondButton" && wordIndex == 0) || (button.name == "GreenRespondButton" && wordIndex == 1)
                           || (button.name == "BlueRespondButton" && wordIndex == 2) || (button.name == "YellowRespondButton" && wordIndex == 3))
        {
            respondClickTime = Time.time;
            CalculateResponseTime();
            StartCoroutine(ChangeWordOfBox());
        }
    }
}
