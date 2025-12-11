using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
public class SpinManager : MonoBehaviour
{
    [SerializeField] SlotColumn[] slotColumns;
    [SerializeField] private float waitTime = 2;
    [SerializeField] private Button spinButton;

    private Symbol[,] symbols = new Symbol[3,5];

    [SerializeField] private Paytable paytable;

    [SerializeField] private TextMeshProUGUI creditText;

    [SerializeField] private int credits;

    [SerializeField] private Transform highlightContent;
    private Transform[] highlights;

    [SerializeField] private AnimationCounter coinCounter;

    [SerializeField] Animator creditAnim;

    private bool startSpin;

    void Start()
    {
        startSpin = false;
        creditAnim.gameObject.SetActive(false);
        InitHighlights();
    }

    /// <summary>
    /// Inicia los highlights los obtiene del higjlightContent para vaciarlo en un listado
    /// </summary>
    private void InitHighlights(){
        highlights = new Transform[highlightContent.childCount];
        for (int i = 0; i < highlightContent.childCount; i++){
            highlights[i] = highlightContent.GetChild(i);
            highlights[i].gameObject.SetActive(false);
        }
        
    }

    /// <summary>
    /// Pone los hihglights en la posicion de los simbolos y la posición en el arreglo
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="it"></param> 
    private void SetHighlight(Vector3 pos, int it){
        highlights[it].position = pos;
        highlights[it].gameObject.SetActive(true);

    }

    /// <summary>
    /// Regresa a falso los highlights para reusarlos
    /// </summary> 
    private void ResetHighlights(){
        for (int i = 0; i < highlightContent.childCount; i++){
            highlights[i].gameObject.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !startSpin){
            startSpin = true;
            StartCoroutine(Spin());
            ResetHighlights();
        }
    }

    /// <summary>
    /// Con una corutina se inicia el spin, con una variable waitTime hace una espera para cada columna y espera a que
    /// todas las columnas ser paren
    /// </summary>
    /// <returns></returns> 
    IEnumerator Spin(){
        if (startSpin){
        creditAnim.gameObject.SetActive(false);
        spinButton.interactable = false;
        foreach(SlotColumn slotColumn in slotColumns){
            slotColumn.StartSpin();
            yield return new WaitForSeconds(waitTime);
        }
        yield return new WaitUntil(() => AllColumnsStopped());

        spinButton.interactable = true;
        startSpin = false;
    }
    }

    /// <summary>
    /// regresa verdadero o falso para saber si ya estan todas las columnas y checa que premio se gano
    /// </summary>
    /// <returns></returns> 
    private bool AllColumnsStopped()
    {
        foreach (SlotColumn slot in slotColumns)
        {
            
            if (slot.isSpinning)
                return false;
        }

        int it_column = 0;
        foreach (SlotColumn slot in slotColumns)
        {
            List<Symbol> symbolVisible = slot.GetVisibleSymbolNames();
            for (int i = 0; i < Constants.MAX_ROW; i++){
                symbols[i, it_column] = symbolVisible[i];
            }
            it_column++;
        }
        DebugMatrix();
        
        int rewardTemp = ProcessReward();
        if (rewardTemp != 0){
        creditAnim.gameObject.SetActive(true);
        if (creditAnim.transform.GetChild(0).gameObject.activeSelf)
            creditAnim.transform.GetComponentInChildren<TextMeshProUGUI>().text = rewardTemp.ToString();
        }
        credits += rewardTemp;
        coinCounter.SetAnimation(coinCounter.totalCoins,rewardTemp);
        coinCounter.totalCoins = credits;

        //creditText.text = "Creditos: " + credits;
        

        return true;
    }

    /// <summary>
    /// Checa el patron de línea, se usa para ver los patrones aparte de la linea horizontal
    /// </summary>
    /// <param name="pattern"></param>
    /// <param name="targetSymbolId"></param>
    /// <returns></returns>

