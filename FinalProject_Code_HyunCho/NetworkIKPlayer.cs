using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkIKPlayer : MonoBehaviourPunCallbacks
{
    [System.Serializable]
    public class RigTransforms
    {
        public Transform head;
        public Transform lHand;
        public Transform rHand;
    }

    public RigTransforms realVrRig;
    public RigTransforms outputRig;
    public GameObject speechRecognitionRoot;

    private void Awake()
    {
        // Activate speech recognition only for the local player
        speechRecognitionRoot.SetActive(this.photonView.IsMine);
    }

    void Start()
    {
        VRRig vrik = this.GetComponentInChildren<VRRig>();

        if (photonView.IsMine)
        {
            OVRCameraRig rig = FindObjectOfType<OVRCameraRig>();
            rig.transform.parent = this.transform;
            rig.transform.localPosition = Vector3.zero;
            rig.transform.localRotation = Quaternion.identity;

            realVrRig.head = rig.transform.Find("TrackingSpace/CenterEyeAnchor");
            realVrRig.lHand = rig.transform.Find("TrackingSpace/LeftHandAnchor");
            realVrRig.rHand = rig.transform.Find("TrackingSpace/RightHandAnchor");

            vrik.head.vrTarget = realVrRig.head;
            vrik.leftHand.vrTarget = realVrRig.lHand;
            vrik.rightHand.vrTarget = realVrRig.rHand;

            ThrustController thrust = this.GetComponent<ThrustController>();
            JetpackInputBase lInput = null;
            JetpackInputBase rInput = null;

            foreach (JetpackInputBase jpi in GameObject.FindObjectsOfType<JetpackInputBase>())
            {
                if (jpi.name.StartsWith("L"))
                {
                    lInput = jpi;
                }
                else
                {
                    rInput = jpi;
                }
            }
            thrust.SetUp(lInput, rInput, true);
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            // Synchronize VR rig transforms for head and hands
            outputRig.head.position = realVrRig.head.position;
            outputRig.head.rotation = realVrRig.head.rotation;

            outputRig.lHand.position = realVrRig.lHand.position;
            outputRig.lHand.rotation = realVrRig.lHand.rotation;

            outputRig.rHand.position = realVrRig.rHand.position;
            outputRig.rHand.rotation = realVrRig.rHand.rotation;
        }
    }
}
