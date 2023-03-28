using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArmasCreator.UI
{
    public class MainmenuSettingController : MonoBehaviour
    {
        [SerializeField]
        private Animator anim;

        public void OnShow()
        {
            anim.SetTrigger("Show");
        }

        public void OnHide()
        {
            anim.SetTrigger("Hide");
        }
    }
}
