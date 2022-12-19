using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KeySystem{
    public class KeyDoorController : MonoBehaviour
    {
        private Animator doorAnim;
        private bool doorOpen = false;

        [Header("Animation Names")]
        [SerializeField] private string openAnimationName = "elevatorOpenAnimation";
        [SerializeField] private string closeAnimationName = "elevatorCloseAnimation";

        [SerializeField] private int timeToShowUI = 1;
        [SerializeField] private GameObject showDoorLockedUI = null;

        [SerializeField] private KeyInventory _keyInventory = null;
        [SerializeField] private int waitTimer = 1;
        [SerializeField] private bool pauseInteraction = false;

           private void Awake()
        {
            doorAnim = gameObject.GetComponent<Animator>();
        }

        private IEnumerator PauseDoorInteraction()
        {
            pauseInteraction = true;
            yield return new WaitForSeconds(waitTimer);
            pauseInteraction = false;
        }

            public void PlayAnimation()
            {
                if (_keyInventory.hasRedKey)
                {
                    if(!doorOpen && !pauseInteraction)
                    {
                        doorAnim.Play(openAnimationName, 0, 0.0f);
                        doorOpen = true;
                        StartCoroutine(PauseDoorInteraction());
                    }else if(doorOpen && !pauseInteraction)
                    {
                        doorAnim.Play(closeAnimationName, 0, 0.0f);
                        doorOpen = false;
                        StartCoroutine(PauseDoorInteraction());
                    }
                }
                else
                {
                    StartCoroutine(ShowDoorLocked());
                }
            }

            IEnumerator ShowDoorLocked()
            {
                showDoorLockedUI.SetActive(true);
                yield return new WaitForSeconds(timeToShowUI);
                showDoorLockedUI.SetActive(false);
            }
    }

}
