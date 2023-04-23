using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AllySlot : MonoBehaviour, IDropHandler, IPointerExitHandler, IPointerEnterHandler
{
    #region VARIABLES

    [SerializeField]
    bool _empty = true;
    [SerializeField]
    bool _pointerEnter = false;
    Image _image = null;
    Color32 _default = new Color32(255, 255, 255, 0);
    Color32 _highlighted = new Color32(255, 255, 255, 30);

    #endregion

    #region PROPERTIES

    public bool IsEmpty { get => _empty; set => _empty = value; }

    #endregion

    #region UNITY

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    //private void Update()
    //{
    //    if (!_empty)
    //        _image.color = Color.red;
    //    else
    //        _image.color = Color.white;
    //}

    #endregion

    #region INTERFACES

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount != 0) return;

        GameObject dropped = eventData.pointerDrag;
        DraggableAlly draggableAlly = dropped.GetComponent<DraggableAlly>();
        _empty = false;

        if (!draggableAlly) return;

        draggableAlly.ParentAfterDrag = transform;       
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _image.color = _highlighted;
    }

    //Note Optimization : This is triggered whenever the pointer goes out. Use something less costly like Drag for this.
    public void OnPointerExit(PointerEventData eventData)
    {
        _image.color = _default;

        if (transform.childCount == 0)
            _empty = true;
    }

    #endregion
}
