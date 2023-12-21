using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    protected MissionDatabase missionDatabase;

    public MissionManager(MissionDatabase _missionDatabase){
        missionDatabase = _missionDatabase;
    }

    public void AddMissionData(Mission mission)
    {
        missionDatabase.missionList.Add(mission);
    }

    public Mission findById(int id){
        return missionDatabase.findById(id);
    }

    public List<Mission> findByStatus(Mission.Status status){
        return missionDatabase.findByStatus(status);
    }
}
