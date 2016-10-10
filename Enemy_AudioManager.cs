using UnityEngine;
using System.Collections;

namespace GameManager
{
	public class Enemy_AudioManager : MonoBehaviour
    {
        private Enemy_Master enemyMaster;
        private Enemy_Animation enemyAnimationScript;
        private Enemy_Health enemyHealthScript;
        private AudioSource myAudioSource;
        public AudioClip idleAudio;
        public AudioClip walkAudio;
        public AudioClip struckAudio;
        public AudioClip attackAudio;
        public AudioClip dieAudio;

        void OnEnable()
		{
            SetInitialReferences();
            enemyMaster.EventEnemyDie += DisableThis;
            enemyMaster.EventEnemyReachedNavTarget += PlayIdleAudio;
            enemyMaster.EventEnemyWalking += PlayWalkAudio;
            enemyMaster.EventEnemyAttack += PlayAttackAudio;
            enemyMaster.EventEnemyHealthLow += PlayStruckAudio;
            enemyMaster.EventEnemyDie += PlayDeathAudio;
        }
		
		void OnDisable()
		{
            enemyMaster.EventEnemyDie -= DisableThis;
            enemyMaster.EventEnemyReachedNavTarget -= PlayIdleAudio;
            enemyMaster.EventEnemyWalking -= PlayWalkAudio;
            enemyMaster.EventEnemyAttack -= PlayAttackAudio;
            enemyMaster.EventEnemyHealthLow -= PlayStruckAudio;
            enemyMaster.EventEnemyDie -= PlayDeathAudio;
        }
		
		void SetInitialReferences()
		{
            enemyMaster = GetComponent<Enemy_Master>();
            enemyAnimationScript = GetComponent<Enemy_Animation>();
            enemyHealthScript = GetComponent<Enemy_Health>();
            myAudioSource = GetComponent<AudioSource>();
		}

        void PlayIdleAudio()
        {
            if (enemyAnimationScript.myAnimator.GetBool("isPursuing") == false && !myAudioSource.isPlaying)
            {
                myAudioSource.PlayOneShot(idleAudio);
            }

        }

        void PlayWalkAudio()
        {
            if (enemyAnimationScript.myAnimator.GetBool("isPursuing") == true && !myAudioSource.isPlaying)
            {
                myAudioSource.PlayOneShot(walkAudio);
            }

        }

        void PlayStruckAudio()
        {
            if (enemyAnimationScript.shouldStruck)
            {
                myAudioSource.Stop();
                myAudioSource.PlayOneShot(struckAudio);
            }
        }

        void PlayAttackAudio()
        {
            if (enemyAnimationScript.shouldAttack)
            {
                myAudioSource.Stop();
                myAudioSource.PlayOneShot(attackAudio);
            }
        }

        void PlayDeathAudio()
        {
            if (enemyHealthScript.enemyHealth <= 0)
            {
                myAudioSource.Stop();
                myAudioSource.PlayOneShot(dieAudio);
            }
        }

        void DisableThis()
        {
            this.enabled = false;
        }
    }
}

