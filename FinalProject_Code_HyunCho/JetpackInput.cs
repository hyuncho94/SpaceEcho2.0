using UnityEngine;
using Photon.Pun;
using Photon.Voice.Unity;

public class JetpackInput : JetpackInputBase
{
    public enum InputType { Trigger, Voice }
    [SerializeField] private InputType inputType = InputType.Trigger;

    [SerializeField] private OVRInput.Controller controller = OVRInput.Controller.RTouch;
    [SerializeField] private float maxVoiceVolume = 0.04f;

    private Recorder recorder;
    private MText.vomitting vomit;

    private void Start()
    {
        if (inputType == InputType.Voice)
        {
            recorder = FindObjectOfType<Recorder>();
            vomit = GetComponentInParent<MText.vomitting>();
        }

        ThrustInputChanged(0f, Vector3.forward); // Initialize
    }

    private void Update()
    {
        if (!PhotonNetwork.InRoom) return;
        ProcessInput();
    }

    public override void ProcessInput()
    {
        float newInput = inputType == InputType.Trigger
            ? GetThrustInput(controller)
            : GetThrustInput(recorder, vomit);

        if (Mathf.Abs(newInput - currentThrustInput) > 0.001f)
        {
            Quaternion rotation = OVRInput.GetLocalControllerRotation(controller);
            Vector3 direction = rotation * Vector3.forward;
            ThrustInputChanged(newInput, direction);
        }
    }

    // Overload for trigger input
    public float GetThrustInput(OVRInput.Controller controller)
    {
        float triggerInput = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller);
        return Mathf.Pow(triggerInput, 1.5f);
    }

    // Overload for voice input
    public float GetThrustInput(Recorder recorder, MText.vomitting vomit)
    {
        float voiceValue = recorder?.LevelMeter?.CurrentAvgAmp ?? 0f;
        float vomitBonus = (vomit != null && vomit.isVomit > 0) ? vomit.isVomit / 100f : 0f;
        return Mathf.InverseLerp(0.001f, maxVoiceVolume, voiceValue + vomitBonus);
    }
}
