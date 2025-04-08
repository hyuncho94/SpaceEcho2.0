using UnityEngine;
using Photon.Pun;

public class NetworkPlayer : MonoBehaviourPunCallbacks
{
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    public Transform headRig;
    public Transform leftHandRig;
    public Transform rightHandRig;

    void Update()
    {
        // Update rig transforms only if this is the local player
        if (!photonView.IsMine) return;

        headRig.position = head.position;
        headRig.rotation = head.rotation;

        leftHandRig.position = leftHand.position;
        leftHandRig.rotation = leftHand.rotation;

        rightHandRig.position = rightHand.position;
        rightHandRig.rotation = rightHand.rotation;
    }
}
