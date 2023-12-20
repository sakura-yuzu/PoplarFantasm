using Cysharp.Threading.Tasks;
using ScenarioFlow;
using System;
using System.Threading;
using UnityEngine;

[ScenarioMethod("mission")]
class MissionReceiver : IReflectable
{
	private MissionManager missionManager;

	public MissionReceiver(MissionManager _missionManager){
		missionManager = _missionManager;
	}

	[ScenarioMethod("receive")]
	public void Receive(int missionID)
	{
		Mission mission = missionManager.findById(missionID);
		mission.Receive();
	}
}