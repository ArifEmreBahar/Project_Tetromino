using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    #region VARIABLES

    [SerializeField]
    Transform _slotHolder = null;
    List<AllySlot> _allySlots = new List<AllySlot>();

    #endregion

    #region UNITY

    void Awake()
    {
        FillAllySlots();
    }

    #endregion

    #region PRIVATE

    void FillAllySlots()
    {
        foreach (Transform transform in _slotHolder)
        {
            AllySlot allySlot = transform.GetComponent<AllySlot>();
            if(allySlot)
                _allySlots.Add(allySlot);
        }
    }

    #endregion

    #region PUBLIC

    public void AddSlotMember(AllySlot allySlot)
    {
        _allySlots.Add(allySlot);
    }

    public AllySlot GetEmptySlot()
    {
        foreach (AllySlot slot in _allySlots)
        {
            if (slot.IsEmpty)
                return slot;
        }

        return null;
    }

    #endregion
}
