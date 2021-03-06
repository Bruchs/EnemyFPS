﻿using UnityEngine;
using System.Collections;

namespace GameManager
{
	public class Enemy_Attack : MonoBehaviour
    {
        private Enemy_Master enemyMaster;
        private Enemy_NavPaused enemyNavPaused;
        private Transform attackTarget;
        private Transform myTransform;
        public float attackRate = 1f;
        private float nextAttack;
        public float attackRange = 2f;
        public int attackDamage = 10;

		void OnEnable()
		{
            SetInitialReferences();
            enemyMaster.EventEnemyDie += DisableThis;
            enemyMaster.EventEnemySetNavTarget += SetAttackTarget;
		}
		
		void OnDisable()
		{
            enemyMaster.EventEnemyDie -= DisableThis;
            enemyMaster.EventEnemySetNavTarget -= SetAttackTarget;
		}

        void Update()
        {
            TryToAttack();
        }
		
		void SetInitialReferences()
		{
            enemyMaster = GetComponent<Enemy_Master>();
            enemyNavPaused = GetComponent<Enemy_NavPaused>();
            myTransform = transform;
		}

        void SetAttackTarget(Transform targetTransform)
        {
            attackTarget = targetTransform;
        }

        void TryToAttack()
        {
            if (attackTarget != null)
            {
                if (Time.time > nextAttack)
                {
                    nextAttack = Time.time + attackRate;
                    if (Vector3.Distance(myTransform.position, attackTarget.position) <= attackRange)
                    {
                        Vector3 lookAtPlayer = new Vector3(attackTarget.position.x, myTransform.position.y, attackTarget.position.z);
                        enemyNavPaused.PauseNavMeshAgent(1);
                        myTransform.LookAt(lookAtPlayer);
                        enemyMaster.CallEventEnemyAttack();
                        enemyMaster.isOnRoute = false;
                    }
                }
            }
        }

        public void OnEnemyAttack()
        {
            if (attackTarget != null)
            {
                if (Vector3.Distance(myTransform.position, attackTarget.position) <= attackRange && attackTarget.GetComponent<Player_Master>() != null)
                {
                    Vector3 toOther = attackTarget.position - myTransform.position; Debug.Log(Vector3.Dot(toOther, myTransform.forward).ToString());
                    if (Vector3.Dot(toOther, myTransform.forward) > 0.5f)
                    {
                        attackTarget.GetComponent<Player_Master>().CallEventPlayerHealthDeduction(attackDamage);
                    }
                }
            }
        }

        void DisableThis()
        {
            this.enabled = false;
        }
	}
}

