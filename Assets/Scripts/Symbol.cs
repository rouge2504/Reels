using UnityEngine;

public class Symbol : MonoBehaviour
{
    public enum SymbolID {NONE, WATER, FIRE, EARTH, AIR, DARK, LIGHT};

    public SymbolID symbolID;
    public int id;
    public Sprite symbolSprite;
}
