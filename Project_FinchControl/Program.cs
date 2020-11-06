using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FinchAPI;

namespace Project_FinchControl
{
    public enum Command
    {
        NONE, MOVEFORWARD, MOVEBACKWARD, STOPMOTORS, WAIT, TURNRIGHT, TURNLEFT, LEDON, LEDOFF, NOTEON, NOTEOFF, GETTEMPERATURE, DONE
    }

    // **************************************************
    //
    // Title: Finch Control - S5 (Persistence)
    // Author: Quentin Elam
    // Description: An application that allows the user to
    //              program the finch robot
    // Application Type: Console
    // Dated Created: 11/4/2020
    // Last Modified: 11/6/2020
    //
    // **************************************************

    class Program
    {
        /// <summary>
        /// first method run when the app starts up
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            DisplaySetTheme();

            DisplayWelcomeScreen();
            DisplayMenuScreen();
            DisplayClosingScreen();
        }
        /// <summary>
        /// setup the console theme
        /// </summary>
        static void DisplaySetTheme()
        {
            (ConsoleColor foregroundColor, ConsoleColor backgroundcolor) themeColors;
            bool themeChosen = true;
            bool validResponse = true;
            bool invalidResponse = true;

            string userResponse;

            //
            // set current theme from data
            //

            themeColors = ReadThemeData();

            Console.ForegroundColor = themeColors.foregroundColor;
            Console.BackgroundColor = themeColors.backgroundcolor;

            do
            {
                DisplayScreenHeader("Set Application Theme");

                Console.WriteLine($"\tCurrent foreground color: {Console.ForegroundColor}");
                Console.WriteLine($"\tCurrent background color: {Console.BackgroundColor}");
                Console.WriteLine();

                Console.Write("\tWould you like to change the current theme? [ yes | no ]:");
                userResponse = Console.ReadLine().ToLower();

                if (userResponse == "yes")
                {
                    do
                    {
                        Console.WriteLine();
                        themeColors.foregroundColor = GetConsoleColorFromUser("foreground");
                        themeColors.backgroundcolor = GetConsoleColorFromUser("background");

                        //
                        // Set the new theme
                        //

                        Console.ForegroundColor = themeColors.foregroundColor;
                        Console.BackgroundColor = themeColors.backgroundcolor;
                        Console.Clear();

                        DisplayScreenHeader("Set Application Theme");
                        Console.WriteLine($"\tNew foreground color: {Console.ForegroundColor}");
                        Console.WriteLine($"\tNew background color: {Console.BackgroundColor}");
                        do
                        {
                            Console.WriteLine();
                            Console.Write("\tWould you like to keep this theme? [ yes | no ]:");
                            userResponse = Console.ReadLine().ToLower();

                            if (userResponse == "no")
                            {
                                themeChosen = true;
                                invalidResponse = false;
                            }
                            else if (userResponse == "yes")
                            {
                                themeChosen = false;
                                validResponse = true;
                                invalidResponse = false;
                                WriteThemeData(themeColors.foregroundColor, themeColors.backgroundcolor);
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("\tInvalid Response. Pease enter \"yes\" or \"no\"");
                                invalidResponse = true;
                            }

                        } while (invalidResponse);

                    } while (themeChosen);

                }

                else if (userResponse == "no")
                {
                    validResponse = true;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("\tInvalid Response. Pease enter \"yes\" or \"no\"");
                    validResponse = false;
                    DisplayContinuePrompt();
                }

            } while (!validResponse);

            DisplayContinuePrompt();
        }

        /// <summary>
        /// Writes the theme data to the text file
        /// </summary>
        /// <param name="foregroundColor"></param>
        /// <param name="backgroundcolor"></param>
        static void WriteThemeData(ConsoleColor foregroundColor, ConsoleColor backgroundcolor)
        {
            string dataPath = @"Data\Theme.txt";

            File.WriteAllText(dataPath, foregroundColor.ToString() + "\n");
            File.AppendAllText(dataPath, backgroundcolor.ToString());
        }

        /// <summary>
        /// Gets the foreground and background colors from the user
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        static ConsoleColor GetConsoleColorFromUser(string property)
        {
            ConsoleColor consoleColor;
            bool validConsoleColor;

            do
            {
                Console.Write($"\tChoose a color for the {property}: ");
                validConsoleColor = Enum.TryParse<ConsoleColor>(Console.ReadLine(), true, out consoleColor);

                if (!validConsoleColor)
                {
                    Console.WriteLine("\n\t***** It appears you did not enter a valid console color. Please try again. *****\n");
                }
                else
                {
                    validConsoleColor = true;
                }

            } while (!validConsoleColor);

            return consoleColor;
        }

        /// <summary>
        /// Reads the theme data from the text file
        /// </summary>
        /// <returns></returns>
        static (ConsoleColor foregroundColor, ConsoleColor backgroundcolor) ReadThemeData()
        {
            string dataPath = @"Data\Theme.txt";
            string[] themeColors;

            ConsoleColor foregroundColor;
            ConsoleColor backgroundColor;

            themeColors = File.ReadAllLines(dataPath);

            Enum.TryParse(themeColors[0], true, out foregroundColor);
            Enum.TryParse(themeColors[1], true, out backgroundColor);

            return (foregroundColor, backgroundColor);
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Main Menu                                 *
        /// *****************************************************************
        /// </summary>
        static void DisplayMenuScreen()
        {
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;

            Finch finchRobot = new Finch();

            do
            {
                DisplayScreenHeader("Main Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Connect Finch Robot");
                Console.WriteLine("\tb) Talent Show");
                Console.WriteLine("\tc) Data Recorder");
                Console.WriteLine("\td) Alarm System");
                Console.WriteLine("\te) User Programming");
                Console.WriteLine("\tf) Disconnect Finch Robot");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayConnectFinchRobot(finchRobot);
                        break;

                    case "b":
                        DisplayTalentShowMenuScreen(finchRobot);
                        break;

                    case "c":
                        DataRecorderDisplayMenuScreen(finchRobot);
                        break;

                    case "d":
                        LightAlarmDisplayMenuScreen(finchRobot);
                        break;

                    case "e":
                        UserProgrammingDisplayMenuScreen(finchRobot);
                        break;

                    case "f":
                        DisplayDisconnectFinchRobot(finchRobot);
                        break;

                    case "q":
                        DisplayDisconnectFinchRobot(finchRobot);
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }

        #region TALENT SHOW

        /// <summary>
        /// *****************************************************************
        /// *                     Talent Show Menu                          *
        /// *****************************************************************
        /// </summary>
        static void DisplayTalentShowMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitTalentShowMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Talent Show Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Light and Sound");
                Console.WriteLine("\tb) Dance");
                Console.WriteLine("\tc) Mixing It Up");
                Console.WriteLine("\td) ");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayLightAndSound(finchRobot);
                        break;

                    case "b":
                        DisplayDance(finchRobot);
                        break;

                    case "c":
                        MixingItUp(finchRobot);
                        break;

                    case "d":

                        break;

                    case "q":
                        quitTalentShowMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitTalentShowMenu);
        }


