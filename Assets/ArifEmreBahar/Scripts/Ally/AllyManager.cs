using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyManager : MonoBehaviour
{
    #region VARIABLES

    List<BlockAlly> _allies = new List<BlockAlly>();

    #endregion

    #region PROPERTIES

    public List<BlockAlly> Allies { get => _allies; }

    #endregion

    #region PUBLIC

    public void AddAlly(BlockAlly ally)
    {
        _allies.Add(ally);
    }

    public void RemoveAlly(BlockAlly ally)
    {
        _allies.Remove(ally);
    }

    public void RemoveAll()
    {
        _allies.Clear();
    }

    public void PauseAll()
    {
        foreach (BlockAlly ally in _allies)
            ally.Pause();
    }

    public void ResumeAll()
    {
        foreach (BlockAlly ally in _allies)
            ally.Resume();
    }

    public void DestroyAll()
    {
        foreach (BlockAlly ally in _allies)
            Destroy(ally.gameObject);

        RemoveAll();
    }

    public void ModifyRangeAll(float multiplayer)
    {
        foreach (BlockAlly ally in _allies)
            ally.DetectionRange *= multiplayer;
    }

    #endregion
}
