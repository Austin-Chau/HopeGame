using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBehaviour : StateMachineBehaviour
{
    [SerializeField]
    [Tooltip("Amount of force sprung forward when attacking.")]
    private float AttackForce = 200;

    Rigidbody2D rb;
    Enemy enemy;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Enemy>();
        rb = animator.GetComponent<Rigidbody2D>();

        float dir = enemy.facingRight ? 1 : -1;
        rb.AddForce(new Vector2(dir * AttackForce, 0));
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }

}
