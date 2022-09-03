using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 0~1の値を送信することでStatusBarの状態を変化させることができる
/// </summary>
public class StatusBarReceiver : SingletonMonoBehaviour<StatusBarReceiver>
{
    [SerializeField,Header("_hpBarImageMask(HP 1_Mask)")] private Image _hpBarImageMask;
    [SerializeField,Header("_spBarImageMask(HP 4_Mask)")] private Image _spBarImageMask;

    private Subject<float> _hpSubject = new Subject<float>();
    public  IObserver<float> HpBarRegister => _hpSubject;
    
    private Subject<float> _spSubject = new Subject<float>();
    public IObserver<float> SpBarRegister => _spSubject;
    
    /*減衰の処理入れようとしたけど時間かかりそうだから一旦やめとく
    private float _oldHP;
    private float _oldSP;

    [SerializeField,Header("減衰速度")]private float _attenuationSpeed = 1;
    [SerializeField,Header("減衰時間")]private float _attenuationTime = 1;
    */
    void Start()
    {
        _hpSubject
            .Where(x => (0 <= x && x <= 1))
            .Subscribe(x =>
            {
                _hpBarImageMask.fillAmount = x;
            }).AddTo(this);
        
        _spSubject
            .Where(x => (0 <= x && x <= 1))
            .Subscribe(x =>
            {
                _spBarImageMask.fillAmount = x;
            }).AddTo(this);
    }
}
