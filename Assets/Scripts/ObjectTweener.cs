using System.Collections;
using UnityEngine;

namespace AnalyticalApproach.OrbAscent
{
    public class ObjectTweener : MonoBehaviour
    {
        [SerializeField] private Transform targetTransform; 

        private Vector3 originalPosition;  
        private Quaternion originalRotation; 
        private Coroutine tweenCoroutine; 

        void Start()
        {
            originalPosition = transform.localPosition;
            originalRotation = transform.localRotation;           
        }

        public void TweenDest(bool toTarget)
        {
            if (tweenCoroutine != null)
            {
                StopCoroutine(tweenCoroutine);
            }

            tweenCoroutine = StartCoroutine(TweenTransform(toTarget ? targetTransform.localPosition : originalPosition, toTarget ? targetTransform.localRotation : originalRotation));
        }

        private IEnumerator TweenTransform(Vector3 targetPosition, Quaternion targetRotation)
        {
            Vector3 startPosition = transform.localPosition;
            Quaternion startRotation = transform.localRotation;
            float elapsedTime = 0f;

            while (elapsedTime < TransitionTimeConstants.TRANSITION_TIME)
            {
                transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / TransitionTimeConstants.TRANSITION_TIME);
                transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / TransitionTimeConstants.TRANSITION_TIME);
                elapsedTime += Time.deltaTime;
                yield return null; 
            }

            transform.localPosition = targetPosition;
            transform.localRotation = targetRotation;
            tweenCoroutine = null;
        }
    }
}
