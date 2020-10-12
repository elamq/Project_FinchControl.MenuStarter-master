using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FinchAPI;

namespace Project_FinchControl
{

    // **************************************************
    //
    // Title: Finch Control - S2 (Data Recorder)
    // Author: Quentin Elam
    // Description: An application that allows the finch to
    //              record and display data
    // Application Type: Console
    // Dated Created: 10/07/2020
    // Last Modified: 10/12/2020
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
            SetTheme();

            DisplayWelcomeScreen();
            DisplayMenuScreen();
            DisplayClosingScreen();
        }
        /// <summary>
        /// setup the console theme
        /// </summary>
        static void SetTheme()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.White;
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

                        break;

                    case "e":

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

            DisplayMenuPrompt("Main Menu");
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
    }
}
