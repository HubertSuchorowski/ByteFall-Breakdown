using UnityEngine; 

public class PlayerManager : MonoBehaviour
{
   [field: SerializeField]
   public PlayerMovement PlayerMovement { get; private set;}
   [field: SerializeField]
   public PlayerStatsSystem PlayerStatsSystem { get; private set; }
   [field: SerializeField]
   public GunsMenager GunsMenager { get; private set; }
   [field: SerializeField]
   public WaveManager WaveMenager { get; private set; }
   [field: SerializeField]
   public PlayerInventory PlayerInventory { get; private set; }
}
