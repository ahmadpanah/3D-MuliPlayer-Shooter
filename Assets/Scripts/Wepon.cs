using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class Wepon : MonoBehaviour
{
    public int damage;

    public float fireRate;

    public Camera camera;

    private float nextFire;

    [Header("Ammo")]
    public int mag = 5;
    public int ammo = 30;
    public int magAmmo = 30;

    [Header("VFX")]
    public GameObject hitVFX;

    [Header("UI")]
    public TextMeshProUGUI magText;
    public TextMeshProUGUI ammoText;

    [Header("Animation")]
    public Animation animation;
    public AnimationClip reload;

    [Header("Recoil Settings")]
    // [Range(0,1)]
    // public float recoilPercant = 0.3f;

    [Range(0,2)]
    public float recoverPercent = 0.7f;

    [Space]
    public float recoilUp = 0.05f;
    public float recoilBack = 0.05f;


    private Vector3 originalPosition;
    private Vector3 recoilVelocity = Vector3.zero;

    private float recoilLenght;
    private float recoverLenght;
    private bool recoiling;
    private bool recovering;



    void Start() {
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;
        originalPosition = transform.localPosition;

        recoilLenght =  0;
        recoverLenght =  1 / fireRate * recoverPercent;
        
    }
    // Update is called once per frame
    void Update()
    {
        if (nextFire > 0) {
            nextFire -= Time.deltaTime;
        }

        if (Input.GetButton("Fire1") && nextFire <=0 && ammo > 0 && animation.isPlaying == false) {
            nextFire = 1 / fireRate;

            ammo--;

        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;

            Fire();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            Reload();
        }

        if (recoiling) {
            Recoil();
        }

        if (recovering) {
            Recovering();
        }
    }

    void Reload(){

        animation.Play(reload.name);

        if (mag > 0) {
            mag--;

            ammo = magAmmo;
        }

        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;
    }

    void Fire() {

        recoiling = true;
        recovering = false;

        Ray ray = new Ray(camera.transform.position,camera.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f)) {
            PhotonNetwork.Instantiate(hitVFX.name,hit.point,Quaternion.identity);
            if (hit.transform.gameObject.GetComponent<Health>()) {
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All,damage);
            }
        }
    }

    void Recoil() {
        Vector3 finalPosition = new Vector3(originalPosition.x,originalPosition.y+recoilUp,originalPosition.z-recoilBack);
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition,finalPosition,ref recoilVelocity,recoilLenght);
        if (transform.localPosition == finalPosition) {
            recoiling = false;
            recovering = true;
        }
    }

        void Recovering() {
        Vector3 finalPosition = originalPosition;
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition,finalPosition,ref recoilVelocity,recoverLenght);
        if (transform.localPosition == finalPosition) {
            recoiling = false;
            recovering = false;
        }
    }
}
