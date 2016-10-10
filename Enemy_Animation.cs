using UnityEngine;
using System.Collections;

namespace GameManager
{
	public class Enemy_Animation : MonoBehaviour
    {
        private Enemy_Master enemyMaster;
        private Enemy_Health enemyHealthScript;
        public Animator myAnimator;
        public bool shouldAttack;
        public bool shouldStruck = true;

        void OnEnable()
		{
            SetInitialReferences();
            enemyMaster.EventEnemyDie += DisableAnimator;
            enemyMaster.EventEnemyWalking += SetAnimationToWalk;
            enemyMaster.EventEnemyReachedNavTarget += SetAnimationToIdle;
            enemyMaster.EventEnemyAttack += SetAnimationToAttack;
            enemyMaster.EventEnemyDeductHealth += SetAnimationToStruck;
		}
		
		void OnDisable()
		{
            enemyMaster.EventEnemyDie -= DisableAnimator;
            enemyMaster.EventEnemyWalking -= SetAnimationToWalk;
            enemyMaster.EventEnemyReachedNavTarget -= SetAnimationToIdle;
            enemyMaster.EventEnemyAttack -= SetAnimationToAttack;
            enemyMaster.EventEnemyDeductHealth -= SetAnimationToStruck;
		}
		
		void SetInitialReferences()
		{
            enemyMaster = GetComponent<Enemy_Master>();
            enemyHealthScript = GetComponent<Enemy_Health>();
            shouldStruck = true;

            if (GetComponent<Animator>() != null)
            {
                myAnimator = GetComponent<Animator>();
            }
		}

        void SetAnimationToWalk()
        {
            if (myAnimator != null && myAnimator.enabled)
            {
                myAnimator.SetBool("isPursuing", true);
                enemyMaster.isNavPaused = false;
            }
        }

        void SetAnimationToIdle()
        {
            if (myAnimator != null && myAnimator.enabled)
            {
                myAnimator.SetBool("isPursuing", false);
                enemyMaster.isNavPaused = false;
            }
        }

        public void SetAnimationToAttack()
        {
            if (myAnimator != null && myAnimator.enabled)
            {
                myAnimator.SetTrigger("Attack");
                enemyMaster.isNavPaused = true;
                shouldAttack = true;
            }
            else
            {
                shouldAttack = false;
            }
        }

        public void SetAnimationToStruck(int dummy)
        {
            if (enemyHealthScript.enemyHealth <= enemyHealthScript.healthLow )
            {
                if (myAnimator != null && myAnimator.enabled && shouldStruck)
                {           
                    myAnimator.SetTrigger("Struck");
                    enemyMaster.isNavPaused = true;
                    shouldStruck = false;
                }
            }
        }

        void DisableAnimator()
        {
            if (myAnimator != null)
            {
                myAnimator.enabled = false;
            }
        }
    }
}

