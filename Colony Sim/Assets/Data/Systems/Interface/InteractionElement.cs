using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace ColonySim
{
    public abstract class InteractionElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDeselectHandler, ISelectHandler, IPointerClickHandler,
        IPointerDownHandler, IPointerUpHandler
    {
        public bool Interactable
        {
            get => interactable; set
            {
                if (interactable != value)
                {
                    if (!value)
                    {
                        DisableInteraction();
                    }
                    else
                    {
                        EnableInteraction();
                    }
                }
                interactable = value;
            }
        }
        public Graphic SelectionImage;

        [SerializeField]
        private Color32 Pressed;
        protected abstract Color32 UIPressed { get; set; }
        [SerializeField]
        private Color32 Inactive;
        protected abstract Color32 UIInactive { get; set; }
        [SerializeField]
        private Color32 Selected;
        protected abstract Color32 UISelected { get; set; }
        [SerializeField]
        private Color32 Highlight;
        protected abstract Color32 UIHighlight { get; set; }
        [SerializeField]
        private Color32 Disabled;
        protected virtual Color32 UIDisabled { get; set; } = new Color32(0, 0, 0, 0);

        protected virtual Color32 CurrentState { get; } = new Color32(255, 255, 255, 0);
        protected bool IsSelected = false;
        protected bool IsFocused = false;
        protected bool interactable = true;
        protected bool checkState = false;

        public event Action<InteractionElement, PointerEventData> OnClicked;
        public event Action<InteractionElement, PointerEventData> OnDoubleClick;
        public event Action<InteractionElement, BaseEventData> OnSelected;
        public event Action<InteractionElement, BaseEventData> OnDeselected;
        public event Action<InteractionElement, BaseEventData> OnFocus;
        public event Action<InteractionElement, BaseEventData> OnUnfocus;


        private const float fadeTime = 0.25f;
        private float currentTime;
        private float setTime;
        private Color32 currentColour;
        private Color32 setColour;
        [SerializeField] private CanvasGroup CanvasGroup;
        private bool forceTransitionCompletion = false;
        private Coroutine RunningCoroutine;

        public virtual void Start()
        {
            FindCanvasGroup();
            Init();
        }

        public virtual void Awake()
        {
            FindCanvasGroup();
        }

        public virtual void Init()
        {
            UIPressed = Pressed;
            UIInactive = Inactive;
            UIDisabled = Disabled;
            UISelected = Selected;
            UIHighlight = Highlight;
        }

        private void FindCanvasGroup()
        {
            if (!CanvasGroup)
            {
                CanvasGroup = gameObject.GetComponent<CanvasGroup>();
                if (!CanvasGroup)
                {
                    CanvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
            }
        }

        public virtual void DisableInteraction()
        {
            SetColor(UIInactive, 0);
            interactable = false;
            CanvasGroup.blocksRaycasts = false;
            CanvasGroup.interactable = false;
        }

        public virtual void EnableInteraction()
        {
            interactable = true;
            CanvasGroup.blocksRaycasts = true;
            CanvasGroup.interactable = true;
            UpdateState();
        }

        protected virtual bool CurrentlySelected()
        {
            return EventSystem.current.currentSelectedGameObject == gameObject || IsSelected;
        }

        public virtual void UpdateState(bool forceCheckFocus = false)
        {
            if (!gameObject.activeSelf)
            {
                SelectionImage.color = UIInactive;
                //Debug.Log("Disabled::"+gameObject.name);
                return;
            }
            if (!interactable)
            {
                SetColor(UIDisabled);
                //Debug.Log("Uninteractable::" + gameObject.name);
                return;
            }
            if (!forceCheckFocus && CurrentlySelected())
            {
                if (IsFocused)
                {
                    //Debug.Log("FocusedAndActive::" + gameObject.name);
                    SetColor(new Color32(0, 0, 0, (byte)(UISelected.a + UIHighlight.a)));
                }
                else
                {
                    //Debug.Log("Selected::" + gameObject.name);
                    SetColor(UISelected);
                }
            }
            else
            {
                if (IsFocused)
                {
                    //Debug.Log("Focused::" + gameObject.name);
                    SetColor(UIHighlight, 0.05f);
                }
                else
                {
                    //Debug.Log("Inactive::" + gameObject.name);
                    SetColor(UIInactive);
                }
            }
        }

        protected void SetColor(Color32 Colour, float time = fadeTime)
        {
            if (RunningCoroutine != null)
            {
                //If our colour is already being set.
                if (setColour.Equals(Colour))
                {
                    return;
                }
                //If the current transition is un-interruptable
                if (forceTransitionCompletion)
                {
                    return;
                }
            }
            //If we're inactive and can't perform a transition
            if (!gameObject.activeInHierarchy || !SelectionImage.gameObject.activeInHierarchy)
            {
                SelectionImage.color = UIInactive;
                return;
            }
            currentColour = SelectionImage.color;
            setColour = new Color32((byte)(CurrentState.r + Colour.r), (byte)(CurrentState.g + Colour.g), (byte)(CurrentState.b + Colour.b), (byte)(CurrentState.a + Colour.a));

            currentTime = 0;
            setTime = time;
            if (RunningCoroutine == null)
            {
                RunningCoroutine = StartCoroutine(ChangeColour());
            }
        }

        private IEnumerator ChangeColour()
        {
            while (currentTime < setTime)
            {
                currentTime += Time.deltaTime;
                SelectionImage.color = Color32.Lerp(currentColour, setColour, currentTime / setTime);
                yield return null;
            }
            RunningCoroutine = null;
            if (checkState || forceTransitionCompletion)
            {
                checkState = false;
                forceTransitionCompletion = false;
                UpdateState();
            }
            yield break;
        }

        public void OnDisable()
        {
            RunningCoroutine = null;
            checkState = false;
            forceTransitionCompletion = false;
            currentTime = 0;
            setTime = 0;
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            checkState = true;
            IsFocused = true;
            UpdateState();
            OnFocus?.Invoke(this, eventData);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            checkState = true;
            IsFocused = false;
            UpdateState();
            OnUnfocus?.Invoke(this, eventData);
        }

        public virtual void OnDeselect(BaseEventData eventData)
        {
            checkState = true;
            UpdateState();
            OnDeselected?.Invoke(this, eventData);
        }

        public virtual void OnSelect(BaseEventData eventData)
        {
            checkState = true;
            UpdateState();
            OnSelected?.Invoke(this, eventData);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            //Debug.Log("Clicked!");           
            SetColor(UIPressed, 0.05f);
            forceTransitionCompletion = true;
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            UpdateState();
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke(this, eventData);
            if (eventData.clickCount == 1)
            {
                OnDoubleClick?.Invoke(this, eventData);
            }
        }
    }
}
