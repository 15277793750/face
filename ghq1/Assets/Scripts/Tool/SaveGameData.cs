using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

public class SaveGameData : MySingleton<SaveGameData>
{
	public bool StartTime = false;

	public float time = 0;

	void Update()
	{
		if (StartTime)
		{
			time += Time.deltaTime;
		}
	}

	/// <summary>
	/// 设置是否开始计时
	/// </summary>
	public void SetStartTime(bool b)
	{
		StartTime = b;
	}

	/// <summary>
	/// 记录游戏时间
	/// </summary>
	public void SaveGameTime(string fileName)
	{
		File.AppendAllText(fileName, time.ToString(CultureInfo.InvariantCulture) + Environment.NewLine, Encoding.UTF8);
		time = 0;
	}
}
