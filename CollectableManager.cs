using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// giải thích thuật toán:
// ví dụ 3 loại item có tỉ lệ lần lượt là 10 20 30
// dùng prefix sum => 10 30 50
// random từ 1 - > 50 trả về là 20 < 30(index = 1)(30 là giá trị lớn hơn gần nhất trong mảng) => index của item trong mảng là 1
public class CollectableManager : MonoBehaviour
{
    public CollectableItem[] GameObjects;
    int[] accumulatedPercentages; // mảng prefix sum của tỉ lệ spawn

    private void Start()
    {
        if(GameObjects != null && GameObjects.Length > 0) CalculateAccumulatedPercentages();
    }

    // tính prefix sum 
    void CalculateAccumulatedPercentages()
    {
        accumulatedPercentages = new int[GameObjects.Length];
        accumulatedPercentages[0] = GameObjects[0].Rate;
        
        for(int index = 1; index < GameObjects.Length; index++)
        {
            accumulatedPercentages[index] = accumulatedPercentages[index - 1] + GameObjects[index].Rate;
        }
    }
    void Spawn(Vector3 PositionSpawn)
    {
        int totalPercentage = accumulatedPercentages[accumulatedPercentages.Length - 1];
        if(totalPercentage <= 0)
        {
            Debug.Log("all item has rate is 0");
            return;
        }
        int randomValue = UnityEngine.Random.Range(1, totalPercentage + 1);
        int itemIndex = BinarySearch(randomValue);
        if(itemIndex != -1) Instantiate(GameObjects[itemIndex].Preflab, PositionSpawn, Quaternion.identity);        
    }
    // Binary search bên dưới để tìm kiếm vị trí phần tử lớn hơn(giá trị lớn hơn gần nhất) hoặc bằng giá trị cần tìm
    int BinarySearch(int value)
    {
        int left = 0;
        int right = accumulatedPercentages.Length - 1;
        while (left < right)
        {
            int mid = (left + right) / 2;
            if (accumulatedPercentages[mid] < value) left = mid + 1;
            else if (accumulatedPercentages[mid] >= value) right = mid;
        }
        return accumulatedPercentages[left] >= value? left : -1;
    }

    // method để test, vui lòng viết lại để sử dụng
    public void SpawnTest()
    {
        if (GameObjects != null && GameObjects.Length > 0) Spawn(Vector3.zero);
    }
}
[System.Serializable]
public class CollectableItem
{
    [Range(0, 100)] 
    public int Rate;
    public GameObject Preflab;
}
