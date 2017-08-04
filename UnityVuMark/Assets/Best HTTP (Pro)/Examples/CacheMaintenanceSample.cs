/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using System;
using System.Collections.Generic;

using UnityEngine;
using BestHTTP;
using BestHTTP.Caching;

public sealed class CacheMaintenanceSample : MonoBehaviour
{
    /// <summary>
    /// An enum for better readability
    /// </summary>
    enum DeleteOlderTypes
    {
        Days,
        Hours,
        Mins,
        Secs
    };

    #region Private Fields

    /// <summary>
    /// What methode to call on the TimeSpan
    /// </summary>
    DeleteOlderTypes deleteOlderType = DeleteOlderTypes.Secs;

    /// <summary>
    /// The value for the TimeSpan.
    /// </summary>
    int value = 10;

    /// <summary>
    /// What's our maximum cache size
    /// </summary>
    int maxCacheSize = 5 * 1024 * 1024;

    #endregion

    #region Unity Events

    void OnGUI()
    {
        GUIHelper.DrawArea(GUIHelper.ClientArea, true, () =>
            {
                GUILayout.BeginHorizontal();
                                                    
                    GUILayout.Label("Delete cached entities older then");

                    GUILayout.Label(value.ToString(), GUILayout.MinWidth(50));
                    value = (int)GUILayout.HorizontalSlider(value, 1, 60, GUILayout.MinWidth(100));

                    GUILayout.Space(10);

                    deleteOlderType = (DeleteOlderTypes)(int)GUILayout.SelectionGrid((int)deleteOlderType, new string[] { "Days", "Hours", "Mins", "Secs" }, 4);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                    GUILayout.Label("Max Cache Size (bytes): ", GUILayout.Width(150));
                    GUILayout.Label(maxCacheSize.ToString("N0"), GUILayout.Width(70));
                    maxCacheSize = (int)GUILayout.HorizontalSlider(maxCacheSize, 1024, 10 * 1024 * 1024);
                GUILayout.EndHorizontal();

                GUILayout.Space(10);

                if (GUILayout.Button("Maintenance"))
                {
                    TimeSpan deleteOlder = TimeSpan.FromDays(14);

                    switch (deleteOlderType)
                    {
                        case DeleteOlderTypes.Days: deleteOlder = TimeSpan.FromDays(value); break;
                        case DeleteOlderTypes.Hours: deleteOlder = TimeSpan.FromHours(value); break;
                        case DeleteOlderTypes.Mins: deleteOlder = TimeSpan.FromMinutes(value); break;
                        case DeleteOlderTypes.Secs: deleteOlder = TimeSpan.FromSeconds(value); break;
                    }

                    // Call the BeginMaintainence function. It will run on a thread to do not block the main thread.
                    HTTPCacheService.BeginMaintainence(new HTTPCacheMaintananceParams(deleteOlder, (ulong)maxCacheSize));
                }
            });
    }

    #endregion
}
