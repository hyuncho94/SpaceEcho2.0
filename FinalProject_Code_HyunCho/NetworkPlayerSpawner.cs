using UnityEngine;
using Photon.Pun;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    public override void OnJoinedRoom()
    {
        // Spawn the player at origin with default rotation when joined room
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
    }

    public override void OnLeftRoom()
    {
        // Destroy the player when leaving the room
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
    }
}
