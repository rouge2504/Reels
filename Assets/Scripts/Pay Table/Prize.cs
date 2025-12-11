public class Prize {
    public int symbolId;
    public int matchCount;

    public Prize(){}


    
    /// <summary>
    /// Constructor para el precio y el machado del simbolo
    /// </summary>
    /// <param name="symbolID"></param>
    /// <param name="matchCount"></param>
    public Prize (int symbolID, int matchCount){
        this.symbolId = symbolID;
        this.matchCount = matchCount;
    }
}