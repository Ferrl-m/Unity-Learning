using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour {

    private void Awake() {
        CuttingCounter.ResetSataticData();
        BaseCounter.ResetSataticData();
        TrashCounter.ResetSataticData();
    }
}
