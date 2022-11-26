using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ArmasCreator.UI
{
    public class LoadingPopupBackGround : MonoBehaviour
    {
        [SerializeField]
        private LoadingPopup LP;

        [SerializeField]
        private Animator anim;

        public void ChangeBG() => LP.ChangeBG();

        public void Reset()
        {
            anim.SetBool("Fade", false);
        }

        public void Fade()
        {
            anim.SetBool("Fade", true);
        }
    }
}

