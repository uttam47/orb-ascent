using System.Collections;
using UnityEngine;

namespace AnalyticalApproach.OrbAscent
{
    public class FallerTriger : MonoBehaviour
    {
        public float destructTime = .5f;
        [SerializeField] private CrystalExploder crystalExploder;
        [SerializeField] private GameObject fallerObject;
        [SerializeField] private GameObject explosionObject;

        private Renderer _parentRenderer;
        private BoxCollider _parentBoxCollider;
        private AudioSource _parentAudioSource;
        private AudioEventChannel _audioEventChannel; 

        private void Awake()
        {
            _parentRenderer = fallerObject.GetComponent<Renderer>();
            _parentBoxCollider = fallerObject.GetComponent<BoxCollider>();
            _parentAudioSource = fallerObject.GetComponent<AudioSource>();
            _audioEventChannel = GameEventManager.GetEventChannel<AudioEventChannel>(); 
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("MainCamera"))
                return;
            DeleteMe();
        }

        private void DeleteMe()
        {
            StartCoroutine(Destruct());
        }

        IEnumerator Destruct()
        {

            yield return new WaitForSeconds(destructTime);
            explosionObject.SetActive(true);
            crystalExploder.Explode();
            _parentRenderer.enabled = false;
            _parentBoxCollider.enabled = false;
            _audioEventChannel.RaisePlayAudioWithAudioSource(_parentAudioSource, AudioType.FallerBreak); 
            Destroy(fallerObject, 0.5f);

        }
    }

}