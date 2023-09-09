using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new equip", menuName = "ScriptableObjects/Equip")]
public class EquipmentItemSO : Item
{
    public ItemDropManager.RarityDropRates ItemRarity;
    public int maxHealth;
    public int att;
    public int speed;
    public float dropRate;
    public float goldMultiplier;
    public float expMultiplier;
    public float attackSpeed;
    //increase all skills you learn by 1/2/3

    public override string GetRarity()
    {
        return ItemRarity.ToString();
    }
    public void OnEquip(PlayerStats ps)
    {
        if (att != 0)
            ps.IncreaseAttack(att);
        if (maxHealth != 0)
            ps.IncreaseMaxHealth(maxHealth);
        if (speed != 0)
            ps.IncreaseSpeed(speed);
        if (attackSpeed != 0)
            ps.IncreaseAttackSpeed(attackSpeed);
        if (dropRate != 0)
            ItemDropManager.Instance.IncreaseDropRate(dropRate);
        if (goldMultiplier != 0)
            GoldManager.Instance.IncreaseGoldMultiplier(goldMultiplier);
        if (expMultiplier != 0)
            ExperienceManager.Instance.IncreaseExpMultiplier(expMultiplier);

    }
    public void OnUnequip(PlayerStats ps)
    {
        if (att != 0)
            ps.IncreaseAttack(-att);
        if (maxHealth != 0)
            ps.IncreaseMaxHealth(-maxHealth);
        if (speed != 0)
            ps.IncreaseSpeed(-speed);
        if (attackSpeed != 0)
            ps.IncreaseAttackSpeed(-attackSpeed);
        if (dropRate != 0)
            ItemDropManager.Instance.IncreaseDropRate(-dropRate);//give it negative might cause problems?
        if (goldMultiplier != 0)
            GoldManager.Instance.IncreaseGoldMultiplier(-goldMultiplier);
        if (expMultiplier != 0)
            ExperienceManager.Instance.IncreaseExpMultiplier(-expMultiplier);
    }
    public override void InitDescription()
    {
        description = "";
        if (att != 0)
            description += att + " Attack\n";
        if (maxHealth != 0)
            description += maxHealth + " Max health\n";
        if (speed != 0)
            description += speed + " Speed\n";
        if (attackSpeed != 0)
            description += attackSpeed + " Attack Speed\n";
        if (dropRate != 0)
            description += dropRate + "% Drop Rate\n";
        if (goldMultiplier != 0)
            description += goldMultiplier + "% Gold\n";
        if (expMultiplier != 0)
            description += expMultiplier + "% Exp\n";
    }
}
