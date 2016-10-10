using UnityEngine;
using System.Collections;

namespace GameManager
{
	public class Enemy_NavWander : MonoBehaviour
    {
        private Enemy_Master enemyMaster;
        private NavMeshAgent myNavMeshAgent;
        private float checkRate;
        private float nextCheck;
        private Transform myTransform;
        private float wanderRange = 10f;
        private NavMeshHit navHit;
        private Vector3 wanderTarget;

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
                CheckWanderState();
            }
		}
		
		void SetInitialReferences()
		{
            enemyMaster = GetComponent<Enemy_Master>();
            myTransform = transform;

            if (GetComponent<NavMeshAgent>() != null)
            {
                myNavMeshAgent = GetComponent<NavMeshAgent>();
            }
            checkRate = Random.Range(0.2f, 0.4f);
        }

        void CheckWanderState()
        {
            if (enemyMaster.myTarget == null && !enemyMaster.isOnRoute && !enemyMaster.isNavPaused )
            {
                if (RandomWanderTarget(myTransform.position, wanderRange, out wanderTarget))
                {
                    myNavMeshAgent.SetDestination(wanderTarget);
                    enemyMaster.isOnRoute = true;
                    enemyMaster.CallEventEnemyWalking();
                }
            }
        }

        bool RandomWanderTarget(Vector3 centre, float range, out Vector3 result)
        {
            Vector3 randomPoint = centre + Random.insideUnitSphere * wanderRange;
            if (NavMesh.SamplePosition(randomPoint, out navHit, 1.0f, NavMesh.AllAreas))
            {
                result = navHit.position;
                return true;
            }
            else
            {
                result = centre;
                return false;
            }
        }

        void DisableThis()
        {
            this.enabled = false;
        }
	}
}