        /// <summary>
        /// *****************************************************************
        /// *               Talent Show > Light and Sound                   *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayLightAndSound(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Light and Sound");

            Console.WriteLine("\tThe Finch robot will now show off its lights and the sounds it can make!");
            DisplayContinuePrompt();

            Parallel.Invoke(() => ExploreLeds(finchRobot), () => ExploreSounds(finchRobot));
        }

        static void ExploreSounds(Finch finchRobot)
        {
            finchRobot.noteOn(523);
            finchRobot.wait(100);
            finchRobot.noteOn(587);
            finchRobot.wait(100);
            finchRobot.noteOn(659);
            finchRobot.wait(100);
            finchRobot.noteOn(698);
            finchRobot.wait(100);
            finchRobot.noteOn(784);
            finchRobot.wait(100);
            finchRobot.noteOn(880);
            finchRobot.wait(100);
            finchRobot.noteOn(988);
            finchRobot.wait(100);
            finchRobot.noteOn(1047);
            finchRobot.wait(100);
            finchRobot.noteOff();

            for (int soundValue = 100; soundValue < 15000; soundValue = soundValue + 80)
            {
                finchRobot.noteOn(soundValue);
                finchRobot.wait(1);
                finchRobot.noteOff();
            }
        }

        static void ExploreLeds(Finch finchRobot)
        {
            for (int ledValue = 0; ledValue < 255; ledValue = ledValue + 5)
            {
                finchRobot.setLED(ledValue, ledValue, ledValue);
                finchRobot.wait(10);
            }

            finchRobot.setLED(255, 0, 0);
            finchRobot.wait(500);
            finchRobot.setLED(255, 100, 0);
            finchRobot.wait(500);
            finchRobot.setLED(255, 240, 0);
            finchRobot.wait(500);
            finchRobot.setLED(100, 255, 0);
            finchRobot.wait(500);
            finchRobot.setLED(0, 255, 225);
            finchRobot.wait(500);
            finchRobot.setLED(0, 0, 255);
            finchRobot.wait(500);
            finchRobot.setLED(255, 0, 255);
            finchRobot.wait(500);
            finchRobot.setLED(0, 0, 0);


            Console.Clear();
            DisplayContinuePrompt();
        }


        /// ****************************************
        /// *        Talent Show > Dance           *
        /// ****************************************

        static void DisplayDance(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Dance");

            Console.WriteLine("\tThe Finch robott will now show off a dance called \"Hop Step.\"");
            DisplayContinuePrompt();

            for (int count = 0; count < 5; count++)
            {
                finchRobot.setMotors(100, 100);
                finchRobot.wait(700);
                finchRobot.setMotors(-255, -255);
                finchRobot.wait(200);
                finchRobot.setMotors(255, 255);
                finchRobot.wait(200);
                finchRobot.setMotors(0, 0);
                finchRobot.wait(200);
                finchRobot.setMotors(-100, 100);
                finchRobot.wait(800);
                finchRobot.setMotors(0, 0);
            }
            Console.Clear();
            DisplayContinuePrompt();
        }


        /// ******************************************
        /// *       Talent Show > Mixing It Up       *
        /// ******************************************

        static void MixingItUp(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Mixing It Up");

            Console.WriteLine("\tThe Finch robot will now sing the Mario Theme, dance, and light up!");

            DisplayContinuePrompt();

            Parallel.Invoke(() => MarioDance(finchRobot), () => MarioTheme(finchRobot), () => MarioLights(finchRobot));

        }

        //
        // Dance for "Mixing It Up"
        //

        static void MarioDance(Finch finchRobot)
        {
            for (int count = 0; count < 2; count++)
            {
                finchRobot.setMotors(30, 50);
                finchRobot.wait(500);
                finchRobot.setMotors(50, 30);
                finchRobot.wait(500);
            }

            for (int count = 0; count < 2; count++)
            {
                finchRobot.setMotors(-50, -30);
                finchRobot.wait(500);
                finchRobot.setMotors(-30, -50);
                finchRobot.wait(500);
            }

            for (int count = 0; count < 3; count++)
            {
                finchRobot.setMotors(50, 50);
                finchRobot.wait(1000);
                finchRobot.setMotors(0, 0);
                finchRobot.wait(300);
            }

            for (int count = 0; count < 3; count++)
            {
                finchRobot.setMotors(-50, -50);
                finchRobot.wait(1000);
                finchRobot.setMotors(0, 0);
                finchRobot.wait(300);
            }

            finchRobot.setMotors(0, 50);
            finchRobot.wait(3000);
            finchRobot.setMotors(50, 0);
            finchRobot.wait(3000);

            for (int count = 0; count < 5; count++)
            {
                finchRobot.setMotors(100, 100);
                finchRobot.wait(100);
                finchRobot.setMotors(-100, -100);
                finchRobot.wait(100);
            }
            finchRobot.setMotors(0, 0);
        }

        //
        // Lights for "Mixing It Up"
        //

        static void MarioLights(Finch finchRobot)
        {
            for (int Count = 0; Count < 1.5; Count++)
            {
                finchRobot.setLED(255, 0, 0);
                finchRobot.wait(2000);
                finchRobot.setLED(0, 0, 0);
                finchRobot.wait(500);
                finchRobot.setLED(0, 255, 0);
                finchRobot.wait(2000);
                finchRobot.setLED(0, 0, 0);
                finchRobot.wait(500);
                finchRobot.setLED(255, 0, 155);
                finchRobot.wait(2000);
                finchRobot.setLED(0, 0, 0);
                finchRobot.wait(500);
                finchRobot.setLED(0, 0, 255);
                finchRobot.wait(2000);
                finchRobot.setLED(0, 0, 0);
                finchRobot.wait(500);
            }
            Console.Clear();
        }

        //
        // Song for "Mixing It Up"
        //

        static void MarioTheme(Finch finchRobot)
        {
            LineOne(finchRobot);
            LineTwo(finchRobot);
            LineThree(finchRobot);
            LineTwo(finchRobot);
            LineThree(finchRobot);
            LineSix(finchRobot);
            LineSeven(finchRobot);
            LineEight(finchRobot);
            LineSix(finchRobot);
            LineSeven(finchRobot);
            LineEleven(finchRobot);

            DisplayContinuePrompt();
        }

        static void LineOne(Finch finchRobot)
        {
            // E
            finchRobot.noteOn(659);
            finchRobot.wait(150);
            finchRobot.noteOff();
            finchRobot.wait(30);
            // E
            finchRobot.noteOn(659);
            finchRobot.wait(150);
            finchRobot.noteOff();
            finchRobot.wait(80);
            // E
            finchRobot.noteOn(659);
            finchRobot.wait(150);
            finchRobot.noteOff();
            finchRobot.wait(100);
            // C
            finchRobot.noteOn(523);
            finchRobot.wait(150);
            finchRobot.noteOff();
            finchRobot.wait(50);
            // E
            finchRobot.noteOn(659);
            finchRobot.wait(200);
            finchRobot.noteOff();
            finchRobot.wait(100);
            // G
            finchRobot.noteOn(784);
            finchRobot.wait(100);
            finchRobot.noteOff();
            finchRobot.wait(500);
            // G
            finchRobot.noteOn(784);
            finchRobot.wait(100);
            finchRobot.noteOff();
            finchRobot.wait(500);
        }

        static void LineTwo(Finch finchRobot)
        {

            // C
            finchRobot.noteOn(523);
            finchRobot.wait(200);
            finchRobot.noteOff();
            finchRobot.wait(200);
            // G
            finchRobot.noteOn(784);
            finchRobot.wait(200);
            finchRobot.noteOff();
            finchRobot.wait(200);
            // E
            finchRobot.noteOn(659);
            finchRobot.wait(200);
            finchRobot.noteOff();
            finchRobot.wait(150);
            // A
            finchRobot.noteOn(880);
            finchRobot.wait(200);
            finchRobot.noteOff();
            finchRobot.wait(150);
            // B
            finchRobot.noteOn(988);
            finchRobot.wait(200);
            finchRobot.noteOff();
            finchRobot.wait(100);
            // A#
            finchRobot.noteOn(932);
            finchRobot.wait(150);
            finchRobot.noteOff();
            finchRobot.wait(30);
            // A
            finchRobot.noteOn(880);
            finchRobot.wait(150);
            finchRobot.noteOff();
            finchRobot.wait(150);
        }

        static void LineThree(Finch finchRobot)
        {
            // G
            finchRobot.noteOn(784);
            finchRobot.wait(150);
            finchRobot.noteOff();
            finchRobot.wait(50);
            // E
            finchRobot.noteOn(659);
            finchRobot.wait(150);
            finchRobot.noteOff();
            finchRobot.wait(80);
            // G
            finchRobot.noteOn(784);
            finchRobot.wait(150);
            finchRobot.noteOff();
            finchRobot.wait(50);
            // A
            finchRobot.noteOn(880);
            finchRobot.wait(150);
            finchRobot.noteOff();
            finchRobot.wait(80);
            // F
            finchRobot.noteOn(698);
            finchRobot.wait(150);
            finchRobot.noteOff();
            finchRobot.wait(50);
            // G
            finchRobot.noteOn(784);
            finchRobot.wait(150);
            finchRobot.noteOff();
            finchRobot.wait(50);
            // E
            finchRobot.noteOn(659);
            finchRobot.wait(150);
            finchRobot.noteOff();
            finchRobot.wait(200);
            // C
            finchRobot.noteOn(523);
            finchRobot.wait(150);
            finchRobot.noteOff();
            finchRobot.wait(30);
            // B
            finchRobot.noteOn(988);
            finchRobot.wait(150);
            finchRobot.noteOff();
            finchRobot.wait(30);
            // D
            finchRobot.noteOn(587);
            finchRobot.wait(150);
            finchRobot.noteOff();
            finchRobot.wait(250);
        }

        static void LineSix(Finch finchRobot)
        {
            // G
            finchRobot.noteOn(784);
            finchRobot.wait(120);
            finchRobot.noteOff();
            finchRobot.wait(50);
            // F#
            finchRobot.noteOn(740);
            finchRobot.wait(120);
            finchRobot.noteOff();
            finchRobot.wait(50);
            // F
            finchRobot.noteOn(698);
            finchRobot.wait(120);
            finchRobot.noteOff();
            finchRobot.wait(50);
            // D#
            finchRobot.noteOn(622);
            finchRobot.wait(120);
            finchRobot.noteOff();
            finchRobot.wait(150);
            // E
            finchRobot.noteOn(659);
            finchRobot.wait(120);
            finchRobot.noteOff();
            finchRobot.wait(150);
        }

        static void LineSeven(Finch finchRobot)
        {
            // G#
            finchRobot.noteOn(830);
            finchRobot.wait(120);
            finchRobot.noteOff();
            finchRobot.wait(50);
            // A
            finchRobot.noteOn(880);
            finchRobot.wait(120);
            finchRobot.noteOff();
            finchRobot.wait(50);
            // C
            finchRobot.noteOn(523);
            finchRobot.wait(120);
            finchRobot.noteOff();
            finchRobot.wait(250);
            // A
            finchRobot.noteOn(880);
            finchRobot.wait(120);
            finchRobot.noteOff();
            finchRobot.wait(50);
            // C
            finchRobot.noteOn(523);
            finchRobot.wait(120);
            finchRobot.noteOff();
            finchRobot.wait(50);
            // D
            finchRobot.noteOn(587);
            finchRobot.wait(120);
            finchRobot.noteOff();
            finchRobot.wait(200);
        }

        static void LineEight(Finch finchRobot)
        {
            // G
            finchRobot.noteOn(784);
            finchRobot.wait(120);
            finchRobot.noteOff();
            finchRobot.wait(50);
            // F#
            finchRobot.noteOn(740);
            finchRobot.wait(120);
            finchRobot.noteOff();
            finchRobot.wait(50);
            // F
            finchRobot.noteOn(698);
            finchRobot.wait(120);
            finchRobot.noteOff();
            finchRobot.wait(50);
            // D#
            finchRobot.noteOn(622);
            finchRobot.wait(120);
            finchRobot.noteOff();
            finchRobot.wait(150);
            // E
            finchRobot.noteOn(659);
            finchRobot.wait(120);
            finchRobot.noteOff();
            finchRobot.wait(100);
            // C
            finchRobot.noteOn(523);
            finchRobot.wait(150);
            finchRobot.noteOff();
            finchRobot.wait(200);
            // C
            finchRobot.noteOn(523);
            finchRobot.wait(150);
            finchRobot.noteOff();
            finchRobot.wait(50);
            // C
            finchRobot.noteOn(523);
            finchRobot.wait(150);
            finchRobot.noteOff();
            finchRobot.wait(150);

        }

        static void LineEleven(Finch finchRobot)
        {
            // D#
            finchRobot.noteOn(622);
            finchRobot.wait(200);
            finchRobot.noteOff();
            finchRobot.wait(200);
            // D
            finchRobot.noteOn(587);
            finchRobot.wait(200);
            finchRobot.noteOff();
            finchRobot.wait(200);
            // C
            finchRobot.noteOn(523);
            finchRobot.wait(200);
            finchRobot.noteOff();

        }
        #endregion

        #region DATA RECORDER

        static void DataRecorderDisplayMenuScreen(Finch finchRobot)
        {
            int numberOfDataPoints = 0;
            double dataPointFrequency = 0;
            double[] temperatures = null;

            Console.CursorVisible = true;

            bool quitMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Data Recorder Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Number of Data Points");
                Console.WriteLine("\tb) Frequency of Data Points");
                Console.WriteLine("\tc) Get Data");
                Console.WriteLine("\td) Show Data");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        numberOfDataPoints = DataRecorderDisplayGetNumberOfDataPoints();
                        break;

                    case "b":
                        dataPointFrequency = DataRecorderDisplayGetDataPointFrequency();
                        break;

                    case "c":
                        temperatures = DataRecorderDisplayGetData(numberOfDataPoints, dataPointFrequency, finchRobot);
                        break;

                    case "d":
                        DataRecorderDisplayData(temperatures);
                        break;

                    case "q":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);

        }


        /// <summary>
        /// displays the data recorded
        /// </summary>
        /// <param name="temperatures"></param>
        private static void DataRecorderDisplayData(double[] temperatures)
        {
            DisplayScreenHeader("Show Data");

            DataRecorderDisplayTableTemperatures(temperatures);

            DisplayContinuePrompt();
        }

        /// <summary>
        /// displays a table format
        /// </summary>
        /// <param name="temperatures"></param>

        static void DataRecorderDisplayTableTemperatures(double[] temperatures)
        {
            //
            // Display table headers
            //

            Console.WriteLine(
                "Recording #".PadLeft(15) +
                "Reading".PadLeft(15)
                );
            Console.WriteLine(
                "__________".PadLeft(15) +
                "__________".PadLeft(15)
                );

            //
            // display table data
            // 

            for (int index = 0; index < temperatures.Length; index++)
            {
                Console.WriteLine(
               (index + 1).ToString().PadLeft(15) +
               temperatures[index].ToString("n1").PadLeft(15)
               );
            }
        }

        /// <summary>
        /// display the data collected
        /// </summary>
        /// <param name="numberOfDataPoints"></param>
        /// <param name="dataPointFrequency"></param>
        /// <param name="finchRobot"></param>
        /// <returns>temperatures</returns>

        static double[] DataRecorderDisplayGetData(int numberOfDataPoints, double dataPointFrequency, Finch finchRobot)
        {
            double[] average = new double[numberOfDataPoints];
            double[] temperatures = new double[numberOfDataPoints];
            double[] leftLightSensor = new double[numberOfDataPoints];
            double[] rightLightSensor = new double[numberOfDataPoints];
            double[] temperaturesFahrenheit = new double[numberOfDataPoints];


            DisplayScreenHeader("Get Data");

            Console.WriteLine($"\tNumber of data points: {numberOfDataPoints}");
            Console.WriteLine($"\tData point frequency: {dataPointFrequency}");
            Console.WriteLine();
            Console.WriteLine("\tThe Finch Robot is ready to beging recording the data.");
            Console.WriteLine("\tDo you want to record temperature data or light sensor data.");
            Console.WriteLine();
            Console.WriteLine("\tPlease type \"Temperature\" or \"Light\"");
            string userResponse = Console.ReadLine().ToLower();

            if (userResponse == "temperature")
            {

                Console.WriteLine();
                Console.WriteLine($"\tYou entered: {userResponse}");
                Console.WriteLine();
                Console.WriteLine("The Finch Robot will now record tempurature data.");
                DisplayContinuePrompt();

                for (int index = 0; index < numberOfDataPoints; index++)
                {
                    temperatures[index] = finchRobot.getTemperature();

                    temperaturesFahrenheit[index] = ConvertCelsiusToFahrenheit(temperatures[index]);

                    Console.WriteLine($"\tReading {index + 1}: {temperaturesFahrenheit[index].ToString("n1")}");
                    int waitInSeconds = (int)(dataPointFrequency * 1000);
                    finchRobot.wait(waitInSeconds);
                }

                Console.WriteLine();
                Console.WriteLine("\tThe data recording is complete.");
                DisplayContinuePrompt();

                DisplayScreenHeader("Show Data");

                Console.WriteLine("\tTable of Temperatures");
                Console.WriteLine();

                DataRecorderDisplayTableTemperatures(temperaturesFahrenheit);

                DisplayContinuePrompt();


            }
            else if (userResponse == "light")
            {
                Console.WriteLine();
                Console.WriteLine($"\tYou entered: {userResponse}");
                Console.WriteLine();
                Console.WriteLine("The Finch Robot will now record light data.");
                DisplayContinuePrompt();

                for (int index = 0; index < numberOfDataPoints; index++)
                {
                    leftLightSensor[index] = finchRobot.getLeftLightSensor();
                    rightLightSensor[index] = finchRobot.getRightLightSensor();

                    average[index] = (leftLightSensor[index] + rightLightSensor[index]) / 2;

                    Console.WriteLine($"\tReading Average {index + 1}: {average[index]}");
                    int waitInSeconds = (int)(dataPointFrequency * 1000);
                    finchRobot.wait(waitInSeconds);

                }

                Console.WriteLine();
                Console.WriteLine("\tThe data recording is complete.");
                DisplayContinuePrompt();

                DisplayScreenHeader("Show Data");

                Console.WriteLine("\tTable of Light Readings");
                Console.WriteLine();

                Console.WriteLine(
                "Recording #".PadLeft(15) +
                "Reading".PadLeft(15)
                );
                Console.WriteLine(
                    "__________".PadLeft(15) +
                    "__________".PadLeft(15)
                    );

                //
                // display table data
                // 

                for (int index = 0; index < numberOfDataPoints; index++)
                {
                    Console.WriteLine(
                   (index + 1).ToString().PadLeft(15) +
                   average[index].ToString("n1").PadLeft(15)
                   );
                }

                DisplayContinuePrompt();



            }
            return temperaturesFahrenheit;
        }

        /// <summary>
        /// Converts from celsius to fahrenheit
        /// </summary>
        /// <param name="v"></param>
        private static double ConvertCelsiusToFahrenheit(double temperaturesCelsius)
        {
            double fahrenheit;

            fahrenheit = (temperaturesCelsius * 9 / 5) + 32;

            return fahrenheit;
        }


        /// <summary>
        ///  get the frequency of data points from user
        /// </summary>
        /// <returns>frequency of data points</returns>
        static double DataRecorderDisplayGetDataPointFrequency()
        {

            bool validDouble;
            double DataPointFrequency;
            string userResponse;

            do
            {
                validDouble = true;
                DisplayScreenHeader("Data Point Frequency");

                Console.Write("Enter the frequency (in seconds) of data points you want to collect: ");

                //
                // validate user input
                //

                userResponse = Console.ReadLine();

                if (!double.TryParse(userResponse, out DataPointFrequency))
                {
                    validDouble = false;

                    Console.WriteLine();
                    Console.WriteLine("You did not enter a valid number. Please enter a valid number.");
                    Console.WriteLine();

                    DisplayContinuePrompt();
                }

            } while (!validDouble);

            Console.WriteLine();
            Console.WriteLine($"You entered: {DataPointFrequency}");
            DisplayContinuePrompt();

            return DataPointFrequency;
        }

        /// <summary>
        /// get the number of data points from the user
        /// </summary>
        /// <returns>number of data points</returns>
        static int DataRecorderDisplayGetNumberOfDataPoints()
        {
            bool validInterger;
            int numberOfDataPoints;
            string userResponse;

            do
            {
                validInterger = true;
                DisplayScreenHeader("Number of Data Points");

                Console.Write("Enter the number of data points you want to collect: ");
                userResponse = Console.ReadLine();

                //
                // validate user input
                //
                if (!int.TryParse(userResponse, out numberOfDataPoints))
                {
                    validInterger = false;

                    Console.WriteLine();
                    Console.WriteLine("You did not enter a valid number. Please enter a valid number.");
                    Console.WriteLine();

                    DisplayContinuePrompt();
                }
            } while (!validInterger);

            Console.WriteLine();
            Console.WriteLine($"You entered: {numberOfDataPoints}");
            DisplayContinuePrompt();

            return numberOfDataPoints;
        }



        #endregion

        #region ALARM SYSTEM

        /// <summary>
        /// Light Alarm Menu
        /// </summary>
        /// <param name="finchRobot"></param>

        static void LightAlarmDisplayMenuScreen(Finch finchRobot)
        {

            Console.CursorVisible = true;

            bool quitMenu = false;
            string menuChoice;

            string sensorsToMonitor = "";
            string rangeType = "";
            int minMaxThresholdValue = 0;
            int timeToMonitor = 0;

            do
            {
                DisplayScreenHeader("Alarm Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Set Sensors to Monitor");
                Console.WriteLine("\tb) Set Range Type");
                Console.WriteLine("\tc) Set Minimum/Maximum Threshold Value");
                Console.WriteLine("\td) Set Time to Monitor");
                Console.WriteLine("\te) Set Alarm");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        sensorsToMonitor = LightAlarmDisplaySetSensorsToMonitor();
                        break;

                    case "b":
                        rangeType = LightAlarmDisplaySetRangeType();
                        break;

                    case "c":
                        minMaxThresholdValue = LightAlarmSetMinMaxThresholdValue(rangeType, finchRobot);
                        break;

                    case "d":
                        timeToMonitor = LightAlarmSetTimeToMonitor();
                        break;

                    case "e":
                        LightAlarmSetAlarm(finchRobot, sensorsToMonitor, rangeType, minMaxThresholdValue, timeToMonitor);
                        break;
                    case "q":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);


        }

        /// <summary>
        /// Sets the alarm on the finch
        /// </summary>
        /// <param name="finchRobot"></param>
        /// <param name="sensorsToMonitor"></param>
        /// <param name="rangeType"></param>
        /// <param name="minMaxThresholdValue"></param>
        /// <param name="timeToMonitor"></param>
        private static void LightAlarmSetAlarm(Finch finchRobot, string sensorsToMonitor, string rangeType, int minMaxThresholdValue, int timeToMonitor)
        {

            bool thresholdExceeded = false;
            bool validInput = true;

            int currentLightSensorValue = 0;
            double currentTemperatureSensorValue = 0;
            int secondsElapsed = 0;

            string alarmType;
            string temperature = "temperature";
            string light = "light";
            string both = "both";

            DisplayScreenHeader("Set Alarm");

            Console.WriteLine($"\tSensors to monitor: {sensorsToMonitor}");
            Console.WriteLine($"\tRange Type: {rangeType}");
            Console.WriteLine($"\tMin/Max Threshold Value: {minMaxThresholdValue}");
            Console.WriteLine($"\tTime to Monitor: {timeToMonitor}");
            Console.WriteLine();

            Console.WriteLine("\tWould you like to set you alarm for temperature or light?");
            Console.Write("\t[Enter \"Temperature\", \"Light\", or \"Both\"]:");
            alarmType = Console.ReadLine().ToLower();

            do
            {
                if (alarmType != temperature && alarmType != light && alarmType != both)
                {
                    validInput = false;

                    Console.WriteLine("Your input was invalid. Please enter \"Temperature\", \"Light\", or \"Both\"");
                    Console.WriteLine();
                    DisplayContinuePrompt();
                }
                else if (alarmType == light)
                {
                    Console.WriteLine($"\tYou entered {alarmType}.");
                    Console.WriteLine();

                    Console.WriteLine("\tPress any key to begin monitoring.");
                    Console.ReadKey();
                    Console.WriteLine();

                    LightAlarmSetAlarmLoop(secondsElapsed, timeToMonitor, thresholdExceeded, sensorsToMonitor, currentLightSensorValue, rangeType, minMaxThresholdValue, finchRobot);
                }
                else if (alarmType == temperature)
                {
                    Console.WriteLine();
                    Console.WriteLine($"\tYou entered {alarmType}.");
                    Console.WriteLine();

                    Console.WriteLine("\tPress any key to begin monitoring.");
                    Console.ReadKey();
                    Console.WriteLine();

                    LightAlarmSetTemperatureAlarmLoop(secondsElapsed, timeToMonitor, thresholdExceeded, sensorsToMonitor, currentTemperatureSensorValue, rangeType, minMaxThresholdValue, finchRobot);
                }
                else if (alarmType == both)
                {
                    Console.WriteLine();
                    Console.WriteLine($"\tYou entered {alarmType}.");
                    Console.WriteLine();

                    Console.WriteLine("\tPress any key to begin monitoring.");
                    Console.ReadKey();
                    Console.WriteLine();

                    Parallel.Invoke(() => LightAlarmSetTemperatureAlarmLoop(secondsElapsed,
                                                                            timeToMonitor,
                                                                            thresholdExceeded,
                                                                            sensorsToMonitor,
                                                                            currentTemperatureSensorValue,
                                                                            rangeType, minMaxThresholdValue,
                                                                            finchRobot),
                                                () => LightAlarmSetAlarmLoop(secondsElapsed,
                                                                             timeToMonitor, thresholdExceeded,
                                                                             sensorsToMonitor,
                                                                             currentLightSensorValue,
                                                                             rangeType,
                                                                             minMaxThresholdValue,
                                                                             finchRobot));
                }
            } while (!validInput);

        }
        /// <summary>
        /// sets the temperature alarm
        /// </summary>
        /// <param name="secondsElapsed"></param>
        /// <param name="timeToMonitor"></param>
        /// <param name="thresholdExceeded"></param>
        /// <param name="sensorsToMonitor"></param>
        /// <param name="currentTemperatureSensorValue"></param>
        /// <param name="rangeType"></param>
        /// <param name="minMaxThresholdValue"></param>
        /// <param name="finchRobot"></param>
        private static void LightAlarmSetTemperatureAlarmLoop(int secondsElapsed, int timeToMonitor, bool thresholdExceeded, string sensorsToMonitor, double currentTemperatureSensorValue, string rangeType, int minMaxThresholdValue, Finch finchRobot)
        {
            while (secondsElapsed < timeToMonitor && !thresholdExceeded)
            {
                switch (sensorsToMonitor)
                {
                    case "left":
                        currentTemperatureSensorValue = finchRobot.getTemperature();
                        break;

                    case "right":
                        currentTemperatureSensorValue = finchRobot.getTemperature();
                        break;

                    case "both":
                        currentTemperatureSensorValue = (finchRobot.getTemperature() + finchRobot.getTemperature()) / 2;
                        break;
                }

                switch (rangeType)
                {
                    case "minimum":
                        if (currentTemperatureSensorValue < minMaxThresholdValue)
                        {
                            thresholdExceeded = true;
                        }
                        break;

                    case "maximum":
                        if (currentTemperatureSensorValue > minMaxThresholdValue)
                        {
                            thresholdExceeded = true;
                        }
                        break;
                }

                finchRobot.wait(1000);
                secondsElapsed++;
            }

            if (thresholdExceeded)
            {
                finchRobot.noteOn(1000);

                Console.WriteLine("\t***************************************************************************");
                Console.WriteLine($"\tThe {rangeType} threshold value of {minMaxThresholdValue} was exceeded");
                Console.WriteLine($"\tby the current temperture sensor value of {currentTemperatureSensorValue.ToString("n1")}");
                Console.WriteLine("\t***************************************************************************");
            }
            else
            {
                Console.WriteLine($"\tThe {rangeType} threshold value of {minMaxThresholdValue} was not exceeded by the current temperture sensor value of {currentTemperatureSensorValue.ToString("n1")}.");
            }

            DisplayMenuPrompt("Light Alarm");
            finchRobot.noteOff();
        }

        /// <summary>
        /// sets the light alarm
        /// </summary>
        /// <param name="secondsElapsed"></param>
        /// <param name="timeToMonitor"></param>
        /// <param name="thresholdExceeded"></param>
        /// <param name="sensorsToMonitor"></param>
        /// <param name="currentLightSensorValue"></param>
        /// <param name="rangeType"></param>
        /// <param name="minMaxThresholdValue"></param>
        /// <param name="finchRobot"></param>
        private static void LightAlarmSetAlarmLoop(int secondsElapsed, int timeToMonitor, bool thresholdExceeded, string sensorsToMonitor, int currentLightSensorValue, string rangeType, int minMaxThresholdValue, Finch finchRobot)
        {
            while (secondsElapsed < timeToMonitor && !thresholdExceeded)
            {
                switch (sensorsToMonitor)
                {
                    case "left":
                        currentLightSensorValue = finchRobot.getLeftLightSensor();
                        break;

                    case "right":
                        currentLightSensorValue = finchRobot.getRightLightSensor();
                        break;

                    case "both":
                        currentLightSensorValue = (finchRobot.getRightLightSensor() + finchRobot.getLeftLightSensor()) / 2;
                        break;
                }

                switch (rangeType)
                {
                    case "minimum":
                        if (currentLightSensorValue < minMaxThresholdValue)
                        {
                            thresholdExceeded = true;
                        }
                        break;

                    case "maximum":
                        if (currentLightSensorValue > minMaxThresholdValue)
                        {
                            thresholdExceeded = true;
                        }
                        break;
                }

                finchRobot.wait(1000);
                secondsElapsed++;
            }

            if (thresholdExceeded)
            {
                finchRobot.noteOn(1000);

                Console.WriteLine("\t***************************************************************************");
                Console.WriteLine($"\t\tThe {rangeType} threshold value of {minMaxThresholdValue} was exceeded \t");
                Console.WriteLine($"\t\tby the current light sensor value of {currentLightSensorValue.ToString("n1")}");
                Console.WriteLine("\t***************************************************************************");
            }
            else
            {
                Console.WriteLine($"\tThe {rangeType} threshold value of {minMaxThresholdValue} was not exceeded by the current light sensor value of {currentLightSensorValue.ToString("n1")}.");
            }

            DisplayMenuPrompt("Light Alarm");
            finchRobot.noteOff();
        }

        /// <summary>
        /// Sets the amount of time to monitor light
        /// </summary>
        /// <param name="rangeType"></param>
        /// <param name="finchRobot"></param>
        /// <returnstimeToMonitor></returns>
        static int LightAlarmSetTimeToMonitor()
        {
            int timeToMonitor;
            bool validInput = true;

            //
            // Validate Value
            //

            do
            {
                DisplayScreenHeader("Time to Monitor");
                Console.Write($"\tEnter time to Monitor [in seconds]:");


                if (!int.TryParse(Console.ReadLine(), out timeToMonitor))
                {
                    validInput = false;

                    Console.WriteLine();
                    Console.WriteLine("\tYour input was invalid. Pleas enter an interger.");
                    Console.WriteLine();
                    DisplayContinuePrompt();
                }

                //
                // echo value
                //

                else
                {
                    validInput = true;

                    Console.WriteLine();
                    Console.WriteLine($"\tYou entered: {timeToMonitor}");
                    Console.WriteLine();
                    DisplayMenuPrompt("Light Alarm");
                }


            } while (!validInput);

            return timeToMonitor;


        }
        /// <summary>
        /// Sets the minimum or maximum threshold value
        /// </summary>
        /// <param name="rangeType"></param>
        /// <param name="finchRobot"></param>
        /// <returns>minMaxThresholdValue</returns>
        static int LightAlarmSetMinMaxThresholdValue(string rangeType, Finch finchRobot)
        {
            int minMaxThresholdValue;

            bool validInput = true;

            DisplayScreenHeader("Minimum/Maximum Threshold Value");

            Console.WriteLine($"\tLeft light sensor ambient value: {finchRobot.getLeftLightSensor()}");
            Console.WriteLine($"\tRight Light sensor ambient value: {finchRobot.getRightLightSensor()}");
            Console.WriteLine();
            Console.WriteLine($"\tTemperature sensor ambient value: {finchRobot.getTemperature().ToString("n1")}");
            Console.WriteLine();
            //
            // Validate Value
            //
            do
            {
                Console.Write($"\tEnter the {rangeType} sensor value:");


                if (!int.TryParse(Console.ReadLine(), out minMaxThresholdValue))
                {
                    validInput = false;

                    Console.WriteLine();
                    Console.WriteLine("\tYour input was invalid. Pleas enter an interger.");
                    Console.WriteLine();
                    DisplayContinuePrompt();
                }

                //
                // echo value
                //

                else
                {
                    validInput = true;

                    Console.WriteLine();
                    Console.WriteLine($"\tYou entered: {minMaxThresholdValue}");
                    Console.WriteLine();
                    DisplayMenuPrompt("Light Alarm");
                }


            } while (!validInput);

            return minMaxThresholdValue;

        }

        /// <summary>
        /// Sets the range type
        /// </summary>
        /// <returns>rangeType</returns>
        static string LightAlarmDisplaySetRangeType()
        {
            string rangeType;
            string minimum = "minimum";
            string maximum = "maximum";
            bool validInput = true;
            DisplayScreenHeader("Range Type");

            do
            {
                Console.Write("\tRange Type [Minimum, Maximum]:");
                rangeType = Console.ReadLine().ToLower();

                if (rangeType != minimum && rangeType != maximum)
                {
                    validInput = false;

                    Console.WriteLine();
                    Console.WriteLine("\tYour input was invalid. Pleas enter \"Minimum\", or \"Maximum\".");
                    Console.WriteLine();
                    DisplayContinuePrompt();
                }

                //
                // echo value
                //

                else
                {
                    validInput = true;

                    Console.WriteLine();
                    Console.WriteLine($"\tYou entered: {rangeType}");
                    Console.WriteLine();
                    DisplayMenuPrompt("Light Alarm");
                }


            } while (!validInput);

            return rangeType;
        }
        /// <summary>
        /// Sets which sensors to monitor
        /// </summary>
        /// <returns>sensorsToMonitor</returns>
        static string LightAlarmDisplaySetSensorsToMonitor()
        {
            string sensorsToMonitor;
            string left = "left";
            string right = "right";
            string both = "both";
            bool validInput = true;

            DisplayScreenHeader("Sensors to Monitor");

            //
            // validate value
            //

            do
            {
                Console.Write("\tSensors to monitor [left, right, both]:");
                sensorsToMonitor = Console.ReadLine().ToLower();

                if (sensorsToMonitor != left && sensorsToMonitor != right && sensorsToMonitor != both)
                {
                    validInput = false;

                    Console.WriteLine();
                    Console.WriteLine("\tYour input was invalid. Pleas enter \"left\", \"right\", or \"both\".");
                    Console.WriteLine();
                    DisplayContinuePrompt();
                }

                //
                // echo value
                //

                else
                {
                    validInput = true;

                    Console.WriteLine();
                    Console.WriteLine($"\tYou entered: {sensorsToMonitor}");
                    Console.WriteLine();
                    DisplayMenuPrompt("Light Alarm");
                }


            } while (!validInput);

            return sensorsToMonitor;

        }
        #endregion

        #region USER PROGRAMMING

        /// <summary>
        /// **********************************************
        /// *         User Programming Menu              *
        /// **********************************************
        /// </summary>
        /// <param name="finchRobot"></param>
        static void UserProgrammingDisplayMenuScreen(Finch finchRobot)
        {
            string menuChoice;
            bool quitMenu = true;

            //
            // Tuple to store all three command parameters
            //

            (int motorSpeed, int ledbrightness, int noteFrequency, double waitSeconds) commandParameters;
            commandParameters.motorSpeed = 0;
            commandParameters.ledbrightness = 0;
            commandParameters.waitSeconds = 0;
            commandParameters.noteFrequency = 0;

            List<Command> commands = new List<Command>();

            do
            {
                DisplayScreenHeader("User Programming Menu");

                //
                // get user menu choice
                //

                Console.WriteLine("\ta) Set Command Parameters");
                Console.WriteLine("\tb) Add Commands");
                Console.WriteLine("\tc) View Commands");
                Console.WriteLine("\td) Execute Commands");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // Process user menu choice
                //

                switch (menuChoice)
                {
                    case "a":
                        commandParameters = UserProgrammingDisplayGetCommandParameters();
                        break;
                    case "b":
                        UserProgrammingDisplayGetFinchCommands(commands);
                        break;
                    case "c":
                        UserProgrammingDisplayFinchCommands(commands);
                        break;
                    case "d":
                        UserProgrammingDisplayExecuteCommands(finchRobot, commands, commandParameters);
                        break;
                    case "q":
                        quitMenu = false;
                        break;
                }




            } while (quitMenu == true);

        }
        /// <summary>
        /// ********************************************************
        /// *       Executes commands entered by the user          *
        /// ********************************************************
        /// </summary>
        /// <param name="finchRobot"></param>
        /// <param name="commands"></param>
        /// <param name="commandParameters"></param>
        private static void UserProgrammingDisplayExecuteCommands(Finch finchRobot, List<Command> commands, (int motorSpeed, int ledbrightness, int noteFrequency, double waitSeconds) commandParameters)
        {
            int motorSpeed = commandParameters.motorSpeed;
            int ledBrightness = commandParameters.ledbrightness;
            int waitMilliSeconds = (int)(commandParameters.waitSeconds * 1000);
            int noteFrequency = commandParameters.noteFrequency;
            string commandFeedback = "";
            const int TURNING_MOTOR_SPEED = 100;

            DisplayScreenHeader("Execute Finch Commands");

            Console.WriteLine("\tThe Finch robot is ready to execute the list of commands.");
            DisplayContinuePrompt();

            foreach (Command command in commands)
            {
                switch (command)
                {
                    case Command.NONE:
                        break;

                    case Command.MOVEFORWARD:
                        finchRobot.setMotors(motorSpeed, motorSpeed);
                        commandFeedback = Command.MOVEFORWARD.ToString();
                        break;

                    case Command.MOVEBACKWARD:
                        finchRobot.setMotors(-motorSpeed, -motorSpeed);
                        commandFeedback = Command.MOVEBACKWARD.ToString();
                        break;

                    case Command.STOPMOTORS:
                        finchRobot.setMotors(0, 0);
                        commandFeedback = Command.STOPMOTORS.ToString();
                        break;

                    case Command.WAIT:
                        finchRobot.wait(waitMilliSeconds);
                        commandFeedback = Command.WAIT.ToString();
                        break;

                    case Command.TURNRIGHT:
                        finchRobot.setMotors(TURNING_MOTOR_SPEED, -TURNING_MOTOR_SPEED);
                        commandFeedback = Command.TURNRIGHT.ToString();
                        break;

                    case Command.TURNLEFT:
                        finchRobot.setMotors(-TURNING_MOTOR_SPEED, TURNING_MOTOR_SPEED);
                        commandFeedback = Command.TURNLEFT.ToString();
                        break;

                    case Command.LEDON:
                        finchRobot.setLED(ledBrightness, ledBrightness, ledBrightness);
                        commandFeedback = Command.LEDON.ToString();
                        break;

                    case Command.LEDOFF:
                        finchRobot.setLED(0, 0, 0);
                        commandFeedback = Command.LEDOFF.ToString();
                        break;
                    case Command.NOTEON:
                        finchRobot.noteOn(noteFrequency);
                        commandFeedback = Command.NOTEON.ToString();
                        break;
                    case Command.NOTEOFF:
                        finchRobot.noteOff();
                        commandFeedback = Command.NOTEOFF.ToString();
                        break;
                    case Command.GETTEMPERATURE:
                        commandFeedback = $"Temperature: {finchRobot.getTemperature().ToString("n2")}\n";
                        break;
                    case Command.DONE:
                        commandFeedback = Command.DONE.ToString();
                        break;

                    default:
                        break;

                }
                Console.WriteLine($"\t{commandFeedback}");
            }
            DisplayMenuPrompt("User Programming");
        }

        /// <summary>
        /// ********************************************************
        /// *       Displays commands entered by the user          *
        /// ********************************************************
        /// </summary>
        /// <param name="commands"></param>
        private static void UserProgrammingDisplayFinchCommands(List<Command> commands)
        {
            DisplayScreenHeader("Finch Robot Commands");

            foreach (Command command in commands)
            {
                Console.WriteLine($"\t{command}");
            }

            DisplayMenuPrompt("User Programming");
        }

        /// <summary>
        /// ********************************************************
        /// *       Get Commands for the finch robot From User     *
        /// ********************************************************
        /// </summary>
        /// <param name="commands"></param>
        private static void UserProgrammingDisplayGetFinchCommands(List<Command> commands)
        {
            Command command = Command.NONE;

            DisplayScreenHeader("Finch Robot Commands");

            //
            // List Commands
            //

            int commandCount = 1;
            Console.WriteLine("\tList of Available Commands");
            Console.WriteLine();


            foreach (string commandName in Enum.GetNames(typeof(Command)))
            {
                Console.WriteLine($"\t- {commandName.ToLower()} -");
                commandCount++;
            }
            Console.WriteLine();

            while (command != Command.DONE)
            {
                Console.Write("\tEnter Command: ");

                if (Enum.TryParse(Console.ReadLine().ToUpper(), out command))
                {
                    commands.Add(command);
                }
                else
                {
                    Console.WriteLine("\t----------------------------------------------");
                    Console.WriteLine("\tPlease enter a command from the list above.");
                    Console.WriteLine("\t----------------------------------------------");
                    Console.WriteLine();
                }
            }
            //
            // Echo commands
            //

            UserProgrammingDisplayFinchCommands(commands);
        }

        /// <summary>
        /// **********************************************
        /// *       Get Command Parameters From User     *
        /// **********************************************
        /// </summary>
        /// <returns></returns>
        private static (int motorSpeed, int ledbrightness, int noteFrequency, double waitSeconds) UserProgrammingDisplayGetCommandParameters()
        {
            DisplayScreenHeader("Command Parameters");

            (int motorSpeed, int ledBrightness, int noteFrequency, double waitSeconds) commandParameters;
            commandParameters.motorSpeed = 0;
            commandParameters.ledBrightness = 0;
            commandParameters.waitSeconds = 0;
            commandParameters.noteFrequency = 0;

            GetValidInterger("\tEnter Motor Speed [1 - 255]:", 1, 255, out commandParameters.motorSpeed);
            GetValidInterger("\tEnter LED Brightness [1 - 255]:", 1, 255, out commandParameters.ledBrightness);
            GetValidInterger("\tEnter Note Frequency [1 - 20000]", 1, 20000, out commandParameters.noteFrequency);
            GetValidDouble("\tEnter Wait in Seconds [0 - 10]:", 0, 10, out commandParameters.waitSeconds);

            Console.WriteLine();
            Console.WriteLine($"\tMotor speed: {commandParameters.motorSpeed}");
            Console.WriteLine($"\tLED brightness: {commandParameters.ledBrightness}");
            Console.WriteLine($"\tNote Frequency: {commandParameters.noteFrequency}");
            Console.WriteLine($"\tWait command duration: {commandParameters.waitSeconds}");

            DisplayMenuPrompt("User Programming");




            return commandParameters;
        }

        #endregion

        #region FINCH ROBOT MANAGEMENT

        /// <summary>
        /// *****************************************************************
        /// *               Disconnect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayDisconnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;



            DisplayScreenHeader("Disconnect Finch Robot");

            Console.WriteLine("\tAbout to disconnect from the Finch robot.");
            DisplayContinuePrompt();

            finchRobot.disConnect();


            Console.WriteLine("\tThe Finch robot is now disconnected.");

            DisplayContinuePrompt();
        }

        /// <summary>
        /// *****************************************************************
        /// *                  Connect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static bool DisplayConnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            bool robotConnected;

            DisplayScreenHeader("Connect Finch Robot");

            Console.WriteLine("\tAbout to connect to Finch robot. Please be sure the USB cable is connected to the robot and computer now.");
            DisplayContinuePrompt();

            robotConnected = finchRobot.connect();

            // Test connection and provide user feedback - text, lights, sounds

            if (robotConnected == true)
            {
                Console.WriteLine("\tFinch robot successfully connected");
                finchRobot.setLED(0, 255, 0);
                finchRobot.noteOn(500);
                finchRobot.wait(500);
                finchRobot.noteOn(1000);
                finchRobot.wait(500);
                finchRobot.noteOff();
                finchRobot.setLED(0, 0, 0);
            }
            else
            {
                Console.WriteLine("\tFailed to connect. Please try again");
                finchRobot.setLED(255, 0, 0);
                finchRobot.noteOn(100);
                finchRobot.wait(1000);
                finchRobot.noteOff();
            }

            DisplayMenuPrompt("Main Menu");

            //
            // reset finch robot
            //
            finchRobot.setLED(0, 0, 0);
            finchRobot.noteOff();

            return robotConnected;
        }

        #endregion

        #region USER INTERFACE

        /// <summary>
        /// *****************************************************************
        /// *                     Welcome Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tFinch Control");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Closing Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using Finch Control!");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\tPress any key to continue.");
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// display menu prompt
        /// </summary>
        static void DisplayMenuPrompt(string menuName)
        {
            Console.WriteLine();
            Console.WriteLine($"\tPress any key to return to the {menuName} Menu.");
            Console.ReadKey();
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        #endregion

        #region HELPER METHODS
        /// <summary>
        /// Gets a valid interger from the user
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="parameter"></param>
        private static int GetValidInterger(string prompt, int minValue, int maxValue, out int parameter)
        {

            bool validInput = false;

            do
            {
                Console.Write(prompt);


                if (!int.TryParse(Console.ReadLine(), out parameter))
                {
                    validInput = false;

                    Console.WriteLine();
                    Console.WriteLine("\tYou did not enter a valid number. Please enter a number within the given range.");
                    Console.WriteLine();

                    DisplayContinuePrompt();
                }
                else if (parameter >= minValue && parameter <= maxValue)
                {
                    validInput = true;

                    Console.WriteLine();
                    Console.WriteLine($"\tYou entered: {parameter}");
                    Console.WriteLine();

                    DisplayContinuePrompt();
                }
                else if (parameter < minValue || parameter > maxValue)
                {
                    validInput = false;

                    Console.WriteLine();
                    Console.WriteLine("\tYou did not enter a valid number. Please enter a number within the given range.");
                    Console.WriteLine();

                    DisplayContinuePrompt();
                }



            } while (!validInput);

            return parameter;
        }

        /// <summary>
        /// Gets a valid double from the user
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="parameter"></param>
        private static double GetValidDouble(string prompt, int minValue, int maxValue, out double parameter)
        {

            bool validMotorSpeed = false;

            do
            {
                Console.Write(prompt);


                if (!double.TryParse(Console.ReadLine(), out parameter))
                {
                    validMotorSpeed = false;

                    Console.WriteLine();
                    Console.WriteLine("\tYou did not enter a valid number. Please enter a number between 0 and 10");
                    Console.WriteLine();

                    DisplayContinuePrompt();
                }
                else if (parameter >= minValue && parameter <= maxValue)
                {
                    validMotorSpeed = true;

                    Console.WriteLine();
                    Console.WriteLine($"\tYou entered: {parameter}");
                    Console.WriteLine();

                    DisplayContinuePrompt();
                }
                else if (parameter < minValue || parameter > maxValue)
                {
                    validMotorSpeed = false;

                    Console.WriteLine();
                    Console.WriteLine("\tYou did not enter a valid number. Please enter a number between 0 and 10");
                    Console.WriteLine();

                    DisplayContinuePrompt();
                }



            } while (!validMotorSpeed);

            return parameter;
        }


        #endregion


    }
}
