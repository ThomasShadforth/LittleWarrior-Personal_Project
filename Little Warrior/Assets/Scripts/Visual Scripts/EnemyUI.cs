using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    EnemyAi baseEnemyObj;
    public Image enemyHealth;
    // Start is called before the first frame update
    void Start()
    {
        baseEnemyObj = GetComponentInParent<EnemyAi>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateHealth()
    {
        enemyHealth.fillAmount = baseEnemyObj.health / baseEnemyObj.MaxHealth;

        if(enemyHealth.fillAmount < .4)
        {
            enemyHealth.color = Color.red;
        }
        else
        {
            enemyHealth.color = Color.green;
        }
    }
}
