using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace AnalyticalApproach.OrbAscent
{
    public class CrystalExploder : MonoBehaviour
    {
        [SerializeField]private List<Transform> transforms;
        public float explosionForce = 220f;

        private void Awake()
        {
            foreach (Transform t in transforms)
            {
                t.gameObject.SetActive(false);
            }
        }

        public void Explode()
        {

            foreach (Transform t in transforms)
            {
                t.gameObject.SetActive(true);
                Rigidbody rigidbody = t.AddComponent<Rigidbody>(); 
                rigidbody.AddExplosionForce(explosionForce, transform.position + transform.up * .5f, 2);
                rigidbody.angularVelocity = new Vector3 (Random.Range(2, 15), Random.Range(2, 15), Random.Range(2, 15));  
                Destroy(rigidbody.gameObject, .1f); 
            }

        }
    }

}