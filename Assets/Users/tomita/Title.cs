using Cysharp.Threading.Tasks;
using UnityEngine;

public class Title : MonoBehaviour
{
    private async void Start()
    {
        await UniTask.WaitUntil(() => SoundManager.Instance.CanPlayable);

        SoundManager.Instance.StopBgm();
        SoundManager.Instance.PlayBgm(MusicDef.First);
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) || SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.A))
#elif UNITY_SWITCH
        if (SwitchInputManager.Instance.GetKeyDown(SwitchInputManager.JoyConButton.A))
#endif
        {
            FadeContllor2.Instance.LoadScene(1, GameScene.Lobby);
        }
    }
}
