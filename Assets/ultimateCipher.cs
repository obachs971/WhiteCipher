using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KModkit;
using System;
using System.Linq;

public class ultimateCipher : MonoBehaviour {
    
    public TextMesh[] screenTexts;
    public string[] wordList;
    public KMBombInfo Bomb;
    public KMBombModule module;
    public AudioClip[] sounds;
    public KMAudio Audio;
    public TextMesh submitText;
   
    
    private string[] matrixWordList =
      {
                "ACID",
                "BUST",
                "CODE",
                "DAZE",
                "ECHO",
                "FILM",
                "GOLF",
                "HUNT",
                "ITCH",
                "JURY",
                "KING",
                "LIME",
                "MONK",
                "NUMB",
                "ONLY",
                "PREY",
                "QUIT",
                "RAVE",
                "SIZE",
                "TOWN",
                "URGE",
                "VERY",
                "WAXY",
                "XYLO",
                "YARD",
                "ZERO",
                "ABORT",
                "BLEND",
                "CRYPT",
                "DWARF",
                "EQUIP",
                "FANCY",
                "GIZMO",
                "HELIX",
                "IMPLY",
                "JOWLS",
                "KNIFE",
                "LEMON",
                "MAJOR",
                "NIGHT",
                "OVERT",
                "POWER",
                "QUILT",
                "RUSTY",
                "STOMP",
                "TRASH",
                "UNTIL",
                "VIRUS",
                "WHISK",
                "XERIC",
                "YACHT",
                "ZEBRA",
                "ADVICE",
                "BUTLER",
                "CAVITY",
                "DIGEST",
                "ELBOWS",
                "FIXURE",
                "GOBLET",
                "HANDLE",
                "INDUCT",
                "JOKING",
                "KNEADS",
                "LENGTH",
                "MOVIES",
                "NIMBLE",
                "OBTAIN",
                "PERSON",
                "QUIVER",
                "RACHET",
                "SAILOR",
                "TRANCE",
                "UPHELD",
                "VANISH",
                "WALNUT",
                "XYLOSE",
                "YANKED",
                "ZODIAC",
                "ALREADY",
                "BROWSED",
                "CAPITOL",
                "DESTROY",
                "ERASING",
                "FLASHED",
                "GRIMACE",
                "HIDEOUT",
                "INFUSED",
                "JOYRIDE",
                "KETCHUP",
                "LOCKING",
                "MAILBOX",
                "NUMBERS",
                "OBSCURE",
                "PHANTOM",
                "QUIETLY",
                "REFUSAL",
                "SUBJECT",
                "TRAGEDY",
                "UNKEMPT",
                "VENISON",
                "WARSHIP",
                "XANTHIC",
                "YOUNGER",
                "ZEPHYRS",
                "ADVOCATE",
                "BACKFLIP",
                "CHIMNEYS",
                "DISTANCE",
                "EXPLOITS",
                "FOCALIZE",
                "GIFTWRAP",
                "HOVERING",
                "INVENTOR",
                "JEALOUSY",
                "KINSFOLK",
                "LOCKABLE",
                "MERCIFUL",
                "NOTECARD",
                "OVERCAST",
                "PERILOUS",
                "QUESTION",
                "RAINCOAT",
                "STEALING",
                "TREASURY",
                "UPDATING",
                "VERTICAL",
                "WISHBONE",
                "XENOLITH",
                "YEARLONG",
                "ZEALOTRY"
        };

    private string[][] pages;
    private string answer;
    private int page;
    private bool submitScreen;
    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    public KMSelectable leftArrow;
    public KMSelectable rightArrow;
    public KMSelectable submit;
    public KMSelectable[] keyboard;
    void Awake()
    {
        moduleId = moduleIdCounter++;
        leftArrow.OnInteract += delegate () { left(leftArrow); return false; };
        rightArrow.OnInteract += delegate () { right(rightArrow); return false; };
        submit.OnInteract += delegate () { submitWord(submit); return false; };
        foreach(KMSelectable keybutton in keyboard)
        {
            KMSelectable pressedButton = keybutton;
            pressedButton.OnInteract += delegate () { letterPress(pressedButton); return false; };
        }
    }
        // Use this for initialization
        void Start ()
    
