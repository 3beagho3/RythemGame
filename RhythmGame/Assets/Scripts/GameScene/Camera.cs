using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState
{
    Default , Shake
}

public class Camera : MonoBehaviour
{
    private GameObject player;
    private Vector3 cameraStartPos;

    public float cameraFollowTime = 0.1f;

    [Header("CameraShake")]
    public float cameraShakeAmount = 0.1f;
    public float cameraShakeTime = 0.1f;
    private float cameraShakeTimer = 0;

    private CameraState cameraState = CameraState.Default;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cameraStartPos = transform.position;
    }

    void Update()
    {
        if(cameraState.Equals(CameraState.Default)) DefaultMove();
    }

    void DefaultMove()
    {
        Vector3 movePos = player.transform.position;
        movePos.z = cameraStartPos.z;

        transform.position = Vector3.Lerp(transform.position , movePos , cameraFollowTime);
    }

    public IEnumerator CameraShake()
    {
        cameraState = CameraState.Shake;
        while(cameraShakeTimer < cameraShakeTime)
        {
            cameraShakeTimer += Time.deltaTime;

            float xRand = Random.Range(-cameraShakeAmount, cameraShakeAmount);
            float yRand = Random.Range(-cameraShakeAmount, cameraShakeAmount);

            transform.position = new Vector3(transform.position.x + xRand, transform.position.y + yRand, cameraStartPos.z);
            yield return null;
        }
        cameraShakeTimer = 0;
        transform.position = cameraStartPos;
        cameraState = CameraState.Default;
    }
}
