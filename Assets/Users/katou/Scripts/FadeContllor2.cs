using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class FadeContllor2 : SingletonMonoBehaviour<FadeContllor2>
{
    private static Canvas canvas;
    private static Image image;
    private bool _canLoadScene = true;

    public static SceneDataPack PreviousSceneData;
   

    private static void Init()
    {
        GameObject canvasObject = new GameObject("CanvasFade");
        canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        canvasObject.AddComponent<GraphicRaycaster>();

        image = new GameObject("ImageFade").AddComponent<Image>();
        image.transform.SetParent(canvas.transform, false);
        image.rectTransform.anchoredPosition = Vector3.zero;
        Camera camera = Camera.main;
        image.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        image.color = Color.clear;
        image.raycastTarget = false;

        DontDestroyOnLoad(canvas.gameObject);

        DontDestroyOnLoad(Instance.gameObject);

        canvasObject.AddComponent<FadeContllor2>();

        

    }
    private void Start()
    {
        Init();
    }
   
    
    public void LoadScene(float interval, GameScene gameScene,SceneDataPack sceneDataPack = null )
    {
        

        if (_canLoadScene)
        {
            //LoadingópÉVÅ[ÉìÇì«Ç›çûÇﬁ
            PreviousSceneData = new ToLoadingSceneDataPack(GameScene.Title,gameScene,sceneDataPack);
            StartCoroutine(Fade(interval, GameScene.Loading.ToString()));
        }
    }
    private IEnumerator Fade(float interval, string taransition2)
    {

        yield return FadeOutAsync(interval);

        yield return LoadSceneAsync(taransition2);
        
        yield return FadeInAsync(interval);  

    }

    public AsyncOperation LoadSceneAsync(string Scenename)
    {
        return SceneManager.LoadSceneAsync(Scenename);
    }
    public IEnumerator FadeOutAsync(float interval)
    {
        _canLoadScene = false;
        float time = 0f;
        canvas.enabled = true;

        while (time <= interval)
        {
            float fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);
            image.color = new Color(0.0f, 0.0f, 0.0f, fadeAlpha);
            time += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator FadeInAsync(float interval)
    {
        _canLoadScene = true;
        float time = 0f;
        time = 0f;
        while(time <= interval)
        {
            float fadeAlpha = Mathf.Lerp(1f, 0f, time / interval);
            image.color = new Color(0.0f, 0.0f, 0.0f, fadeAlpha);
            time += Time.deltaTime;
            yield return null;
        }
        canvas.enabled = false;
        
    }

    public void FadeOut(float interval)
    {
       StartCoroutine(FadeOutAsync(interval));
          
    }

    public void FadeIn(float interval)
    {
        StartCoroutine(FadeInAsync(interval));
    }


}
