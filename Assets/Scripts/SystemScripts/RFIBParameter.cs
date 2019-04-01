using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public static class RFIBParameter
{
    public static readonly int stageRow = 5;
    public static readonly int stageCol = 9;
    public static readonly int maxHight = 2;

    public static readonly int blockNum = stageRow * stageCol;

    public static readonly int touchRow = 15;
    public static readonly int touchCol = 27;
    public static readonly int notTouchGap = 30;
    public static readonly int maxTouch = 20;

    // 允許甚麼編號被接受
    public static readonly string[] AllowBlockType = {
        "9999",     // 99 floor
        "7101",     // 71 file 1
        "7201",     // 72 file 2
        "7601",     // 76 box
	};

    // RFIB_ID對應的instance_ID
    public static int SearchCard(string idStr)
    {
        switch (idStr)
        {
            case "7101": return 0;      // 71 file
            case "7201": return 1;      // 72 file
            case "7601": return 10;     // 76 box

            case "0000": return -1;
        }
        return -1;
    }
}
