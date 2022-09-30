using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using Proyecto26;

public class GameController : MonoBehaviour
{
    [Serializable]
    public class CookieClass
    {
        public string access_token;
        public string token_type;
        public int expires_in;
        public string refresh_token;
    }

    [Serializable]
    public class ResponseClass
    {
        public int pacPackageTypeSN;
        public int LastLevet;
    }

    [Serializable]
    public class ResultClass
    {
        public int FinalScore;
        public int LastCoefficient;
        public int ErrorCounts;
        public int CoefficientOfFirstError;
        public List<coefficientsErrorCount> CoefficientsErrorCount=new List<coefficientsErrorCount>();
        public int TimeAverageOfCorrectAnswers;
        public int TimeAverageOfErrors;
    }

    [Serializable]
    public class coefficientsErrorCount
    {
        public int Coefficient;
        public int ErrorCount;
    }

    CookieClass myObject = new CookieClass();
    ResponseClass responseClass = new ResponseClass();
    ResultClass resultClass = new ResultClass();
    coefficientsErrorCount coefficientsErrorCounto = new coefficientsErrorCount();
    #region variable
    public GameObject tutorialPanel, gameplayPanel, pausePanel, endgamePanel, startTimer, resetbtn;
    public List<Sprite> cardsImg;
    public Image preCardImage, currentCardImage, rightImg, wrongImg;
    public GameObject[] card;
    public GameObject nextCard, currentCard, fillCard, emptyFillCard;
    public int prevIndex, currentIndex;
    public Text timeTxt, scoreTxt, winTxt, zaribTxt;
    private bool checkLevel;
    private bool reset;

    //for tutorial :
    public List<Sprite> imgTutorial;
    private int currentImgIndex;
    public Image currentImg;
    public GameObject prevbtn, nextbtn;

    //for game play :
    public int timer;
    public enum State { CountDown, loading, playing, checking, finish, showingResult, tutorial };
    public State state;

    public Animator helpTextAnim;
    public List<Image> zaribsImg;
    //for server
    public int rightAnswer, WrongAnswer, score, zarib,firstWrongZarib;
    public List<float> answerTime;
    public List<bool> answers;
    public float avgAnswerTime, avgRightAwnserTime, avgWrongAnswerTime;
    public List<int> zaribs;
    public Dictionary<int,int> zarib_wrongCountAnswer;

    public string cookie = "WFfxcsPy6yPJXiT47t1UnOGMnymnwU8GtXq5kkrzMpiNN8eA2p1-xgNjrDVfpso0r5DXQg6ZokH7CJp1g6MoAlcYOtMUIxocG6W8sFJ9uiqoNO15q02fDkVlokzp1dMmaFou1O8pkeuesFWQSHaGXkZZTw9qsZunETlUml-9yvBF7Az8fHl_GmuaE1xI390g_oSlTfq32mFTnRR_XSNlSkN-dMWsIjxlMOUK9UiufYEBMPCi462ajyLfX9ua7NWUXSecdaNWOEIm-zVQ5wp9UU8B0Yjw93GoEr_xvvjz5qi9gOhq2gKk4DA107fDs9nOORVUHIQEkDJTCHlU91ITWg";
    public string getCookieResult = "";
    private int pacPackageTypeSN;
    private int LastLevel;
    public int tempwrongAns, temprightAns;
    
    public int[] z = new int [10];

    #endregion variable

    //js :

    [DllImport("__Internal")]
    private static extern void setCookie(string cname, string cvalue, int exdays);

    [DllImport("__Internal")]
    private static extern string getCookie(string cname);

    void Start()
    {
        init();
        StartCoroutine( startTutorial());
        Application.targetFrameRate = 60;
        //js code
        cookie = getCookie("tokencookie");
        getCookieResult = cookie;
        splitCookie();
    }

    void Update()
    {
        inputManager();
    }
    private void init()
    {
        resetbtn.SetActive(false);
        prevIndex = UnityEngine.Random.Range(0, 3);
        nextCard.GetComponent<Image>().sprite = cardsImg[prevIndex];
        //timer = 45;
        rightAnswer = 0;
        WrongAnswer = 0;
        timeTxt.text = "00 : " + timer;
        /*currentCard = card[0];
        nextCard = card[1];*/
        currentCard.GetComponent<Image>().sprite = nextCard.GetComponent<Image>().sprite;
        answers = new List<bool>();
        answerTime = new List<float>();
        zaribs = new List<int>();
        zarib_wrongCountAnswer = new Dictionary<int, int>();
        emptyFillCard.SetActive(false);
        for (int i = 0; i < zaribsImg.Count; i++)
        {
            var tempcolor = zaribsImg[i].color;
            tempcolor.a = 0.3f;
            zaribsImg[i].color = tempcolor;
        }
    }

