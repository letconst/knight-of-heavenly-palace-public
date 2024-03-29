using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class HPbar
{
    /// <summary>
    /// HPのゲージを設定している
    /// </summary>
    /// <param name="material">HPゲージ用のMaterial</param>
    /// <param name="value">設定したい値</param>
    public static void SetAmount(Material material, float value)
    {
        if(material == null)
        {
            return;          
        }
        //ゲージが０１の間のみのため０１で指定している
        value = Mathf.Clamp01(value);
        material.SetFloat("_FillAmount", value);

    }


}
