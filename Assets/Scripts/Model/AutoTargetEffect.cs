using SerializeReferenceEditor;
using UnityEngine;

[System.Serializable]
public class AutoTargetEffect
{
    [field: SerializeReference, SR] public TargetMode TargetMode { get; private set; }
    //대상에게 적용할 effect
    [field: SerializeReference, SR] public Effect Effect { get; private set; }
}
