using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TaskSystem;
using NPCDependencies;

public class TaskObjectScript : MonoBehaviour, ITaskableObject
{
    public TaskableObjectInfo taskInformation;
    public void ConsumeObject(PlayerController controller){
        controller.ReceiveTaskableObject(taskInformation);
    }
    public void OnObjectComsumed(){

    }
    public void OnObjectSpawned(){

    }
}
