using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class HPbar
{
    /// <summary>
    /// HP�̃Q�[�W��ݒ肵�Ă���
    /// </summary>
    /// <param name="material">HP�Q�[�W�p��Material</param>
    /// <param name="value">�ݒ肵�����l</param>
    public static void SetAmount(Material material, float value)
    {
        if(material == null)
        {
            return;          
        }
        //�Q�[�W���O�P�̊Ԃ݂̂̂��߂O�P�Ŏw�肵�Ă���
        value = Mathf.Clamp01(value);
        material.SetFloat("_FillAmount", value);

    }


}
