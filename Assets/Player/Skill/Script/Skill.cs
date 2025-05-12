using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    // SkillManager
    public float shootSpeed = 10f;
    public int arrowCount = 1;
    public bool addGhost = false;
    public bool addSword = false;
    
    // ArrowManager
    public bool addBurn = false; // 불 화살 
    public bool addPenetrates = false; // 관통
}
