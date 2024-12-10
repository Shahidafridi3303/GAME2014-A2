using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private float openHeight = 5f; 
    [SerializeField] private float openSpeed = 2f; 
    private Vector3 closedPosition; 
    private Vector3 openPosition; 
    private bool isOpening = false; 
    private bool DoOnce = true;

    private void Start()
    {
        closedPosition = transform.position;

        // Calculate the open position
        openPosition = closedPosition + new Vector3(0, openHeight, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isOpening = true; 
        }
    }

    private void Update()
    {
        if (isOpening && DoOnce)
        {
            transform.position = Vector3.Lerp(transform.position, openPosition, Time.deltaTime * openSpeed);

            if (Vector3.Distance(transform.position, openPosition) < 0.01f)
            {
                isOpening = false;
            }
        }
    }
}