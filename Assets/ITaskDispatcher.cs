using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TaskSystem;

public interface ITaskDispatcher
{
    void OnSendTask();
    void OnAccomplishedTask();
    void SendTask();
}