    IEnumerator  startTutorial()
    {
        
        
        tutorialPanel.SetActive(true);
        gameplayPanel.SetActive(false);
        pausePanel.SetActive(false);
        endgamePanel.SetActive(false);
        
        tutorialPanel.GetComponent<Animator>().Play("enterPanel");
        yield return new WaitForSeconds(.5f);
        state = State.tutorial;
    }


    public void nextBtn()
    {
        audioController.instance.popup();
        chapterMenuSliding.refrence.moveDir = chapterMenuSliding.MoveDir.left;
        chapterMenuSliding.refrence.changeChapter();
        /* if (chapterMenuSliding.refrence.page == chapterMenuSliding.Page.page1)
         {
             chapterMenuSliding.refrence.page = chapterMenuSliding.Page.page2;
             prevbtn.SetActive(true);
             nextbtn.SetActive(true);
         }
         else if (chapterMenuSliding.refrence.page == chapterMenuSliding.Page.page2)
         {
             chapterMenuSliding.refrence.page = chapterMenuSliding.Page.page2;
             nextbtn.SetActive(false);
         }
         else if (currentImgIndex == 2)
         {
             currentImgIndex = 2;
             nextbtn.SetActive(false);
         }
 
         currentImg.sprite = imgTutorial[currentImgIndex];*/
    }

    public void prevBtn()
    {
        audioController.instance.popup();
        chapterMenuSliding.refrence.moveDir = chapterMenuSliding.MoveDir.right;
        chapterMenuSliding.refrence.changeChapter();
        /*if (currentImgIndex == 2)
        {
            currentImgIndex = 1;
            prevbtn.SetActive(true);
            nextbtn.SetActive(true);
        }
        else if (currentImgIndex == 1)
        {
            currentImgIndex = 0;
            nextbtn.SetActive(true);
            prevbtn.SetActive(false);
        }
        else if (currentImgIndex == 0)
        {
            currentImgIndex = 0;
            nextbtn.SetActive(true);
            prevbtn.SetActive(false);
        }

        currentImg.sprite = imgTutorial[currentImgIndex];*/
    }

    public void playGameBtn()
    {
        audioController.instance.popup();
        StartCoroutine(playGame());
    }

    IEnumerator playGame()
    {
        tutorialPanel.GetComponent<Animator>().Play("exitPanel");
        yield return new WaitForSeconds(.3f);
        gameplayPanel.SetActive(true);
        nextCard.SendMessage("enter");
        yield return new WaitForSeconds(.5f);
        tutorialPanel.SetActive(false);

        gameplayPanel.GetComponent<Animator>().Play("enterCanvas");
        StartCoroutine(startCountDown(3));
    }

    public void pauseBtn()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void yesBtn()
    {
        if (state == State.playing)
        {
            checkPlayerMove(1);
           
        }

    }

    public void noBtn()
    {
        if (state == State.playing)
        {
            checkPlayerMove(0);
        }
    }

    private IEnumerator startCountDown(int time)
    {
        state = State.CountDown;
        startTimer.SendMessage("countDown");
        while (time > 0)
        {
            startTimer.transform.GetChild(0).GetComponent<Text>().text = time.ToString();
            audioController.instance.counter();
            time--;
            yield return new WaitForSeconds(1);

        }
        helpTextAnim.Play("enterHelpText");
        
        startTimer.SendMessage("idle");
        // StartBtnGame();
        StartCoroutine(countDowntimer(timer));
        //loadLevel();
        StartCoroutine(startGame());

    }

    private void loadLevel()
    {
        if (timer > 1)
        {
            state = State.loading;
            currentIndex = UnityEngine.Random.Range(0, 3);
            //currentCardImage.sprite = cardsImg[currentIndex];
            
            
            //preCardImage.sprite = cardsImg[prevIndex];
            if (currentIndex == prevIndex)
            {
                checkLevel = true;
                temprightAns++;
                tempwrongAns = 0;
            }
            else
            {
                checkLevel = false;
                tempwrongAns++;
                temprightAns = 0;
            }

            if (tempwrongAns > 2 )
            {
                loadLevel();
            }
            else
            {
                currentCard.GetComponent<Image>().sprite = nextCard.GetComponent<Image>().sprite;
                nextCard.GetComponent<Image>().sprite = cardsImg[currentIndex];
            }
        }
    }

