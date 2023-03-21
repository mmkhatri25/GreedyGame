using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// this is used to store position for bots
/// so this position will be used to move the chips
/// it will also hold the chip type and spot type
/// </summary>
/// 

namespace Shared
{

    [CreateAssetMenu]
    public class BetSpots : ScriptableObject
    {
        public List<ChipDate> chipDetails;
        public void AddData(ChipDate data)
        {
            chipDetails.Add(data);
        }
        public List<ChipDate> GetData()
        {
            return chipDetails;
        }
    }

}