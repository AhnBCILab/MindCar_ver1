using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cop_CameraControl : MonoBehaviour
{
    public GameObject Player;
    public Vector3 fix = new Vector3(0, 3, -6);  // The fixed position value for Car
    Transform PlayerT;

    private float pre_value;
    public float CarVelocity;

    void Start()
    {
        PlayerT = Player.transform;
    }
    void Update()
    {   // 새로운 위치는 두번째 argument의 방향으로 둘 사이의 거리 * 전 프레임에 걸린시간 만큼 이동하게 되는 것.
        pre_value = transform.position.z;
        transform.position = Vector3.Lerp(transform.position, PlayerT.position + fix, 2f * Time.deltaTime);

        CarVelocity = transform.position.z - pre_value;  // for car audio

    }


}