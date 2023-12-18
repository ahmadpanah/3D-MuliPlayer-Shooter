using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;
    public GameObject player;

    [Space]
    public Transform  spawnPoint;

    [Space]
    public GameObject roomCam;
    // Start is called before the first frame update
    
    void Awake() {
        instance = this;
    }
    void Start()
    {
        Debug.Log("Connecting...");

        PhotonNetwork.ConnectUsingSettings();
    }


    public override void OnConnectedToMaster() {
        base.OnConnectedToMaster();

        Debug.Log("Connected To Server!");

        PhotonNetwork.JoinLobby();

    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        PhotonNetwork.JoinOrCreateRoom("test", null , null);

        Debug.Log("We're Connected and in a room now!");

        Invoke("PhotonInit", 4);
    }

    void PhotonInit() {

    roomCam.SetActive(false);
       GameObject _player = PhotonNetwork.Instantiate (player.name, spawnPoint.position,Quaternion.identity);

       _player.GetComponent<PlayerSetup>().IsLocalPlayer();

    }

    public void RespawnPlayer() {
        GameObject _player = PhotonNetwork.Instantiate (player.name, spawnPoint.position,Quaternion.identity);

        _player.GetComponent<PlayerSetup>().IsLocalPlayer();

    }
}
