using TMPro;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StroopController4 : MonoBehaviour
{
    public GameObject levelInfoPanel, sceneFaderPanel;
    public TextMeshProUGUI responseText, trialsText, wordText;
    public Button[] respondButtons;


    private Color defaultBoxColor = new Color(255, 255, 255, 255);
    private Color[] colors = { new Color(255, 0, 0, 255),
                               new Color(0, 255, 0, 255),
                               new Color(0, 0, 255, 255),
                               new Color(255, 255, 0, 255) };
    private string[] words = { "RED", "GREEN", "BLUE", "YELLOW" };
    private int colorIndex, wordIndex, moveCount = 0, totalTrials = 3, trialsRemaining, noOfSameTrialsRemaining, noOfDifferentTrialsRemaining, differentTrials = 4;
    private float colorChangeTime, respondClickTime, responseTime, randomTime, responseTimeSeparator = -1f;
    private string stroopLevel4FileName = "StroopLevel4.txt";
    List<float> allResponseTimes = new List<float>();

    IEnumerator ChangeWordAndColor()
    {
        UpdateTrials();
        ResetWordAndColor();
        RespondButtonsInteractable(false);
        randomTime = Random.Range(2.0f, 4.0f);                  // Randomly Generating Time Gap
        yield return new WaitForSeconds(randomTime);
        colorChangeTime = Time.time;
        wordText.text = words[wordIndex];
        wordText.color = colors[colorIndex];
        RespondButtonsInteractable(true);
    }

    private void SameTrial()
    {
        noOfSameTrialsRemaining--;
        colorIndex = Random.Range(0, 4);                        // Randomly Generating Color Index
        wordIndex = colorIndex;
        StartCoroutine(ChangeWordAndColor());
    }

    private void DifferentTrial()
    {
        AddResponseTimeSeparator();
        noOfDifferentTrialsRemaining--;
        colorIndex = Random.Range(0, 4);                        // Randomly Generating Color Index
        wordIndex = Random.Range(0, 4);
        while (wordIndex == colorIndex)
            wordIndex = Random.Range(0, 4);
        StartCoroutine(ChangeWordAndColor());
    }

    private void InitTrials()
    {
        AddResponseTimeSeparator();
        noOfSameTrialsRemaining = Random.Range(4, 8);
        noOfDifferentTrialsRemaining = differentTrials;
    }

    private void AddResponseTimeSeparator()
    {
        allResponseTimes.Add(responseTimeSeparator);
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

    private void ResetWordAndColor()
    {
        wordText.text = "";
        wordText.color = defaultBoxColor;
    }

    private void CalculateResponseTime()
    {
        responseTime = respondClickTime - colorChangeTime;
        responseText.text = "Response Time: " + responseTime.ToString();
        allResponseTimes.Add(responseTime);
    }

    private void WriteResponseTimes()
    {
        string stroopLevel4Path = Path.Combine(Application.persistentDataPath, stroopLevel4FileName);
        using (StreamWriter stroopLevel4Writer = new StreamWriter(stroopLevel4Path))
        {
            for (int i = 0; i < allResponseTimes.Count; i++)
            {
                stroopLevel4Writer.WriteLine(allResponseTimes[i]);
            }
        }
    }

    public void OnStartClick()
    {
        levelInfoPanel.GetComponent<Animator>().SetTrigger("startLevelTrigger");
        InitTrials();
        Debug.Log("OnStart  " + noOfSameTrialsRemaining);
        SameTrial();
    }

    public void OnRespondButtonClick(Button button)
    {
        if ((button.name == "RedRespondButton" && colorIndex == 0) || (button.name == "GreenRespondButton" && colorIndex == 1)
             || (button.name == "BlueRespondButton" && colorIndex == 2) || (button.name == "YellowRespondButton" && colorIndex == 3))
        {
            respondClickTime = Time.time;
            CalculateResponseTime();

            Debug.Log("OnRespond  " + noOfSameTrialsRemaining);

            if (noOfDifferentTrialsRemaining <=0 && noOfDifferentTrialsRemaining <=0)
            {
                InitTrials();
            }
            if (noOfSameTrialsRemaining > 0)
            {
                SameTrial();
            }            
            else
            {
                DifferentTrial();
            }                
        }
    }
}

