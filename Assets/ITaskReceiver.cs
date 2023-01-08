using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TaskSystem;

public interface ITaskReceiver
{
    void OnReceiveTask();
    void OnFinishTask();
    void ReceiveTask();
}
