using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[SerializeField]
public class MissionDatabase : ScriptableObject
{
    public List<Mission> missionList = new List<Mission>();

    public List<Mission> findByStatus(Mission.Status _status){
        return missionList.FindAll(x => x.status == _status);
    }

    public Mission findById(int _id){
        return missionList.Find(x => x.id == _id);
    }
}
