using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public AudioSource audioSource;
    public Camera cam;
    public Animator animator;

    [Header("Attacking")]
    public float attackDistance = 3f;
    public float attackDelay = .4f;
    public float attackSpeed = 1f;
    public float attackDamage = 1f;
    public LayerMask attackLayer;

    public GameObject hitEffect;
    AudioClip swing;
    AudioClip hitSound;

    bool attacking = false;
    bool readyToAttack = true;
    int attackCount;

    EquiptItem equiptItemHandler;
    public ItemContainer playerInventory;

    // Start is called before the first frame update
    void Start()
    {
        equiptItemHandler = GetComponent<EquiptItem>();

        Debug.Log("EquiptableItem Start");
    }

    // Update is called once per frame
    void Update()
    {
        //on click use item
        if (Input.GetMouseButtonDown(0))
        {
            if (equiptItemHandler.equiptedItem != null)
            {
                swing = equiptItemHandler.equiptedItem.swingSound;
                hitSound = equiptItemHandler.equiptedItem.hitSound;
            }
            Debug.Log("Attacking");
            ExecuteAttack();
        }

        SetAnimations();

    }

    void ExecuteAttack()
    {
        if (!readyToAttack || attacking) return;

        readyToAttack = false;
        attacking = true;

        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRaycast), attackDelay);

        audioSource.pitch = Random.Range(.9f, 1.1f);
        audioSource.PlayOneShot(swing);

        if (attackCount == 0)
        {
            ChangeAnimationState(ATTACK1);
            attackCount++;
        }
        else
        {
            ChangeAnimationState(ATTACK1);
            attackCount = 0;
        }
    }

    void ResetAttack()
    {
        attacking = false;
        readyToAttack = true;
    }

    void AttackRaycast()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, attackDistance, attackLayer))
        {
            HitTarget(hit.point);
            Debug.Log(hit.collider.tag);

            switch (hit.collider.tag)
            {
                case "Harvestable":
                    Debug.Log("Harvesting Harvestable");
                    Harvestable harvestable = hit.collider.GetComponent<Harvestable>();
                    
                    ItemSlot itemToAdd = new ItemSlot(harvestable.yieldItem.itemName, harvestable.yieldAmount, -1);

                    playerInventory.AddItem(itemToAdd);

                    break;
            }
        }
    }

    void HitTarget(Vector3 pos)
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(hitSound);

        // GameObject GO = Instantiate(hitEffect, pos, Quaternion.identity);
        // Destroy(GO, 20);
    }

    //Animation

    public const string IDLE = "Idle";
    public const string ATTACK1 = "Attack 1";
    public const string ATTACK2 = "Attack 2";

    string currentAnimationState;

    void ChangeAnimationState(string newState)
    {
        if (currentAnimationState == newState) return;

        currentAnimationState = newState;
        animator.CrossFadeInFixedTime(currentAnimationState, 0.02f);
    }

    void SetAnimations()
    {
        if (!attacking)
        {
            ChangeAnimationState(IDLE);
        }
    }

}
