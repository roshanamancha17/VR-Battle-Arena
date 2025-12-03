using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRButtonTroop : MonoBehaviour
{
    public TroopSpawnerVR spawner;
    public enum TroopKind { Knight, Archer, Tank }
    public TroopKind type;

    public void PressButton()
    {
        if (type == TroopKind.Knight) spawner.SpawnKnight();
        if (type == TroopKind.Archer) spawner.SpawnArcher();
        if (type == TroopKind.Tank)  spawner.SpawnTank();
    }
}
