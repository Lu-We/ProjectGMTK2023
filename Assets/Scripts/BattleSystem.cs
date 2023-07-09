using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState {START, PLAYERTURN, ENEMYTURN, WON, LOST, ATTACKING};

public class BattleSystem : MonoBehaviour
{
    public AudioManager audioManager;
    public MusicManager musicManager;
    public GameObject playerPrefab;
    public GameObject enemy1Prefab;
    public GameObject enemy2Prefab;
    public GameObject enemy3Prefab;

    public GameObject FireBall;
    public Transform FireBallSpawn;

    public Button Button1;
    public Button Button2;
    public Button Button3;
    public Button Button4;

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
        DisableButton();
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

        yield return new WaitForSeconds(1.5f);

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
        state = BattleState.ATTACKING;
        //check if the attacked enemy is aleady dead
        if(enemyUnit.currentHP == 0)
        {
            dialogueText.text = "the enemy is aleardy dead, choose an other one";
            yield return new WaitForSeconds(1.2f);
            
            state = BattleState.PLAYERTURN;
            
        }
        else
        {    
            Destroy(Instantiate(FireBall, FireBallSpawn),0.75f);
            yield return new WaitForSeconds(0.75f);
            //Damage the enemy
            enemyUnit.TakeDamage(playerUnit.damage);

            enemyHUD.SetHP(enemyUnit.currentHP);
            dialogueText.text = "the attack is succesful";


            yield return new WaitForSeconds(1f);
        
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
            
            if( state == BattleState.PLAYERTURN){
                //Restart player turn
                PlayerTurn();
            }
            else{
                //Enemy's turn
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemiesTurn());
            }
        }   
    }

    IEnumerator PlayerHeal()
    {
        audioManager.playHealSFX();

        playerUnit.Heal(50);

        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = "You feel renewed strength!";


        yield return new WaitForSeconds(1.1f);

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
        audioManager.playEnnemyTurnSFX();
        yield return new WaitForSeconds(0.2f);

        audioManager.PlayArcherAtkSFX();
        yield return new WaitForSeconds(0.75f);
        bool isDead = EnemyAction(enemy1Unit);
        yield return new WaitForSeconds(0.3f);
        if (enemy1Unit.currentHP != 0) enemy1Unit.transform.position += new Vector3(1,0,0);
        yield return new WaitForSeconds(0.8f);
        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else 
        {
            
            isDead = EnemyAction(enemy2Unit);
            yield return new WaitForSeconds(0.3f);
            if (enemy2Unit.currentHP != 0) enemy2Unit.transform.position += new Vector3(1,0,0);
            yield return new WaitForSeconds(1.1f);
            if (isDead)
            {
                state = BattleState.LOST;
                EndBattle();
            }
            else
            {
                isDead = EnemyAction(enemy3Unit);
                yield return new WaitForSeconds(0.3f);
                if (enemy3Unit.currentHP != 0) enemy3Unit.transform.position += new Vector3(1,0,0);
                yield return new WaitForSeconds(1.1f);
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
        yield return new WaitForSeconds(0.2f);



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

            if(enemyUnit.unitName == "Mage") audioManager.PlayMageSFX();
            else if (enemyUnit.unitName == "Knight") audioManager.PlayKnightAtkSFX();
            //else if (enemyUnit.unitName == "Archer") audioManager.PlayArcherAtkSFX();

            enemyUnit.transform.position += new Vector3(-1,0,0);
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
            musicManager.StopMusic();
            audioManager.PlayWinSFX();
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated...";
            musicManager.StopMusic();
        }
    }

    void PlayerTurn()
    {   
        audioManager.playPlayerTurnSFX();
        Button1.interactable = true;
        Button2.interactable = true;
        Button3.interactable = true;
        Button4.interactable = true;
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

    void DisableButton(){
        Button1.interactable = false;
        Button2.interactable = false;
        Button3.interactable = false;
        Button4.interactable = false;
    }
    public void OnAttack1Button()
    {
        if(state != BattleState.PLAYERTURN)
            return;
        
        DisableButton();
        audioManager.PlayClickSFX();
        StartCoroutine(PlayerAttack(enemy1Unit, enemy1HUD));
    }

    public void OnAttack2Button()
    {
        if(state != BattleState.PLAYERTURN)
            return;
        
        DisableButton();
        audioManager.PlayClickSFX();
        StartCoroutine(PlayerAttack(enemy2Unit, enemy2HUD));
    }

    public void OnAttack3Button()
    {
        if(state != BattleState.PLAYERTURN)
            return;
        
        DisableButton();
        audioManager.PlayClickSFX();
        StartCoroutine(PlayerAttack(enemy3Unit, enemy3HUD));
    }
    public void OnHealButton()
    {
        if(state != BattleState.PLAYERTURN)
            return;
        
        DisableButton();
        audioManager.PlayClickSFX();
        StartCoroutine(PlayerHeal());
    }


}
