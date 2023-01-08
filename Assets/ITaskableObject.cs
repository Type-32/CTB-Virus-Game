using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TaskSystem;

public interface ITaskableObject
{
    void OnObjectComsumed();
    void OnObjectSpawned();
    void ConsumeObject(PlayerController controller);
}
