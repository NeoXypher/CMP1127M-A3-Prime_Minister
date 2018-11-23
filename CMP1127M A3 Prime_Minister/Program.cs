using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace CMP1127M_A3_Prime_Minister
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] Name = new string[76];
            string[] startDate = new string[76];
            string[] endDate = new string[76];
            Console.WriteLine("The Prime Minister Game");
            while (true)
            {
                Game NewGame = new Game();
                Console.WriteLine("Menu: 1) Who First 2) Which one when 3) Quit ");
                string response = Console.ReadLine();
                if (response == "1")
                {
                    Game.LoadData(ref Name, ref startDate, ref endDate);
                    NewGame.PlayGameMode1(Name, startDate);
                }
                else if (response == "2")
                {
                    Game.LoadData(ref Name, ref startDate, ref endDate);
                    NewGame.PlayGameMode2(Name, startDate, endDate);
                }
                else if (response == "3")
                {
                    Environment.Exit(1);
                }
                else
                {
                    Console.WriteLine("Invalid Input, Please Try Again.");
                }
            }  
        }
    }

    class Game
    {
        string[] options = new string[3];
        string response;
        bool correct = false;
        int chosenAnswer, chosenEntry, number;
        Player NewPlayer = new Player();
        Random rnd = new Random();
        DateTime InitialDate = new DateTime(1995, 1, 1);
        DateTime EndDate = DateTime.Today;

        public static void LoadData(ref string[] Name, ref string[] startDate, ref string[] endDate)
        {
            StreamReader SR = new StreamReader("list_of_prime_ministers_of_uk-1.csv");
            String Line = SR.ReadLine();
            for (int i = 0; i < 76; i++)
            {
                Line = SR.ReadLine();
                string[] item = Line.Split(',');
                Name[i] = item[1];
                startDate[i] = item[3];
                endDate[i] = item[4];
                if (endDate[i] == "Incumbent")
                    endDate[i] = Convert.ToString(DateTime.Today);
                DateTime getdate = Convert.ToDateTime(startDate[i]);
                //Console.WriteLine("Prime Minister: {0}, Start Date: {1}", Name[i], getdate.ToString("dd/MM/yyyy"));
            }
        }

        public void PlayGameMode1(string[] Name, string[] startDate)
        {
            for (int turns = 1; turns <= 5; turns++)
            {
                for (int i = 0; i < 3; i++)
                {
                    options[i] = Name[rnd.Next(0, 76)];
                    if ((i == 1 && options[i] == options[i - 1]) || (i == 2 && (options[i] == options[i - 1] || options[i] == options[i - 2])))
                        i -= 1;
                }
                Console.WriteLine("Which Prime Minister Led First? 1) {0} 2) {1} 3) {2}", options[0], options[1], options[2]);
                response = Console.ReadLine();
                switch (response)
                {
                    case "1":
                        correct = checkAnswer(options[0], options[1], options[2], Name, startDate);
                        break;
                    case "2":
                        correct = checkAnswer(options[1], options[0], options[2], Name, startDate);
                        break;
                    case "3":
                        correct = checkAnswer(options[2], options[0], options[1], Name, startDate);
                        break;
                    default:
                        correct = false;
                        break;
                }
                AddScore();
            }
            EndGame();
            if (response == "Y" || response == "y")
                PlayGameMode1(Name, startDate);
            else if (response != "N" || response != "n")
            { }
        }

        public void PlayGameMode2(string[] Name, string[] startDate, string[] endDate)
        {
            
            for (int turns = 1; turns <= 5; turns++)
            {
                chosenAnswer = rnd.Next(0, 3);
                for (int i = 0; i < 3; i++)
                {
                    chosenEntry = rnd.Next(0, 76);
                    //Console.WriteLine(chosenEntry);
                    options[i] = Name[chosenEntry];
                    if ((i == 1 && options[i] == options[i - 1]) || (i == 2 && (options[i] == options[i - 1] || options[i] == options[i - 2])))
                        i -= 1;
                    if (chosenAnswer == i)
                    {
                        InitialDate = Convert.ToDateTime(startDate[chosenEntry]);
                        //Console.WriteLine(InitialDate);
                        EndDate = Convert.ToDateTime(endDate[chosenEntry]);
                        //Console.WriteLine(EndDate);
                        if (InitialDate > EndDate)
                            i -= 1;
                    }    
                }
                int RangeOfDates = (EndDate - InitialDate).Days;
                DateTime RandomDate = InitialDate.AddDays(rnd.Next(RangeOfDates));
                Console.WriteLine("Which Prime Minister Led on {0}? 1) {1} 2) {2} 3) {3}", RandomDate.ToString("dd/MM/yyyy"), options[0], options[1], options[2]);
                response = Console.ReadLine();
                Int32.TryParse(response, out int number);
                AddScore();
            }
            EndGame();
            if (response == "Y" || response == "y")
                PlayGameMode2(Name, startDate, endDate);
            else if (response != "N" || response != "n")
            { }
        }

        public static bool checkAnswer(string ChosenName, string Alternate1, string Alternate2, string[] Name, string[] startDate)
        {
            string[] Dates = new string[3];
            bool[] found = new bool[3];
            bool result = false;
            for (int i = 0; i < 76; i++)
            {
                if (Name[i] == ChosenName && found[0] == false)
                {
                    found[0] = true;
                    Dates[0] = startDate[i];
                }
                else if (Name[i] == Alternate1 && found[1] == false)
                {
                    found[1] = true;
                    Dates[1] = startDate[i];
                }
                else if (Name[i] == Alternate2 && found[2] == false)
                {
                    found[2] = true;
                    Dates[2] = startDate[i];
                }
            }
            if (Convert.ToDateTime(Dates[0]) < Convert.ToDateTime(Dates[1]) && Convert.ToDateTime(Dates[0]) < Convert.ToDateTime(Dates[2]))
                result = true;
            return result;
        }

        public void AddScore()
        {
            if (number == chosenAnswer + 1 || correct == true)
            {
                Console.WriteLine("Correct!");
                NewPlayer.PlayerScore = 1;
            }
            else
                Console.WriteLine("Incorrect!");
        }

        public void EndGame()
        {
            Console.WriteLine("You got {0} out of 5!", NewPlayer.PlayerScore);
            NewPlayer.ResetScore();
            Console.WriteLine("Would you like to play again? Y/N");
            response = Console.ReadLine();
        }

    }

    class Player
    {
        private int playerScore;

        public int PlayerScore
        {
            get { return playerScore; }
            set { playerScore += value; }
        }

        public void ResetScore()
        {
            playerScore = 0;
        }
    }

}
