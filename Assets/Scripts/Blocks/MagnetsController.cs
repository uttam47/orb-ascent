using System.Collections;
using UnityEngine;

namespace AnalyticalApproach.OrbAscent
{

    public class MagnetsController : MonoBehaviour
    {
        [SerializeField] GameObject anchor;
        [SerializeField] float attractSpeed = 20;
        [SerializeField] GameObject visuals; 
        [SerializeField] GameObject pushIcon; 

        private Vector3 position = Vector3.zero;
        private AudioSource audioPlayer;
        private BoxCollider boxCollider;
        private AudioEventChannel _audioEventChannel; 

        public void Awake()
        {
            audioPlayer = GetComponent<AudioSource>();
            boxCollider = GetComponent<BoxCollider>();
            _audioEventChannel = GameEventManager.GetEventChannel<AudioEventChannel>(); 
            visuals.SetActive(false);   
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("PlaybleUnit"))
                return;

            position = anchor.transform.position;

            boxCollider.enabled = false;
            anchor.transform.SetParent(null);
            _audioEventChannel.RaisePlayAudioWithAudioSource(audioPlayer,AudioType.MagnetsAttract); 

            StartCoroutine(Attract()); 
        }

        public IEnumerator Attract()
        {

            while ( (position - transform.position).magnitude >0.3f)
            {
                transform.position -= (transform.position - position).normalized * Time.deltaTime * attractSpeed;
                
                yield return null; 
            }

            transform.position = position;
            boxCollider.enabled = true;
            boxCollider.size = transform.localScale;
            boxCollider.center = Vector3.zero;
            boxCollider.isTrigger = false;
            anchor.transform.SetParent(transform);
            visuals.SetActive(true);
            transform.up = Vector3.up; 
            pushIcon.SetActive(false);   
            Destroy(this);
        }
    }

}