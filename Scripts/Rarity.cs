using UnityEngine;
public static class Rarity
{
    public enum RarityType
    {
        Common = 1, Uncommon, Rare, Epic, Legendary
    }

    public readonly struct RarityStats
    {
        private RarityType Rarity { get; }
        private int DamageBonus { get; }
        private float AttackSpeedBonus { get; }
        
        public int GetDamageBonus()
        {
            return DamageBonus;
        }
        public float GetAttackSpeedBonus()
        {
            return AttackSpeedBonus;
        }
        public RarityType GetRarityType()
        {
            return Rarity;
                    
        }
                
        public RarityStats(RarityType r, int dmgBonus, float attSpdBonus)
        {
            Rarity = r;
            DamageBonus = dmgBonus;
            AttackSpeedBonus = attSpdBonus;
        }
       
    }

    public static RarityStats GetRarity()
    {
        int rand = Random.Range(1, 100);
        switch (rand)
        {
            case < 51:
                RarityStats currentStats = new RarityStats(RarityType.Common, 0, 1.0f);  
                return currentStats;
            case < 81:
                RarityStats currentStats2 = new RarityStats(RarityType.Uncommon, 1, 1.1f);  
                return currentStats2;
            case < 91:
                RarityStats currentStats3 = new RarityStats(RarityType.Rare, 2, 1.2f);  
                return currentStats3;
            case < 99:
                RarityStats currentStats4 = new RarityStats(RarityType.Epic, 3, 1.3f);  
                return currentStats4;
            case < 101:
                RarityStats currentStats5 = new RarityStats(RarityType.Legendary, 4, 1.5f);  
                return currentStats5;
            default:
                return new RarityStats();
        }
    }
    
}