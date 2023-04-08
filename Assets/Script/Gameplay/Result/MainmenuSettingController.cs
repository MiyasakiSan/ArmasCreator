using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArmasCreator.UI
{
    public class MainmenuSettingController : MonoBehaviour
    {
        [SerializeField]
        private Animator anim;

        public void OnFadeIn()
        {
            anim.SetTrigger("First");
        }

        public void OnFadeOut()
        {
            anim.SetTrigger("HideOption");
            anim.SetTrigger("HideCredit");
            anim.SetTrigger("Out");
        }

        public void OnShow()
        {
            anim.SetTrigger("HideCredit");
            anim.SetTrigger("ShowOption");
        }

        public void OnHide()
        {
            anim.SetTrigger("HideOption");
        }

        public void OnShowCredit()
        {
            anim.SetTrigger("HideOption");
            anim.SetTrigger("ShowCredit");
        }

        public void OnHideCredit()
        {
            anim.SetTrigger("HideCredit");
        }

        public void OnResetTrigger(string triggerName)
        {
            anim.ResetTrigger(triggerName);
        }
    }
}
