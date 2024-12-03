using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    [SerializeField] private int fireDelay = 30;
    [SerializeField] private GameObject bulletGameObject;

    private PlayerDetection playerDetection;
    private bool bHasLOS;

    // Start is called before the first frame update
    void Start()
    {
        playerDetection = GetComponentInChildren<PlayerDetection>();
    }

    // Update is called once per frame
    void Update()
    {
        bHasLOS = playerDetection.GetLOSStatus();
    }

    private void FixedUpdate()
    {
        if (bHasLOS && Time.frameCount % fireDelay == 0)
        {
            FireRangedAttack();
        }
    }

    public void FireRangedAttack()
    {
        GameObject bullet = Instantiate(bulletGameObject, transform.position, Quaternion.identity);

    }
}
