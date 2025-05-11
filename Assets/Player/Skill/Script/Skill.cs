using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    // SkillManager
    public float shootSpeed = 10f;
    public int arrowCount = 1;
    public bool addGhost = false; 
    public bool addBomb = false; // í­íƒ„
    
    
    // ArrowManager
    public bool addBurn = false; // ë¶ˆ í™”ì‚´ 
    public bool addPenetrates = false; // ê´€í†µ
    public bool addSpread = false; // í™•ì‚°

    // ChaseMonster
    public bool addChase = false; // ìœ ë„
}
