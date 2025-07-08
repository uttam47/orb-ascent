using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnalyticalApproach.OrbAscent
{
    public abstract class UIScreen : MonoBehaviour
    {
        private UIDocument _document;
        private List<Coroutine> _myInitiateCoroutines; 

        protected VisualElement root;
        protected abstract ScreenType screenType { get; }
        protected UIEventChannel uiEventChannel;
        protected AudioEventChannel audioEventChannel;
        List<Button> _buttons; 

        private void Awake()
        {     
            _document = GetComponent<UIDocument>();
            root = _document.rootVisualElement;
            uiEventChannel = GameEventManager.GetEventChannel<UIEventChannel>();
            audioEventChannel = GameEventManager.GetEventChannel<AudioEventChannel>(); 
            root.style.display = DisplayStyle.None; 
            RegisterMenuSound(); 
            Initialize();

        }

        private void RegisterMenuSound()
        {
            _buttons = root.GetAllChilderenOfType<Button>();
            
            foreach (Button button in _buttons)
            {
                if (button != null)
                {
                    button.clicked += PlayMenuButtonSound; 
                }
            }
        }

        private void EnableButtons(bool value)
        {
            foreach (Button button in _buttons)
            {
                if (button != null)
                {
                    button.SetEnabled(value);
                }
            }
        }

        private void PlayMenuButtonSound()
        {
            audioEventChannel.RaisePlayMenuAudio(AudioType.MenuButton); 
        }

        public ScreenType GetScreenTpye()
        {
            return screenType; 
        }

        public virtual void Initialize() { }

        public virtual void Show(bool value, Action OnComplete)
        {
            if(value)
            {
                Show(OnComplete); 
            }
            else
            {
                Hide(OnComplete); 
            }
        }

        public virtual void Hide(Action OnComplete = null)
        {
            StopMyAllCoroutines();
            EnableButtons(false); 
            StartCoroutine(FadeCoroutine(0, OnComplete));
        }

        public virtual void Show(Action OnComplete = null)
        {
            StopMyAllCoroutines();
            EnableButtons(true); 
            StartCoroutine(FadeCoroutine(1, OnComplete));
        }

        private IEnumerator FadeCoroutine(float targetOpacity, Action OnComplete)
        {
            root.style.display = DisplayStyle.Flex;
            float startOpacity = root.style.opacity.value;
            float elapsedTime = 0f;

            while (elapsedTime < TransitionTimeConstants.UI_FADE_TIME)
            {
                float newOpacity = Mathf.Lerp(startOpacity, targetOpacity, elapsedTime / TransitionTimeConstants.UI_FADE_TIME);
                root.style.opacity = newOpacity;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            root.style.opacity = targetOpacity; 
            root.style.display = targetOpacity == 1.0f ? DisplayStyle.Flex : DisplayStyle.None;
            OnComplete?.Invoke(); 
        }

        private void StopMyAllCoroutines()
        {
            _myInitiateCoroutines ??= new List<Coroutine>(); 
            foreach(Coroutine coroutine in _myInitiateCoroutines)
            {
                if(coroutine != null)
                {
                    StopCoroutine(coroutine); 
                }
            }
            _myInitiateCoroutines.Remove(null); 
        }
    }
}
