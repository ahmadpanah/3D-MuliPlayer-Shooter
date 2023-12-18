using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;

    [Header("UI")]
    public TextMeshProUGUI healthText;
    
    [PunRPC]
    public void TakeDamage(int _damage) {
        health -= _damage;
        
        healthText.text = health.ToString();

        if (health <= 0) {
            RoomManager.instance.RespawnPlayer();
            
            Destroy(gameObject);
        }
    }
}
