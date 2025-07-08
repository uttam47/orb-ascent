using System.Collections.Generic;
using UnityEngine;

namespace AnalyticalApproach.OrbAscent
{
    public class ObjectDisabler : MonoBehaviour
    {
        public List<BoxCollider> boxCollider;

        private void OnTriggerEnter(Collider other)
        {
            foreach (BoxCollider monoBehaviour in boxCollider) 
            {
                monoBehaviour.enabled = false; 
            }
        }

        private void OnTriggerExit(Collider other)
        {
            foreach (BoxCollider monoBehaviour in boxCollider)
            {
                monoBehaviour.enabled = true;
            }
        }

    }

}