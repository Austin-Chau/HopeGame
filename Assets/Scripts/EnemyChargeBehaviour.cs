using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChargeBehaviour : StateMachineBehaviour
{
    [SerializeField]
    [Tooltip("Amount of time charged.")]
    private float chargeTime = .5f;

    private Enemy enemy;
    private float startTime;
    private float timeElapsed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Charge");
        enemy = animator.GetComponent<Enemy>();
        startTime = Time.time;
        enemy.isVulnerable = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(startTime + chargeTime < Time.time)
        {
            animator.SetTrigger("Attack");
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.isVulnerable = false;
    }

}
