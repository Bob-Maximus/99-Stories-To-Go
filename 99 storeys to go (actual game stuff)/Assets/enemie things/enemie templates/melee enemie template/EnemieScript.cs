using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieScript : MonoBehaviour
{
    private Rigidbody rb;
    private CurrentHealth CurrentHealth;
    private Animator anim;

    private RaycastHit rayHit;
    private RaycastHit rayHitEnemy;

    public EnemyData enemyData;
    public Transform groundCheck;

    private bool facingRight;
    private float timePaused;
    private float waitSecondsRand;
    private float moveDir = 0;
    private string currentState;
    private bool attacking;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        CurrentHealth = GetComponent<CurrentHealth>();
        anim = GetComponent<Animator>();

        CurrentHealth.currentHealth = enemyData.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        IsGrounded();
        GeneralProcceses();
        SeesPlayer();
        CheckIfCanRun();
        CheckIfCanAttack();

        if (CurrentHealth.currentHealth <= 0)
        {
            Die();
        }
    }

    private void CheckIfCanRun()
    {
        if (IsGrounded() == true)
        {
            if (SeesPlayer())
            {
                float moveDir;
                Vector3 enemyLocationRelativeToSelf = transform.position - rayHitEnemy.point;

                if (enemyLocationRelativeToSelf.x < 0)
                {
                    moveDir = 1;
                } else
                {
                    moveDir = -1;
                }
                Move(moveDir);
            } else
            {
                Wander();
            }
        }
    }

    private void Move(float moveDir)
    {
        if (moveDir > 0)
        {
            transform.rotation = Quaternion.Euler(rb.rotation.x, 180f, rb.rotation.z);
            facingRight = false;
        }
        else if (moveDir < 0)
        {
            transform.rotation = Quaternion.Euler(rb.rotation.x, 0f, rb.rotation.z);
            facingRight = true;
        } else if (moveDir == 0 && !attacking)
        {
            PlayAnimation("idle");
        }
        rb.velocity = new Vector3(moveDir * enemyData.speed, rb.velocity.y, rayHit.point.z);
        PlayAnimation("walking");
    }

    private bool IsGrounded()
    {
        Physics.Raycast(transform.position, -transform.up, out rayHit);
        if (Vector3.Distance(rayHit.point, groundCheck.position) <= 0.1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool SeesPlayer()
    {
        for (int i = 0; i < enemyData.attackLayer.Count; i++)
        {
            if (facingRight == true)
            {
                Physics.Raycast(transform.position, Vector3.left, out rayHitEnemy, enemyData.sightrange, enemyData.attackLayer[i]);
            }
            else
            {
                Physics.Raycast(transform.position, Vector3.right, out rayHitEnemy, enemyData.sightrange, enemyData.attackLayer[i]);
            }

            if (rayHitEnemy.collider != null)
            {
                return true;
            } else
            {
                return false;
            }
        }
        return SeesPlayer();
    }

    private void CheckIfCanAttack()
    {
        if (Vector3.Distance(rayHitEnemy.point, transform.position) <= enemyData.attachrange && SeesPlayer())
        {
            attacking = true;
            Attack();
        } else
        {
            attacking = false;
        }
    }

    private void Attack()
    {
        PlayAnimation("use");
        Move(0);
        rayHitEnemy.rigidbody.AddForce(enemyData.knockBack * 50 * moveDir, 1, 0);
        if (rayHitEnemy.collider.gameObject.GetComponent<CurrentHealth>() != null)
        {
            rayHitEnemy.collider.gameObject.GetComponent<CurrentHealth>().currentHealth = rayHitEnemy.collider.gameObject.GetComponent<CurrentHealth>().currentHealth - enemyData.damage;
        }
    }

    private void Wander()
    {
        timePaused += Time.deltaTime;

        float randMove;
        float i = Random.Range(0, 3);
        if (i == 0)
        {
            randMove = -1;
        }
        else if (i == 1)
        {
            randMove = 1;
        }
        else
        {
            randMove = 0;
        }

        if (waitSecondsRand <= timePaused)
        {
            waitSecondsRand = Random.Range(0, 4);
            timePaused = 0;
            moveDir = randMove;
        }

        Move(moveDir / 2);
    }

    private void PlayAnimation(string newState)
    {
        if (currentState == newState)
        {
            return;
        }
        anim.Play(newState);
        currentState = newState;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void GeneralProcceses()
    {
        Mathf.Clamp(rb.velocity.x, enemyData.speed, 0);
        Mathf.Clamp(rb.velocity.y, enemyData.speed, 0);
        Mathf.Clamp(transform.rotation.z, 0, 0);
    }
}
