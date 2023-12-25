using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
 public Movement movement;

 public GameObject camera;

 public string nickname;

 public TextMeshPro nicknameText;
public void IsLocalPlayer(){
    movement.enabled = true;
    camera.SetActive(true);
}

[PunRPC]
public void SetNickName(string _name) {
    nickname = _name;

    nicknameText.text = nickname;
}

}
