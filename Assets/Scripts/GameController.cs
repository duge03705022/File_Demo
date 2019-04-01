using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    public TouchHandler touchHandler;
    public CardHandler cardHandler;

    public GameObject txtMask;

    public GameObject[] txtPrefabs;
    public bool ifPlaceSelect;
    public bool[] ifCopy;
    public int selectCount;
    public int sourceX;
    public int sourceY;


    // Start is called before the first frame update
    void Start()
    {
        ifPlaceSelect = false;
        selectCount = 0;
        sourceX = -1;
        sourceY = -1;

        ifCopy = new bool[5];
        for (int i = 0; i < 5; i++)
        {
            ifCopy[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetSelect()
    {
        selectCount = 0;
        for (int i = 0; i < 5; i++)
        {
            ifCopy[i] = false;
        }
        sourceX = -1;
        sourceY = -1;
    }

    //public IEnumerator CopyPhoto(int selectCount, Tuple<int, int>[] selectPhoto, Tuple<int, int> targetFile)
    //{
    //    for (int i = 0; i < selectCount; i++)
    //    {
    //        copyInstances[i] = Instantiate(photoSeries[selectPhoto[i].Item1 * RFIBParameter.stageRow + selectPhoto[i].Item2], copyParent.transform);
    //        copyInstances[i].transform.localPosition = new Vector3(
    //                selectPhoto[i].Item1 * GameParameter.stageGap,
    //                selectPhoto[i].Item2 * GameParameter.stageGap,
    //                0);
    //        copyInstances[i].GetComponent<SpriteRenderer>().sortingOrder = 40;
    //        StartCoroutine(MovePhoto(i, selectPhoto[i].Item1, selectPhoto[i].Item2, targetFile.Item1, targetFile.Item2));
    //        yield return new WaitForSeconds(0.5f);
    //    }

    //    StartCoroutine(touchHandler.ResetSelect(GameParameter.moveTime + 0.2f));
    //}

    //IEnumerator MovePhoto(int instanceId, int fromX, int fromY, int toX, int toY)
    //{
    //    for (int i = 0; i < GameParameter.moveStep; i++)
    //    {
    //        copyInstances[instanceId].transform.localPosition += new Vector3(
    //            (toX - fromX) * GameParameter.stageGap / GameParameter.moveStep,
    //            (toY - fromY) * GameParameter.stageGap / GameParameter.moveStep,
    //            0f);
    //        copyInstances[instanceId].transform.localScale -= new Vector3(
    //            0.7f / GameParameter.moveStep,
    //            0.7f / GameParameter.moveStep,
    //            0.7f / GameParameter.moveStep);
    //        yield return new WaitForSeconds(GameParameter.moveTime / GameParameter.moveStep);
    //    }

    //    cardHandler.cardInstance[toX, toY].GetComponent<FileController>().AddPhoto(photoSeries[fromX * RFIBParameter.stageRow + fromY]);
    //    Destroy(copyInstances[instanceId]);
    //}
}
