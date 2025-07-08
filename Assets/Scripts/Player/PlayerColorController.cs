using System.Collections;
using UnityEngine;

namespace AnalyticalApproach.OrbAscent
{
    public class PlayerColorController : MonoBehaviour
    {
        private const string BaseColorProperty = "_BaseColor";
        private const string RimStepProperty = "_RimStep";

        [Range(0.0f,1.0f)]
        [SerializeField] private float rimWidth = .75f;
        [SerializeField] private Color rimColor;
        [SerializeField] private float colorShiftTime;
        [SerializeField] private Color playingColor;
        [SerializeField] private Color deselectableColor;
        [SerializeField] private Color selectableColor;

        private Coroutine _colorChangeCoroutine;
        private Coroutine _rimCoroutine;
        private Material _material;

        private void Start()
        {
            Renderer renderer = GetComponent<Renderer>();
            renderer.material = new Material(renderer.material);
            _material = renderer.material;
            _material.SetColor("_RimColor", rimColor); 
        }

        public void SetPlayerColor(PlayableColorState state)
        {
            if (_colorChangeCoroutine != null)
            {
                StopCoroutine(_colorChangeCoroutine);
            }

            if (_rimCoroutine != null)
            {
                StopCoroutine(_rimCoroutine); 
            }

            switch (state)
            {
                case PlayableColorState.Playing:
                    _rimCoroutine = StartCoroutine(RimChangeCoroutine(false)); 
                    _colorChangeCoroutine = StartCoroutine(ColorChangeCoroutine(playingColor));
                    break;
                case PlayableColorState.NonSelectable:
                    _rimCoroutine = StartCoroutine(RimChangeCoroutine(false)); 
                    _colorChangeCoroutine = StartCoroutine(ColorChangeCoroutine(deselectableColor));
                    break;
                case PlayableColorState.Selectable:
                    _rimCoroutine = StartCoroutine(RimChangeCoroutine(true)); 
                    _colorChangeCoroutine = StartCoroutine(ColorChangeCoroutine(selectableColor));
                    break;
            }
        }

        private IEnumerator ColorChangeCoroutine(Color changeColor)
        {
            Color currentColor = _material.GetColor(BaseColorProperty);
            float elapsedTime = 0f;

            while (elapsedTime < colorShiftTime)
            {
                elapsedTime += Time.deltaTime;
                Color color = Color.Lerp(currentColor, changeColor, elapsedTime / colorShiftTime);
                _material.SetColor(BaseColorProperty, color);

                yield return null;
            }

            _material.SetColor(BaseColorProperty, changeColor);
        }

        private IEnumerator RimChangeCoroutine(bool isRimOn)
        {
            float startRimValue = isRimOn ? 0f : rimWidth;
            float endRimValue = isRimOn ? rimWidth : 0f;
            float elapsedTime = 0f;

            while (elapsedTime < colorShiftTime)
            {
                elapsedTime += Time.deltaTime;
                float rimValue = Mathf.Lerp(startRimValue, endRimValue, elapsedTime / colorShiftTime);
                _material.SetFloat(RimStepProperty, rimValue);

                yield return null;
            }

            _material.SetFloat(RimStepProperty, endRimValue);
        }

    }
}
