using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AnalyticalApproach.OrbAscent
{

    public class PlayerStaticFader : MonoBehaviour
    {
        [SerializeField] float timeInterval; 
        private List<Transform> _logs; 
        private Coroutine _coroutine;
        private List<float> normalScale;
        private Renderer _renderer;
        private BoxCollider _boxCollider; 
        public Material _material; 
        [Range(-1f, 1f)]

        private int FADE_STRENGH = Shader.PropertyToID("_FadeStrength"); 
        
        void Awake()
        {
            _logs  = GetComponentsInChildren<Transform>().ToList();
            _renderer = GetComponent<Renderer>();
            _boxCollider = GetComponent<BoxCollider>();  
            _logs.Remove(transform);

            normalScale =  new List<float>();  
            foreach(Transform log in _logs)
            {
                normalScale.Add(log.localScale.y); 
            }
            _material = _renderer.material = new Material(_material); 
        }


        public void Show(bool value)
        {
            _renderer.enabled = true;
            _boxCollider.enabled = true; 
            foreach(Transform log in _logs)
            {

                log.gameObject.SetActive(true);
            }
            if(_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            _coroutine = StartCoroutine(ScaleLogsCoroutine(value)); 
        }


        private IEnumerator ScaleLogsCoroutine(bool value)
        {
            float elapsedTime = 0f;
            float[] startScales = _logs.Select(log => log.localScale.y).ToArray();
            List<float> targetScale = new  List<float>(); 
            
            foreach(float normalScale in normalScale)
            {
                targetScale.Add(value ? normalScale : 0.0f);
            }

            float targetFade = value ? -1 : 1;
            float startScale = _material.GetFloat(FADE_STRENGH);
            float fadeStrength; 

            while (elapsedTime < timeInterval)
            {
                elapsedTime += Time.deltaTime;
                float scaleFactor = elapsedTime / timeInterval;

                for (int i = 0; i < _logs.Count; i++)
                {
                    Vector3 newScale = _logs[i].localScale;
                    newScale.y = Mathf.Lerp(startScales[i], targetScale[i], scaleFactor);
                    _logs[i].localScale = newScale;
                }

                fadeStrength = Mathf.Lerp(startScale, targetFade, scaleFactor);
                
                _material.SetFloat(FADE_STRENGH, fadeStrength);

                yield return null;
            }

            for (int i = 0; i < _logs.Count; i++)
            {
                Vector3 newScale = _logs[i].localScale;
                newScale.y = targetScale[i];
                _logs[i].localScale = newScale;
                _logs[i].gameObject.SetActive(value); 
            }

            _renderer.enabled = value;
            _boxCollider.enabled = value; 

        }
    }

}