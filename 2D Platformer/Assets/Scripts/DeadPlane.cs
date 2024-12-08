using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeadPlane : MonoBehaviour
{
    public Vector3 _spawnPosition = new Vector3(-119, -342, 0);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.position = _spawnPosition;
        }
    }

    public void UpdateSpawnPosition(Vector3 checkpoint)
    {
        _spawnPosition = checkpoint;
    }
}
