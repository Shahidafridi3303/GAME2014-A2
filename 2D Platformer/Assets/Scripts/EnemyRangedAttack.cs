using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    [SerializeField] private int fireDelay = 60;
    [SerializeField] private GameObject bulletGameObject;

    private PlayerDetection playerDetection;
    private bool bHasLOS;
    private bool bIsInSensingRange;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        playerDetection = GetComponentInChildren<PlayerDetection>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bHasLOS = playerDetection.GetLOSStatus();
        bIsInSensingRange = playerDetection.GetSensingStatus();
    }

    private void FixedUpdate()
    {
        if (bHasLOS && bIsInSensingRange && Time.frameCount % fireDelay == 0)
        {
            FireRangedAttack();
        }
    }

    public void FireRangedAttack()
    {
        GameObject bullet = Instantiate(bulletGameObject, transform.position, Quaternion.identity);
        animator.SetInteger("State", (int)AnimationStates.ATTACK);
    }
}
