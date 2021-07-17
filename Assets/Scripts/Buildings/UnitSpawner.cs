using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject _unitPrefab = null;
    [SerializeField] Transform _unitSpawnLocation = null;

    #region Server
    [Command]
    private void CmdSpawnUnit()
    {
        GameObject unitInstance = Instantiate(_unitPrefab, _unitSpawnLocation.position, Quaternion.identity);

        NetworkServer.Spawn(unitInstance, connectionToClient);
    }
    #endregion

    #region Client
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) { return; }

        if (!hasAuthority) { return; }

        CmdSpawnUnit();
    }
    #endregion
}
