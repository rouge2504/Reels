using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Paytable", menuName = "Slot Machine/Paytable")]
public class Paytable : ScriptableObject
{
    public GameObject[] symbolsChecker;
    public List<PaytableEntry> entries = new List<PaytableEntry>();

    public List<PaytablePattern> patterns = new List<PaytablePattern>();



    /// <summary>
    /// Al ver en el inspector se ve la matriz para patrones más complejos
    /// </summary>
    private void OnEnable()
    {
        if (patterns == null || patterns.Count == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                PaytablePattern p = new PaytablePattern();
                p.pattern.rows = Constants.MAX_ROW;
                p.pattern.columns = Constants.MAX_COLUMN;
                p.pattern.Initialize();
                patterns.Add(p);
            }
        }
    }


    /// <summary>
    /// Obtiene la recompensa ya pre hecha del scriptable object
    /// </summary>
    /// <param name="symbolId"></param>
    /// <param name="matchCount"></param>
    /// <returns></returns>
    public int GetReward(int symbolId, int matchCount)
    {
        foreach (var entry in entries)
        {
            if (entry.symbolId == symbolId && entry.matchCount == matchCount)
                return entry.reward;
        }
        return 0;
    }
}
