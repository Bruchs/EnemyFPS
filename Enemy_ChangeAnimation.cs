using UnityEngine;
using System.Collections;

namespace GameManager
{
	public class Enemy_ChangeAnimation : MonoBehaviour
    {
        private Enemy_Health enemyHealthScript;
        private Enemy_Master enemyMaster;
        private NavMeshAgent myNavMeshAgent;
        private Animator myAnimator;
        private bool struckAnimation;
        public RuntimeAnimatorController newController;

        void OnEnable()
		{
            SetInitialReferences();
            enemyMaster.EventEnemyChangeAnimation += CheckForAnimationChange;
            enemyMaster.EventEnemyDie += DisableThis;
		}

        void OnDisable()
        {
            enemyMaster.EventEnemyChangeAnimation -= CheckForAnimationChange;
            enemyMaster.EventEnemyDie -= DisableThis;
        }
	
		void Update () 
		{
            CheckForAnimationChange();
		}
		
		void SetInitialReferences()
		{
            enemyHealthScript = transform.root.GetComponent<Enemy_Health>();
            myNavMeshAgent = transform.root.GetComponent<NavMeshAgent>();
            enemyMaster = transform.root.GetComponent<Enemy_Master>();
            myAnimator = GetComponent<Animator>();
        }

        void CheckForAnimationChange()
        {
            if (enemyHealthScript.enemyHealth < enemyHealthScript.maxHealth && enemyHealthScript.enemyHealth > 0)
            {
                ChangeAnimation();
            }
        }

        void ChangeAnimation()
        {
            if (myAnimator != null)
            {
                myAnimator.runtimeAnimatorController = newController;
                enemyMaster.isOnRoute = true;
                myNavMeshAgent.speed = 5.5f;
            }
        }

        void DisableThis()
        {
            if (myAnimator != null)
            {
                myAnimator.enabled = false;
            }
            this.enabled = false;
        }
	}
}

