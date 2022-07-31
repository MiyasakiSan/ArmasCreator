using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorOverider : MonoBehaviour
{
    private Animator _animator;
    // Start is called before the first frame update
    private void Awake()
    {
        _animator = this.GetComponent<PlayEmote>().PlayerAnimator;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
