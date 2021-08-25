using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    bool shouldFadeFromBlack;
    bool shouldFadeToBlack;

    public Image UIFadeImage;

    public static UIFade instance;
    void Start()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);

        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldFadeFromBlack)
        {
            
            UIFadeImage.color = new Color(UIFadeImage.color.r, UIFadeImage.color.g, UIFadeImage.color.b, Mathf.MoveTowards(UIFadeImage.color.a, 0f, 2 * Time.deltaTime));

            if(UIFadeImage.color.a == 0)
            {
                UIFadeImage.gameObject.SetActive(false);
                shouldFadeFromBlack = false;
            }
        }

        if (shouldFadeToBlack)
        {
            UIFadeImage.gameObject.SetActive(true);
            UIFadeImage.color = new Color(UIFadeImage.color.r, UIFadeImage.color.g, UIFadeImage.color.b, Mathf.MoveTowards(UIFadeImage.color.a, 1f, 2 * Time.deltaTime));

            if (UIFadeImage.color.a == 1)
            {
                shouldFadeToBlack = false;
                
            }
        }
    }

    public void fadeToBlack()
    {
        shouldFadeToBlack = true;
        shouldFadeFromBlack = false;
    }

    public void fadeFromBlack()
    {
        shouldFadeFromBlack = true;
        shouldFadeToBlack = false;
    }
}
