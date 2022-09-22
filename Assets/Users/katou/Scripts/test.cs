using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class test : MonoBehaviour
{
    public void Fade()
    {
        FadeContllor2.Instance.LoadScene(1f, GameScene.Title);
    }

    // Start is called before the first frame update
   async void Start()
    {
        await UniTask.Yield();
        // ToResultSceneDataPack dataPack = new ToResultSceneDataPack(GameScene.Title, 1);
        // FadeContllor2.Instance.LoadScene(1, GameScene.MainGame, dataPack);
    }

    // Update is called once per frame
    void Update()
    {
       // if (SoundManager.Instance.CanPlayable)
          //  SoundManager.Instance.PlaySe(SoundDef.Sample);
    }
}
