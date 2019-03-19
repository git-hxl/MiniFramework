using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class SkillBase : MonoBehaviour
{
    //使用次数
    public int UseTimes;
    //冷却时间
    public float CoolTime;
    //施法时间
    public float CastTime;
    public abstract void Use();
    public abstract void Cancel();
}