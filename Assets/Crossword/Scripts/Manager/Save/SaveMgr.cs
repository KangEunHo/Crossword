using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HealingJam.Crossword.Save
{
    public class SaveMgr : Singleton<SaveMgr>
    {
        public string FilePath { get { return Path.Combine(Application.persistentDataPath, "save.dat"); } }
        public string ProgressFilePath { get { return Path.Combine(Application.persistentDataPath, "progress.dat"); } }
        public event Action<int> CoinChangeAction = null;
        public event Action<int> CoinAddAction = null;

        private SaveData saveData = null;
        public Dictionary<int, ProgressData> progressDatas = null;

        private SaveMgr() { }

        public bool Loaded { get; private set; } = false;

        public void Load(bool loadGoogle, bool loadGoogleAndSaveLocal, Action<bool> saveCallBack)
        {
            if (Loaded)
                return;
            Loaded = true;

            if (File.Exists(FilePath))
            {
                try
                {
                    string text = File.ReadAllText(FilePath);
                    saveData = JsonConvert.DeserializeObject<SaveData>(EncryptUtils.Decrypt(text));
                }
                catch(Exception e)
                {
                    Debug.LogWarning(e.Message);
                    saveData = new SaveData();
                }
            }
            else
            {
                saveData = new SaveData();
            }

            if (File.Exists(ProgressFilePath))
            {
                try
                {
                    string text = File.ReadAllText(ProgressFilePath);
                    progressDatas = JsonConvert.DeserializeObject<Dictionary<int, ProgressData>>(EncryptUtils.Decrypt(text));
                }
                catch(Exception e)
                {
                    Debug.LogWarning(e.Message);
                    progressDatas = new Dictionary<int, ProgressData>();
                }
            }
            else
            {
                progressDatas = new Dictionary<int, ProgressData>();
            }

            if (loadGoogle)
            {
                LoadGoogleGameService(loadGoogleAndSaveLocal, saveCallBack);
            }
        }

        public void LoadGoogleGameService(bool saveLocal, Action<bool> saveCallBack)
        {
            GPGSMgr.Instance.ReadSavedGame((success, data) => {
                if (success)
                {
                    if (data != null && data.Length > 0)
                    {
                        string decryptData = Convert.ToBase64String(data);
                        string serializeData = EncryptUtils.Decrypt(decryptData);
                        var googleSaveData = JsonConvert.DeserializeObject<SaveData>(serializeData);

                        if (saveData == null)
                            saveData = googleSaveData;
                        else
                        {
                            bool useGoogleSave = false;

                            int localLastUnlockPackIndex = saveData.LastUnlockPackIndex();
                            int googleLastUnlockPackIndex = googleSaveData.LastUnlockPackIndex();
                            if (localLastUnlockPackIndex < googleLastUnlockPackIndex)
                            {
                                useGoogleSave = true;
                            }
                            else if (localLastUnlockPackIndex == googleLastUnlockPackIndex)
                            {
                                int localunlockLevelIndex = saveData.GetUnlockLevel();
                                int googleunlockLevelIndex = googleSaveData.GetUnlockLevel();

                                if (localunlockLevelIndex < googleunlockLevelIndex)
                                {
                                    useGoogleSave = true;
                                }
                                else if (localunlockLevelIndex == googleunlockLevelIndex)
                                {
                                    if (saveData.coin < googleSaveData.coin)
                                    {
                                        useGoogleSave = true;
                                    }
                                    else
                                    {
                                        useGoogleSave = false;
                                    }
                                }
                                else
                                {
                                    useGoogleSave = false;
                                }
                            }
                            else
                            {
                                useGoogleSave = false;
                            }

                            if (useGoogleSave)
                            {
                                saveData = googleSaveData;
                                Save();
                            }
                            else
                            {
                                SaveGoogleService();
                            }
                        }
                    }
                }
                saveCallBack?.Invoke(success);
            });
        }

        public void Save()
        {
            if (saveData == null)
                return;

            string serializeData = JsonConvert.SerializeObject(saveData);
            string encryptData = EncryptUtils.Encrypt(serializeData);
            File.WriteAllText(FilePath, encryptData);
        }

        public void SaveProgressData()
        {
            if (progressDatas == null)
                return;

            string serializeData = JsonConvert.SerializeObject(progressDatas);
            string encryptData = EncryptUtils.Encrypt(serializeData);
            File.WriteAllText(ProgressFilePath, encryptData);
        }

        public void SaveGoogleService()
        {
            if (saveData == null)
                return;
            if (saveData.loginType != SaveData.LoginType.Google)
                return;

            if (GPGSMgr.Instance.IsOpened)
            {
                string serializeData = JsonConvert.SerializeObject(saveData);
                string encryptData = EncryptUtils.Encrypt(serializeData);
                byte[] writeData = Convert.FromBase64String(encryptData);
                GPGSMgr.Instance.WriteSavedGame(writeData, null);
            }
            else
            {
                GPGSMgr.Instance.OpenSavedGame((openSuccess) =>
                {
                    if (openSuccess)
                    {
                        string serializeData = JsonConvert.SerializeObject(saveData);
                        string encryptData = EncryptUtils.Encrypt(serializeData);
                        byte[] writeData = Convert.FromBase64String(encryptData);
                        GPGSMgr.Instance.WriteSavedGame(writeData, null);
                    }
                });
            }
        }

        public bool GetCompleteData(int index)
        {
            if (saveData == null)
                return false;

            while (saveData.completeDatas.Count <= index)
            {
                saveData.completeDatas.Add(false);
            }

            return saveData.completeDatas[index];
        }

        public void SetCompleteData(int index, bool completeData)
        {
            if (saveData == null)
                return;

            while(saveData.completeDatas.Count <= index)
            {
                saveData.completeDatas.Add(false);
            }

            saveData.completeDatas[index] = completeData;
        }

        public bool TryGetProgressData(int index, out ProgressData progressData)
        {
            progressData = null;

            if (progressDatas == null)
                return false;

            return progressDatas.TryGetValue(index, out progressData);
        }

        public void SetProgressData(int index, ProgressData progressData)
        {
            if (saveData == null)
                return;

            progressDatas[index] = progressData;
        }

        public void DeleteProgressData(int index)
        {
            if (saveData == null)
                return;

            progressDatas.Remove(index);
        }

        public LevelData GetLevelData(int index)
        {
            if (saveData == null)
                return null;

            while (saveData.levelDatas.Count <= index)
            {
                saveData.levelDatas.Add(new LevelData());
            }

            return saveData.levelDatas[index];
        }

        public void SetLevelData(int index, LevelData levelData)
        {
            if (saveData == null)
                return;

            while (saveData.levelDatas.Count < index + 1)
            {
                saveData.levelDatas.Add(new LevelData());
            }

            saveData.levelDatas[index] = levelData;
        }

        public int GetUnlockLevel()
        {
            if (saveData == null)
                return 0;

            return saveData.GetUnlockLevel();
        }

        public void SetCoin(int coin)
        {
            if (saveData == null)
                return;

            if (coin < 0)
                coin = 0;

            saveData.coin = coin;
            CoinChangeAction?.Invoke(coin);
        }

        public void AddCoin(int amount)
        {
            if (saveData == null)
                return;

            SetCoin(saveData.coin + amount);
            CoinAddAction?.Invoke(amount);
        }

        public int GetCoin()
        {
            if (saveData == null)
                return 0;
            return saveData.coin;
        }

        public void SetLoginType(SaveData.LoginType loginType)
        {
            if (saveData == null)
                return;
            saveData.loginType = loginType;
        }

        public SaveData.LoginType GetLoginType()
        {
            if (saveData == null)
                return  SaveData.LoginType.None;
            return saveData.loginType;
        }

        public void SetAdRemove(bool value)
        {
            if (saveData == null)
                return;
            saveData.isAdRemoved = value;
        }

        public bool GetIsAdRemoved()
        {
            if (saveData == null)
                return false;
            return saveData.isAdRemoved;
        }

        public bool GetPlayedCommonSenseTest()
        {
            if (saveData == null)
                return false;
            return saveData.playedCommonSenseTest;
        }

        public void SetPlayedCommonSenseTest(bool value)
        {
            if (saveData == null)
                return;
            saveData.playedCommonSenseTest = value;
        }
    }
}