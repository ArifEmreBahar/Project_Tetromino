using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShadow : MonoBehaviour
{
    public Vector2 offset = new Vector2(-3, -3);

    SpriteRenderer rndCaster;
    SpriteRenderer rndShadow;

    Transform caster;
    Transform shadow;

    public Color shadowColor;

    void Start()
    {
        caster = transform;
        shadow = new GameObject().transform;
        shadow.parent = caster;
        shadow.gameObject.name = "shadow";

        rndCaster = GetComponent<SpriteRenderer>();
        rndShadow = shadow.gameObject.AddComponent<SpriteRenderer>();

        rndShadow.transform.localRotation = Quaternion.identity;
        rndShadow.transform.localScale = Vector3.one;
        rndShadow.color = shadowColor;
        rndShadow.sortingLayerName = rndCaster.sortingLayerName;
        rndShadow.sortingOrder = rndCaster.sortingOrder - 1;
    }

    void LateUpdate()
    {
        shadow.position = new Vector3(caster.position.x + offset.x,
            caster.position.y + offset.y, caster.position.z);

        rndShadow.sprite = rndCaster.sprite;
    }
}
