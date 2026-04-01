using UnityEngine;
using System.Collections;

public class PlayerStatsSystem : MonoBehaviour 
{ 
    [field: SerializeField] 
    public PlayerHealth PlayerHealth { get; private set; } 
    [field: SerializeField]
    public PlayerMovement PlayerMovement { get; private set; }
    [field: SerializeField] 
    public PlayerInventory PlayerInventory { get; private set; } 
    public Player playerBaseStats;
    public PlayerInventory playerInventory;


    private void OnEnable(){
        PlayerInventory.OnInventoryChanged += ApplyAllItemEffects;
    }

    private void OnDisable(){
        PlayerInventory.OnInventoryChanged -= ApplyAllItemEffects;
    }


    public void ApplyAllItemEffects() 
    { 
        PlayerHealth.maxHealth = playerBaseStats.maxHealth;
        PlayerHealth.regenRate = playerBaseStats.regenRate;
        foreach (var entry in PlayerInventory.inventory) 
        { 
            ItemData data = entry.Value.itemData; 
            int currentStacks = entry.Value.stackSize; 
            foreach (var effect in data.Effects) 
            { 
                if (effect != null) 
                { 
                    effect.Apply(this, currentStacks); 
                } 
            }   
            foreach (var gunEffect in data.GunEffects) 
            { 
                GunStatsSystem activeGun = GetComponentInChildren<GunStatsSystem>();
                if (gunEffect != null) 
                { 
                    gunEffect.Apply(activeGun, currentStacks); 
                } 
            }
        } 
        Debug.Log("Efekty przedmiotów zostały nałożone.");
    } 

    public void DebugStats()
    { 
        if(Input.GetKeyDown(KeyCode.P))
        { 
            ApplyAllItemEffects();
            Debug.Log($"Health: {PlayerHealth.currentHealth}/{PlayerHealth.maxHealth}"); 
        } 
    } 

    void Update() 
    { 
        PlayerHealth.HealthRegen();
        DebugStats(); 
    } 
}