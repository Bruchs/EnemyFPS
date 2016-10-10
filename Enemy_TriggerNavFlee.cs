using UnityEngine;
using System.Collections;

namespace GameManager
{
    public class Enemy_TriggerNavFlee : MonoBehaviour
    {
        private Enemy_Master enemyMaster;
        private Enemy_Health enemyHealthScript;

        void OnEnable()
        {
            SetInitialReferences();
            enemyMaster.EventEnemyIncreaseHealth += IncreaseHealth;
        }

        void OnDisable()
        {
            enemyMaster.EventEnemyIncreaseHealth -= IncreaseHealth;
        }

        void SetInitialReferences()
        {
            enemyMaster = transform.root.GetComponent<Enemy_Master>();
            enemyHealthScript = transform.root.GetComponent<Enemy_Health>();
        }

        public void CheckHealthFraction()
        {
            if (enemyHealthScript.enemyHealth <= enemyHealthScript.healthLow && enemyHealthScript.enemyHealth > 0)
            {
                enemyMaster.CallEventEnemyHealthLow();
            }

            else if (enemyHealthScript.enemyHealth > enemyHealthScript.healthLow)
            {
                enemyMaster.CallEventEnemyHealthRecovered();
            }
        }

        void IncreaseHealth(int healthChange)
        {
            enemyHealthScript.enemyHealth += healthChange;
            if (enemyHealthScript.enemyHealth > enemyHealthScript.maxHealth)
            {
                enemyHealthScript.enemyHealth = enemyHealthScript.maxHealth;
            }

            CheckHealthFraction();
        }
	}
}

