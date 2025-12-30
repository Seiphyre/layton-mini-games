using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DragCancelReason
{
    ReleasedNoTarget,
    TargetRejected,
    CancelledByUser,
    CancelledBySystem
}
