using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;

    public int damage;

    public int maxHP;
    public int currentHP;

    private Vector3 originalPos;

    private void Start() {
        originalPos = transform.position;
    }

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;
        StartCoroutine(blink());
        StartCoroutine(Shake(0.2f,0.1f));

        if(currentHP <= 0)
        {
            currentHP = 0;
            GetComponent<Image>().color = new Color32(100,100,100,255);
            return true;
        }
        else
            return false;        
    }
    
    public void Heal(int amount)
    {
        currentHP += amount;
        if(currentHP > maxHP)
            currentHP = maxHP;
        
    }

    public IEnumerator blink(){
        float elapsedTime = 0f;
        float waitTime = 0.1f; 
        bool blink = true;
        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.fixedDeltaTime;
            if(blink){
                GetComponent<Image>().color = new Color32(255,75,75,255);
               
            }
            else{
                GetComponent<Image>().color = new Color32(255,255,255,255);
            }
            blink = !blink;
            // Yield here
            yield return new WaitForSeconds(0.07f);
        }  
        // Make sure we got there
       
        GetComponent<Image>().color = new Color32(255,255,255,255);
        if(currentHP <= 0)
        {
            currentHP = 0;
            GetComponent<Image>().color = new Color32(100,100,100,255);
        }

        yield return new WaitForSeconds(4f);
    }

    public IEnumerator Shake (float duration, float magnitude)
    {

        float elapsed = 0.0f;

        while ( elapsed < duration){
            float x = Random.Range(-1f,1f) * magnitude ;
            //float y = Random.Range(-1f,1f) * magnitude ;
            
            transform.position = new Vector3(originalPos.x+x, originalPos.y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPos;
    }

}
