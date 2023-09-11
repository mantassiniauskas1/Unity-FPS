using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class staminaManager : MonoBehaviour
{
    public float maxStamina;
    public float currentStamina;

    

    public PlayerMovement playerScript;
    public Image staminaBar;
    
    // Start is called before the first frame update
    void Start()
    { 
        playerScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        maxStamina = playerScript.staminaMax;
        currentStamina = playerScript.GetStamina();
        staminaBar.fillAmount = currentStamina / maxStamina;
    }
}
