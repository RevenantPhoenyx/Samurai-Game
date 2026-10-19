using UnityEngine;

public class Stars : MonoBehaviour
{
    [SerializeField] private float blinkSpeed = 2f;   // How fast the star fades
    [SerializeField] private float minAlpha = 0.2f;   // How dim it gets
    [SerializeField] private float maxAlpha = 1f;     // How bright it gets

    private SpriteRenderer sr;
    private float t;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        t = Random.Range(0f, 10f); // random offset so stars don't blink together
    }

    private void Update()
    {
        t += Time.deltaTime * blinkSpeed;
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(t) + 1f) * 0.5f);

        Color c = sr.color;
        c.a = alpha;
        sr.color = c;
    }
}
