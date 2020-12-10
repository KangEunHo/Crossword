using UnityEngine;
using System;
using EasyMobile;
using UnityEngine.SocialPlatforms;

public class GPGSMgr : Singleton<GPGSMgr>
{
    private const string SAVE_KEY = "com.healingjam.crossword_save_game";
    public enum LoginState
    {
        None, Failed, Successed
    }
    // To store the opened saved game.
    private SavedGame mySavedGame;

    public Action<bool> _loginCallBack = null;
    public Action<bool> _openSavedGameCallBack = null;

    private GPGSMgr()
    {
        GameServices.UserLoginSucceeded += OnUserLoginSucceeded;
        GameServices.UserLoginFailed += OnUserLoginFailed;
    }

    public bool IsOpened
    {
        get { return mySavedGame != null && mySavedGame.IsOpen; }
    }

    public bool IsInitialized()
    {
        return GameServices.IsInitialized();
    }

    public void Initialized(Action<bool> loginCallback = null)
    {
        _loginCallBack = loginCallback;

        if (!GameServices.IsInitialized())
        {
            GameServices.Init();
        }
        else
        {
            loginCallback?.Invoke(true);
        }
    }

    // Event handlers
    void OnUserLoginSucceeded()
    {
        Debug.Log("User logged in successfully.");
        _loginCallBack?.Invoke(true);
    }

    void OnUserLoginFailed()
    {
        Debug.Log("User login failed.");
        _loginCallBack?.Invoke(false);
    }

    // Open a saved game with automatic conflict resolution
    public void OpenSavedGame(Action<bool> openSavedGame)
    {
        this._openSavedGameCallBack = openSavedGame;
        // Open a saved game named "My_Saved_Game" and resolve conflicts automatically if any.
        GameServices.SavedGames.OpenWithAutomaticConflictResolution(SAVE_KEY, OpenSavedGameCallback);
    }

    // Open saved game callback
    void OpenSavedGameCallback(SavedGame savedGame, string error)
    {
        if (string.IsNullOrEmpty(error))
        {
            Debug.Log("Saved game opened successfully!");
            mySavedGame = savedGame;        // keep a reference for later operations
            _openSavedGameCallBack?.Invoke(true);
        }
        else
        {
            Debug.Log("Open saved game failed with error: " + error);
            _openSavedGameCallBack?.Invoke(false);
        }
    }

    public void WriteSavedGame(byte[] data, Action<bool> writeCallback)
    {
        if (!GameServices.IsInitialized())
        {
            writeCallback?.Invoke(false);
            return;
        }

        if (IsOpened)
        {
            // The saved game is open and ready for writing
            GameServices.SavedGames.WriteSavedGameData(
                mySavedGame,
                data,
                (SavedGame updatedSavedGame, string error) =>
                {
                    if (string.IsNullOrEmpty(error))
                    {
                        writeCallback?.Invoke(true);
                        Debug.Log("Saved game data has been written successfully!");
                    }
                    else
                    {
                        writeCallback?.Invoke(false);
                        Debug.Log("Writing saved game data failed with error: " + error);
                    }
                }

            );
        }
        else
        {
            writeCallback?.Invoke(false);
            // The saved game is not open. You can optionally open it here and repeat the process.
            Debug.Log("You must open the saved game before writing to it.");
        }
    }

    /// <summary>
    /// 저장된 데이터를 불러옵니다.
    /// </summary>
    /// <param name="readCallback"> 
    /// bool : 불러오기 성공여부.
    /// byte[] : 불러온 데이터. 실패시 null.
    /// </param>
    public void ReadSavedGame(Action<bool, byte[]> readCallback)
    {
        if (!GameServices.IsInitialized())
        {
            readCallback?.Invoke(false, null);
            return;
        }

        if (IsOpened)
        {
            // The saved game is open and ready for reading
            GameServices.SavedGames.ReadSavedGameData(
                mySavedGame,
                (SavedGame game, byte[] data, string error) =>
                {
                    if (string.IsNullOrEmpty(error))
                    {
                        Debug.Log("Saved game data has been retrieved successfully!");
                        // Here you can process the data as you wish.
                        if (data.Length > 0)
                        {
                            readCallback?.Invoke(true, data);
                            return;
                        }
                        else
                        {
                            readCallback?.Invoke(false, null);
                            Debug.Log("The saved game has no data!");
                            return;
                        }
                    }
                    else
                    {
                        readCallback?.Invoke(false, null);
                        Debug.Log("Reading saved game data failed with error: " + error);
                    }

                }
            );
        }
        else
        {
            readCallback?.Invoke(false, null);
            // The saved game is not open. You can optionally open it here and repeat the process.
            Debug.Log("You must open the saved game before reading its data.");
        }
    }

 
}
