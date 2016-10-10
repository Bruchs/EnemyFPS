using UnityEngine;
using System.Collections;

namespace GameManager
{
	public class Enemy_Health : MonoBehaviour
    {
        private Enemy_Master enemyMaster;
        private Enemy_TriggerNavFlee enemyTriggerNavFlee;
        public int enemyHealth = 100;
        public int healthLow;
        public int maxHealth;

		void OnEnable()
		{
            SetInitialReferences();
            enemyMaster.EventEnemyDeductHealth += DeductHealth;
		}
		
		void OnDisable()
		{
            enemyMaster.EventEnemyDeductHealth -= DeductHealth;
		}
		
		void SetInitialReferences()
		{
            maxHealth = enemyHealth;
            enemyMaster = GetComponent<Enemy_Master>();
            enemyTriggerNavFlee = transform.root.GetComponent<Enemy_TriggerNavFlee>();
		}

        void DeductHealth(int healthChange)
        {
            enemyHealth -= healthChange;
            if (enemyHealth <= 0)
            {
                enemyHealth = 0;
                enemyMaster.CallEventEnemyDie();
                Destroy(gameObject, Random.Range(7f, 11f));
            }

            if (enemyTriggerNavFlee != null)
            {
                enemyTriggerNavFlee.CheckHealthFraction();
            } 
        }
	}
}

