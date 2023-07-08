using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    //public TextMeshProUGUI levelText;
    //public Slider hpSlider;
    public Image healthBar;

    private float _maxHP;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        //levelText.text = "Lvl " + unit.unitLevel;
        _maxHP = unit.maxHP;
        healthBar.fillAmount = unit.currentHP / _maxHP;
    }

    public void SetHP(int hp)
    {
        healthBar.fillAmount = hp / _maxHP;
    }
    
}
