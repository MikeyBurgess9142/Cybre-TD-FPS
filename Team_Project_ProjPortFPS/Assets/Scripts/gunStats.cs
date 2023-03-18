using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class gunStats : ScriptableObject
{
    [Header("Gun Transforms")]
    public Vector3 gunPosition;
    public Vector3 gunRotation;
    public Vector3 gunScale;
    public Vector3 gunModelADS;
    public Vector3 gunModelDefaultPos;

    [Header("Gun Aiming")]
    public float zoomMaxFov;
    public int zoomInSpd;
    public int zoomOutSpd;
    public int adsSpd;
    public int notADSSpd;

    [Header("Gun Stats")]
    public int ammoCount;
    public float shtRate;
    public int shtDist;
    public int shtDmg;
    public GameObject gunModel;
    public AudioClip gunShotAud;
}
