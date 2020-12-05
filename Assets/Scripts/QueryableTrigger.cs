using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueryableTrigger : MonoBehaviour
{
    private HashSet<Collider> overlaps = new HashSet<Collider>();

    public HashSet<Collider> getOverlaps()
    {
        overlaps.RemoveWhere(x => x == null);
        return overlaps;
    }

    private void OnTriggerEnter(Collider other)
    {
        overlaps.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        overlaps.Remove(other);
    }
}
