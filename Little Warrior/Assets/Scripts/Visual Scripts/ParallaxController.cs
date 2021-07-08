using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ParallaxController : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    public Transform player;

    Vector2 startPosition;
    Vector2 travel => (Vector2) cam.transform.position - startPosition;

    float distanceFromPlayer => transform.position.z - player.transform.position.z;
    float clippingPlane => cam.transform.position.z + (distanceFromPlayer > 0 ? cam.m_Lens.FarClipPlane : cam.m_Lens.NearClipPlane);

    float parallaxFactor => Mathf.Abs(distanceFromPlayer) / clippingPlane;

    float startZ;

    float twoAspect => cam.m_Lens.Aspect * 2;
    float tileWidth => (twoAspect > 3 ? twoAspect : 3);
    float viewWidth => loopSpriteRenderer.sprite.rect.width / loopSpriteRenderer.sprite.pixelsPerUnit;
    public SpriteRenderer loopSpriteRenderer;

    public bool axisX;
    public bool axisY;
    public bool InfiniteLoop;


    void Start()
    {
        startPosition = transform.position;
        startZ = transform.position.z;

        if(loopSpriteRenderer != null && InfiniteLoop)
        {
            float spriteSizeX = loopSpriteRenderer.sprite.rect.width / loopSpriteRenderer.sprite.pixelsPerUnit;
            float spriteSizeY = loopSpriteRenderer.sprite.rect.height / loopSpriteRenderer.sprite.pixelsPerUnit;

            loopSpriteRenderer.drawMode = SpriteDrawMode.Tiled;
            loopSpriteRenderer.size = new Vector2(spriteSizeX * tileWidth, spriteSizeY);
            //transform.localScale = Vector3.one;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPos = startPosition + travel * parallaxFactor;
        transform.position = new Vector3(axisX ? newPos.x: startPosition.x, axisY? newPos.y : startPosition.y, startZ);

        if (InfiniteLoop)
        {
            Vector2 totalTravel = cam.transform.position - transform.position;
            float boundOffset = (viewWidth / 2) * (totalTravel.x > 0 ? 1 : -1);
            float screens = (int)((totalTravel.x + boundOffset) / viewWidth);
            transform.position += new Vector3(screens * viewWidth, 0);
        }
    }
}
