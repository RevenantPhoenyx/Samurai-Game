using UnityEngine;

public class Clouds : MonoBehaviour
{
    public float speed = 2f;
    private float destroyX = 88f;     // adjust if clouds disappear too early

    private void Update()
    {
        transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        if (transform.position.x > 120f)  // adjust to just past your right screen edge
            Destroy(gameObject);
    }
}