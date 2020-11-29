using TMPro;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StroopController0 : MonoBehaviour
{
    public GameObject levelInfoPanel, colorBox, sceneFaderPanel;
    public Button respond;
    public TextMeshProUGUI responseText, trialsText;

    private Color defaultBoxColor = new Color(255, 255, 255, 255);
    private Color[] colors = { new Color(255, 0, 0, 255),
                               new Color(0, 255, 0, 255),
                               new Color(0, 0, 255, 255),
                               new Color(255, 255, 0, 255) };
    private float colorChangeTime, respondClickTime, responseTime, randomTime;
    private int colorIndex, moveCount = 0, totalTrials = 3, trialsRemaining;
    private string stroopLevel0FileName = "StroopLevel0.txt";
    List<float> allResponseTimes = new List<float>();

    IEnumerator ChangeColorOfBox()
    {
        UpdateTrials();
        ResetColorBox();
        RespondButtonInteractable(false);
        randomTime = Random.Range(2.0f, 4.0f);                  // Randomly Generating Time Gap
        colorIndex = Random.Range(0, 4);                        // Randomly Generating Color Index
        yield return new WaitForSeconds(randomTime);
        colorChangeTime = Time.time;
        colorBox.GetComponent<Image>().color = colors[colorIndex];
        RespondButtonInteractable(true);
    }

    private void UpdateTrials()
    {
        moveCount++;
        trialsRemaining = totalTrials - moveCount;
        if (trialsRemaining < 0)
        {
            WriteResponseTimes();
            sceneFaderPanel.GetComponent<LoadNextScene>().FadeOutAndLoadNextScene("Stroop1");
        }
        else
        {
            trialsText.text = "Trials Remaining: " + trialsRemaining.ToString();
        }
    }

    private void RespondButtonInteractable(bool toggle)
    {
        respond.interactable = toggle;
    }

    private void ResetColorBox()
    {
        colorBox.GetComponent<Image>().color = defaultBoxColor;
    }

    private void CalculateResponseTime()
    {
        responseTime = respondClickTime - colorChangeTime;
        responseText.text = "Response Time: " + responseTime.ToString();
        allResponseTimes.Add(responseTime);
    }

    private void WriteResponseTimes()
    {
        string stroopLevel0Path = Path.Combine(Application.persistentDataPath, stroopLevel0FileName);
        using (StreamWriter stroopLevel0Writer = new StreamWriter(stroopLevel0Path))
        {
            for (int i = 0; i < allResponseTimes.Count; i++)
            {
                stroopLevel0Writer.WriteLine(allResponseTimes[i]);
            }
        }
    }

    public void OnStartClick()
    {
        levelInfoPanel.GetComponent<Animator>().SetTrigger("startLevelTrigger");
        StartCoroutine(ChangeColorOfBox());
    }

    public void OnRespondButtonClick()
    {
        respondClickTime = Time.time;
        CalculateResponseTime();
        StartCoroutine(ChangeColorOfBox());
    }
}
