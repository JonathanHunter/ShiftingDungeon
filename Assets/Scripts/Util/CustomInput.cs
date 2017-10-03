namespace ShiftingDungeon.Util
{
    using System.Xml;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary> Wraps Keyboard and Gamepad inputs into one set of booleans and raw float data. </summary>
    public class CustomInput : MonoBehaviour
    {

        [SerializeField]
        private bool UseConfigFile;

        /// <summary> This is used to define user inputs, changed to add or remove buttons. </summary>
        public enum UserInput
        {
            Up, Down, Left, Right, Attack, NextWeapon, PrevWeapon, Pause, Accept, Cancel, Target
        }

        /// <summary> The file to save the bindings to. </summary>
        private const string filename = "config.xml";

        /// <summary> This is used to define whether to return a positive or negative value for a specific raw input. </summary>
        public static void RawSign()
        {
            if (rawSign == null)
                throw new System.AccessViolationException(UnitializedMessage);

            rawSign[(int)UserInput.Up] = 1;
            rawSign[(int)UserInput.Down] = -1;
            rawSign[(int)UserInput.Left] = -1;
            rawSign[(int)UserInput.Right] = 1;
            rawSign[(int)UserInput.Attack] = 1;
            rawSign[(int)UserInput.NextWeapon] = 1;
            rawSign[(int)UserInput.PrevWeapon] = 1;
            rawSign[(int)UserInput.Pause] = 1;
            rawSign[(int)UserInput.Accept] = 1;
            rawSign[(int)UserInput.Cancel] = 1;
            rawSign[(int)UserInput.Target] = 1;
        }

        /// <summary> 
        /// This is used to define the default keybindings. 
        /// Syntax: keyBoard[INPUT, PLAYER_NUM] = KEYCODE;
        /// PLAYER_NUM is any number from 0 - 6, where 0 represents all controllers and 1-6 represents their respective player.
        /// </summary>
        public static void DefaultKey()
        {
            if (keyboard == null)
                throw new System.AccessViolationException(UnitializedMessage);

            keyboard[(int)UserInput.Up, 0] = KeyCode.W;
            keyboard[(int)UserInput.Down, 0] = KeyCode.S;
            keyboard[(int)UserInput.Left, 0] = KeyCode.A;
            keyboard[(int)UserInput.Right, 0] = KeyCode.D;
            keyboard[(int)UserInput.Attack, 0] = KeyCode.J;
            keyboard[(int)UserInput.NextWeapon, 0] = KeyCode.E;
            keyboard[(int)UserInput.PrevWeapon, 0] = KeyCode.Q;
            keyboard[(int)UserInput.Pause, 0] = KeyCode.Space;
            keyboard[(int)UserInput.Accept, 0] = KeyCode.Return;
            keyboard[(int)UserInput.Cancel, 0] = KeyCode.Escape;
            keyboard[(int)UserInput.Target, 0] = KeyCode.K;
        }

        /// <summary> 
        /// This is used to define the default controller bindings.
        /// gamePad[INPUT, PLAYER_NUM] = ONE OF THE STRING CONSTANTS BELOW;
        /// PLAYER_NUM is any number from 0 - 6, where 0 represents all controllers and 1-6 represents their respective player. 
        /// </summary>
        public static void DefaultPad()
        {
            if (gamepad == null)
                throw new System.AccessViolationException(UnitializedMessage);

            gamepad[(int)UserInput.Up, 0] = LEFT_STICK_UP;
            gamepad[(int)UserInput.Down, 0] = LEFT_STICK_DOWN;
            gamepad[(int)UserInput.Left, 0] = LEFT_STICK_LEFT;
            gamepad[(int)UserInput.Right, 0] = LEFT_STICK_RIGHT;
            gamepad[(int)UserInput.Attack, 0] = A;
            gamepad[(int)UserInput.NextWeapon, 0] = RB;
            gamepad[(int)UserInput.PrevWeapon, 0] = LB;
            gamepad[(int)UserInput.Pause, 0] = START;
            gamepad[(int)UserInput.Accept, 0] = A;
            gamepad[(int)UserInput.Cancel, 0] = B;
            gamepad[(int)UserInput.Cancel, 0] = RIGHT_TRIGGER;
        }

        public static float MouseX
        {
            get { return Input.mousePosition.x; }
        }

        public static float MouseY
        {
            get { return Input.mousePosition.y; }
        }

        // NOTE: Modification of the code below this should be unecessary.

        #region String constants
        /// <summary> Constant used to define a possible controller button. </summary>
        public const string LEFT_STICK_RIGHT = "Left Stick Right";
        /// <summary> Constant used to define a possible controller button. </summary>
        public const string LEFT_STICK_LEFT = "Left Stick Left";
        /// <summary> Constant used to define a possible controller button. </summary>
        public const string LEFT_STICK_UP = "Left Stick Up";
        /// <summary> Constant used to define a possible controller button. </summary>
        public const string LEFT_STICK_DOWN = "Left Stick Down";
        /// <summary> Constant used to define a possible controller button. </summary>
        public const string RIGHT_STICK_RIGHT = "Right Stick Right";
        /// <summary> Constant used to define a possible controller button. </summary>
        public const string RIGHT_STICK_LEFT = "Right Stick Left";
        /// <summary> Constant used to define a possible controller button. </summary>
        public const string RIGHT_STICK_UP = "Right Stick Up";
        /// <summary> Constant used to define a possible controller button. </summary>
        public const string RIGHT_STICK_DOWN = "Right Stick Down";
        /// <summary> Constant used to define a possible controller button. </summary>
        public const string DPAD_RIGHT = "Dpad Right";
        /// <summary> Constant used to define a possible controller button. </summary>
        public const string DPAD_LEFT = "Dpad Left";
        /// <summary> Constant used to define a possible controller button. </summary>
        public const string DPAD_UP = "Dpad Up";
        /// <summary> Constant used to define a possible controller button. </summary>
        public const string DPAD_DOWN = "Dpad Down";
        /// <summary> Constant used to define a possible controller button. </summary>
        public const string LEFT_TRIGGER = "Left Trigger";
        /// <summary> Constant used to define a possible controller button. </summary>
        public const string RIGHT_TRIGGER = "Right Trigger";
        /// <summary> Constant used to define a possible controller button. </summary>
        public const string A = "A";
        /// <summary> Constant used to define a possible controller button. </summary>
        public const string B = "B";
        /// <summary> Constant used to define a possible controller button. </summary>
        public const string X = "X";
        /// <summary> Constant used to define a possible controller button. </summary>
        public const string Y = "Y";
        /// <summary> Constant used to define a possible controller button. </summary>
        public const string LB = "LB";
        /// <summary> Constant used to define a possible controller button. </summary>
        public const string RB = "RB";
        /// <summary> Constant used to define a possible controller button. </summary>
        public const string BACK = "Back";
        /// <summary> Constant used to define a possible controller button. </summary>
        public const string START = "Start";
        /// <summary> Constant used to define a possible controller button. </summary>
        public const string LEFT_STICK_CLICK = "Left Stick Click";
        /// <summary> Constant used to define a possible controller button. </summary>
        public const string RIGHT_STICK_CLICK = "Right Stick Click";

        //  Note all of the following strings should not be edited and should match the ones in Unity's input manager for the project

        /// <summary> Constant used to access controller data via Unity's innput manager. </summary>
        private const string LEFT_OSX_TRIGGER = "LeftOSXTrigger";
        /// <summary> Constant used to access controller data via Unity's innput manager. </summary>
        private const string LEFT_LINUX_TRIGGER = "LeftLinuxTrigger";
        /// <summary> Constant used to access controller data via Unity's innput manager. </summary>
        private const string LEFT_WIN_TRIGGER = "LeftWinTrigger";
        /// <summary> Constant used to access controller data via Unity's innput manager. </summary>
        private const string RIGHT_OSX_TRIGGER = "RightOSXTrigger";
        /// <summary> Constant used to access controller data via Unity's innput manager. </summary>
        private const string RIGHT_LINUX_TRIGGER = "RightLinuxTrigger";
        /// <summary> Constant used to access controller data via Unity's innput manager. </summary>
        private const string RIGHT_WIN_TRIGGER = "RightWinTrigger";
        /// <summary> Constant used to access controller data via Unity's innput manager. </summary>
        private const string RIGHT_OSX_STICK_Y = "RightOSXStickY";
        /// <summary> Constant used to access controller data via Unity's innput manager. </summary>
        private const string RIGHT_WIN_STICK_Y = "RightWinStickY";
        /// <summary> Constant used to access controller data via Unity's innput manager. </summary>
        private const string RIGHT_OSX_STICK_X = "RightOSXStickX";
        /// <summary> Constant used to access controller data via Unity's innput manager. </summary>
        private const string RIGHT_WIN_STICK_X = "RightWinStickX";
        /// <summary> Constant used to access controller data via Unity's innput manager. </summary>
        private const string RIGHT_PS_STICK_X = "RightPSStickX";
        /// <summary> Constant used to access controller data via Unity's innput manager. </summary>
        private const string RIGHT_PS_STICK_Y = "RightPSStickY";
        /// <summary> Constant used to access controller data via Unity's innput manager. </summary>
        private const string LEFT_STICK_Y = "LeftStickY";
        /// <summary> Constant used to access controller data via Unity's innput manager. </summary>
        private const string LEFT_STICK_X = "LeftStickX";
        /// <summary> Constant used to access controller data via Unity's innput manager. </summary>
        private const string DPAD_WIN_STICK_Y = "DpadWinStickY";
        /// <summary> Constant used to access controller data via Unity's innput manager. </summary>
        private const string DPAD_WIN_STICK_X = "DpadWinStickX";
        /// <summary> Constant used to access controller data via Unity's innput manager. </summary>
        private const string DPAD_LINUX_STICK_X = "DpadLinuxStickX";
        /// <summary> Constant used to access controller data via Unity's innput manager. </summary>
        private const string DPAD_LINUX_STICK_Y = "DpadLinuxStickY";
        /// <summary> Constant used to access controller data via Unity's innput manager. </summary>
        private const string DPAD_PS_STICK_X = "DpadPSStickX";
        /// <summary> Constant used to access controller data via Unity's innput manager. </summary>
        private const string DPAD_PS_STICK_Y = "DpadPSStickY";
        /// <summary> Constant used to access controller data via Unity's innput manager. </summary>
        private const string RIGHT_PS4_STICK_X = "RightPS4StickX";
        /// <summary> Constant used to access controller data via Unity's innput manager. </summary>
        private const string RIGHT_PS4_STICK_Y = "RightPS4StickY";
        /// <summary> Constant used to access controller data via Unity's innput manager. </summary>
        private const string DPAD_PS4_STICK_X = "DpadPS4StickX";
        /// <summary> Constant used to access controller data via Unity's innput manager. </summary>
        private const string DPAD_PS4_STICK_Y = "DpadPS4StickY";

        /// <summary> Error message showed when the Input GameObject isn't in the scene. </summary>
        private const string UnitializedMessage = "Input has not been initialized. Make sure it is in the scene.";
        #endregion 

        /// <summary> Simple enum for defining the possible controller types. </summary>
        public enum ControlType { Xbox, PS3, PS4 };

        /// <summary> A tuple for defining a controllers type and number. </summary>
        public struct Controller { public ControlType type; public int joysticNum; }

        /// <summary> Array used to store input booleans. </summary>
        private static bool[,] bools;
        /// <summary> Array used to store input booleans. </summary>
        private static bool[,] boolsUp;
        /// <summary> Array used to store input booleans. </summary>
        private static bool[,] boolsHeld;
        /// <summary> Array used to store input booleans. </summary>
        private static bool[,] boolsFreshPress;
        /// <summary> Array used to store input booleans. </summary>
        private static bool[,] boolsFreshPressAccessed;
        /// <summary> Array used to store input booleans. </summary>
        private static bool[,] boolsFreshPressDeleteOnRead;

        /// <summary> Array used to store raw input data for analog input. </summary>
        private static float[,] raws;
        /// <summary> Array used to store raw input data for analog input. </summary>
        private static float[,] rawsUp;
        /// <summary> Array used to store raw input data for analog input. </summary>
        private static float[,] rawsHeld;
        /// <summary> Array used to store raw input data for analog input. </summary>
        private static float[,] rawsFreshPress;
        /// <summary> Array used to store raw input data for analog input. </summary>
        private static bool[,] rawsFreshPressAccessed;
        /// <summary> Array used to store raw input data for analog input. </summary>
        private static float[,] rawsFreshPressDeleteOnRead;

        #region Data Access Properties
        /// <summary> Getter for if a button is pressed. </summary>
        /// <param name="input"> The button to check. </param>
        /// <returns> True as long as the button is held. </returns>
        public static bool Bool(UserInput input, int playerNumber = 0)
        {
            if (bools == null)
                throw new System.AccessViolationException(UnitializedMessage);
            return bools[(int)input, playerNumber];
        }

        /// <summary> Getter for if a button has been released. </summary>
        /// <param name="input"> The button to check. </param>
        /// <returns> True for one frame after button is let go. returns>
        public static bool BoolUp(UserInput input, int playerNumber = 0)
        {
            if (boolsUp == null)
                throw new System.AccessViolationException(UnitializedMessage);
            return boolsUp[(int)input, playerNumber];
        }

        /// <summary> Getter for if a button is held. </summary>
        /// <param name="input"> The button to check. </param>
        /// <returns> True until the button is let go.  </returns>
        public static bool BoolHeld(UserInput input, int playerNumber = 0)
        {
            if (boolsHeld == null)
                throw new System.AccessViolationException(UnitializedMessage);
            return boolsHeld[(int)input, playerNumber];
        }

        /// <summary> Getter for if a button has been pressed. </summary>
        /// <param name="input"> The button to check. </param>
        /// <returns> True as until the end of the frame after the data is read or the key is released. </returns>
        public static bool BoolFreshPress(UserInput input, int playerNumber = 0)
        {
            if (boolsFreshPress == null)
                throw new System.AccessViolationException(UnitializedMessage);
            boolsFreshPressAccessed[(int)input, playerNumber] = true;
            return boolsFreshPress[(int)input, playerNumber];
        }

        /// <summary> Getter for if a button has been pressed. </summary>
        /// <param name="input"> The button to check. </param>
        /// <returns> True as until the data is read or the key is released. </returns>
        public static bool BoolFreshPressDeleteOnRead(UserInput input, int playerNumber = 0)
        {
            if (boolsFreshPressDeleteOnRead == null)
                throw new System.AccessViolationException(UnitializedMessage);
            bool temp = boolsFreshPressDeleteOnRead[(int)input, playerNumber];
            boolsFreshPressDeleteOnRead[(int)input, playerNumber] = false;
            return temp;
        }

        /// <summary> Getter for if a button is pressed. </summary>
        /// <param name="input"> The button to check. </param>
        /// <returns> A non-zero value as long as the button is held. </returns>
        public static float Raw(UserInput input, int playerNumber = 0)
        {
            if (raws == null)
                throw new System.AccessViolationException(UnitializedMessage);
            return raws[(int)input, playerNumber];
        }

        /// <summary> Getter for if a button has been released. </summary>
        /// <param name="input"> The button to check. </param>
        /// <returns> A non-zero value for one frame after button is let go. returns>
        public static float RawUp(UserInput input, int playerNumber = 0)
        {
            if (rawsUp == null)
                throw new System.AccessViolationException(UnitializedMessage);
            return rawsUp[(int)input, playerNumber];
        }

        /// <summary> Getter for if a button is held. </summary>
        /// <param name="input"> The button to check. </param>
        /// <returns> A non-zero value until the button is let go.  </returns>
        public static float RawHeld(UserInput input, int playerNumber = 0)
        {
            if (rawsHeld == null)
                throw new System.AccessViolationException(UnitializedMessage);
            return rawsHeld[(int)input, playerNumber];
        }

        /// <summary> Getter for if a button has been pressed. </summary>
        /// <param name="input"> The button to check. </param>
        /// <returns> A non-zero value as until the end of the frame after the data is read or the key is released. </returns>
        public static float RawFreshPress(UserInput input, int playerNumber = 0)
        {
            if (rawsFreshPress == null)
                throw new System.AccessViolationException(UnitializedMessage);
            rawsFreshPressAccessed[(int)input, playerNumber] = true;
            return rawsFreshPress[(int)input, playerNumber];
        }

        /// <summary> Getter for if a button has been pressed. </summary>
        /// <param name="input"> The button to check. </param>
        /// <returns> A non-zero value as until the data is read or the key is released. </returns>
        public static float RawFreshPressDeleteOnRead(UserInput input, int playerNumber = 0)
        {
            if (rawsFreshPressDeleteOnRead == null)
                throw new System.AccessViolationException(UnitializedMessage);
            float temp = rawsFreshPressDeleteOnRead[(int)input, playerNumber];
            rawsFreshPressDeleteOnRead[(int)input, playerNumber] = 0;
            return temp;
        }
        #endregion

        /// <summary> Array to hold which keys correspond to which inputs. </summary>
        private static KeyCode[,] keyboard;

        /// <summary> Getter for the keys attached to inputs. </summary>
        /// <param name="input"> The key to get. </param>
        /// <returns> The keycode corresponding to that input. </returns>
        public static KeyCode keyboardKey(UserInput input, int playerNumber = 0)
        {
            if (keyboard == null)
                throw new System.AccessViolationException(UnitializedMessage);
            return keyboard[(int)input, playerNumber];
        }

        /// <summary> Setter used to define which keys correspond to which inputs. </summary>
        /// <param name="input"> The button to define. </param>
        /// <param name="key"> The key to attach to it. </param>
        public static void setKeyboardKey(UserInput input, KeyCode key, int playerNumber = 0)
        {
            if (keyboard == null)
                throw new System.AccessViolationException(UnitializedMessage);
            keyboard[(int)input, playerNumber] = key;
        }

        /// <summary> Array to hold which buttons correspond to which inputs. </summary>
        private static string[,] gamepad;

        /// <summary> Getter for the buttons attached to inputs. </summary>
        /// <param name="input"> The button to get. </param>
        /// <returns> The string corresponding to that input. </returns>
        public static string gamepadButton(UserInput input, int playerNumber = 0)
        {
            if (gamepad == null)
                throw new System.AccessViolationException(UnitializedMessage);
            return gamepad[(int)input, playerNumber];
        }

        /// <summary> Setter used to define which buttons correspond to which inputs. </summary>
        /// <param name="input"> The button to define. </param>
        /// <param name="button"> The button to attach to it. </param>
        public static void setGamepadButton(UserInput input, string button, int playerNumber = 0)
        {
            if (gamepad == null)
                throw new System.AccessViolationException(UnitializedMessage);
            if (invalidString(button))
                throw new System.InvalidOperationException("Given string is not a valid input.  Use one of the CustomInput string constants.");
            gamepad[(int)input, playerNumber] = button;
        }

        /// <summary> Array to for the user to specify the sign of the number they want from raw data </summary>
        private static int[] rawSign;

        /// <summary> Boolean as to whether or not a controller is being used. </summary>
        private static bool usingPad = false;

        /// <summary> Is the player using a controller. </summary>
        public static bool UsingPad
        {
            get { return usingPad; }
        }

        /// <summary> Array for holding which controller name to reference </summary>
        private static Controller[] gamepadMapping;

        /// <summary> Used to make this class a singleton. </summary>
        private static CustomInput instance;

        void Awake()
        {
            if (instance == null)
            {
                DontDestroyOnLoad(this.gameObject);
                instance = this;
            }
            else if (this != instance)
            {
                Destroy(this.gameObject);
                return;
            }

            // Detect level loads to clear input buffers.
            SceneManager.sceneLoaded += delegate (Scene s, LoadSceneMode m)
            {
                for (int r = 0; r < System.Enum.GetNames(typeof(UserInput)).Length; r++)
                {
                    for (int c = 0; c < 7; c++)
                    {
                        bools[r, c] = false;
                        boolsUp[r, c] = false;
                        boolsHeld[r, c] = false;
                        boolsFreshPress[r, c] = false;
                        boolsFreshPressAccessed[r, c] = false;
                        boolsFreshPressDeleteOnRead[r, c] = false;

                        raws[r, c] = 0;
                        rawsUp[r, c] = 0;
                        rawsHeld[r, c] = 0;
                        rawsFreshPress[r, c] = 0;
                        rawsFreshPressAccessed[r, c] = false;
                        rawsFreshPressDeleteOnRead[r, c] = 0;
                    }
                }
            };

            bools = new bool[System.Enum.GetNames(typeof(UserInput)).Length, 7];
            boolsUp = new bool[System.Enum.GetNames(typeof(UserInput)).Length, 7];
            boolsHeld = new bool[System.Enum.GetNames(typeof(UserInput)).Length, 7];
            boolsFreshPress = new bool[System.Enum.GetNames(typeof(UserInput)).Length, 7];
            boolsFreshPressAccessed = new bool[System.Enum.GetNames(typeof(UserInput)).Length, 7];
            boolsFreshPressDeleteOnRead = new bool[System.Enum.GetNames(typeof(UserInput)).Length, 7];

            raws = new float[System.Enum.GetNames(typeof(UserInput)).Length, 7];
            rawsUp = new float[System.Enum.GetNames(typeof(UserInput)).Length, 7];
            rawsHeld = new float[System.Enum.GetNames(typeof(UserInput)).Length, 7];
            rawsFreshPress = new float[System.Enum.GetNames(typeof(UserInput)).Length, 7];
            rawsFreshPressAccessed = new bool[System.Enum.GetNames(typeof(UserInput)).Length, 7];
            rawsFreshPressDeleteOnRead = new float[System.Enum.GetNames(typeof(UserInput)).Length, 7];

            keyboard = new KeyCode[System.Enum.GetNames(typeof(UserInput)).Length, 7];
            gamepad = new string[System.Enum.GetNames(typeof(UserInput)).Length, 7];
            rawSign = new int[System.Enum.GetNames(typeof(UserInput)).Length];

            gamepadMapping = new Controller[7];
            gamepadMapping[0].type = ControlType.Xbox;
            gamepadMapping[0].joysticNum = 0;

            RawSign();

            if (UseConfigFile && FileExists())
                Load();
            else
            {
                Default();
                Store();
            }
        }

        /// <summary> Resets all the bindings to default. </summary>
        public static void Default()
        {
            DefaultKey();
            DefaultPad();
        }

        /// <summary> Determines if the Input bindings file exists. </summary>
        /// <returns> True if the file exists. </returns>
        public static bool FileExists()
        {
            return System.IO.File.Exists(filename);
        }

        /// <summary> Loads the input bindings file from disk.  Assumes the file exists.  Any errors encounterd simply cause it to remake the file. </summary>
        public static void Load()
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    for (int p = 0; p < 7; p++)
                    {
                        reader.ReadToFollowing("Player" + p);
                        for (int i = 0; i < System.Enum.GetNames(typeof(UserInput)).Length; i++)
                        {
                            reader.ReadToFollowing("Keyboard_" + System.Enum.GetNames(typeof(UserInput))[i]);
                            keyboard[i, p] = (KeyCode)System.Enum.Parse(typeof(KeyCode), reader.ReadElementContentAsString());
                        }
                        for (int i = 0; i < System.Enum.GetNames(typeof(UserInput)).Length; i++)
                        {
                            reader.ReadToFollowing("Gamepad_" + System.Enum.GetNames(typeof(UserInput))[i]);
                            gamepad[i, p] = reader.ReadElementContentAsString();
                        }
                    }
                    reader.Close();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message + " occured and was caught");
                Default();
                Store();
            }
        }

        /// <summary> Writes the current input bindings to a file on disk. </summary>
        public static void Store()
        {
            XmlDocument bindings = new XmlDocument();
            XmlNode node;
            XmlElement element, child;
            XmlElement root = bindings.CreateElement("Controls");
            bindings.InsertAfter(root, bindings.DocumentElement);
            for (int p = 0; p < 7; p++)
            {
                element = bindings.CreateElement("Player" + p);
                for (int i = 0; i < System.Enum.GetNames(typeof(UserInput)).Length; i++)
                {
                    child = bindings.CreateElement("Keyboard_" + System.Enum.GetNames(typeof(UserInput))[i]);
                    node = bindings.CreateTextNode("Keyboard_" + System.Enum.GetNames(typeof(UserInput))[i]);
                    node.Value = keyboard[i, p].ToString();
                    child.AppendChild(node);
                    element.AppendChild(child);
                }
                for (int i = 0; i < System.Enum.GetNames(typeof(UserInput)).Length; i++)
                {
                    child = bindings.CreateElement("Gamepad_" + System.Enum.GetNames(typeof(UserInput))[i]);
                    node = bindings.CreateTextNode("Gamepad_" + System.Enum.GetNames(typeof(UserInput))[i]);
                    node.Value = gamepad[i, p];
                    element.AppendChild(node);
                    child.AppendChild(node);
                    element.AppendChild(child);
                }
                root.AppendChild(element);
            }
            bindings.Save(filename);
        }

        /// <summary> Simple method for determining if the given string is one of the valid string constants. </summary>
        /// <param name="button"> The string in question. </param>
        /// <returns> False if the string matches one of the constants. </returns>
        private static bool invalidString(string button)
        {
            if (button == LEFT_STICK_RIGHT)
                return false;
            if (button == LEFT_STICK_LEFT)
                return false;
            if (button == LEFT_STICK_UP)
                return false;
            if (button == LEFT_STICK_DOWN)
                return false;
            if (button == RIGHT_STICK_RIGHT)
                return false;
            if (button == RIGHT_STICK_LEFT)
                return false;
            if (button == RIGHT_STICK_UP)
                return false;
            if (button == RIGHT_STICK_DOWN)
                return false;
            if (button == DPAD_RIGHT)
                return false;
            if (button == DPAD_LEFT)
                return false;
            if (button == DPAD_UP)
                return false;
            if (button == DPAD_DOWN)
                return false;
            if (button == LEFT_TRIGGER)
                return false;
            if (button == RIGHT_TRIGGER)
                return false;
            if (button == A)
                return false;
            if (button == B)
                return false;
            if (button == X)
                return false;
            if (button == Y)
                return false;
            if (button == LB)
                return false;
            if (button == RB)
                return false;
            if (button == BACK)
                return false;
            if (button == START)
                return false;
            if (button == LEFT_STICK_CLICK)
                return false;
            if (button == RIGHT_STICK_CLICK)
                return false;
            return true;
        }

        void Update()
        {
            if (AnyPadInput())
                usingPad = true;
            else if (Input.anyKey)
                usingPad = false;
            if (usingPad)
            {
                UpdateGamePadArray();
                for (int i = 0; i < System.Enum.GetNames(typeof(UserInput)).Length; i++)
                {
                    for (int p = 0; p < 7; p++)
                    {
                        if (gamepad[i, p] != null)
                            updatePad(i, p);
                    }
                }
            }
            else
            {
                for (int i = 0; i < System.Enum.GetNames(typeof(UserInput)).Length; i++)
                {
                    for (int p = 0; p < 7; p++)
                    {
                        if (keyboard[i, p] != KeyCode.None)
                        {
                            updateKey(i, p);
                        }
                    }
                }
            }
        }

        /// <summary> Update the GamePad Array to reference the indices and type of active controllers. </summary>
        private void UpdateGamePadArray()
        {
            int controller = 1;
            int ps3 = 0, ps4 = 0, xbox = 0;
            string[] arr = Input.GetJoystickNames();
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] != null && arr[i] != "")
                {
                    if (arr[i].ToLower().Contains("playstation") || arr[i].ToLower().Contains("ps3"))
                    {
                        gamepadMapping[controller].type = ControlType.PS3;
                        ps3++;
                    }
                    else if (arr[i].ToLower().Contains("ps4") || arr[i].ToLower().Contains("wireless controller"))
                    {
                        gamepadMapping[controller].type = ControlType.PS4;
                        ps4++;
                    }
                    else
                    {
                        gamepadMapping[controller].type = ControlType.Xbox;
                        xbox++;
                    }
                    gamepadMapping[controller].joysticNum = i + 1;
                    controller++;
                }
            }
            for (; controller < 7; controller++)
            {
                gamepadMapping[controller].type = ControlType.Xbox;
                gamepadMapping[controller].joysticNum = -1;
            }
            int max = Mathf.Max(ps3, ps4, xbox);
            if (max == ps3)
                gamepadMapping[0].type = ControlType.PS3;
            else if (max == ps4)
                gamepadMapping[0].type = ControlType.PS4;
            else
                gamepadMapping[0].type = ControlType.Xbox;
        }

        /// <summary> Used to see if the user has pressed any monitored input. </summary>
        /// <returns> True if any of the buttons have been pressed. </returns>
        public static bool AnyInput()
        {
            foreach (bool b in bools)
                if (b) return true;
            foreach (bool b in boolsUp)
                if (b) return true;
            foreach (bool b in boolsHeld)
                if (b) return true;
            foreach (bool b in boolsFreshPress)
                if (b) return true;
            foreach (bool b in boolsFreshPressDeleteOnRead)
                if (b) return true;
            return false;
        }

        /// <summary> Used to see if the user hit any button on any controller. </summary>
        /// <returns> True if the user hit any input on any controller. </returns>
        public static bool AnyPadInput()
        {
            if (GetAxis(LEFT_STICK_LEFT, gamepadMapping[0]) != 0)
                return true;
            if (GetAxis(LEFT_STICK_RIGHT, gamepadMapping[0]) != 0)
                return true;
            if (GetAxis(LEFT_STICK_UP, gamepadMapping[0]) != 0)
                return true;
            if (GetAxis(LEFT_STICK_DOWN, gamepadMapping[0]) != 0)
                return true;
            if (GetAxis(RIGHT_STICK_LEFT, gamepadMapping[0]) != 0)
                return true;
            if (GetAxis(RIGHT_STICK_RIGHT, gamepadMapping[0]) != 0)
                return true;
            if (GetAxis(RIGHT_STICK_UP, gamepadMapping[0]) != 0)
                return true;
            if (GetAxis(RIGHT_STICK_DOWN, gamepadMapping[0]) != 0)
                return true;
            if (GetAxis(DPAD_LEFT, gamepadMapping[0]) != 0)
                return true;
            if (GetAxis(DPAD_RIGHT, gamepadMapping[0]) != 0)
                return true;
            if (GetAxis(DPAD_UP, gamepadMapping[0]) != 0)
                return true;
            if (GetAxis(DPAD_DOWN, gamepadMapping[0]) != 0)
                return true;
            if (GetTrigger(LEFT_TRIGGER, gamepadMapping[0]) != 0)
                return true;
            if (GetTrigger(RIGHT_TRIGGER, gamepadMapping[0]) != 0)
                return true;
            if (Input.GetKey(GetKeyCode(A, gamepadMapping[0])))
                return true;
            if (Input.GetKey(GetKeyCode(B, gamepadMapping[0])))
                return true;
            if (Input.GetKey(GetKeyCode(X, gamepadMapping[0])))
                return true;
            if (Input.GetKey(GetKeyCode(Y, gamepadMapping[0])))
                return true;
            if (Input.GetKey(GetKeyCode(LB, gamepadMapping[0])))
                return true;
            if (Input.GetKey(GetKeyCode(RB, gamepadMapping[0])))
                return true;
            if (Input.GetKey(GetKeyCode(BACK, gamepadMapping[0])))
                return true;
            if (Input.GetKey(GetKeyCode(START, gamepadMapping[0])))
                return true;
            if (Input.GetKey(GetKeyCode(LEFT_STICK_CLICK, gamepadMapping[0])))
                return true;
            if (Input.GetKey(GetKeyCode(RIGHT_STICK_CLICK, gamepadMapping[0])))
                return true;
            return false;
        }

        /// <summary> Updates all the values for a specific input based on the keyboard. </summary>
        /// <param name="input"> The input to update. </param>
        private void updateKey(int input, int playerNumber)
        {
            bool key = false, keyUp = false;
            if (Input.GetKey(keyboard[input, playerNumber]))
                key = true;
            else if (Input.GetKeyUp(keyboard[input, playerNumber]))
                keyUp = true;
            UpdateBools(key, keyUp, input, 1f, playerNumber);
        }

        /// <summary> Updates all the values for a specific input based on a controller. </summary>
        /// <param name="input"> The input to update. </param>
        private void updatePad(int input, int playerNumber)
        {
            switch (gamepad[input, playerNumber])
            {
                case LEFT_STICK_RIGHT:
                case LEFT_STICK_LEFT:
                case LEFT_STICK_UP:
                case LEFT_STICK_DOWN:
                case RIGHT_STICK_RIGHT:
                case RIGHT_STICK_LEFT:
                case RIGHT_STICK_UP:
                case RIGHT_STICK_DOWN:
                case DPAD_RIGHT:
                case DPAD_LEFT:
                case DPAD_UP:
                case DPAD_DOWN: UpdateAxis(input, GetAxis(gamepad[input, playerNumber], gamepadMapping[playerNumber]), playerNumber); break;
                case LEFT_TRIGGER:
                case RIGHT_TRIGGER: UpdateAxis(input, GetTrigger(gamepad[input, playerNumber], gamepadMapping[playerNumber]), playerNumber); break;
                default: UpdateButton(input, playerNumber); break;
            }
        }

        /// <summary> Update the buttons corresponding to axis. </summary>
        /// <param name="input"> The input to update. </param>
        /// <param name="data"> The data from the axis. </param>
        private void UpdateAxis(int input, float data, int playerNumber)
        {
            bool key = false, keyUp = false;

            if (gamepad[input, playerNumber] == LEFT_STICK_LEFT || gamepad[(int)input, playerNumber] == LEFT_STICK_UP || gamepad[(int)input, playerNumber] == RIGHT_STICK_LEFT ||
                gamepad[(int)input, playerNumber] == RIGHT_STICK_UP || gamepad[input, playerNumber] == DPAD_LEFT || gamepad[input, playerNumber] == DPAD_DOWN)
            {
                if (data < 0)
                    key = true;
                else if (bools[input, playerNumber])
                    keyUp = true;
            }
            else
            {
                if (data > 0)
                    key = true;
                else if (bools[input, playerNumber])
                    keyUp = true;
            }
            UpdateBools(key, keyUp, input, data, playerNumber);
        }

        /// <summary> Update the buttons corresponding to buttons. </summary>
        /// <param name="input"> The input to update. </param>
        private void UpdateButton(int input, int playerNumber)
        {
            bool key = false, keyUp = false;
            if (gamepadMapping[playerNumber].joysticNum >= 0)
            {
                if (Input.GetKey(GetKeyCode(gamepad[input, playerNumber], gamepadMapping[playerNumber])))
                    key = true;
                else if (Input.GetKeyUp(GetKeyCode(gamepad[input, playerNumber], gamepadMapping[playerNumber])))
                    keyUp = true;
            }
            UpdateBools(key, keyUp, input, 1f, playerNumber);
        }

        /// <summary> Actually does the updating of the bools. </summary>
        /// <param name="key"> Whether this input has been activated. </param>
        /// <param name="keyUp"> Whether this input has just been released. </param>
        /// <param name="input"> The input to update. </param>
        /// <param name="data"> The value for the raw data. </param>
        private void UpdateBools(bool key, bool keyUp, int input, float data, int playerNumber)
        {
            if (boolsFreshPressAccessed[input, playerNumber])
            {
                boolsFreshPressAccessed[input, playerNumber] = false;
                boolsFreshPress[input, playerNumber] = false;
                boolsFreshPressDeleteOnRead[input, playerNumber] = false;

                rawsFreshPressAccessed[input, playerNumber] = false;
                rawsFreshPress[input, playerNumber] = 0f;
                rawsFreshPressDeleteOnRead[input, playerNumber] = 0f;
            }
            if (!bools[input, playerNumber] && key)
            {
                boolsFreshPress[input, playerNumber] = true;
                boolsFreshPressDeleteOnRead[input, playerNumber] = true;

                rawsFreshPress[input, playerNumber] = data;
                if (Mathf.Sign(rawsFreshPress[input, playerNumber]) != Mathf.Sign(rawSign[input]))
                    rawsFreshPress[input, playerNumber] = -rawsFreshPress[input, playerNumber];
                rawsFreshPressDeleteOnRead[input, playerNumber] = data;
                if (Mathf.Sign(rawsFreshPressDeleteOnRead[input, playerNumber]) != Mathf.Sign(rawSign[input]))
                    rawsFreshPressDeleteOnRead[input, playerNumber] = -rawsFreshPressDeleteOnRead[input, playerNumber];
            }
            if (key)
            {
                bools[input, playerNumber] = true;
                boolsHeld[input, playerNumber] = true;
                boolsUp[input, playerNumber] = false;

                raws[input, playerNumber] = data;
                if (Mathf.Sign(raws[input, playerNumber]) != Mathf.Sign(rawSign[input]))
                    raws[input, playerNumber] = -raws[input, playerNumber];
                rawsHeld[input, playerNumber] = data;
                if (Mathf.Sign(rawsHeld[input, playerNumber]) != Mathf.Sign(rawSign[input]))
                    rawsHeld[input, playerNumber] = -rawsHeld[input, playerNumber];
                rawsUp[input, playerNumber] = 0f;
            }
            else if (keyUp)
            {
                bools[input, playerNumber] = false;
                boolsHeld[input, playerNumber] = false;
                boolsFreshPress[input, playerNumber] = false;
                boolsFreshPressDeleteOnRead[input, playerNumber] = false;
                boolsFreshPressAccessed[input, playerNumber] = false;
                boolsUp[input, playerNumber] = true;

                raws[input, playerNumber] = 0f;
                rawsHeld[input, playerNumber] = 0f;
                rawsFreshPress[input, playerNumber] = 0f;
                rawsFreshPressDeleteOnRead[input, playerNumber] = 0f;
                rawsFreshPressAccessed[input, playerNumber] = false;
                rawsUp[input, playerNumber] = data;
                if (Mathf.Sign(rawsUp[input, playerNumber]) != Mathf.Sign(rawSign[input]))
                    rawsUp[input, playerNumber] = -rawsUp[input, playerNumber];
            }
            else
            {
                boolsUp[input, playerNumber] = false;
                rawsUp[input, playerNumber] = 0f;
            }
        }

        /// <summary> Gets the raw input data for a given input axis. </summary>
        /// <param name="axisName"> The axis to check. </param>
        /// <param name="controller"> The controller to get input from. </param>
        /// <returns> The value of Input.GetAxisRaw for the given axis. </returns>
        private static float GetAxis(string axisName, Controller controller)
        {
            if (controller.joysticNum < 0)
                return 0;
            ControlType inControlType = controller.type;
            int joyStickNumber = controller.joysticNum;
            switch (axisName)
            {
                #region RightStickY
                case RIGHT_STICK_UP:
                case RIGHT_STICK_DOWN:
                    if (inControlType == ControlType.PS3)
                        return Input.GetAxisRaw(RIGHT_PS_STICK_Y + joyStickNumber);
                    if (inControlType == ControlType.PS4)
                        return Input.GetAxisRaw(RIGHT_PS4_STICK_Y + joyStickNumber);
                    switch (Application.platform)
                    {
                        case RuntimePlatform.OSXDashboardPlayer:
                        case RuntimePlatform.OSXEditor:
                        case RuntimePlatform.OSXPlayer: return Input.GetAxisRaw(RIGHT_OSX_STICK_Y + joyStickNumber);
                        default: return Input.GetAxisRaw(RIGHT_WIN_STICK_Y + joyStickNumber);
                    }
                #endregion
                #region RightStickX
                case RIGHT_STICK_LEFT:
                case RIGHT_STICK_RIGHT:
                    if (inControlType == ControlType.PS3)
                        return Input.GetAxisRaw(RIGHT_PS_STICK_X + joyStickNumber);
                    if (inControlType == ControlType.PS4)
                        return Input.GetAxisRaw(RIGHT_PS4_STICK_X + joyStickNumber);
                    switch (Application.platform)
                    {
                        case RuntimePlatform.OSXDashboardPlayer:
                        case RuntimePlatform.OSXEditor:
                        case RuntimePlatform.OSXPlayer: return Input.GetAxisRaw(RIGHT_OSX_STICK_X + joyStickNumber);
                        default: return Input.GetAxisRaw(RIGHT_WIN_STICK_X + joyStickNumber);
                    }
                #endregion
                #region LeftStickY
                case LEFT_STICK_UP:
                case LEFT_STICK_DOWN: return Input.GetAxisRaw(LEFT_STICK_Y + joyStickNumber);
                #endregion
                #region LeftStickX
                case LEFT_STICK_LEFT:
                case LEFT_STICK_RIGHT: return Input.GetAxisRaw(LEFT_STICK_X + joyStickNumber);
                #endregion
                #region DpadY
                case DPAD_UP:
                case DPAD_DOWN:
                    if (inControlType == ControlType.PS3)
                        return Input.GetAxisRaw(DPAD_PS_STICK_Y + joyStickNumber);
                    if (inControlType == ControlType.PS4)
                        return Input.GetAxisRaw(DPAD_PS4_STICK_Y + joyStickNumber);
                    switch (Application.platform)
                    {
                        case RuntimePlatform.OSXDashboardPlayer:
                        case RuntimePlatform.OSXEditor:
                        case RuntimePlatform.OSXPlayer:
                            switch (joyStickNumber)
                            {
                                case 1: return (Input.GetKey(KeyCode.Joystick1Button6) ? -1f : (Input.GetKey(KeyCode.Joystick1Button5) ? 1f : 0f));
                                case 2: return (Input.GetKey(KeyCode.Joystick2Button6) ? -1f : (Input.GetKey(KeyCode.Joystick2Button5) ? 1f : 0f));
                                case 3: return (Input.GetKey(KeyCode.Joystick3Button6) ? -1f : (Input.GetKey(KeyCode.Joystick3Button5) ? 1f : 0f));
                                case 4: return (Input.GetKey(KeyCode.Joystick3Button6) ? -1f : (Input.GetKey(KeyCode.Joystick4Button5) ? 1f : 0f));
                                case 5: return (Input.GetKey(KeyCode.Joystick4Button6) ? -1f : (Input.GetKey(KeyCode.Joystick5Button5) ? 1f : 0f));
                                case 6: return (Input.GetKey(KeyCode.Joystick5Button6) ? -1f : (Input.GetKey(KeyCode.Joystick6Button5) ? 1f : 0f));
                                default: return (Input.GetKey(KeyCode.Joystick6Button6) ? -1f : (Input.GetKey(KeyCode.JoystickButton5) ? 1f : 0f));
                            }
                        case RuntimePlatform.LinuxPlayer: return Input.GetAxisRaw(DPAD_LINUX_STICK_Y + joyStickNumber);
                        default: return Input.GetAxisRaw(DPAD_WIN_STICK_Y + joyStickNumber);
                    }
                #endregion
                #region DpadX
                case DPAD_LEFT:
                case DPAD_RIGHT:
                    if (inControlType == ControlType.PS3)
                        return Input.GetAxisRaw(DPAD_PS_STICK_X + joyStickNumber);
                    if (inControlType == ControlType.PS4)
                        return Input.GetAxisRaw(DPAD_PS4_STICK_X + joyStickNumber);
                    switch (Application.platform)
                    {
                        case RuntimePlatform.OSXDashboardPlayer:
                        case RuntimePlatform.OSXEditor:
                        case RuntimePlatform.OSXPlayer:
                            switch (joyStickNumber)
                            {
                                case 1: return (Input.GetKey(KeyCode.Joystick1Button7) ? -1f : (Input.GetKey(KeyCode.Joystick1Button8) ? 1f : 0f));
                                case 2: return (Input.GetKey(KeyCode.Joystick2Button7) ? -1f : (Input.GetKey(KeyCode.Joystick2Button8) ? 1f : 0f));
                                case 3: return (Input.GetKey(KeyCode.Joystick3Button7) ? -1f : (Input.GetKey(KeyCode.Joystick3Button8) ? 1f : 0f));
                                case 4: return (Input.GetKey(KeyCode.Joystick4Button7) ? -1f : (Input.GetKey(KeyCode.Joystick4Button8) ? 1f : 0f));
                                case 5: return (Input.GetKey(KeyCode.Joystick5Button7) ? -1f : (Input.GetKey(KeyCode.Joystick5Button8) ? 1f : 0f));
                                case 6: return (Input.GetKey(KeyCode.Joystick6Button7) ? -1f : (Input.GetKey(KeyCode.Joystick6Button8) ? 1f : 0f));
                                default: return (Input.GetKey(KeyCode.JoystickButton7) ? -1f : (Input.GetKey(KeyCode.JoystickButton8) ? 1f : 0f));
                            }
                        case RuntimePlatform.LinuxPlayer: return Input.GetAxisRaw(DPAD_LINUX_STICK_X + joyStickNumber);
                        default: return Input.GetAxisRaw(DPAD_WIN_STICK_X + joyStickNumber);
                    }
                #endregion
                default: return 0f;
            }
        }

        /// <summary> Gets the raw input data for a given trigger. </summary>
        /// <param name="trgName"> The trigger in question. </param>
        /// <param name="controller"> The controller to get input from. </param>
        /// <returns> The value of Input.GetAxisRaw for the given trigger. </returns>
        private static float GetTrigger(string trgName, Controller controller)
        {
            if (controller.joysticNum < 0)
                return 0f;
            ControlType inControlType = controller.type;
            int joyStickNumber = controller.joysticNum;
            switch (trgName)
            {
                case LEFT_TRIGGER:
                    if (inControlType == ControlType.PS3 || inControlType == ControlType.PS4)
                    {
                        switch (joyStickNumber)
                        {
                            case 1: return (Input.GetKey(KeyCode.Joystick1Button6) ? 1f : 0f);
                            case 2: return (Input.GetKey(KeyCode.Joystick2Button6) ? 1f : 0f);
                            case 3: return (Input.GetKey(KeyCode.Joystick3Button6) ? 1f : 0f);
                            case 4: return (Input.GetKey(KeyCode.Joystick4Button6) ? 1f : 0f);
                            case 5: return (Input.GetKey(KeyCode.Joystick5Button6) ? 1f : 0f);
                            case 6: return (Input.GetKey(KeyCode.Joystick6Button6) ? 1f : 0f);
                            default: return (Input.GetKey(KeyCode.JoystickButton6) ? 1f : 0f);
                        }
                    }
                    switch (Application.platform)
                    {
                        case RuntimePlatform.OSXDashboardPlayer:
                        case RuntimePlatform.OSXEditor:
                        case RuntimePlatform.OSXPlayer: return Input.GetAxisRaw(LEFT_OSX_TRIGGER + joyStickNumber);
                        case RuntimePlatform.LinuxPlayer: return Input.GetAxisRaw(LEFT_LINUX_TRIGGER + joyStickNumber);
                        default: return Input.GetAxisRaw(LEFT_WIN_TRIGGER + joyStickNumber);
                    }
                case RIGHT_TRIGGER:
                    if (inControlType == ControlType.PS3 || inControlType == ControlType.PS4)
                    {
                        switch (joyStickNumber)
                        {
                            case 1: return (Input.GetKey(KeyCode.Joystick1Button7) ? 1f : 0f);
                            case 2: return (Input.GetKey(KeyCode.Joystick2Button7) ? 1f : 0f);
                            case 3: return (Input.GetKey(KeyCode.Joystick3Button7) ? 1f : 0f);
                            case 4: return (Input.GetKey(KeyCode.Joystick4Button7) ? 1f : 0f);
                            case 5: return (Input.GetKey(KeyCode.Joystick5Button7) ? 1f : 0f);
                            case 6: return (Input.GetKey(KeyCode.Joystick6Button7) ? 1f : 0f);
                            default: return (Input.GetKey(KeyCode.JoystickButton7) ? 1f : 0f);
                        }
                    }
                    switch (Application.platform)
                    {
                        case RuntimePlatform.OSXDashboardPlayer:
                        case RuntimePlatform.OSXEditor:
                        case RuntimePlatform.OSXPlayer:return Input.GetAxisRaw(RIGHT_OSX_TRIGGER + joyStickNumber);
                        case RuntimePlatform.LinuxPlayer: return Input.GetAxisRaw(RIGHT_LINUX_TRIGGER + joyStickNumber);
                        default: return Input.GetAxisRaw(RIGHT_WIN_TRIGGER + joyStickNumber);
                    }
                default: return 0f;
            }
        }

        /// <summary> Gets the KeyCode for the specified button. </summary>
        /// <param name="btn"> The button in question. </param>
        /// <param name="controller"> The controller to get input from. </param>
        /// <returns> A KeyCode coresponding to the button on the controller specified. </returns>
        private static KeyCode GetKeyCode(string btn, Controller controller)
        {
            if (controller.joysticNum < 0)
                return KeyCode.None;
            ControlType inControlType = controller.type;
            int joyStickNumber = controller.joysticNum;
            switch (btn)
            {
                #region A
                case A: //X for playstation
                    if (inControlType == ControlType.PS3 || inControlType == ControlType.PS4)
                    {
                        switch (joyStickNumber)
                        {
                            case 1: return KeyCode.Joystick1Button1;
                            case 2: return KeyCode.Joystick2Button1;
                            case 3: return KeyCode.Joystick3Button1;
                            case 4: return KeyCode.Joystick4Button1;
                            case 5: return KeyCode.Joystick5Button1;
                            case 6: return KeyCode.Joystick6Button1;
                            default: return KeyCode.JoystickButton1;
                        }
                    }
                    switch (Application.platform)
                    {
                        case RuntimePlatform.OSXDashboardPlayer:
                        case RuntimePlatform.OSXEditor:
                        case RuntimePlatform.OSXPlayer:
                            switch (joyStickNumber)
                            {
                                case 1: return KeyCode.Joystick1Button16;
                                case 2: return KeyCode.Joystick2Button16;
                                case 3: return KeyCode.Joystick3Button16;
                                case 4: return KeyCode.Joystick4Button16;
                                case 5: return KeyCode.Joystick5Button16;
                                case 6: return KeyCode.Joystick6Button16;
                                default: return KeyCode.JoystickButton16;
                            }

                        default:
                            switch (joyStickNumber)
                            {
                                case 1: return KeyCode.Joystick1Button0;
                                case 2: return KeyCode.Joystick2Button0;
                                case 3: return KeyCode.Joystick3Button0;
                                case 4: return KeyCode.Joystick4Button0;
                                case 5: return KeyCode.Joystick5Button0;
                                case 6: return KeyCode.Joystick6Button0;
                                default: return KeyCode.JoystickButton0;
                            }
                    }
                #endregion
                #region B
                case B: //Circle for playstation
                    if (inControlType == ControlType.PS3 || inControlType == ControlType.PS4)
                    {
                        switch (joyStickNumber)
                        {
                            case 1: return KeyCode.Joystick1Button2;
                            case 2: return KeyCode.Joystick2Button2;
                            case 3: return KeyCode.Joystick3Button2;
                            case 4: return KeyCode.Joystick4Button2;
                            case 5: return KeyCode.Joystick5Button2;
                            case 6: return KeyCode.Joystick6Button2;
                            default: return KeyCode.JoystickButton2;
                        }

                    }
                    switch (Application.platform)
                    {
                        case RuntimePlatform.OSXDashboardPlayer:
                        case RuntimePlatform.OSXEditor:
                        case RuntimePlatform.OSXPlayer:
                            switch (joyStickNumber)
                            {
                                case 1: return KeyCode.Joystick1Button17;
                                case 2: return KeyCode.Joystick2Button17;
                                case 3: return KeyCode.Joystick3Button17;
                                case 4: return KeyCode.Joystick4Button17;
                                case 5: return KeyCode.Joystick5Button17;
                                case 6: return KeyCode.Joystick6Button17;
                                default: return KeyCode.JoystickButton17;
                            }

                        default:
                            switch (joyStickNumber)
                            {
                                case 1: return KeyCode.Joystick1Button1;
                                case 2: return KeyCode.Joystick2Button1;
                                case 3: return KeyCode.Joystick3Button1;
                                case 4: return KeyCode.Joystick4Button1;
                                case 5: return KeyCode.Joystick5Button1;
                                case 6: return KeyCode.Joystick6Button1;
                                default: return KeyCode.JoystickButton1;
                            }
                    }
                #endregion
                #region X
                case X:  //Square for playstation
                    if (inControlType == ControlType.PS3 || inControlType == ControlType.PS4)
                    {
                        switch (joyStickNumber)
                        {
                            case 1: return KeyCode.Joystick1Button0;
                            case 2: return KeyCode.Joystick2Button0;
                            case 3: return KeyCode.Joystick3Button0;
                            case 4: return KeyCode.Joystick4Button0;
                            case 5: return KeyCode.Joystick5Button0;
                            case 6: return KeyCode.Joystick6Button0;
                            default: return KeyCode.JoystickButton0;
                        }
                    }
                    switch (Application.platform)
                    {
                        case RuntimePlatform.OSXDashboardPlayer:
                        case RuntimePlatform.OSXEditor:
                        case RuntimePlatform.OSXPlayer:
                            switch (joyStickNumber)
                            {
                                case 1: return KeyCode.Joystick1Button18;
                                case 2: return KeyCode.Joystick2Button18;
                                case 3: return KeyCode.Joystick3Button18;
                                case 4: return KeyCode.Joystick4Button18;
                                case 5: return KeyCode.Joystick5Button18;
                                case 6: return KeyCode.Joystick6Button18;
                                default: return KeyCode.JoystickButton18;
                            }

                        default:
                            switch (joyStickNumber)
                            {
                                case 1: return KeyCode.Joystick1Button2;
                                case 2: return KeyCode.Joystick2Button2;
                                case 3: return KeyCode.Joystick3Button2;
                                case 4: return KeyCode.Joystick4Button2;
                                case 5: return KeyCode.Joystick5Button2;
                                case 6: return KeyCode.Joystick6Button2;
                                default: return KeyCode.JoystickButton2;
                            }

                    }
                #endregion
                #region Y
                case Y: //Triangle for playstation
                    if (inControlType == ControlType.PS3 || inControlType == ControlType.PS4)
                    {
                        switch (joyStickNumber)
                        {
                            case 1: return KeyCode.Joystick1Button3;
                            case 2: return KeyCode.Joystick2Button3;
                            case 3: return KeyCode.Joystick3Button3;
                            case 4: return KeyCode.Joystick4Button3;
                            case 5: return KeyCode.Joystick5Button3;
                            case 6: return KeyCode.Joystick6Button3;
                            default: return KeyCode.JoystickButton3;
                        }
                    }
                    switch (Application.platform)
                    {
                        case RuntimePlatform.OSXDashboardPlayer:
                        case RuntimePlatform.OSXEditor:
                        case RuntimePlatform.OSXPlayer:
                            switch (joyStickNumber)
                            {
                                case 1: return KeyCode.Joystick1Button19;
                                case 2: return KeyCode.Joystick2Button19;
                                case 3: return KeyCode.Joystick3Button19;
                                case 4: return KeyCode.Joystick4Button19;
                                case 5: return KeyCode.Joystick5Button19;
                                case 6: return KeyCode.Joystick6Button19;
                                default: return KeyCode.JoystickButton19;
                            }
                        default:
                            switch (joyStickNumber)
                            {
                                case 1: return KeyCode.Joystick1Button3;
                                case 2: return KeyCode.Joystick2Button3;
                                case 3: return KeyCode.Joystick3Button3;
                                case 4: return KeyCode.Joystick4Button3;
                                case 5: return KeyCode.Joystick5Button3;
                                case 6: return KeyCode.Joystick6Button3;
                                default: return KeyCode.JoystickButton3;
                            }
                    }
                #endregion
                #region RB
                case RB:   //R1 for playstation
                    if (inControlType == ControlType.PS3 || inControlType == ControlType.PS4)
                    {
                        switch (joyStickNumber)
                        {
                            case 1: return KeyCode.Joystick1Button5;
                            case 2: return KeyCode.Joystick2Button5;
                            case 3: return KeyCode.Joystick3Button5;
                            case 4: return KeyCode.Joystick4Button5;
                            case 5: return KeyCode.Joystick5Button5;
                            case 6: return KeyCode.Joystick6Button5;
                            default: return KeyCode.JoystickButton5;
                        }
                    }
                    switch (Application.platform)
                    {
                        case RuntimePlatform.OSXDashboardPlayer:
                        case RuntimePlatform.OSXEditor:
                        case RuntimePlatform.OSXPlayer:
                            switch (joyStickNumber)
                            {
                                case 1: return KeyCode.Joystick1Button14;
                                case 2: return KeyCode.Joystick2Button14;
                                case 3: return KeyCode.Joystick3Button14;
                                case 4: return KeyCode.Joystick4Button14;
                                case 5: return KeyCode.Joystick5Button14;
                                case 6: return KeyCode.Joystick6Button14;
                                default: return KeyCode.JoystickButton14;
                            }
                        default:
                            switch (joyStickNumber)
                            {
                                case 1: return KeyCode.Joystick1Button5;
                                case 2: return KeyCode.Joystick2Button5;
                                case 3: return KeyCode.Joystick3Button5;
                                case 4: return KeyCode.Joystick4Button5;
                                case 5: return KeyCode.Joystick5Button5;
                                case 6: return KeyCode.Joystick6Button5;
                                default: return KeyCode.JoystickButton5;
                            }
                    }
                #endregion
                #region LB
                case LB:    //L1 for playstation
                    if (inControlType == ControlType.PS3)
                    {
                        switch (joyStickNumber)
                        {
                            case 1: return KeyCode.Joystick1Button6;
                            case 2: return KeyCode.Joystick2Button6;
                            case 3: return KeyCode.Joystick3Button6;
                            case 4: return KeyCode.Joystick4Button6;
                            case 5: return KeyCode.Joystick5Button6;
                            case 6: return KeyCode.Joystick6Button6;
                            default: return KeyCode.JoystickButton6;
                        }
                    }
                    if (inControlType == ControlType.PS4)
                    {
                        switch (joyStickNumber)
                        {
                            case 1: return KeyCode.Joystick1Button4;
                            case 2: return KeyCode.Joystick2Button4;
                            case 3: return KeyCode.Joystick3Button4;
                            case 4: return KeyCode.Joystick4Button4;
                            case 5: return KeyCode.Joystick5Button4;
                            case 6: return KeyCode.Joystick6Button4;
                            default: return KeyCode.JoystickButton4;
                        }
                    }
                    switch (Application.platform)
                    {
                        case RuntimePlatform.OSXDashboardPlayer:
                        case RuntimePlatform.OSXEditor:
                        case RuntimePlatform.OSXPlayer:
                            switch (joyStickNumber)
                            {
                                case 1: return KeyCode.Joystick1Button13;
                                case 2: return KeyCode.Joystick2Button13;
                                case 3: return KeyCode.Joystick3Button13;
                                case 4: return KeyCode.Joystick4Button13;
                                case 5: return KeyCode.Joystick5Button13;
                                case 6: return KeyCode.Joystick6Button13;
                                default: return KeyCode.JoystickButton13;
                            }
                        default:
                            switch (joyStickNumber)
                            {
                                case 1: return KeyCode.Joystick1Button4;
                                case 2: return KeyCode.Joystick2Button4;
                                case 3: return KeyCode.Joystick3Button4;
                                case 4: return KeyCode.Joystick4Button4;
                                case 5: return KeyCode.Joystick5Button4;
                                case 6: return KeyCode.Joystick6Button4;
                                default: return KeyCode.JoystickButton4;
                            }
                    }
                #endregion
                #region Back
                case BACK:      //Select for playstation
                    if (inControlType == ControlType.PS3 || inControlType == ControlType.PS4)
                    {
                        switch (joyStickNumber)
                        {
                            case 1: return KeyCode.Joystick1Button8;
                            case 2: return KeyCode.Joystick2Button8;
                            case 3: return KeyCode.Joystick3Button8;
                            case 4: return KeyCode.Joystick4Button8;
                            case 5: return KeyCode.Joystick5Button8;
                            case 6: return KeyCode.Joystick6Button8;
                            default: return KeyCode.JoystickButton8;
                        }
                    }
                    switch (Application.platform)
                    {
                        case RuntimePlatform.OSXDashboardPlayer:
                        case RuntimePlatform.OSXEditor:
                        case RuntimePlatform.OSXPlayer:
                            switch (joyStickNumber)
                            {
                                case 1: return KeyCode.Joystick1Button10;
                                case 2: return KeyCode.Joystick2Button10;
                                case 3: return KeyCode.Joystick3Button10;
                                case 4: return KeyCode.Joystick4Button10;
                                case 5: return KeyCode.Joystick5Button10;
                                case 6: return KeyCode.Joystick6Button10;
                                default: return KeyCode.JoystickButton10;
                            }
                        default:
                            switch (joyStickNumber)
                            {
                                case 1: return KeyCode.Joystick1Button6;
                                case 2: return KeyCode.Joystick2Button6;
                                case 3: return KeyCode.Joystick3Button6;
                                case 4: return KeyCode.Joystick4Button6;
                                case 5: return KeyCode.Joystick5Button6;
                                case 6: return KeyCode.Joystick6Button6;
                                default: return KeyCode.JoystickButton6;
                            }
                    }
                #endregion
                #region Start
                case START:     //Start for playstation
                    if (inControlType == ControlType.PS3 || inControlType == ControlType.PS4)
                    {
                        switch (joyStickNumber)
                        {
                            case 1: return KeyCode.Joystick1Button9;
                            case 2: return KeyCode.Joystick2Button9;
                            case 3: return KeyCode.Joystick3Button9;
                            case 4: return KeyCode.Joystick4Button9;
                            case 5: return KeyCode.Joystick5Button9;
                            case 6: return KeyCode.Joystick6Button9;
                            default: return KeyCode.JoystickButton9;
                        }
                    }
                    switch (Application.platform)
                    {
                        case RuntimePlatform.OSXDashboardPlayer:
                        case RuntimePlatform.OSXEditor:
                        case RuntimePlatform.OSXPlayer:
                            switch (joyStickNumber)
                            {
                                case 1: return KeyCode.Joystick1Button9;
                                case 2: return KeyCode.Joystick2Button9;
                                case 3: return KeyCode.Joystick3Button9;
                                case 4: return KeyCode.Joystick4Button9;
                                case 5: return KeyCode.Joystick5Button9;
                                case 6: return KeyCode.Joystick6Button9;
                                default: return KeyCode.JoystickButton9;
                            }
                        default:
                            switch (joyStickNumber)
                            {
                                case 1: return KeyCode.Joystick1Button7;
                                case 2: return KeyCode.Joystick2Button7;
                                case 3: return KeyCode.Joystick3Button7;
                                case 4: return KeyCode.Joystick4Button7;
                                case 5: return KeyCode.Joystick5Button7;
                                case 6: return KeyCode.Joystick6Button7;
                                default: return KeyCode.JoystickButton7;
                            }
                    }
                #endregion
                #region Right stick click
                case RIGHT_STICK_CLICK:
                    if (inControlType == ControlType.PS3)
                    {
                        switch (joyStickNumber)
                        {
                            case 1: return KeyCode.Joystick1Button13;
                            case 2: return KeyCode.Joystick2Button13;
                            case 3: return KeyCode.Joystick3Button13;
                            case 4: return KeyCode.Joystick4Button13;
                            case 5: return KeyCode.Joystick5Button13;
                            case 6: return KeyCode.Joystick6Button13;
                            default: return KeyCode.JoystickButton13;
                        }
                    }
                    if (inControlType == ControlType.PS4)
                    {
                        switch (joyStickNumber)
                        {
                            case 1: return KeyCode.Joystick1Button11;
                            case 2: return KeyCode.Joystick2Button11;
                            case 3: return KeyCode.Joystick3Button11;
                            case 4: return KeyCode.Joystick4Button11;
                            case 5: return KeyCode.Joystick5Button11;
                            case 6: return KeyCode.Joystick6Button11;
                            default: return KeyCode.JoystickButton11;
                        }
                    }
                    switch (Application.platform)
                    {
                        case RuntimePlatform.OSXDashboardPlayer:
                        case RuntimePlatform.OSXEditor:
                        case RuntimePlatform.OSXPlayer:
                            switch (joyStickNumber)
                            {
                                case 1: return KeyCode.Joystick1Button12;
                                case 2: return KeyCode.Joystick2Button12;
                                case 3: return KeyCode.Joystick3Button12;
                                case 4: return KeyCode.Joystick4Button12;
                                case 5: return KeyCode.Joystick5Button12;
                                case 6: return KeyCode.Joystick6Button12;
                                default: return KeyCode.JoystickButton12;
                            }
                        case RuntimePlatform.LinuxPlayer:
                            switch (joyStickNumber)
                            {
                                case 1: return KeyCode.Joystick1Button10;
                                case 2: return KeyCode.Joystick2Button10;
                                case 3: return KeyCode.Joystick3Button10;
                                case 4: return KeyCode.Joystick4Button10;
                                case 5: return KeyCode.Joystick5Button10;
                                case 6: return KeyCode.Joystick6Button10;
                                default: return KeyCode.JoystickButton10;
                            }
                        default:
                            switch (joyStickNumber)
                            {
                                case 1: return KeyCode.Joystick1Button9;
                                case 2: return KeyCode.Joystick2Button9;
                                case 3: return KeyCode.Joystick3Button9;
                                case 4: return KeyCode.Joystick4Button9;
                                case 5: return KeyCode.Joystick5Button9;
                                case 6: return KeyCode.Joystick6Button9;
                                default: return KeyCode.JoystickButton9;
                            }
                    }
                #endregion
                #region Left stick click
                case LEFT_STICK_CLICK:
                    if (inControlType == ControlType.PS3)
                    {
                        switch (joyStickNumber)
                        {
                            case 1: return KeyCode.Joystick1Button12;
                            case 2: return KeyCode.Joystick2Button12;
                            case 3: return KeyCode.Joystick3Button12;
                            case 4: return KeyCode.Joystick4Button12;
                            case 5: return KeyCode.Joystick5Button12;
                            case 6: return KeyCode.Joystick6Button12;
                            default: return KeyCode.JoystickButton12;
                        }
                    }
                    if (inControlType == ControlType.PS4)
                    {
                        switch (joyStickNumber)
                        {
                            case 1: return KeyCode.Joystick1Button10;
                            case 2: return KeyCode.Joystick2Button10;
                            case 3: return KeyCode.Joystick3Button10;
                            case 4: return KeyCode.Joystick4Button10;
                            case 5: return KeyCode.Joystick5Button10;
                            case 6: return KeyCode.Joystick6Button10;
                            default: return KeyCode.JoystickButton10;
                        }
                    }
                    switch (Application.platform)
                    {
                        case RuntimePlatform.OSXDashboardPlayer:
                        case RuntimePlatform.OSXEditor:
                        case RuntimePlatform.OSXPlayer:
                            switch (joyStickNumber)
                            {
                                case 1: return KeyCode.Joystick1Button11;
                                case 2: return KeyCode.Joystick2Button11;
                                case 3: return KeyCode.Joystick3Button11;
                                case 4: return KeyCode.Joystick4Button11;
                                case 5: return KeyCode.Joystick5Button11;
                                case 6: return KeyCode.Joystick6Button11;
                                default: return KeyCode.JoystickButton11;
                            }
                        case RuntimePlatform.LinuxPlayer:
                            switch (joyStickNumber)
                            {
                                case 1: return KeyCode.Joystick1Button9;
                                case 2: return KeyCode.Joystick2Button9;
                                case 3: return KeyCode.Joystick3Button9;
                                case 4: return KeyCode.Joystick4Button9;
                                case 5: return KeyCode.Joystick5Button9;
                                case 6: return KeyCode.Joystick6Button9;
                                default: return KeyCode.JoystickButton9;
                            }
                        default:
                            switch (joyStickNumber)
                            {
                                case 1: return KeyCode.Joystick1Button8;
                                case 2: return KeyCode.Joystick2Button8;
                                case 3: return KeyCode.Joystick3Button8;
                                case 4: return KeyCode.Joystick4Button8;
                                case 5: return KeyCode.Joystick5Button8;
                                case 6: return KeyCode.Joystick6Button8;
                                default: return KeyCode.JoystickButton8;
                            }
                    }
                #endregion
                default: return KeyCode.None;
            }
        }
    }
}
