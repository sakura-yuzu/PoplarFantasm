using Cysharp.Threading;
using Cysharp.Threading.Tasks;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using TMPro;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

class MissionList : MonoBehaviour
{
	private Mission.Status status;
	private MissionDatabase missionDatabase;
	private List<Mission> missionList;
	private MissionManager missionManager;
	private Transform missionTable;

	public MissionList(Transform _missionTable, MissionDatabase _missionDatabase, Mission.Status _status)
	{
		missionDatabase = _missionDatabase;
		missionTable = _missionTable;
		status = _status;
		missionManager = new MissionManager(missionDatabase);
		Prepare();
	}

	private async UniTask Prepare()
	{
		missionList = missionManager.findByStatus(status);
		var missionRowPrefab = await Addressables.LoadAssetAsync<GameObject>("MissionRow").Task;

		foreach (Mission mission in missionList)
		{
			var line = Instantiate(missionRowPrefab, missionTable, false);
			line.transform.Find("MissionTitle").GetComponent<TextMeshProUGUI>().text = mission.title;
			// var entry = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "TextData", tableEntryReference: mission.missionName);
			// GameObject missionCell = Instantiate(missionCellPrefab, line.transform);
			// missionCell.transform.Find("Image").GetComponent<Image>().sprite =  mission.image;
		}
	}
}