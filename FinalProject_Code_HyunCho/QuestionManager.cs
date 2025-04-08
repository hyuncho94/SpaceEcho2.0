using System.Collections;
using UnityEngine;
using Photon.Pun;

public class QuestionManager : MonoBehaviourPunCallbacks
{
    public static QuestionManager Instance;

    public string[] questions;
    private int currentQuestionIndex = 0;
    public UIManager uiManager;

    void Awake()
    {
        Instance = this;
    }

    [PunRPC]
    public void TriggerQuestion()
    {
        if (currentQuestionIndex < questions.Length)
        {
            string question = questions[currentQuestionIndex];
            uiManager.ShowQuestionPopup(question);
            currentQuestionIndex++;
        }
    }
}
