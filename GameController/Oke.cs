using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Oke:MonoBehaviour
{
    public enum Option{Satu, Dua, Tiga};
    public Option angka = new Option();
    public float koma;
    public string g_string;
}