    IEnumerator startGame()
    {
        
        loadLevel();
        
        //pre level go to left
        currentCard.SendMessage("rotateCard");
        //fillCard.SendMessage("enterfill");
        audioController.instance.popup();
        yield return new WaitForSeconds(.2f);
        //emptyFillCard.SetActive(false);
        //currentCard.SendMessage("entercurrent");
        yield return new WaitForSeconds(.1f);

        //current level go to middle from right
        //currentCard.SendMessage("enter");
        //currentCard.SendMessage("entercurrent");

        emptyFillCard.SetActive(true);
        //wait for player
        state = State.playing;
        //fillCard.SendMessage("idle");
    }

    public void checkPlayerMove(int answer)
    {
        //if click yes
        if(answer == 1)
        {
            #region yes
            if (checkLevel)
            {
                rightAnswer++;
                var tempcolor = zaribsImg[rightAnswer - 1].color;
                tempcolor.a = 1f;
                zaribsImg[rightAnswer - 1].color = tempcolor;
                if (rightAnswer == 4)
                {
                    zarib++;
                    rightAnswer = 0;
                    for (int i = 0; i < zaribsImg.Count; i++)
                    {
                        var tempcolor2 = zaribsImg[i].color;
                        tempcolor2.a = 0.3f;
                        zaribsImg[i].color = tempcolor2;
                    }
                }
                score += (50 * zarib);
                scoreTxt.text = score.ToString();
                zaribTxt.text = zarib.ToString();
                //rightImg.gameObject.SetActive(true);
                StartCoroutine(showResult(1));
            }
            else
            {
                WrongAnswer++;
                if (WrongAnswer == 1)
                {
                    firstWrongZarib = zarib;
                }
                if (rightAnswer == 0 && zarib > 1)
                {
                    zarib--;
                }
                rightAnswer = 0;
                for (int i = 0; i < zaribsImg.Count; i++)
                {
                    var tempcolor = zaribsImg[i].color;
                    tempcolor.a = 0.3f;
                    zaribsImg[i].color = tempcolor;
                }
                zaribTxt.text = zarib.ToString();
                //wrongImg.gameObject.SetActive(true);
                StartCoroutine(showResult(0));
            }
            #endregion yes
        }

        //if click no
        else
        {
            #region no

            if (checkLevel)
            {
                WrongAnswer++;
                if (WrongAnswer == 1)
                {
                    firstWrongZarib = zarib;
                }
                if (rightAnswer == 0 && zarib > 1)
                {
                    zarib--;
                }
                rightAnswer = 0;
                for (int i = 0; i < zaribsImg.Count; i++)
                {
                    var tempcolor = zaribsImg[i].color;
                    tempcolor.a = 0.3f;
                    zaribsImg[i].color = tempcolor;
                }
                zaribTxt.text = zarib.ToString();
                //wrongImg.gameObject.SetActive(true);
                StartCoroutine(showResult(0));
            }
            else
            {
                rightAnswer++;
                var tempcolor = zaribsImg[rightAnswer - 1].color;
                tempcolor.a = 1f;
                zaribsImg[rightAnswer - 1].color = tempcolor;
                if (rightAnswer == 4)
                {
                    zarib++;
                    rightAnswer = 0;
                    for (int i = 0; i < zaribsImg.Count; i++)
                    {
                        var tempcolor2 = zaribsImg[i].color;
                        tempcolor2.a = 0.3f;
                        zaribsImg[i].color = tempcolor2;
                    }
                }
                score += (50 * zarib);
                scoreTxt.text = score.ToString();
                zaribTxt.text = zarib.ToString();
                //rightImg.gameObject.SetActive(true);
                StartCoroutine(showResult(1));
            }

            #endregion no
        }
    }

    IEnumerator showResult(int state)
    {
        
        if(state == 1)
        {
            print("win");
            answers.Add(true);
            // rightImg.gameObject.SetActive(true);
            rightImg.transform.parent.SendMessage("showCorrect");
            audioController.instance.rightAns();
        }
        else
        {
            print("loose");
            answers.Add(false);
            zaribs.Add(zarib);
            // wrongImg.gameObject.SetActive(true);
            wrongImg.transform.parent.SendMessage("showWrong");
            audioController.instance.wrongAns();
        }
        answerTime.Add(timer);
        yield return new WaitForSeconds(.3f);
        StartCoroutine(nextLevel());

    }


