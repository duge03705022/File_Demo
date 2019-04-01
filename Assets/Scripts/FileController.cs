using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileController : MonoBehaviour
{
    public GameController gameController;

    # region File Parameters 
    public GameObject txtParent;
    public GameObject[] txts;
    public GameObject noFile;
    public GameObject addFile;
    public bool ifOpen;
    public int txtCount;
    public int nowPage;

    # endregion

    // Start is called before the first frame update
    void Start()
    {
        ifOpen = false;

        ShowPage();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenSelect()
    {
        for (int i = 0; i < transform.Find("txtParent").childCount; i++)
        {
            txts[i].GetComponent<Txt>().box.SetActive(true);
        }

        gameController.ifPlaceSelect = true;
    }

    public void CloseSelect()
    {
        for (int i = 0; i < transform.Find("txtParent").childCount; i++)
        {
            txts[i].GetComponent<Txt>().box.SetActive(false);
        }

        gameController.ifPlaceSelect = false;
    }

    public IEnumerator SwipeTxt(string direction)
    {
        Vector3 movePos = new Vector3();
        if (direction == "Up")
        {
            movePos.Set(0, 3.96f / 10, 0);
            if (txtCount / 3 > nowPage)
            {
                yield return StartCoroutine(MoveTxt(movePos));
                nowPage++;
            }
            else
                yield return StartCoroutine(LittleMoveTxt(movePos));
        }
        else if (direction == "Down")
        {
            movePos.Set(0, -3.96f / 10, 0);
            if (nowPage > 0)
            {
                yield return StartCoroutine(MoveTxt(movePos));
                nowPage--;
            }
            else
                yield return StartCoroutine(LittleMoveTxt(movePos));
        }

        gameController.txtMask.SetActive(false);
        ShowPage();
    }

    private IEnumerator MoveTxt(Vector3 movePos)
    {
        for (int i = 0; i < 10; i++)
        {
            txtParent.transform.localPosition += movePos;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator LittleMoveTxt(Vector3 movePos)
    {
        for (int i = 0; i < 5; i++)
        {
            txtParent.transform.localPosition += movePos;
            yield return new WaitForSeconds(0.1f);
        }
        for (int i = 0; i < 5; i++)
        {
            txtParent.transform.localPosition -= movePos;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void SelectTxt(int pos, int x, int y)
    {
        int id = pos + nowPage * 3;
        if (!txts[id].GetComponent<Txt>().check.activeSelf)
        {
            txts[id].GetComponent<Txt>().check.SetActive(true);
            gameController.ifCopy[id] = true;
            gameController.selectCount++;
            gameController.sourceX = x;
            gameController.sourceY = y;
        }
        else
        {
            txts[id].GetComponent<Txt>().check.SetActive(false);
            gameController.ifCopy[id] = false;
            gameController.selectCount--;
            if (gameController.selectCount == 0)
            {
                gameController.sourceX = -1;
                gameController.sourceY = -1;
            }
        }
    }

    public void AddTxt()
    {
        for (int i = 0; i < 5; i++)
        {
            if (gameController.ifCopy[i])
            {
                txts[txtCount] = Instantiate(gameController.txtPrefabs[i], txtParent.transform);
                txts[txtCount].transform.localPosition = new Vector3(0, (float)-1.32 * txtCount, 0);
                txts[txtCount].SetActive(false);
                txtCount++;
            }
        }
        ResetCheck();
        gameController.ResetSelect();
    }

    public void ResetCheck()
    {
        for (int i = 0; i < txtCount; i++)
        {
            txts[i].GetComponent<Txt>().check.SetActive(false);
        }
    }

    //public IEnumerator SwipePage(bool ifHold, string direction)
    //{
    //    switch (direction)
    //    {
    //        case "Right":
    //            if (showPage < (txtCount - 1) / 9)
    //            {
    //                showPage = showPage + 1;
    //            }
    //            break;
    //        case "Left":
    //            if (showPage > 0)
    //            {
    //                showPage = showPage - 1;
    //            }
    //            break;
    //        default:
    //            break;
    //    }

    //    //if (showPage != hidePage)
    //    //{
    //    //    yield return StartCoroutine(MovePhoto(direction));
    //    //}
    //    //else
    //    //{
    //    //    yield return StartCoroutine(LittleMovePhoto(direction));
    //    //}
    //}

    public void ShowAllTxt()
    {
        for (int i = 0; i < txtCount; i++)
        {
            txts[i].SetActive(true);
        }
    }

    public void ShowPage()
    {
        for (int i = 0; i < txtCount; i++)
        {
            if (i / 3 == nowPage)
            {
                txts[i].SetActive(true);
            }
            else
            {
                txts[i].SetActive(false);
            }
        }
    }

    //public void AddPhoto(GameObject photo)
    //{
    //    photosInFile[photoCount] = Instantiate(photo, photoParent.transform);
    //    photosInFile[photoCount].transform.localPosition = new Vector3(
    //        (float)((photoCount / 3 - 1) * 1.3),
    //        (float)((1 - photoCount % 3) * 1.3),
    //        0);
    //    photosInFile[photoCount].transform.localScale = new Vector3(
    //        0.24f,
    //        0.24f,
    //        0.24f);
    //    photosInFile[photoCount].GetComponent<SpriteRenderer>().sortingOrder = 21;

    //    photoCount++;
    //    if (photoCount > 9)
    //    {
    //        photosInFile[photoCount - 1].SetActive(false);
    //    }
    //}



    //    HideOtherPhoto();
    //}

    //public void ShowAllPhoto()
    //{
    //    ResetSelectPhoto();
    //    //Destroy(photoMaskInstance);
    //    //photoMaskInstance = Instantiate(photoMask, transform.parent);
    //    for (int i = 0; i < photoCount; i++)
    //    {
    //        photosInFile[i].SetActive(true);
    //    }
    //}

    //public void HideAllPhoto()
    //{
    //    ResetSelectPhoto();
    //    //Destroy(photoMaskInstance);
    //    for (int i = 0; i < photoCount; i++)
    //    {
    //        photosInFile[i].SetActive(false);
    //    }
    //}

    //public void HideOtherPhoto()
    //{
    //    //Destroy(photoMaskInstance);
    //    for (int i = 0; i < photoCount; i++)
    //    {
    //        if (i / 9 != showPage)
    //        {
    //            photosInFile[i].SetActive(false);
    //        }
    //        else if (i / 9 == showPage)
    //        {
    //            photosInFile[i].SetActive(true);
    //        }
    //    }
    //}

    //private IEnumerator MovePhoto(string direction)
    //{
    //    for (int i = 0; i < 10; i++)
    //    {
    //        switch (direction)
    //        {
    //            case "Right":
    //                photoParent.transform.localPosition -= new Vector3(3.9f / 10, 0, 0);
    //                break;
    //            case "Left":
    //                photoParent.transform.localPosition += new Vector3(3.9f / 10, 0, 0);
    //                break;
    //            default:
    //                break;
    //        }
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}

    //private IEnumerator LittleMovePhoto(string direction)
    //{
    //    for (int i = 0; i < 2; i++)
    //    {
    //        switch (direction)
    //        {
    //            case "Right":
    //                photoParent.transform.localPosition += new Vector3(-3.9f / 10, 0, 0);
    //                break;
    //            case "Left":
    //                photoParent.transform.localPosition += new Vector3(3.9f / 10, 0, 0);
    //                break;
    //            default:
    //                break;
    //        }
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //    for (int i = 0; i < 2; i++)
    //    {
    //        switch (direction)
    //        {
    //            case "Right":
    //                photoParent.transform.localPosition += new Vector3(3.9f / 10, 0, 0);
    //                break;
    //            case "Left":
    //                photoParent.transform.localPosition += new Vector3(-3.9f / 10, 0, 0);
    //                break;
    //            default:
    //                break;
    //        }
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}

    //public void SelectPhoto(int x, int y)
    //{
    //    if (x * 3 - y + 2 + showPage * 9 < photoCount)
    //    {
    //        photosInFile[x * 3 - y + 2 + showPage * 9].GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f);
    //    }
    //}

    //public IEnumerator ZoomInPhoto(int x, int y)
    //{
    //    ResetSelectPhoto();
    //    HideAllPhoto();
    //    photosInFile[x * 3 - y + 2 + showPage * 9].SetActive(true);
    //    Vector3 oldPos = photosInFile[x * 3 - y + 2 + showPage * 9].transform.localPosition;
    //    for (int i = 0; i < 5; i++)
    //    {
    //        photosInFile[x * 3 - y + 2 + showPage * 9].transform.localScale += new Vector3((0.82f - 0.24f) / 5, (0.8f - 0.24f) / 5, (0.8f - 0.24f) / 5);
    //        photosInFile[x * 3 - y + 2 + showPage * 9].transform.localPosition += new Vector3((0f - oldPos.x) / 5, (0f - oldPos.y) / 5, (0f - oldPos.z) / 5);

    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}

    //public void ResetZoomIn()
    //{
    //    for (int i = 0; i < photoCount; i++)
    //    {
    //        photosInFile[i].transform.localPosition = new Vector3(
    //        (float)((i / 3 - 1) * 1.3),
    //        (float)((1 - i % 3) * 1.3),
    //        0);
    //        photosInFile[i].transform.localScale = new Vector3(
    //            0.24f,
    //            0.24f,
    //            0.24f);
    //        photosInFile[i].GetComponent<SpriteRenderer>().sortingOrder = 21;
    //    }

    //    HideOtherPhoto();
    //}

    //public void ResetSelectPhoto()
    //{
    //    for (int i = 0; i < photoCount; i++)
    //    {
    //        photosInFile[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
    //    }
    //}

    //public void SetTopMask(string color)
    //{
    //    topMask.SetActive(true);
    //    switch (color)
    //    {
    //        case "r":
    //            topMask.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 0.5f);
    //            break;
    //        case "g":
    //            topMask.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0f, 0.5f);
    //            break;
    //        case "b":
    //            topMask.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 1f, 0.5f);
    //            break;
    //        default:
    //            break;
    //    }
    //}

    //public void HideTopMask()
    //{
    //    topMask.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
    //    topMask.SetActive(false);
    //}
}
