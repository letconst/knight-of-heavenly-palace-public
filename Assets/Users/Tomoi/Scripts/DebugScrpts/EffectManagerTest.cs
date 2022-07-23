using Unity.Mathematics;
using UnityEngine;

public class EffectManagerTest : MonoBehaviour
{
    [SerializeField]
    private EffectType _A;

    [SerializeField]
    private Transform AParent;
    
    [SerializeField]
    private EffectType _B;
    
    [SerializeField]
    private Transform BParent;
    private void OnGUI()
    {
        GUIStyleState style = new GUIStyleState();  
        style.textColor = Color.red;
        GUIStyle guiStyle = new GUIStyle();
        guiStyle.normal = style;
        GUI.Label(new Rect(200 , 200, 1000, 300), "コルーチン自体は動いているがループ用のエフェクトではないためループしてエフェクトの表示ができない",guiStyle);
        if(GUILayout.Button("プールテストA"))
        {
            EffectManager.Instance.EffectPlay(_A, Vector3.zero, quaternion.identity);
        }
        if(GUILayout.Button("プールテストB"))
        {
            EffectManager.Instance.EffectPlay(_B, Vector3.zero, quaternion.identity);
        }
        if(GUILayout.Button("プールテストAコルーチン"))
        {
            EffectManager.Instance.EffectPlay(_A, Vector3.zero, quaternion.identity,true,5f);
        }
        if(GUILayout.Button("プールテストBコルーチン"))
        {
            EffectManager.Instance.EffectPlay(_B, Vector3.zero, quaternion.identity,true,5f);
        }
        
        if(GUILayout.Button("親オブジェクト変更　プールテストA"))
        {
            EffectManager.Instance.EffectPlay(_A, Vector3.zero, quaternion.identity,AParent);
        }
        if(GUILayout.Button("親オブジェクト変更　プールテストB"))
        {
            EffectManager.Instance.EffectPlay(_B, Vector3.zero, quaternion.identity,BParent);
        }
        if(GUILayout.Button("親オブジェクト変更　プールテストAコルーチン"))
        {
            EffectManager.Instance.EffectPlay(_A, Vector3.zero, quaternion.identity,AParent,true,5f);
        }
        if(GUILayout.Button("親オブジェクト変更　プールテストBコルーチン"))
        {
            EffectManager.Instance.EffectPlay(_B, Vector3.zero, quaternion.identity,BParent,true,5f);
        }
    }
}
