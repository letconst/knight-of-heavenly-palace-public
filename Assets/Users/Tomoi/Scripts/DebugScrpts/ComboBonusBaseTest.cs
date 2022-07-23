using nn.hid;
using UnityEngine;

public class ComboBonusBaseTest : MonoBehaviour
{
    private const float _AccelerationJudgmentalue = 0.8f;

    /*AボタンでJoy-Conを降ったときの角度を保存
     *Bボタンで今保存されているデータを確認
     * 
     */
    
    private ComboBonusBase _base = new ComboBonusBase();
    [SerializeField] private bool istrue;

    void Start()
    {
        _base.Start();
    }
    void Update()
    {
        _base.Update();
        if (SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.A))
        {
            istrue = true;
        } 
        if (SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.X))
        {
            //_base._ComboStockClear();
        }
        if (SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.B))
        {
            Debug.Log(_base._ComboStock[0] + "\n" +
                      _base._ComboStock[1] + "\n" +
                      _base._ComboStock[2] + "\n" +
                      _base._ComboStock[3] + "\n");
        }
        if ((_AccelerationJudgmentalue <= SwitchInputManager.Instance.RightAcceleration.x ||
                 _AccelerationJudgmentalue <= SwitchInputManager.Instance.RightAcceleration.y ||
                 _AccelerationJudgmentalue <= SwitchInputManager.Instance.RightAcceleration.z)&& istrue)
        {
            istrue = false;
            _base.AddComboRegister(NpadJoyDeviceType.Right);
        }
    }
}