    IEnumerator nextLevel()
    {
        if (timer > 1)
        {
           /* GameObject temp = currentCard;
            currentCard = nextCard;
            nextCard = temp;*/

            yield return new WaitForSeconds(0.01f);

            //rightImg.gameObject.SetActive(false);
            //wrongImg.gameObject.SetActive(false);
            //wrongImg.transform.parent.SendMessage("idle");
            prevIndex = currentIndex;
            StartCoroutine(startGame());
        }
    }

   

    private IEnumerator countDowntimer(int time)
    {
        while (time > 0)
        {
            time--;
            timer = time;
            timeTxt.text = "00 : " + timer;
            if (time < 5)
            {
                audioController.instance.counter();
            }
            yield return new WaitForSeconds(1);
            if (reset)
            {
                timer = 45;
                timeTxt.text = "00 : " + timer;
                reset = false;
                time = 0;
                break;
            }
        }

        StartCoroutine(endOfGame());
    }

    private IEnumerator endOfGame()
    {
       
        if (timer < 1)
        {
            audioController.instance.popup();
            gameplayPanel.GetComponent<Animator>().Play("exitCanvas");
            helpTextAnim.Play("exitHelpText");
            //currentCard.GetComponent<Animator>().Play("exit");
            yield return new WaitForSeconds(0.7f);
            //gameplayPanel.SetActive(false);
            gameplayPanel.GetComponent<CanvasGroup>().alpha = 0;
            endgamePanel.SetActive(true);
            audioController.instance.endGame();
            endgamePanel.GetComponent<Animator>().Play("enterEndPanel");
            // winTxt.text = "you earned " + score + " scores!";
            winTxt.text = Fa.faConvert("کارت خوب بود !") + "\n" + "\n" + Fa.faConvert("امتیاز کسب کردی") +" "+ score;
            if (responseClass.pacPackageTypeSN == 1)
            {
                resetbtn.SetActive(true);
            }
            else
            {
                resetbtn.SetActive(false);
            }
            CalculatResultForServer();
        }
    }

    public void CalculatResultForServer()
    {
        float tempanswer = 0, tempright = 0, tempwrong = 0, countRight = 0, countWrong = 0;
       
        for (int i = 0; i < answerTime.Count; i++)
        {
            if (i == 0)
            {
                tempanswer = answerTime[0];
                if (answers[0])
                {
                    tempright = answerTime[0];
                    countRight++;
                }
                else
                {
                    tempwrong = answerTime[0];
                    countWrong++;
                }
            }
            else
            {
                tempanswer += (answerTime[i - 1] - answerTime[i]);
                if (answers[i])
                {
                    tempright += (answerTime[i - 1] - answerTime[i]);
                    countRight++;
                }
                else
                {
                    tempwrong += (answerTime[i - 1] - answerTime[i]);
                    countWrong++;
                }
            }

        }
        avgAnswerTime = (tempanswer / answerTime.Count)*1000;
        avgRightAwnserTime = (tempright / countRight)*1000;
        avgWrongAnswerTime = (tempwrong / countWrong)*1000;

        
        postToServer();
    }


