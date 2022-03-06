using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;


namespace ColonySim
{
    public class ButtonElement : InteractionElement, ISubmitHandler
    {
        public UnityEvent OnClick;
        public TextMeshProUGUI Text;

        protected override Color32 UIPressed { get; set; }
        protected override Color32 UIInactive { get; set; }
        protected override Color32 UISelected { get; set; }
        protected override Color32 UIHighlight { get; set; }

        protected override Color32 CurrentState { get => currentState; }
        private Color32 currentState = new Color32(255, 255, 255, 0);

        public override void Init()
        {
            base.Init();
            OnClicked += Clicked;
        }

        public override void UpdateState(bool forceCheckFocus = false)
        {
            if (!gameObject.activeSelf)
            {
                SelectionImage.color = UIInactive;
                return;
            }
            if (!interactable)
            {
                SetColor(UIDisabled);
                return;
            }
            if (!forceCheckFocus && CurrentlySelected())
            {
                if (IsFocused)
                {
                    SetColor(new Color32(0, 0, 0, (byte)(UISelected.a + UIHighlight.a)));
                }
                else
                {
                    SetColor(UISelected);
                }

            }
            else
            {
                if (IsFocused)
                {
                    SetColor(UIHighlight, 0.05f);
                }
                else
                {
                    SetColor(UIInactive);
                }
            }
        }

        public void SetState(Color32 Color)
        {
            currentState = Color;
            UpdateState();
        }

        public void SetText(string Title)
        {
            Text.text = Title;
        }

        public void Clicked(InteractionElement interactable, BaseEventData Data)
        {
            OnClick?.Invoke();
        }

        public void SetSelectedColour(Color32 SelectColour)
        {
            UISelected = SelectColour;
        }

        public void OnSubmit(BaseEventData eventData)
        {
            Clicked(this, eventData);
        }
    }
}
