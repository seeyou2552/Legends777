using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    // SkillManager
    public float shootSpeed = 10f;
    public int arrowCount = 1;
    public bool addGhost = false; 
    public int addBomb = 0; // 폭탄
    
    
    // ArrowManager
    public bool addBurn = false; // 불 화살 
    public bool addPenetrates = false; // 관통
    public int addSpread = 0; // 확산
    
    // ChaseMonster
    public bool addChase = false; // 유도 기능
}
