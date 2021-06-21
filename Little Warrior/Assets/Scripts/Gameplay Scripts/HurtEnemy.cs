using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class HurtEnemy : MonoBehaviour
{
    public bool beingKnocked;
    float knockDur;
    Rigidbody2D rb;

    CinemachineVirtualCamera cineCam;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cineCam = FindObjectOfType<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hurtEnemyFunc(bool willDealKnock, float damage, float screenShakeIntensity, Vector2 knockback, Transform playerPos)
    {
        if (willDealKnock)
        {
            if(playerPos.position.x < rb.position.x)
            {
                
            }
            else
            {

            }

            if(playerPos.position.y > rb.position.y)
            {

            }
            else
            {

            }
        }

        //Add damage to enemy here

        //Add screen shake here
        shakeCamera(screenShakeIntensity);
        Invoke("DestroyEnemy", .7f);
        
    }

    void shakeCamera(float shake)
    {
        StartCoroutine(shakeScreen(shake));
    }

    public IEnumerator shakeScreen(float noise)
    {
        CinemachineBasicMultiChannelPerlin cinemachineNoise = cineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineNoise.m_AmplitudeGain = noise;
        yield return new WaitForSeconds(.6f);
        cinemachineNoise.m_AmplitudeGain = 0f;
    }

    void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }
}
