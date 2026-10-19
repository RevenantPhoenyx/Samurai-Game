using UnityEngine;

public class Spikehead : Enemy_Damage
{
    [Header ("SpikeHead Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float checkDelay;
    [SerializeField] private LayerMask playerLayer;

    [Header("SFX")]
    [SerializeField] private AudioClip impactSound;

    private float checkTimer;
    private Vector3 destination;
    private bool attacking;

    private Vector3[] directions = new Vector3[4];

    private void OnEnable()
    {
        Stop();
    }

    private void Update()
    {
        //Move spikehead to destination only if attacking
        if(attacking)
        transform.Translate(destination * Time.deltaTime * speed);
        else
        {
            checkTimer += Time.deltaTime;
            if (checkTimer > checkDelay)
                CheckForPlayer();
        }
    }

    private void CheckForPlayer()
    {
        CalculateDirections();

        //Check if spikehead sees player in all 4 directions
        for(int i = 0; i < directions.Length; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);

            if(hit.collider != null && !attacking)
            {
                attacking = true;
                destination = directions[i];
                checkTimer = 0;
            }
        }
    }

    private void CalculateDirections()
    {
        directions[2] = transform.up * range; //up direction
        directions[3] = -transform.up * range; //down direction
    }

    private void Stop()
    {
        destination = transform.position; //Set destination as current position so that it doesnt move
        attacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.instance.PlaySound(impactSound);
        base.OnTriggerEnter2D(collision);
        Stop(); //Stop spikehead once hits something

    }
}
