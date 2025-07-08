using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AnalyticalApproach.OrbAscent
{
    public class TriggerFade : MonoBehaviour
    {

        List<Renderer> renderers; 

        private void Awake()
        {
            renderers = GetComponentsInChildren<Renderer>().ToList();     
        }

        private void OnTriggerEnter(Collider other)
        {
            foreach(Renderer renderer in renderers)
            {
                renderer.enabled = false; 
            }
        }

        private void OnTriggerExit(Collider other)
        {
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = true;
            }

        }
    }

}