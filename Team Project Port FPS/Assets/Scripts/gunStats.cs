using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class gunStats : ScriptableObject
{
    public Vector3 gunPosition;
    public Vector3 gunModelADS;
    public Vector3 gunModelDefaultPos;

    public float zoomMax;
    public int zoomInSpd;
    public int zoomOutSpd;
    public int adsSpd;
    public int notADSSpd;

    public int ammoCount;
    public float shtRate;
    public int shtDist;
    public int shtDmg;
    public GameObject gunModel;
    public AudioClip gunShotAud;
}
