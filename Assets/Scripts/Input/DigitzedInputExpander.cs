using UnityEngine;

namespace AnalyticalApproach.OrbAscent
{
    public class DigitzedInputExpander
    {
        private float _maxExpansion;
        private float _expansionRate;
        private Vector2 _analogInput;
        private Vector2 _individualAxisWeights;

        public DigitzedInputExpander(float maxExpansion, float expansionRate, Vector2 individualAxisWeights = default)
        {
            _maxExpansion = maxExpansion;
            _expansionRate = expansionRate;
            _individualAxisWeights = individualAxisWeights == default ? new Vector2(1, 1) : individualAxisWeights;
        }

        public void SetValues(float maxExpansion, float expansionRate)
        {
            _maxExpansion = maxExpansion;
            _expansionRate = expansionRate;
        }


        public void SetAxisWeights(Vector2 axisWeights)
        {
            _individualAxisWeights = axisWeights;
        }

        public Vector2 GetDecompressedInputValue(Vector2 digitalInput)
        {

            _analogInput.y = Mathf.MoveTowards(_analogInput.y, digitalInput.y * _maxExpansion, _expansionRate * Time.deltaTime);
            _analogInput.x = Mathf.MoveTowards(_analogInput.x, digitalInput.x * _maxExpansion, _expansionRate * Time.deltaTime);
            

            return _analogInput * _individualAxisWeights;
        }

        public void Reset()
        {
            _analogInput = Vector2.zero;
        }
    }
}