    {
        submitText.text = "1";
        //Generating random word
        answer = wordList[UnityEngine.Random.Range(0, wordList.Length)].ToUpper();
        Debug.LogFormat("[White Cipher #{0}] Generated Word: {1}", moduleId, answer);
       
        pages = new string[2][];
        pages[0] = new string[3];
        pages[1] = new string[3];
        pages[0][0] = "";
        pages[0][1] = "";
        pages[0][2] = "";
        string encrypt = whitecipher(answer);
        pages[0][0] = encrypt.ToUpper();
        page = 0;
        getScreens();
    }
    string whitecipher(string word)
    {
        Debug.LogFormat("[White Cipher #{0}] Begin Sean Encryption", moduleId);
        string encrypt = SeanEnc(word);
        string kw1 = encrypt.Split(' ')[1];
        Debug.LogFormat("[White Cipher #{0}] Begin Base Caesar Encryption", moduleId);
        encrypt = BaseCaesarEnc(encrypt.Split(' ')[0]);
        Debug.LogFormat("[White Cipher #{0}] Begin Grille Transposition", moduleId);
        kw1 = GrilleTrans(kw1);
        pages[1][0] = kw1.Substring(0, 8);
        pages[1][1] = kw1.Substring(8);
        return encrypt;
    }
    string SeanEnc(string word)
    {
        string encrypt = "";
        string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string kw1 = "";
        for(int aa = 0; aa < 16; aa++)
            kw1 = kw1 + "" + alpha[UnityEngine.Random.Range(0, alpha.Length)];
        
        string key = getKey(kw1.ToUpper(), "ABCDEFGHIJKLMNOPQRSTUVWXYZ", Bomb.GetOnIndicators().Count() % 2 == 0);
        
        Debug.LogFormat("[White Cipher #{0}] Key: {1}", moduleId, key);
        string[] cipher = { key.Substring(0, 13), key.Substring(13)};
        for (int bb = 0; bb < 6; bb++)
        {
            if (cipher[0].IndexOf(word[bb]) >= 0)
                encrypt = encrypt + "" + cipher[1][cipher[0].IndexOf(word[bb])];
            else
                encrypt = encrypt + "" + cipher[0][cipher[1].IndexOf(word[bb])];
            Debug.LogFormat("[White Cipher #{0}] {1} -> {2} ", moduleId, word[bb], encrypt[bb]);
            char top = cipher[0][12];
            char bot = cipher[1][0];
            cipher[0] = bot + "" + cipher[0].Substring(0, 12);
            cipher[1] = cipher[1].Substring(1) + "" + top;
            Debug.LogFormat("[White Cipher #{0}] Key: {1}", moduleId, cipher[0] + "" + cipher[1]);
        }
        return encrypt + " " + kw1;
    }
    string BaseCaesarEnc(string word)
    {
        int offset = UnityEngine.Random.Range(1, 25) + (26 * UnityEngine.Random.Range(1, 6));
        int sum = 0;
        Debug.LogFormat("[White Cipher #{0}] Generated Offset: {1}", moduleId, offset);
        string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string encrypt = "";
        string logoutput = "";
        for (int aa = 0; aa < 6; aa++)
        {
            encrypt = encrypt + "" + alpha[mod(alpha.IndexOf(word[aa]) - offset, 26)];
            sum += (alpha.IndexOf(encrypt[aa]) + 1);
            logoutput = logoutput + "" + (alpha.IndexOf(encrypt[aa]) + 1) + " + ";
            Debug.LogFormat("[White Cipher #{0}] {1} -> {2}", moduleId, word[aa], encrypt[aa]);
        }
        sum = (sum % 8) + 2;
        logoutput = "((" + logoutput.Substring(0, logoutput.Length - 2) + ") % 8) + 2 = " + sum;
        Debug.LogFormat("[White Cipher #{0}] Generated Base: {1}", moduleId, logoutput);
        string baseOffset = "";
        while (offset != 0)
        {
            baseOffset = (offset % sum) + "" + baseOffset;
            offset = offset / sum;
        }
        pages[0][1] = baseOffset.ToUpper();
        return encrypt;
    }
    string GrilleTrans(string word)
    {
        int num = Bomb.GetPortCount() % 4;
        Debug.LogFormat("[White Cipher #{0}] Clockwise rotations: {1}", moduleId, num);
        int[] pos = { 0, 12, 13, 4, 8, 5, 9, 1, 10, 2, 14, 3, 15, 6, 7, 11 }; ;
        
        for (int aa = 0; aa < 16; aa++)
        {
            for (int bb = 0; bb < num; bb++)
                pos[aa] -= 4;
            pos[aa] = mod(pos[aa], 16);
        }
        string encrypt = "";
        for(int cc = 0; cc < 16; cc++)
            encrypt = encrypt + "" + word[pos[cc]];
        Debug.LogFormat("[White Cipher #{0}] {1} -> {2}", moduleId, word, encrypt);
        return encrypt;
    }
    int mod(int n, int m)
    {
        while (n < 0)
            n += m;
        n = n % m;
        return n;
    }
    string getKey(string k, string alpha, bool start)
    {
        for (int aa = 0; aa < k.Length; aa++)
        {
            for (int bb = aa + 1; bb < k.Length; bb++)
            {
                if (k[aa] == k[bb])
                {
                    k = k.Substring(0, bb) + "" + k.Substring(bb + 1);
                    bb--;
                }
            }
            alpha = alpha.Replace(k[aa].ToString(), "");
        }
        if (start)
            return (k + "" + alpha);
        else
            return (alpha + "" + k);
    }
	int correction(int p, int max)
    {
        while (p < 0)
            p += max;
        while (p >= max)
            p -= max;
        return p;
    }
    void left(KMSelectable arrow)
    {
        if(!moduleSolved)
        {
            Audio.PlaySoundAtTransform(sounds[0].name, transform);
            submitScreen = false;
            arrow.AddInteractionPunch();
            page--;
            page = correction(page, pages.Length);
            getScreens();
        }
    }
    void right(KMSelectable arrow)
    {
        if(!moduleSolved)
        {
            Audio.PlaySoundAtTransform(sounds[0].name, transform);
            submitScreen = false;
            arrow.AddInteractionPunch();
            page++;
            page = correction(page, pages.Length);
            getScreens();
        }
    }
    private void getScreens()
    {
        submitText.text = (page + 1) + "";
        screenTexts[0].text = pages[page][0];
        screenTexts[1].text = pages[page][1];
        screenTexts[2].text = pages[page][2];
        screenTexts[0].fontSize = 40;
        if(page == 0)
        {
            screenTexts[0].fontSize = 40;
            screenTexts[1].fontSize = 40;
        }
        else
        {
            screenTexts[0].fontSize = 35;
            screenTexts[1].fontSize = 35;
        }
            

    }
    void submitWord(KMSelectable submitButton)
    {
        if(!moduleSolved)
        {
            submitButton.AddInteractionPunch();
            if(screenTexts[2].text.Equals(answer))
            {
                Audio.PlaySoundAtTransform(sounds[2].name, transform);
                module.HandlePass();
                moduleSolved = true;
                screenTexts[2].text = "";
            }
            else
            {
                Audio.PlaySoundAtTransform(sounds[3].name, transform);
                module.HandleStrike();
                page = 0;
                getScreens();
                submitScreen = false;
            }
        }
    }
    void letterPress(KMSelectable pressed)
    {
        if(!moduleSolved)
        {
            pressed.AddInteractionPunch();
            Audio.PlaySoundAtTransform(sounds[1].name, transform);
            if (submitScreen)
            {
                if(screenTexts[2].text.Length < 6)
                {
                    screenTexts[2].text = screenTexts[2].text + "" + pressed.GetComponentInChildren<TextMesh>().text;
                }
            }
            else
            {
                submitText.text = "SUB";
                screenTexts[0].text = "";
                screenTexts[1].text = "";
                screenTexts[2].text = pressed.GetComponentInChildren<TextMesh>().text;
                submitScreen = true;
            }
        }
    }
#pragma warning disable 414
    private string TwitchHelpMessage = "Move to other screens using !{0} right|left|r|l|. Submit the decrypted word with !{0} submit qwertyuiopasdfghjklzxcvbnm";
#pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {

        if (command.EqualsIgnoreCase("right") || command.EqualsIgnoreCase("r"))
        {
            yield return null;
            rightArrow.OnInteract();
            yield return new WaitForSeconds(0.1f);

        }
        if (command.EqualsIgnoreCase("left") || command.EqualsIgnoreCase("l"))
        {
            yield return null;
            leftArrow.OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
        string[] split = command.ToUpperInvariant().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        if (split.Length != 2 || !split[0].Equals("SUBMIT") || split[1].Length != 6) yield break;
        int[] buttons = split[1].Select(getPositionFromChar).ToArray();
        if (buttons.Any(x => x < 0)) yield break;

        yield return null;

        yield return new WaitForSeconds(0.1f);
        foreach (char let in split[1])
        {
            keyboard[getPositionFromChar(let)].OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.1f);
        submit.OnInteract();
        yield return new WaitForSeconds(0.1f);
    }

    private int getPositionFromChar(char c)
    {
        return "QWERTYUIOPASDFGHJKLZXCVBNM".IndexOf(c);
    }
}
