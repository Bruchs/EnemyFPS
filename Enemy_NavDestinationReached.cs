﻿using UnityEngine;
using System.Collections;

namespace GameManager
{
	public class Enemy_NavDestinationReached : MonoBehaviour
    {
        private Enemy_Master enemyMaster;
        private NavMeshAgent myNavMeshAgent;
        private float checkRate;
        private float nextCheck;

		void OnEnable()
		{
            SetInitialReferences();
            enemyMaster.EventEnemyDie += DisableThis;
		}
		
		void OnDisable()
		{
            enemyMaster.EventEnemyDie -= DisableThis;
        }
	
		void Update () 
		{
            if (Time.time > nextCheck)
            {
                nextCheck = Time.time + checkRate;
                CheckIfDestinationReached();
            }
		}
		
		void SetInitialReferences()
		{
            enemyMaster = GetComponent<Enemy_Master>();

            if (GetComponent<NavMeshAgent>() != null)
            {
                myNavMeshAgent = GetComponent<NavMeshAgent>();
            }

            checkRate = Random.Range(0.3f, 0.4f);
		}

        void CheckIfDestinationReached()
        {
            if (enemyMaster.isOnRoute)
            {
                if (myNavMeshAgent.remainingDistance < myNavMeshAgent.stoppingDistance)
                {
                    enemyMaster.isOnRoute = false;
                    enemyMaster.CallEventEnemyReachedNavTarget();
                }
            }

        }

        void DisableThis()
        {
            if (myNavMeshAgent != null)
            {
                this.enabled = false;
            }
        }
	}
}

