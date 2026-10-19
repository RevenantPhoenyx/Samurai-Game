using UnityEngine;

public class CoinCollectible : MonoBehaviour
{
    [Header("Coin Value")]
    [SerializeField] private float coinValue = 1f;

    [Header("Coin Sound")]
    [SerializeField] private AudioClip coinGrabSound;

    [Header("Coin Rotation")]
    [SerializeField] private float rotationSpeed = 180f;

    private void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.instance.PlaySound(coinGrabSound);
            collision.GetComponent<Coin>().AddCoins(coinValue);
            gameObject.SetActive(false);
        }
    }
}