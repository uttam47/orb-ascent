using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AnalyticalApproach.OrbAscent
{
    public class CloudCurtainCotnroller : MonoBehaviour
    {
        List<ObjectTweener> _cloudClusters;

        private void Awake()
        {
            _cloudClusters = GetComponentsInChildren<ObjectTweener>().ToList();
            transform.SetParent(Camera.main.transform);
        }

        public void HideCurtains(bool value, Action action = null)
        {
            showing = value;
            foreach (ObjectTweener cloudCluster in _cloudClusters)
            {
                cloudCluster.TweenDest(value);
            }

            StartCoroutine(OnTransitionTimeComplete(action)); 
        }

        private IEnumerator OnTransitionTimeComplete(Action action)
        {
            yield return new WaitForSeconds(TransitionTimeConstants.TRANSITION_TIME);
            action?.Invoke();
        }

        private bool showing; 
        [ContextMenu("ShowCurtain")]
        public void ShowCurtain()
        {
            showing =!showing;

            HideCurtains(showing, null); 

        }
    }
}
