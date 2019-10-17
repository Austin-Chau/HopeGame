using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkBehaviour : StateMachineBehaviour
{
    [SerializeField]
    [Tooltip("How far the enemy can see.")]
    private float EnemySearchDistance = 5f;

    [SerializeField]
    [Tooltip("Speed of enemy as it patrols")]
    private float speed = 5;

    [SerializeField]
    [Tooltip("Chance that enemy will attack every seconds.")]
    private float attackChance = .7f;

    float currentTime = 0;
    Enemy enemy;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Enemy>();
        enemy.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int moveDir;
        if (enemy.facingRight) moveDir = 1;
        else moveDir = -1;
        enemy.transform.Translate(moveDir * new Vector2(speed, 0) * Time.deltaTime);
        if (currentTime + 1f < Time.time)
        {
            if (CheckForPlayer() && Random.value < attackChance)
            {
                animator.ResetTrigger("Attacked");
                animator.SetTrigger("Charge");
            }
            else
            {
                animator.ResetTrigger("Charge");
            }
            currentTime = Time.time;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Charge");
    }

    /// <summary>
    /// Checks if player is in front of enemy within a certain range.
    /// </summary>
    private bool CheckForPlayer()
    {
        Vector2 fireDirection;
        if (enemy.facingRight) fireDirection = Vector2.right;
        else fireDirection = Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, fireDirection, EnemySearchDistance, LayerMask.GetMask("Player"));

        if (hit.collider != null) return true;
        else return false;
    }
}
