using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice;
using Photon.Pun;

public class JetpackInput : MonoBehaviour
{
    //get the info from OVR Input
    //set the current thrust based on that
    //send out info as event to thrustcontroller

    public string levelTests1 = "";
    public string levelTests2 = "";
    public string levelTests3 = "";

    //[SerializeField]
    //private OVRInput.Controller m_controller = OVRInput.Controller.None;

    [SerializeField]
    private Transform engine, fan;

    [SerializeField]
    private float minFanRotaionSpeed, maxFanRotationSpeed;

    private float currentFanRotationSpeed;

    private float currentThrustInput;

    private Vector3 currentForce;

    public Vector3 CurrentForce { get => currentForce; }

    public GameObject other;

    Photon.Voice.Unity.Recorder recorder;
    public MText.vomitting vomitting2;


    [SerializeField]
    private float engineRotationSpeed;

    public event Action<Vector3> EvtThrustChanged = delegate { };
    public event Action<float> EvtThrustInputChanged = delegate { };
    float maxVoiceVolume = .04f;

    //needs to send out actual force, or at least transform and thrust.
    //maybe just use one here. and the thrust controller muliples it. Again Update force changes not the force.

    private void Start()
    {
        ThrustInputChanged(0f);
        recorder = FindObjectOfType<Photon.Voice.Unity.Recorder>();
    }

    private void Update()
    {
        if (vomitting2 == null)
        {
            vomitting2 = this.GetComponentInParent<MText.vomitting>();
            return;
        }

        //levelTests1 = "" + recorder.LevelMeter.AccumAvgPeakAmp.ToString("00.000");
        //levelTests2 = "" + recorder.LevelMeter.CurrentAvgAmp.ToString("00.000");
        //levelTests3 = "" + recorder.LevelMeter.CurrentPeakAmp.ToString("00.000");

        //Debug.Log(this.transform.parent.transform.parent.transform.parent.transform.parent.transform.Find("vomitting"));

        //GameObject vomitting2 = this.transform.parent.transform.parent.transform.parent.transform.parent.Find("vomitting").gameObject; // text version
       // float rawVoiceValue = vomitting2.GetComponent<MText.vomitting>().isVomit / 3.0f; //text version
        
        float rawVoiceValue = recorder.LevelMeter.CurrentAvgAmp / 1.0f; //volume version // 이변수가 속도 큰수로 나눌수록 느려짐 작을수록 빨라짐
        rawVoiceValue += vomitting2.isVomit / 100.0f; // isvomit = 0 or 1 // 글자 토해내고 있는 중이면 1, 그동안에는 속도 추가됨

        float voiceThrust = Mathf.InverseLerp(.001f, maxVoiceVolume, rawVoiceValue);

        /*
         min, max, val

        restrict to 0-1
        (val - min)
        /
        (max - min)

         */



        float newInput = 0;
        //newInput += OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, m_controller);
        newInput += voiceThrust;

        if(newInput != currentThrustInput)
        {
            ThrustInputChanged(newInput);
        }

        //if(OVRInput.Get(OVRInput.Button.One, m_controller))
        //{
        //    engine.Rotate(new Vector3(engineRotationSpeed * Time.deltaTime, 0f, 0f));

        //}

        //else if(OVRInput.Get(OVRInput.Button.Two, m_controller))
        //{
        //    engine.Rotate(new Vector3(-engineRotationSpeed * Time.deltaTime, 0f, 0f));
        //    fan.Rotate(new Vector3(0f, 0f, currentFanRotationSpeed * Time.deltaTime));
           
        //}
    }

    public void ThrustInputChanged(float newInput)
    {
        currentThrustInput = newInput;
        currentForce = newInput * engine.forward *-1;

       
        currentFanRotationSpeed = newInput * maxFanRotationSpeed + (1f - newInput) * minFanRotaionSpeed;

        EvtThrustInputChanged(newInput);
        EvtThrustChanged(currentForce);
    }

   
}
