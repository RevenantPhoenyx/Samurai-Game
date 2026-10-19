using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Behaviour[] components;

    public void AddCoins(float coinValue)
    {
        GameManager.instance.AddCoin();
    }
}