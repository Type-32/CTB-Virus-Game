using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskSystem;
using UnityEngine;

public interface ITaskDispatcher
{
    void OnSendTask();

    void OnAccomplishedTask();

    void SendTask();
}
