using UnityEngine;

public class SpikesDown : MonoBehaviour
{
    [SerializeField] private float upTime = 5f;
    [SerializeField] private float downTime = 5f;

    private Animator anim;
    private float timer;
    private bool spikesUp;


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (!spikesUp && timer >= downTime)
        {
            //Currently down - check if time to go up
            if (timer >= downTime)
            {
                anim.SetTrigger("up");
                spikesUp = true;
                timer = 0f;
            }
        }
        else if (spikesUp && timer >= upTime)
        {
            //currently up - check if time to go down
            if (timer >= upTime)
            {
                anim.SetTrigger("down");
                spikesUp = false;
                timer = 0f;
            }
        }
    }

    public bool IsDangerous()
    {
        return spikesUp;
    }
}
