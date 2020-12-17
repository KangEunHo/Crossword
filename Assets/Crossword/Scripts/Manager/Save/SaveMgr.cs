using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;

namespace HealingJam.Crossword.Save
{
    public class SaveMgr : Singleton<SaveMgr>
    {
        public string FilePath { get { return Path.Combine(Application.persistentDataPath, "save.dat"); } }
        public event Action<int> CoinChangeAction = null;
        public event Action<int> CoinAddAction = null;

        private SaveData saveData = null;

        private SaveMgr() { }

        public bool Loaded { get; private set; } = false;

        public void Load(bool loadGoogle, bool loadGoogleAndSaveLocal, Action<bool> saveCallBack)
        {
            if (Loaded)
                return;
            Loaded = true;

            if (File.Exists(FilePath))
            {
                string text = File.ReadAllText(FilePath);
                saveData = JsonConvert.DeserializeObject<SaveData>(EncryptUtils.Decrypt(text));
            }
            else
            {
                saveData = new SaveData();
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
                        saveData = JsonConvert.DeserializeObject<SaveData>(serializeData);

                        if (saveLocal)
                            Save(false);
                    }
                }
                saveCallBack?.Invoke(success);
            });
        }

        public void Save(bool saveGoogle)
        {
            if (saveData == null)
                return;

            string serializeData = JsonConvert.SerializeObject(saveData);
            string encryptData = EncryptUtils.Encrypt(serializeData);
            File.WriteAllText(FilePath, encryptData);

            if (saveGoogle)
                SaveGoogleService();
        }

        public void Save()
        {
            bool saveGoogle = GetLoginType() == SaveData.LoginType.Google;
            Save(saveGoogle);
        }

        public void SaveGoogleService()
        {
            if (saveData == null)
                return;

            string serializeData = JsonConvert.SerializeObject(saveData);
            string encryptData = EncryptUtils.Encrypt(serializeData);
            byte[] writeData = Convert.FromBase64String(encryptData);

            //if (GPGSMgr.Instance.IsOpened)
            //{
            //    GPGSMgr.Instance.WriteSavedGame(writeData, null);
            //}
            //else
            //{
                GPGSMgr.Instance.OpenSavedGame((openSuccess) =>
                {
                    if (openSuccess)
                    {
                        GPGSMgr.Instance.WriteSavedGame(writeData, null);
                    }
                });
            //}
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

            if (saveData == null)
                return false;

            return saveData.progressDatas.TryGetValue(index, out progressData);
        }

        public void SetProgressData(int index, ProgressData progressData)
        {
            if (saveData == null)
                return;

            saveData.progressDatas[index] = progressData;
        }

        public void DeleteProgressData(int index)
        {
            if (saveData == null)
                return;

            saveData.progressDatas.Remove(index);
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
            for (int i = 0; i < saveData.levelDatas.Count; ++i)
            {
                if (saveData.levelDatas[i].completed == false)
                    return i;
            }
            return saveData.levelDatas.Count;
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