using TMPro;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class RegnDetailsController : MonoBehaviour
{
    public TMP_InputField nameFieldInput, emailFieldInput, ageFieldInput;
    public TextMeshProUGUI nameText, error;
    public Button submit;
    public TMP_Dropdown genderDropdown;
    public GameObject sceneFaderPanel;

    private string userDetailsFileName = "UserDetails.txt";

    private bool CheckDetails()
    {
        ShowErrorMessage(false);
        bool nameEmpty = string.IsNullOrEmpty(nameFieldInput.text);
        bool emailEmpty = string.IsNullOrEmpty(emailFieldInput.text);
        bool ageEmpty = string.IsNullOrEmpty(ageFieldInput.text);
        bool genderEmpty = (genderDropdown.value == 0);
        bool anyEmpty = (nameEmpty || emailEmpty || ageEmpty || genderEmpty);

        if (nameEmpty)
        {
            error.text = "ERROR: The Name Input Field is empty";
        }
        if (emailEmpty)
        {
            error.text = "ERROR: The E-Mail Input Field is empty";
        }
        if (ageEmpty)
        {
            error.text = "ERROR: The Age Input Field is empty";
        }
        if (genderEmpty)
        {
            error.text = "ERROR: The Gender Input Field is empty";
        }
        if (anyEmpty)
        {
            ShowErrorMessage(true);
        }
        return anyEmpty;
    }

    private void SaveUserDetails()
    {
        string userDetailsPath = Path.Combine(Application.persistentDataPath, userDetailsFileName);

        using (StreamWriter userDetailsWriter = new StreamWriter(userDetailsPath))
        {
            userDetailsWriter.WriteLine(nameFieldInput.text);
            userDetailsWriter.WriteLine(emailFieldInput.text);
            userDetailsWriter.WriteLine(ageFieldInput.text);
            userDetailsWriter.WriteLine(genderDropdown.captionText.text);
        }
    }

    private void ShowErrorMessage(bool toggle)
    {
        error.gameObject.SetActive(toggle);
    }

    public void OnGenderChoose()
    {
        genderDropdown.captionText.font = nameText.font;
        genderDropdown.captionText.fontStyle = nameText.fontStyle;
        genderDropdown.captionText.color = nameText.color;
    }

    public void OnSubmitClick()
    {
        if (!CheckDetails())
        {
            sceneFaderPanel.GetComponent<LoadNextScene>().FadeOutAndLoadNextScene("ChooseTest");
            SaveUserDetails();
        }
    }
}