    private int CheckLinePattern(IntMatrix2D pattern, int targetSymbolId)
    {
        int it_prize = 0;
        for (int row = 0; row < pattern.rows; row++)
        {
            for (int col = 0; col < pattern.columns; col++)
            {
                int expected = pattern.Get(row, col);

                if (expected == 1)
                {

                    if (symbols[row, col].id != targetSymbolId){
                        ResetHighlights();
                        return 0; 
                    }else{
                        it_prize++;
                        SetHighlight(symbols[row, col].gameObject.transform.position, it_prize);

                    }
                }
            }
        }
        return it_prize; 
    }

    /// <summary>
    /// Da la recompensa si encuentra un patron o una linea vertical
    /// </summary>
    /// <returns></returns>

    private int ProcessReward(){
        int reward = 0;

        Prize result = CheckMiddleLine();
        reward = paytable.GetReward(result.symbolId, result.matchCount);

        if (reward > 0)
        {
            Debug.Log($"<color=yellow>Premio en línea central: símbolo {result.symbolId} x{result.matchCount} → {reward} créditos</color>");
            return reward;
        }

        Debug.Log("<color=red>No hubo premio en línea central</color>");
        return 0;
    }

    
    /// <summary>
    /// Checa el patron en horizontal y obtiene el premio
    /// </summary>
    /// <param name="id_prize"></param>
    /// <returns></returns>
    private Prize CheckPrize(int id_prize)
    {
        int requiredMatches = 2;
        int it_prize = 0;

        int row = 0;
        int column = 0;

        bool check = false;

        Prize prize = new Prize();

        while (!check)
        {
            if (symbols[row, column].id == id_prize)
            {
                SetHighlight(symbols[row, column].gameObject.transform.position, it_prize);
                it_prize++;
                column++;

                if (it_prize >= requiredMatches)
                {
                    for (int i = column; i < Constants.MAX_COLUMN; i++){
                        if (symbols[row, i].id == id_prize){
                            SetHighlight(symbols[row, i].gameObject.transform.position, it_prize);
                            it_prize++;

                        }else{
                            break;
                        }
                    }
                    Debug.Log($" <color=green>Ganaste en fila {row} el premio {id_prize} con {it_prize} símbolos consecutivos</color>");
                    prize = new Prize(id_prize, it_prize);
                    return prize; 
                }
            }
            else
            {
                it_prize = 0;
                row++;
                column = 0;
                ResetHighlights();
            }

            



            if (column >= Constants.MAX_COLUMN)
            {
                row++;
                column = 0;
                it_prize = 0;
            }

            if (row >= Constants.MAX_ROW)
            {
                check = true;
            }
        }
       
        return prize;
    }


    private Prize CheckMiddleLine()
    {
        // Solo revisa la fila central (row = 1)
        Symbol a = symbols[1, 0];
        Symbol b = symbols[1, 1];
        Symbol c = symbols[1, 2];

        Debug.Log($"Linea central: {a.symbolID}, {b.symbolID}, {c.symbolID}");

        // Caso 1: los tres son iguales
        if (a.id == b.id && b.id == c.id)
        {
            SetHighlight(a.gameObject.transform.position, 0);
            SetHighlight(b.gameObject.transform.position, 1);
            SetHighlight(c.gameObject.transform.position, 2);

            return new Prize(a.id, 3);
        }

        // Caso 2: dos iguales
        if (a.id == b.id)
            return new Prize(a.id, 2);

        if (b.id == c.id)
            return new Prize(b.id, 2);

        if (a.id == c.id)
            return new Prize(a.id, 2);

        // Caso 3: ningún match
        return new Prize(0, 0);
    }


    /// <summary>
    /// Pone en consola la matriz con el id de los premios para visualizar que pasa
    /// </summary>
    private void DebugMatrix(){
        string debugger = "";
        for (int i = 0; i < Constants.MAX_ROW; i++){
            for (int j = 0; j < Constants.MAX_COLUMN; j++){
                debugger += symbols[i,j].id + ",";
            }
            debugger += "\n";
        }

        Debug.Log(debugger);
    }
    

    /// <summary>
    /// Comienza el spin ya se con botón o con barra espaciadora
    /// </summary>
    public void StartSpin(){
        startSpin = true;
        StartCoroutine(Spin());
        ResetHighlights();
    }
}
