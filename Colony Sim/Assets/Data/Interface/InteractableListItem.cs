using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace ColonySim
{
    public class InteractableListItem : InteractionElement
    {
        public TextMeshProUGUI textObject;
        public string Text => textObject.text;
        public string Title;
        public string[] Data;

        //Colour States
        protected override Color32 UIPressed { get; set; }
        protected override Color32 UIInactive { get; set; }
        protected override Color32 UISelected { get; set; }
        protected override Color32 UIHighlight { get; set; }

        public RectTransform ParentList;


        public override void Init()
        {
            base.Init();
        }

        public void SetText(string text)
        {
            textObject.text = text;
        }

        public void SetTitle(string title, bool updateName = true)
        {
            Title = title;
            if (updateName)
            {
                SetText(title);
            }
        }

        public void SetData(string[] data)
        {
            Data = data;
        }

        #region UI Sizing

        //private void RefreshSize(int recursive = 0)
        //{
        //    if (gameObject.activeInHierarchy)
        //    {
        //        StartCoroutine(Refresh(recursive));
        //    }
        //}

        //private IEnumerator Refresh(int recursive = 0)
        //{
        //    yield return null;
        //    do
        //    {
        //        recursive--;
        //        yield return null;
        //    } while (recursive > 0);
        //    LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)gameObject.transform);
        //    if (ParentList != null)
        //    {
        //        LayoutRebuilder.ForceRebuildLayoutImmediate(ParentList);
        //    }
        //}

        #endregion
    }
}
