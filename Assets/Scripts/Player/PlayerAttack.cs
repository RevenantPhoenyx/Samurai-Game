using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Melee Settings")]
    [SerializeField] private float strikeDamage = 1f;
    [SerializeField] private float strikeRange = 0.5f;
    [SerializeField] private float flyKickDamage = 2f;
    [SerializeField] private float flyKickRange = 0.7f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Strike Attack")]
    [SerializeField] private float strikeCooldown = 0.3f;
    [SerializeField] private AudioClip strikeSound;
    private float strikeTimer = Mathf.Infinity;

    [Header("FlyKick Attack")]
    [SerializeField] private float flyKickCooldown = 1.2f;
    [SerializeField] private AudioClip flykickSound;
    private float flyKickTimer = Mathf.Infinity;

    [Header("Fireball")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    [SerializeField] private AudioClip fireballSound;
    [SerializeField] private float fireballCooldown = 0.8f;
    private float fireballTimer = Mathf.Infinity;

    private Animator anim;
    private PlayerMovement playerMovement;
   

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {

        strikeTimer += Time.deltaTime;
        fireballTimer += Time.deltaTime;
        flyKickTimer += Time.deltaTime;

        // Left click = Strike
        if (Input.GetMouseButtonDown(0) && strikeTimer > strikeCooldown && playerMovement.canAttack())
            StrikeAttack();

        // Right click = Fireball
        if (Input.GetKeyDown(KeyCode.Q) && fireballTimer > fireballCooldown && playerMovement.canAttack())
            FireballAttack();

        // Key F = FlyKick
        if (Input.GetMouseButtonDown(1) && flyKickTimer > flyKickCooldown && playerMovement.canAttack())
            FlyKickAttack();
    }

    private void StrikeAttack()
    {
        SoundManager.instance.PlaySound(strikeSound);
        anim.SetTrigger("strike");
        strikeTimer = 0;

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, strikeRange, enemyLayer);

        foreach (Collider2D enemy in hits)
        {
            enemy.GetComponent<Health>().TakeDamage(strikeDamage);
        }
    }

    private void FlyKickAttack()
    {
        SoundManager.instance.PlaySound(flykickSound);
        anim.SetTrigger("flykick");
        flyKickTimer = 0;

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, flyKickRange, enemyLayer);

        foreach (Collider2D enemy in hits)
        {
            enemy.GetComponent<Health>().TakeDamage(flyKickDamage);
        }
    }

    private void FireballAttack()
    {
        SoundManager.instance.PlaySound(fireballSound);
        anim.SetTrigger("fireball");
        fireballTimer = 0;

        int index = FindFireball();
        fireballs[index].transform.position = firePoint.position;
        fireballs[index].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, strikeRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, flyKickRange);
    }

}
