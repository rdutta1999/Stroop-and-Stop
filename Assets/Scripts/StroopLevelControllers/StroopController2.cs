using TMPro;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StroopController2 : MonoBehaviour
{
    public GameObject levelInfoPanel, sceneFaderPanel;
    public TextMeshProUGUI responseText, trialsText, wordText;
    public Button[] respondButtons;

    private Color defaultWordColor = new Color(255, 255, 255, 255);
    private Color[] colors = { new Color(255, 0, 0, 255),
                               new Color(0, 255, 0, 255),
                               new Color(0, 0, 255, 255),
                               new Color(255, 255, 0, 255) }; 
    private int colorIndex, moveCount = 0, totalTrials = 3, trialsRemaining;
    private float colorChangeTime, respondClickTime, responseTime, randomTime;
    private string stroopLevel2FileName = "StroopLevel2.txt";
    List<float> allResponseTimes = new List<float>();

    IEnumerator ChangeWordColorOfBox()
    {
        UpdateTrials();
        ResetWordColor();
        RespondButtonsInteractable(false);
        randomTime = Random.Range(2.0f, 4.0f);                  // Randomly Generating Time Gap
        colorIndex = Random.Range(0, 4);                        // Randomly Generating Color Index
        yield return new WaitForSeconds(randomTime);
        colorChangeTime = Time.time;
        wordText.color = colors[colorIndex];
        RespondButtonsInteractable(true);
    }

    private void UpdateTrials()
    {
        moveCount++;
        trialsRemaining = totalTrials - moveCount;
        if (trialsRemaining < 0)
        {
            WriteResponseTimes();
            sceneFaderPanel.GetComponent<LoadNextScene>().FadeOutAndLoadNextScene("Stroop3");
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

    private void ResetWordColor()
    {
        wordText.color = defaultWordColor;
    }

    private void CalculateResponseTime()
    {
        responseTime = respondClickTime - colorChangeTime;
        responseText.text = "Response Time: " + responseTime.ToString();
        allResponseTimes.Add(responseTime);
    }

    private void WriteResponseTimes()
    {
        string stroopLevel2Path = Path.Combine(Application.persistentDataPath, stroopLevel2FileName);
        using (StreamWriter stroopLevel2Writer = new StreamWriter(stroopLevel2Path))
        {
            for (int i = 0; i < allResponseTimes.Count; i++)
            {
                stroopLevel2Writer.WriteLine(allResponseTimes[i]);
            }
        }
    }

    public void OnStartClick()
    {
        levelInfoPanel.GetComponent<Animator>().SetTrigger("startLevelTrigger");
        StartCoroutine(ChangeWordColorOfBox());
    }

    public void OnRespondButtonClick(Button button)
    {
        if ((button.name == "RedRespondButton" && colorIndex == 0) || (button.name == "GreenRespondButton" && colorIndex == 1)
                           || (button.name == "BlueRespondButton" && colorIndex == 2) || (button.name == "YellowRespondButton" && colorIndex == 3))
        {
            respondClickTime = Time.time;
            CalculateResponseTime();
            StartCoroutine(ChangeWordColorOfBox());
        }
    }

}
