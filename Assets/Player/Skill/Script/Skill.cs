using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    // SkillManager
    public float shootSpeed = 10f;
    public int arrowCount = 1;
    public bool addGhost = false; 
    public bool addBomb = false; // 폭탄
    
    
    // ArrowManager
    public bool addBurn = false; // 불 화살 
    public bool addPenetrates = false; // 관통
    public bool addSpread = false; // 확산

    // ChaseMonster
    public bool addChase = false; // 유도
}
