using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public  static  class DamageCalculator 
{
    public static int CalculateForBig(int swordAttackPower,EnemyBase.BodyPart part)
    {
        int damege = swordAttackPower;

        if(part==EnemyBase.BodyPart.Body)
        {
            damege *= 10 * 1;
        }
        if(part==EnemyBase.BodyPart.Wings)
        {
            damege = (int)(10*damege * 1.5);
        }
        if(part==EnemyBase.BodyPart.Head)
        {
            
            damege = (int)(10 * damege * 1.5);
        }
        return damege;

    }
    public static int CalculateForSmall(int swordAttackPower)
    {
        int damege = swordAttackPower;

        damege = (int)(10 * damege * 2);

        return damege;
    }
}
