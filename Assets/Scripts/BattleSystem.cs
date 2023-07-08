using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState {START, PLAYERTURN, ENEMYTURN, WON, LOST};

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemy1Prefab;
    public GameObject enemy2Prefab;
    public GameObject enemy3Prefab;

    public Transform playerBattleStation;
    public Transform enemy1BattleStation;
    public Transform enemy2BattleStation;
    public Transform enemy3BattleStation;

    Unit playerUnit;
    Unit enemy1Unit;
    Unit enemy2Unit;
    Unit enemy3Unit;
    
    public TextMeshProUGUI dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemy1HUD;
    public BattleHUD enemy2HUD;
    public BattleHUD enemy3HUD;


    public BattleState state;

    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGo = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGo.GetComponent<Unit>();

        GameObject enemy1Go = Instantiate(enemy1Prefab, enemy1BattleStation);
        enemy1Unit = enemy1Go.GetComponent<Unit>();
        GameObject enemy2Go = Instantiate(enemy2Prefab, enemy2BattleStation);
        enemy2Unit = enemy2Go.GetComponent<Unit>();
        GameObject enemy3Go = Instantiate(enemy3Prefab, enemy3BattleStation);
        enemy3Unit = enemy3Go.GetComponent<Unit>();

        dialogueText.text = "Wild " + enemy1Unit.unitName + ", " + enemy2Unit.unitName + " and " + enemy3Unit.unitName + " approaches ...";

        playerHUD.SetHUD(playerUnit);
        enemy1HUD.SetHUD(enemy1Unit);
        enemy2HUD.SetHUD(enemy2Unit);
        enemy3HUD.SetHUD(enemy3Unit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }
/*
    IEnumerator PlayerAttack(Unit enemyUnit)
    {
        //Damage the enemy
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "the attack is succesful";

        state = BattleState.ENEMYTURN;
        yield return new WaitForSeconds(2f);

        //check if the enemy is dead
        if(isDead)
        {
            //End the Battle
            state = BattleState.WON;
            EndBattle();
        }
        else
        {    
            //Enemy's turn
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemiesTurn());
        }   
    }
*/
    IEnumerator PlayerAttack(Unit enemyUnit,BattleHUD enemyHUD)
    {

        //check if the attacked enemy is aleady dead
        if(enemyUnit.currentHP == 0)
        {
            dialogueText.text = "the enemy is aleardy dead, choose an other one";
            yield return new WaitForSeconds(2f);

            PlayerTurn();
        }
        else
        {    
            //Damage the enemy
            enemyUnit.TakeDamage(playerUnit.damage);

            enemyHUD.SetHP(enemyUnit.currentHP);
            dialogueText.text = "the attack is succesful";


            yield return new WaitForSeconds(2f);
        
        }   
        

        //check if all enemies are dead
        if(areDead())
        {
            //End the Battle
            state = BattleState.WON;
            EndBattle();
        }
        else
        {    
            //Enemy's turn
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemiesTurn());
        }   
    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(5);

        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = "You feel renewed strength!";


        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemiesTurn());
           
    }
    /*
    IEnumerator EnemiesTurn()
    {
        dialogueText.text = enemyUnit.unitName + " attacks!";

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }*/

    IEnumerator EnemiesTurn()
    {
        bool isDead = EnemyAction(enemy1Unit);
        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else 
        {
            yield return new WaitForSeconds(2f);
            isDead = EnemyAction(enemy2Unit);
            if (isDead)
            {
                state = BattleState.LOST;
                EndBattle();
            }
            else
            {
                yield return new WaitForSeconds(2f);
                isDead = EnemyAction(enemy3Unit);
                if (isDead)
                {
                    state = BattleState.LOST;
                    EndBattle();
                }
                else
                {
                    state = BattleState.PLAYERTURN;
                    PlayerTurn();
                }
            }
        }
        yield return new WaitForSeconds(2f);



    }

    bool EnemyAction(Unit enemyUnit)
    {
        if (enemyUnit.currentHP == 0)
        {
            return false;
        }
        else
        {
            dialogueText.text = enemyUnit.unitName + " attacks!";

            //yield return new WaitForSeconds(1f);

            bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

            playerHUD.SetHP(playerUnit.currentHP);

            //yield return new WaitForSeconds(1f);

            return isDead;
        }
    }

    bool areDead()
    {
        if ((enemy1Unit.currentHP == 0)&&(enemy2Unit.currentHP == 0)&&(enemy3Unit.currentHP == 0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "Heroes lost the battle!";
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated...";
        }
    }

    void PlayerTurn()
    {
        dialogueText.text = "What you gonna do about it :";
    }
/*
    public void OnAttackButton()
    {
        if(state != BattleState.PLAYERTURN)
            return;
        
        StartCoroutine(PlayerAttack());
    }

*/

    public void OnAttack1Button()
    {
        if(state != BattleState.PLAYERTURN)
            return;
        
        StartCoroutine(PlayerAttack(enemy1Unit, enemy1HUD));
    }

    public void OnAttack2Button()
    {
        if(state != BattleState.PLAYERTURN)
            return;
        
        StartCoroutine(PlayerAttack(enemy2Unit, enemy2HUD));
    }

    public void OnAttack3Button()
    {
        if(state != BattleState.PLAYERTURN)
            return;
        
        StartCoroutine(PlayerAttack(enemy3Unit, enemy3HUD));
    }
    public void OnHealButton()
    {
        if(state != BattleState.PLAYERTURN)
            return;
        
        StartCoroutine(PlayerHeal());
    }


}
