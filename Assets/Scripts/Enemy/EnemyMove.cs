using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    public GameObject gun;
    public bool isReplay = false;

    private EnemyLife _EnemyLife;
    private Animator animator;
    private NavMeshAgent agent;
    private GameObject player;

    private bool reloadGun = true;

    private void Awake()
    {
        _EnemyLife = gameObject.GetComponent<EnemyLife>();
        animator = gameObject.GetComponent<Animator>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        player = PlayerState.GetInstance().Player;
    }

    private void Start()
    {
        MoveToPlayer();
    }

    private void Update()
    {
        if (!CheckIsPlayerAimable() && !_EnemyLife.isDead)
        {
            if (isReplay) return;

            MoveToPlayer();
        }
        else if(!_EnemyLife.isDead)
        {
            StopMove();
            Attack();
        }
    }

    private void MoveToPlayer()
    {
        agent.SetDestination(player.transform.position);
        animator.SetBool("Shot", false);
        animator.SetBool("AimWalk", true);
    }

    private void StopMove()
    {
        agent.SetDestination(gameObject.transform.position);
        gameObject.transform.LookAt(player.transform.position);
        animator.SetBool("Shot", true);
    }

    private bool CheckIsPlayerAimable()
    {
        Vector3 handPos = gameObject.transform.position + Vector3.up;
        Vector3 toPos = player.transform.position + Vector3.up;

        Ray ray = new Ray(handPos, (toPos - handPos).normalized);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f) && hit.transform.CompareTag("Player"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Attack()
    {
        if (gun != null && reloadGun)
        {
            gun.GetComponent<GunForEnemy>().ShotGun();
            reloadGun = false;
            StartCoroutine(nameof(GunReloading));
        }
        else if (gun == null)
        {
            animator.SetBool("Hit1", true);
        }
    }

    IEnumerator GunReloading()
    {
        float timer = 0;
        float reloadDelay = 2f;

        while (timer <= reloadDelay)
        {
            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        reloadGun = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("MainCamera"))
        {
            agent.SetDestination(gameObject.transform.position);
        }
    }
}
