using System;
using SFML.Learning;
using SFML.System;
using SFML.Window;

 class FindACouple : Game
{

    static string sound = LoadSound("sound.wav");
    static string[] iconsName;

    static int[,] cards;
    static int cardCount = 20;
    static int cardWidth = 100;
    static int cardHeight = 100;

    static int countPerLine = 5;
    static int space = 40;
    static int leftOffset = 70;
    static int topOffset = 20;

    static void LoadIcon()
    {
        iconsName = new string[7];

        iconsName[0] = LoadTexture("Icon_close.png");

        for (int i = 1; i < iconsName.Length ; i++)
        {
            iconsName[i] = LoadTexture("Icon_" + i.ToString() + ".png");
        }
    }

    static void Shuffle(int[] arr)
    {
        Random rand = new Random();

        for (int i = arr.Length - 1; i >= 1; i--)
        {
            int j = rand.Next(1, i + 1);

            int tmp = arr[j];
            arr[j] = arr[i];
            arr[i] = tmp;
        }
    }

    static void InitCard()
    {
        Random rnd = new Random();
        cards = new int[cardCount, 6];

        int[] iconId = new int[cards.GetLength(0)];
        int id = 0;
        for (int i = 0; i < iconId.Length; i++)
        {
            if (i % 2 == 0)
            {
                id = rnd.Next(1, 6);
            }

            iconId[i] = id;
        }
        Shuffle(iconId);
        Shuffle(iconId);
        Shuffle(iconId);

        for (int i = 0; i < cards.GetLength(0); i++)
        {
            cards[i, 0] = 0; //state
            cards[i, 1] = (i % countPerLine) * (cardWidth + space) + leftOffset; //posX
            cards[i, 2] = (i / countPerLine) * (cardHeight + space) + topOffset; //posY
            cards[i, 3] = cardWidth; //width
            cards[i, 4] = cardHeight; //height
            cards[i, 5] = iconId[i]; //id
        }
    }

    static void SetStateToAllCards(int state)
    {
        for (int i = 0; i < cards.GetLength(0); i++)
        {
            cards[i, 0] = state;
        }
    }

    static void DrawCards()
    {
        for (int i = 0; i < cards.GetLength(0); i++)
        {
            if (cards[i, 0] == 1) //open
            {

                DrawSprite(iconsName[cards[i, 5]], cards[i, 1], cards[i,2]);
                /*if (cards[i, 5] == 1)
                {
                    SetFillColor(0, 50, 0);
                    FillRectangle(cards[i, 1], cards[i, 2], cards[i, 3], cards[i, 4]);
                }
                if (cards[i, 5] == 2)
                {
                    SetFillColor(0, 0, 50);
                    FillRectangle(cards[i, 1], cards[i, 2], cards[i, 3], cards[i, 4]);
                }
                if (cards[i, 5] == 3)
                {
                    SetFillColor(50, 0, 0);
                    FillRectangle(cards[i, 1], cards[i, 2], cards[i, 3], cards[i, 4]);
                }
                if (cards[i, 5] == 4)
                {
                    SetFillColor(0, 100, 0);
                    FillRectangle(cards[i, 1], cards[i, 2], cards[i, 3], cards[i, 4]);
                }
                if (cards[i, 5] == 5)
                {
                    SetFillColor(0, 0, 100);
                    FillRectangle(cards[i, 1], cards[i, 2], cards[i, 3], cards[i, 4]);
                }
                if (cards[i, 5] == 6)
                {
                    SetFillColor(100, 0, 0);
                    FillRectangle(cards[i, 1], cards[i, 2], cards[i, 3], cards[i, 4]);
                }*/
            }
            if (cards[i, 0] == 0) //close
            {
                DrawSprite(iconsName[0], cards[i, 1], cards[i, 2]);

                /*SetFillColor(30, 30, 30);
                FillRectangle(cards[i, 1], cards[i, 2], cards[i, 3], cards[i, 4]);*/
            }

            
        }

    }
    static int GetIndexCardByMousePosition()
    {
        for (int i = 0; i < cards.GetLength(0); i++)
        {
            if (MouseX >= cards[i, 1] && MouseX <= cards[i, 1] + cards[i, 3] && MouseY >= cards[i, 2] && MouseY <= cards[i, 2] + cards[i, 4] )
            {
                return i;
            }
        }


        return -1;
    }
    static void Main(string[] args)
    {
        int openCardAmount = 0;
        int firstOpenCardIndex = -1;
        int secondOpenCardIndex = -1;
        int remainingCard = cardCount;

        int isWin = -1; // 0 - проигрыш, 1 - победа

        LoadIcon();
        SetFont("comic.ttf");

        InitWindow(800, 600, "Find a couple");

        InitCard();
        SetStateToAllCards(1);

        ClearWindow(26, 46, 92);
        DrawCards();
        DisplayWindow();
        Delay(4000); 
        SetStateToAllCards(0);

        
        DateTime timeStart = DateTime.Now;
        DateTime timeNow = DateTime.Now;
        

        while (true)
        {
            DispatchEvents();

            if (remainingCard == 0)
            {
                isWin = 1;
                break;
            }
            if (timeNow.Subtract(timeStart).Minutes >= 3)
            {
                isWin = 0;
                break;
            }

            if (openCardAmount == 2)
            {
                if (cards[firstOpenCardIndex, 5] == cards[secondOpenCardIndex, 5])
                {
                    cards[firstOpenCardIndex, 0] = -1;
                    cards[secondOpenCardIndex, 0] = -1;

                    remainingCard -= 2;
                }
                else
                {
                    cards[firstOpenCardIndex, 0] = 0;
                    cards[secondOpenCardIndex, 0] = 0;
                }

                firstOpenCardIndex = -1;
                secondOpenCardIndex = -1;
                openCardAmount = 0;

                Delay(1500);
            }

            if(GetMouseButtonDown(0) == true)
            {
                int index = GetIndexCardByMousePosition();

                if (index != -1 && index != firstOpenCardIndex)
                {
                    cards[index, 0] = 1;
                    openCardAmount++;
                }

                if (openCardAmount == 1) firstOpenCardIndex = index;
                if (openCardAmount == 2) secondOpenCardIndex = index;


                PlaySound(sound, 5);
            }
            ClearWindow(26, 46, 92);

            SetFillColor(255, 255, 255);
            timeNow = DateTime.Now;
            DrawText(90, 560, $"Необходимо открыть все карты за три минуты. Прошло времени:   {timeNow.Subtract(timeStart).Minutes.ToString()}:{timeNow.Subtract(timeStart).Seconds.ToString()}", 18);

            DrawCards();

            DisplayWindow();

            Delay(1);
        }


        ClearWindow();
        SetFillColor(255, 255, 255);
        if (isWin == 1)
            DrawText(200, 300, "Поздравляю! Ты открыл все карты!", 24);
        if (isWin == 0)
            DrawText(200, 300, "Время вышло! Ты проиграл!", 24);


        DisplayWindow();

        Console.WriteLine("Выиграл!");
        Delay(5000);

    }
}

