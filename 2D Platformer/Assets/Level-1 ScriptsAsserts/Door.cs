using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private float openHeight = 5f; // How high the door moves up
    [SerializeField] private float openSpeed = 2f; // Speed of the door opening
    private Vector3 closedPosition; // Initial position of the door
    private Vector3 openPosition; // Target position for the door
    private bool isOpening = false; // Tracks if the door is opening
    private bool DoOnce = true;

    private void Start()
    {
        // Save the door's initial position
        closedPosition = transform.position;

        // Calculate the open position
        openPosition = closedPosition + new Vector3(0, openHeight, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isOpening = true; // Start opening the door
        }
    }

    private void Update()
    {
        if (isOpening && DoOnce)
        {
            // Smoothly move the door upwards
            transform.position = Vector3.Lerp(transform.position, openPosition, Time.deltaTime * openSpeed);

            // Stop opening once the door reaches its target
            if (Vector3.Distance(transform.position, openPosition) < 0.01f)
            {
                isOpening = false;
            }
        }
    }
}