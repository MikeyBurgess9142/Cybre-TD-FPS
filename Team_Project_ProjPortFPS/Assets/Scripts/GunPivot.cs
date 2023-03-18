using UnityEngine;

public class GunPivot : MonoBehaviour
{
    [SerializeField] Vector3 gunPivotPosition;
    [SerializeField] Vector3 gunPivotRotation;

    void Start()
    {
        transform.localPosition = gunPivotPosition;
        transform.localEulerAngles = gunPivotRotation;
    }
}
