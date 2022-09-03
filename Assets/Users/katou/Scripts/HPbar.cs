using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class HPbar
{
    /// <summary>
    /// HP‚ÌƒQ[ƒW‚ğİ’è‚µ‚Ä‚¢‚é
    /// </summary>
    /// <param name="material">HPƒQ[ƒW—p‚ÌMaterial</param>
    /// <param name="value">İ’è‚µ‚½‚¢’l</param>
    public static void SetAmount(Material material, float value)
    {
        if(material == null)
        {
            return;          
        }
        //ƒQ[ƒW‚ª‚O‚P‚ÌŠÔ‚Ì‚İ‚Ì‚½‚ß‚O‚P‚Åw’è‚µ‚Ä‚¢‚é
        value = Mathf.Clamp01(value);
        material.SetFloat("_FillAmount", value);

    }


}
