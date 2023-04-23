using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableAlly : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region VARIABLES

    Transform _parentAfterDrag;
    RectTransform _rectTransform;
    Image _image;
    Canvas _canvas;

    #endregion

    #region PROPERTIES

    public Transform ParentAfterDrag { get => _parentAfterDrag; set => _parentAfterDrag = value;  }

    #endregion

    #region UNITY

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _canvas = GetComponentInParent<Canvas>();
    }

    #endregion

    #region INTERFACES

    public void OnBeginDrag(PointerEventData eventData)
    {
        _parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        _image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(_parentAfterDrag);
        _image.raycastTarget = true;
    }

    #endregion
}
