using UnityEngine;

public class Title : MonoBehaviour
{
    
    void Update()
    {
#if UNITY_EDITOR 
        if (Input.GetMouseButtonDown(0)||SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.A))
#elif UNITY_SWITCH
        if (SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.A))
#endif
        {
            FadeContllor2.Instance.LoadScene(1, GameScene.MainGame);
        }
        
    }
   

}
