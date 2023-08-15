using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquiptableItem : MonoBehaviour
{
    public AudioSource audioSource;
    Camera cam;

    [Header("Attacking")]
    public float attackDistance = 3f;
    public float attackDelay = .4f;
    public float attackSpeed = 1f;
    public float attackDamage = 1f;
    public LayerMask attackLayer;

    public GameObject hitEffect;
    public AudioClip swing;
    public AudioClip hitSound;

    bool attacking = false;
    bool readyToAttack = true;
    int attackCount;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //on click use item
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        // SetAnimations();
        
    }

    void Attack()
    {
        if(!readyToAttack || attacking) return;

        readyToAttack = false;
        attacking = true;

        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRaycast), attackDelay);

        audioSource.pitch = Random.Range(.9f, 1.1f);
        audioSource.PlayOneShot(swing);
    }

    void ResetAttack() {
        attacking = false;
        readyToAttack = true;
    }

    void AttackRaycast() {
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, attackDistance, attackLayer)) {
            HitTarget(hit.point);
        }
    }

    void HitTarget(Vector3 pos) {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(hitSound);

        GameObject GO = Instantiate(hitEffect, pos, Quaternion.identity);
        Destroy(GO, 20);
    }
}
