using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject questionPopup;
    public TMP_Text questionText;

    public void ShowQuestionPopup(string question)
    {
        questionPopup.SetActive(true);
        questionText.text = question;
    }

    public void HideQuestionPopup()
    {
        questionPopup.SetActive(false);
    }
}
