using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ShopItemDropZone : DropZone 
{
    private void Awake()
    {
        AddValidationRule(ValidationUtils.IsOfType<PieceDefinition>);
    }
}
