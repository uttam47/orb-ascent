using UnityEngine;

namespace AnalyticalApproach.OrbAscent
{
    [RequireComponent(typeof(StandaloneInputProcessor))]
    [RequireComponent(typeof(TouchInputProcessor))]
    public class PlayerInputHandler: MonoBehaviour
    {
        Player_ActionMap _inputActions;
        private StandaloneInputProcessor _standaloneInput;
        private TouchInputProcessor _toucInputProcessor; 

        private void Awake()
        {
            _inputActions = new Player_ActionMap();
            _standaloneInput = GetComponent<StandaloneInputProcessor>();    
            _toucInputProcessor = GetComponent<TouchInputProcessor>();


            _standaloneInput.SetInputActionMap(_inputActions);
            _toucInputProcessor.SetInputActionMap(_inputActions); 

        }

        public void Enable(bool value)
        {
            if(value)
            {
                Enable(); 
            }
            else
            {
                Disable();
            }
        }

        public void Enable()
        {
            _inputActions.Enable(); 
        }

        public void Disable()
        {
            _inputActions.Disable();
        }

    }
}