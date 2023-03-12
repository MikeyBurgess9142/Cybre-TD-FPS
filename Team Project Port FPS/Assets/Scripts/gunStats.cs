using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class gunStats : ScriptableObject
{
    public float shtRate;
    public int shtDist;
    public int shtDmg;
    public GameObject gunModel;
    public AudioClip gunShotAud;
}
