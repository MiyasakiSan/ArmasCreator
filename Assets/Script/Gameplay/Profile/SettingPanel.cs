using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArmasCreator.UI;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField]
    private ProfilePanelController profilePanelController;

    [SerializeField]
    private Button backButton; 
    // Start is called before the first frame update
    void Start()
    {
        backButton.onClick.AddListener(() =>
        {
            profilePanelController.DeactiveSettingDetailAnim();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
