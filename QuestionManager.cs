using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class QuestionManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static QuestionManager Instance { get; private set; }

    public GameObject questionUIPanel;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI timerText;
    public List<string> questions;
    public List<AudioClip> questionAudioClips; // audio list for each question

    private int currentQuestionIndex = 0;
    private bool isQuestionActive = false;
    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); //If an instance already exists, destroy this instance
        }

        audioSource = gameObject.AddComponent<AudioSource>();  // add AudioSource component to the game object
    }

    void Start()
    {
        if (questionUIPanel == null || questionText == null || timerText == null || questions == null || questions.Count == 0)
        {
            Debug.LogError("[ERROR] 필수 요소가 설정되지 않았습니다!"); //Error ther is no required element
            return;
        }

        // 질문 개수와 오디오 개수가 다르면 경고 출력
        if (questionAudioClips.Count != questions.Count)
        {
            Debug.LogWarning("[WARN] 질문 개수와 오디오 클립 개수가 일치하지 않습니다. 일부 질문에는 오디오가 없을 수 있습니다."); //Warning the number of questions and the number of audio clips do not match. Some questions may not have audio.
        }
    }

    public void TriggerQuestion()
    {
        if (isQuestionActive) return; // if the question is already active, return

        Debug.Log($"[INFO] TriggerQuestion() 실행됨, MasterClient 여부: {PhotonNetwork.IsMasterClient}");   //TriggerQuestion() is executed, MasterClient status: {PhotonNetwork.IsMasterClient}

        if (PhotonNetwork.IsMasterClient) // if the current client is the MasterClient
        {
            photonView.RPC("ShowQuestionRPC", RpcTarget.All, currentQuestionIndex); //ShowQuestionRPC() is executed
        }
    }

    [PunRPC] // Sync the question index and show the question
    void ShowQuestionRPC(int questionIndex) // Show the question
    {
        if (questions == null || questions.Count == 0 || questionUIPanel == null || questionText == null || timerText == null) // if the question list is empty or the question UI is not set, return
        {
            Debug.LogError("[ERROR] ShowQuestionRPC 실행 중 오류 발생!"); //   Error occurred while executing ShowQuestionRPC
            return;
        }

        questionUIPanel.SetActive(true); // show the question UI
        questionText.text = questions[questionIndex]; // set the question text
        questionText.ForceMeshUpdate(); // update the text mesh

        isQuestionActive = true; // set the question active status to true

        PlayQuestionAudio(questionIndex); // play the audio for the question

        // 0번째 질문은 30초 후, 나머지는 120초 후에 사라지도록 설정 (Hide the question after 30 seconds for the first question, and 120 seconds for the rest)
        int displayTime = (questionIndex == 0) ? 30 : 120; // set the display time for the question
        StartCoroutine(HideQuestionAfterTime(displayTime)); // hide the question after the display time
    }

    private void PlayQuestionAudio(int questionIndex) // play the audio for the question
    {
        if (questionIndex < questionAudioClips.Count && questionAudioClips[questionIndex] != null) //   if the audio clip is set for the question, play the audio
        {
            audioSource.clip = questionAudioClips[questionIndex]; // set the audio clip
            audioSource.Play(); //  play the audio
        }
        else
        {
            Debug.LogWarning($"[WARN] 질문 {questionIndex}에 대한 오디오 파일이 설정되지 않았습니다."); //Warning the audio file for question {questionIndex} is not set
        }
    }

    IEnumerator HideQuestionAfterTime(int seconds) //   hide the question after the display time
    {
        while (seconds > 0) //   while the display time is not over, update the timer text
        {
            if (timerText != null) // if the timer text is set, update the timer text
            {
                timerText.text = $"Time: {seconds}"; // set the timer text
            }

            yield return new WaitForSeconds(1f); // wait for 1 second
            seconds--; // decrease the display time
        }

        questionUIPanel.SetActive(false); //        hide the question UI
        isQuestionActive = false; //    set the question active status to false

        if (PhotonNetwork.IsMasterClient) //        if the current client is the MasterClient
        {
            photonView.RPC("NextQuestionRPC", RpcTarget.All); //  NextQuestionRPC() is executed
        }
        {
            currentQuestionIndex = (currentQuestionIndex + 1) % questions.Count; //  increase the question index
            photonView.RPC("UpdateQuestionIndexRPC", RpcTarget.All, currentQuestionIndex); //       Update the question index
        }
    }

    [PunRPC] // Move to the next question
    void UpdateQuestionIndexRPC(int newIndex) //        Update the question index
    {
        currentQuestionIndex = newIndex; //     set the new question index
    }

    public override void OnMasterClientSwitched(Player newMasterClient) //    when the MasterClient is switched
    {
        Debug.Log($"[INFO] MasterClient 변경됨: 새로운 Master = {newMasterClient.NickName}"); //  MasterClient changed: New Master = {newMasterClient.NickName}

        if (PhotonNetwork.IsMasterClient) 
        {
            photonView.RPC("RequestPreviousData", RpcTarget.Others); // Request the previous question index
        }
    }

    [PunRPC]
    public void RequestPreviousData()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SendPreviousData", RpcTarget.MasterClient, currentQuestionIndex);// Send the previous question index
        }
    }

    [PunRPC]
    public void SendPreviousData(int previousIndex)
    {
        currentQuestionIndex = previousIndex; //    set the previous question index
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) //    sync the question index
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentQuestionIndex); //   send the current question index
        }
        else
        {
            currentQuestionIndex = (int)stream.ReceiveNext(); //    receive the current question index
        }
    }
}
