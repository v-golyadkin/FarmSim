using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPersistance
{
    public string Read();

    public void Load(string jsonString);
}
