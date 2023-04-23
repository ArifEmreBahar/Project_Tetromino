using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp
{
    public virtual Sprite Sprite { get; }

    public virtual string SpritePath { get; }

    public virtual bool CanGet { get; }

    public virtual string Description { get;}

    public virtual void Activate()
    {

    }
}
