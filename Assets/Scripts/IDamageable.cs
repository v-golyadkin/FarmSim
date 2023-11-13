using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public interface IDamageable
{
    
    public void CalculateDamage(ref int damage);
    public void ApplyDamage(int damage);
    public void CheckStatus();


}
