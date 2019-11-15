using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Prospector : MonoBehaviour {

    static public Prospector S;

    [Header("Set in Inspector")]
    public TextAsset deckXML;
    public TextAsset layoutXML;
    public float xOffset = 3;
    public float yOffset = -2.5f;
    public Vector3 layoutCenter;
    public Vector2 fsPosMid = new Vector2(0.5f, 0.90f);
    public Vector2 fsPosRun = new Vector2(0.5f, 0.75f);
    public Vector2 fsPosMid2 = new Vector2(0.4f, 1.0f);
    public Vector2 fsPosEnd= new Vector2(0.5f, 0.95f);
    public float reloadDelay = 1f;
    public Text gameOverText, roundResultText, highScoreText;





    [Header("Set Dynamically")]
    public Deck deck;
    public Layout layout;
    public List<CardProspector> drawPile;
    public Transform layoutAnchor;
    public CardProspector target;
    public List<CardProspector> tableu;
    public List<CardProspector> discardPile;
    public FloatingScore fsRun;

    void Awake() {
        S = this; //set up singleton for prospector
        SetUpUITexts();
    }

    void SetUpUITexts()
    {
        GameObject go = GameObject.Find("High Score");
        if (go != null)
        {
            highScoreText = go.GetComponent<Text>();
        }


        int highScore = ScoreManager.HIGH_SCORE;
        string hScore = "High Score: " + Utils.AddCommasToNumber(highScore);
        go.GetComponent<Text>().text = hScore;
        go = GameObject.Find("Game Over");
        if (go != null)
        {
            gameOverText = go.GetComponent<Text>();
        }

        go = GameObject.Find("RoundResult");
        if (go. != null)
        {
            roundResultText = go.GetComponent<Text>();

        }
        ShowResultsUI(false);
    }

    void ShowResultsUI(bool show)
    {
        gameOverText.gameObject.SetActive(show);
        roundResultText.gameObject.SetActive(show);
    }


}


}




        void Start() {
        Scoreboard.S.score = ScoreManager.SCORE;
        deck = GetComponent<Deck>();
        deck.InitDeck(deckXML.text);
        Deck.Shuffle(ref deck.cards);


        layout = GetComponent<Layout>();
        layout.ReadLayout(layoutXML.text);

        drawPile = ConvertListCardsToListCardProspectors( deck.cards);
        LayoutGame();
    }
    List<CardProspector> ConvertListCardsToListCardProspectors(List<Card> 1CD)
    {
        List < CardProspector > 1CP = new List<CardProspector>();
        CardProspector tCP;
        foreach (Card tCD in 1CD ) {
            tCP = tCD as CardProspector;
            1CP.Add(tCP);
        }

        // draw function will pull a single card from drawpile

        CardProspector Draw()
        {
            CardProspector cd = drawPile[0];
            drawPile.RemoveAt(0);
            return (cd);
        }


        //layout game positions


        void LayoutGame() {
            if (layoutAnchor == null)
            {
                GameObject tGo = new GameObject("_LayoutAnchor");
                //create empty object
                layoutAnchor = tGo.transform;
                layoutAnchor.transform.position = layoutCenter;


                foreach (SlotDef tSD in layout.slotDefs)
                {
                    tableu.Add(cpp);
                }

                foreach (CardProspector tCP in tableu)
                {
                    foreach (int hid in tCP.slotDef.hiddenBy)
                    {
                        cp = FindCardByLayoutID(hid);
                        tCP.hiddenBy.Add(cp);
                    }
                }

                MoveToTarget(Draw());
                UpdateDrawPile();




            }


            CardProspector FindCardByLayoutID(int layoutID)
            {
                foreach (CardProspector tCP in tableu)
                {
                    if (tCP.layoutID == layoutID)
                    {
                        return (tCP);
                    }
                }
            }

            return null;




        }

        void SetTableauFaces()
        {
            foreach (CardProspector cd in tableu)
            {
                bool faceUp = true;
                foreach (CardProspector cover in cd.hiddenBy)
                {
                    if (cover.state == eCardState.tableu)
                    {
                        faceUp = false;
                    }


                    cd.faceUp = faceUp;
                }
            }

        public void CardClicked(CardProspector cd)
            {
                switch (cd.state)
                {

                    case eCardState.target:
                        break;



                    case eCardState.drawpile:
                        MoveToDiscard(target);
                        MoveToTarget(Draw());
                        UpdateDrawPile();
                        ScoreManager.EVENT(eScoreEvent.draw);
                        FloatingScoreHandler(eScoreEvent.draw);
                        break;




                    case eCardState.tableu:
                        bool validMatch = true;
                        if (!cd.faceUp)
                        {
                            validMatch = false;
                        }
                        if (!AdjacentRank(cd, target))
                        {
                            validMatch = false;
                        }
                        if (!validMatch) return;
                        tableu.Remove(cd);
                        MoveToTarget(cd);
                        SetTableauFaces();
                        ScoreManager.EVENT(eScoreEvent.mine);
                        FloatingScoreHandler(eScoreEvent.mine);
                        break;
            

            }
            CheckForGameOver();
        }
        void CheckForGameOver();

        if (tableu.Count == 0)
        {
            GameOver(true);
            return;
        }

        foreach (CardProspector cd in tableu)
        {
            if (AdjacentRank(cd, target))
            {
                return;
            }
        }

        GameOver(false);
    }

    void GameOver(bool won)
        {

        int score = ScoreManager.SCORE;
        if (fsRun != null) score += fsRun.score;

        if (won)
        {
            gameOverText.text = "Round Over";
            roundResult.text = "You won this round!\n Round Score: " + score;
            ShowResultUI(true);

            // print("Game Over. You won! : )");
            ScoreManager.EVENT(eScoreEvent.gameWin);
            FloatingScoreHandler(eScoreEvent.gameWin);
        }
        else
        {
            gameOverText.text = "Game OVer";
            if (ScoreManager.HIGH_SCORE <= score)
            {
                string str = "You got the high score!\nHigh score:" + score;
                roundResultText.text = str;


            }
            else
            {
                roundResultText.text = "Your final score was:" + score; 
         
           }
       
    
            else
            {
        //print("Game Over. You Lost. :(");
        ShowResultsUI(true);
        ScoreManager.EVENT(eScoreEvent.gameLoss);
                FloatingScoreHandler(eScoreEvent.gameLoss);


                Invoke("ReloadingLevel", reloadDelay);
            
            }


            void ReloadLevel()
            {
                SceneManager.LoadScene("_Prospector_Scene_0");
            }
           

        }
      

    public bool AdjacentRank(CardProspector c0, CardProspector c1)
    {
        if (!c0.faceUp || !cl.faceUp) return (false);
        if (Mathf.Abs(c0).rank - c1.rank) == 1) {
            return (true);


            void FloatingScoreHandler(eScoreEvent evt)
            {
                List<Vector2> fsPts;
                switch (evt)
                {
                    case eScoreEvent.draw:
                    case eScoreEvent.gameWin:
                    case eScoreEvent.gameLoss:

                        if(fsRun != null)
                        {
                            fsPts = new List<Vector2>();
                            fsPts.Add(fsPosRun);
                            fsPts.Add(fsPosMid2);
                            fsPts.Add(fsPosEnd);
                            fsRun.Init(fsPts, 0, 1);
                            fsRun.fontSizes = new List<float>(new float[] { 28, 36, 4 });
                             fsRun = null;
             
                                 }
                        break;

                    case eScoreEvent.mine:
                        FloatingScore fs;
                        Vector2 p0 = Input.mousePosition;
                        p0.x /= Screen.width;
                        p0.y /= Screen.height;
                        fsPts = new List<Vector2>();
                        fsPts.Add(p0);
                        fsPts.Add(fsPosMid);
                        fsPts.Add(fsPosRun);
                        fs = Scoreboard.S.CreateFloatingScore(ScoreManager.CHAIN, fsPts);
                        fs.fontSizes = new List<float>(new float[] { 4, 50, 28 });
                        if (fsRun == null)
                        {
                            fsRun.reportFinishTo = null;
                        }
                        else
                        {
                            fs.reportFinishTo = fsRun.gameObject;
                        }
                        break;

                }






                }
            }



        }

       



                


    
    if(c0.rank == 1 && c1.rank) == 13) return (true);
        if (c0.rank == 13 && c1.rank == 1) return (true);

        return (false);


    }

        void MoveToDiscard(CardProspector cd)
            {
                cd.state = eCardState.discard;
                discardPile.Add(cd);
                cd.transform.parent = layoutAnchor;

                cd.transform.localPosition = new Vector3(
                layout.multiplier.x * layout.discardPile.x,
                layout.multiplier.y * layout.discardPile.y,
                -layout.discardPile.layerID + 0.5f);
                cd.faceUp = true;
                cd.SetSortingLayerName(layout.discardPile.layerName);
                cd.SetSortOrder(-100 + discardPile.Count);


            }

            void MoveToTarget(CardProspector cd)
            {
                if (target != null) MoveToDiscard(target);
                target = cd;
                cd.state = eCardState.target;
                cd.transform.parent = layoutAnchor;
                cd.transform.localPosition = new Vector3(
                layout.multiplier.x * layout.discardPile.x,
                layout.multiplier.y * layout.discardPile.y,
                -layout.discardPile.layerID);

                cd.faceUp = true;
                cd.SetSortingLayerName(layout.discardPile.layerName);
                cd.SetSortOrder(0);

                    
            }


            void UpdateDrawPile()
            {
                CardProspector cd;
                for (int i=0; i<drawPile.Count; i++)
                {
                    cd = drawPile[i];
                    cd.transform.parent = layoutAnchor;
                }
            

            Vector2 dpStagger = layout.drawPile.stagger;
            cd.transform.localPosition = new Vector3(
             layout.multiplier.x * (layout.drawPile.x + i * dpStagger.x),
            layout.multiplier.y * (layout.drawPile.y + i * dpStagger.y),
            -layout.drawPile.layerID + 0.1f * i);

            cd.faceUp = false;
            cd.state = eCardState.drawpile;
            cd.SetSortingLayerName(layout.drawPile.layerName);
            cd.SetSortOrder(-10 * i);


                )


            CardProspector cp;
            //follow layout
            foreach (SlotDef tSD in layout.slotDefs)
            {
                cp = Draw();
                cp.faceUp = tSD.faceUp;
                cp.transform.parent = layoutAnchor;
                cp.transform.localPosition = new Vector3(
                    layout.multiplier.x * tSD.x,
                    layout.multiplier.y * tSD.y,
                    -tSD.layerID);
                cp.layoutID = tSD.id;
                cp.slotDef = tSD;
                // card prospector in the tableu jave the state cardstate.tableu
                cp.state = eCardState.tableu;
                cp.SetSortingLayerName(tSD.layerName);
                 tableu.Add(cp);



        }




        return ( 1CP);
    }
}