    public void inputManager()
    {
        if (state == State.playing)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                yesBtn();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                noBtn();
            }
        }
    }

    public void getFromServer()
    {
        RestClient.DefaultRequestHeaders["Authorization"] = "Bearer " + cookie;

        RestClient.Get("http://cogame.ir/api/game/GetGameModeAndLastLevel?gamesn=4").Then(response =>
        {
            splitResponse(response.Text);
        });
    }
    public void splitCookie()
    {
        myObject = JsonUtility.FromJson<CookieClass>(getCookieResult);
        cookie = myObject.access_token;
        getFromServer();
    }
    public void splitResponse(string response)
    {
        responseClass = JsonUtility.FromJson<ResponseClass>(response);
    }
    public void postToServer()
    {
        //resultClass.CoefficientsErrorCount = new coefficientsErrorCount[10];
        print(resultClass.CoefficientsErrorCount.Count);

        for (int i = 0; i < zaribs.Count; i++)
        {
            switch (zaribs[i])
            {
                case 1:
                    z[0]++;
                    break;
                case 2:
                    z[1]++;
                    break;
                case 3:
                    z[2]++;
                    break;
                case 4:
                    z[3]++;
                    break;
                case 5:
                    z[4]++;
                    break;
                case 6:
                    z[5]++;
                    break;
                case 7:
                    z[6]++;
                    break;
                case 8:
                    z[7]++;
                    break;
                case 9:
                    z[8]++;
                    break;
                case 10:
                    z[9]++;
                    break;
                    
            }
        }

        for (int i = 0; i < z.Length; i++)
        {
            coefficientsErrorCounto =new coefficientsErrorCount();
            switch (i)
            {
                case 1:
                    coefficientsErrorCounto.Coefficient = 1;
                    coefficientsErrorCounto.ErrorCount = z[0];
                    resultClass.CoefficientsErrorCount.Add(coefficientsErrorCounto);
                    break;
                case 2:
                    coefficientsErrorCounto.Coefficient = 2;
                    coefficientsErrorCounto.ErrorCount = z[1];
                    resultClass.CoefficientsErrorCount.Add(coefficientsErrorCounto);
                    break;
                case 3:
                    coefficientsErrorCounto.Coefficient = 3;
                    coefficientsErrorCounto.ErrorCount = z[2];
                    resultClass.CoefficientsErrorCount.Add(coefficientsErrorCounto);
                    break;
                case 4:
                    coefficientsErrorCounto.Coefficient = 4;
                    coefficientsErrorCounto.ErrorCount = z[3];
                    resultClass.CoefficientsErrorCount.Add(coefficientsErrorCounto);
                    break;
                case 5:
                    coefficientsErrorCounto.Coefficient = 5;
                    coefficientsErrorCounto.ErrorCount = z[4];
                    resultClass.CoefficientsErrorCount.Add(coefficientsErrorCounto);
                    break;
                case 6:
                    coefficientsErrorCounto.Coefficient = 6;
                    coefficientsErrorCounto.ErrorCount = z[5];
                    resultClass.CoefficientsErrorCount.Add(coefficientsErrorCounto);
                    break;
                case 7:
                    coefficientsErrorCounto.Coefficient = 7;
                    coefficientsErrorCounto.ErrorCount = z[6];
                    resultClass.CoefficientsErrorCount.Add(coefficientsErrorCounto);
                    break;
                case 8:
                    coefficientsErrorCounto.Coefficient = 8;
                    coefficientsErrorCounto.ErrorCount = z[7];
                    resultClass.CoefficientsErrorCount.Add(coefficientsErrorCounto);
                    break;
                case 9:
                    coefficientsErrorCounto.Coefficient = 9;
                    coefficientsErrorCounto.ErrorCount = z[8];
                    resultClass.CoefficientsErrorCount.Add(coefficientsErrorCounto);
                    break;
                case 10:
                    coefficientsErrorCounto.Coefficient = 10;
                    coefficientsErrorCounto.ErrorCount = z[9];
                    resultClass.CoefficientsErrorCount.Add(coefficientsErrorCounto);
                    break;
            }
            

        }

                   
        resultClass.FinalScore = score;
        resultClass.LastCoefficient = zarib;
        resultClass.ErrorCounts = WrongAnswer;
        resultClass.CoefficientOfFirstError = firstWrongZarib;
        resultClass.TimeAverageOfCorrectAnswers = (int)avgRightAwnserTime;
        resultClass.TimeAverageOfErrors = (int)avgWrongAnswerTime;
        string t = JsonUtility.ToJson(resultClass);
        print(t);
        RestClient.DefaultRequestHeaders["Authorization"] = "Bearer " + cookie;
        RestClient.Post("http://cogame.ir/api/game/SendSpatialSpeedMatchResult", t).Then(response =>
        {
        });

    }

    #region UI

    public void resumeBtn()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void restartBtn()
    {
        StartCoroutine(restart());
    }
    public IEnumerator restart()
    {

        resetbtn.SetActive(false);
        endgamePanel.GetComponent<Animator>().Play("exitEndPanel");
        yield return new WaitForSeconds(1f);
        endgamePanel.SetActive(false);
        gameplayPanel.SetActive(false);
        tutorialPanel.SetActive(true);
        timer = 45;
        /* init();
         StartCoroutine(startTutorial());
         Application.targetFrameRate = 60;*/
        Application.LoadLevel(0);

    }

    public void quitBtn()
    {
        Application.Quit();
    }

    public void howtoPlayBtn()
    {
        // method kharabe
        reset = true;
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        startTutorial();

    }

    #endregion UI

}
