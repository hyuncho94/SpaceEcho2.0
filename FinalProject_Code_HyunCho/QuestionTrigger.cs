using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class QuestionTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Send question trigger RPC to all clients
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("TriggerQuestion", RpcTarget.All);
        }
    }
}
