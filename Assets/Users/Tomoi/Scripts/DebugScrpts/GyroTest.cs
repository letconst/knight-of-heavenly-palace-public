using UnityEngine;
public class GyroTest : MonoBehaviour
{
    [SerializeField] private GameObject _TestPointer1;
    [SerializeField] private GameObject _TestPointer2;

    void Update()
    {
        //補正
        if (SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.A) )
        {
            JoyConToScreenPointer.Instance.AngleReset();
        }

        //ポインターの移動
        _TestPointer1.GetComponent<RectTransform>().position = JoyConToScreenPointer.Instance.LeftJoyConScreenVector2;
        _TestPointer2.GetComponent<RectTransform>().position = JoyConToScreenPointer.Instance.RightJoyConScreenVector2;
        

    }

}
