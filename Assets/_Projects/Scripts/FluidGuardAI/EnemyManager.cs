using Unity.Netcode;
using UnityEngine;

public class EnemyManager : NetworkBehaviour
{
    private int _healthpoints;

    private void Awake()
    {
        _healthpoints = 10;
    }

    public bool TakeHit()
    {
        _healthpoints -= 10;
        bool isDead = _healthpoints <= 0;
        if (isDead) DieServerRpc();
        return isDead;
    }

    [ServerRpc(RequireOwnership = false)]
    private void DieServerRpc()
    {
        Destroy(gameObject);
    }
}
