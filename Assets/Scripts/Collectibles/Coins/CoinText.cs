using UnityEngine;
using TMPro;

public class CoinText : MonoBehaviour
{
    [SerializeField] private string textIntro;

    private TextMeshProUGUI txt;

    private void Awake()
    {
        txt = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        txt.text = textIntro + GameManager.instance.GetCoinCount().ToString();
    }
}
