using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f;

    private int value = 5;

    public void SetValue(int coinValue)
    {
        value = coinValue;
    }

    public int GetValue()
    {
        return value;
    }

    void Update()
    {
        // Rotate the coin around its Z-axis
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Add coin value to the GameManager
            GameManager.Instance.AddCoins(value);

            // Destroy the coin GameObject
            Destroy(gameObject);
        }
    }
}