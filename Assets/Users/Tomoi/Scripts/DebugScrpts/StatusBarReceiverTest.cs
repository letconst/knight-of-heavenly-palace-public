using UnityEngine;

public class StatusBarReceiverTest : MonoBehaviour
{
    [Header("0~1の値を送信することでStatusBarの状態を変化させることができる")]
    [SerializeField,Range(0f,1f)] private float SetHPValue = 1f;
    [SerializeField,Range(0f,1f)] private float SetSPValue = 1f;
    private  void OnGUI()
    {
        if (GUILayout.Button("SetHP"))
        {
            StatusBarReceiver.Instance.HpBarRegister.OnNext(SetHPValue);
        }
        if (GUILayout.Button("SetSP"))
        {
            StatusBarReceiver.Instance.SpBarRegister.OnNext(SetSPValue);
        }
    }
}
