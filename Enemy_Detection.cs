using UnityEngine;
using System.Collections;

namespace GameManager
{
	public class Enemy_Detection : MonoBehaviour
    {
        private Enemy_Master enemyMaster;
        private Transform myTransform;
        public float detectRadius = 25f;
        private RaycastHit hit;

        public Transform head;
        public LayerMask playerLayer;
        public LayerMask sightLayer;


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
            CarryOutDetection();
		}
		
		void SetInitialReferences()
		{
            enemyMaster = GetComponent<Enemy_Master>();
            myTransform = transform;
        
            if (head == null)
            {
                head = myTransform;
            }
		}

        void CarryOutDetection()
        {

            Collider[] colliders = Physics.OverlapSphere(myTransform.position, detectRadius, playerLayer);

            if (colliders.Length > 0)
            {
                foreach (Collider potentialTarget in colliders)
                {
                    if (potentialTarget.CompareTag("Player"))
                    {
                        if (CanPotentialTargetBeSeen(potentialTarget.transform))
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                enemyMaster.CallEventEnemyLostTarget();
            }              
        }

        bool CanPotentialTargetBeSeen(Transform potentialTarget)
        {
            if (Physics.Linecast(head.position, potentialTarget.position, out hit, sightLayer))
            {
                if (hit.transform == potentialTarget)
                {
                    enemyMaster.CallEventEnemySetNavTarget(potentialTarget);
                    return true;
                }
                else
                {
                    enemyMaster.CallEventEnemyLostTarget();
                    return false;
                }
            }
            else
            {
                enemyMaster.CallEventEnemyLostTarget();
                return false;
            }
        }

        void DisableThis()
        {
            this.enabled = false;
        }
	}
}

