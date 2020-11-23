using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace HealingJam.Crossword.Save
{
    public class SaveMgr : Singleton<SaveMgr>
    {
        public string FilePath { get { return Path.Combine(Application.dataPath, "save.dat"); } }

        private SaveData saveData = null;

        private SaveMgr() { }

        public void Load()
        {
            if (File.Exists(FilePath))
            {
                string text = File.ReadAllText(FilePath);
                saveData = JsonConvert.DeserializeObject<SaveData>(EncryptUtils.Decrypt(text));
            }
            else
            {
                saveData = new SaveData();
            }
        }

        public void Save()
        {
            if (saveData == null)
                return;

            string serializeData = JsonConvert.SerializeObject(saveData);
            File.WriteAllText(FilePath, EncryptUtils.Encrypt(serializeData));
        }

        public CompleteData GetCompleteData(int index)
        {
            if (saveData == null)
                return null;

            while (saveData.completeDatas.Count <= index)
            {
                saveData.completeDatas.Add(null);
            }

            return saveData.completeDatas[index];
        }

        public void SetCompleteData(int index, CompleteData completeData)
        {
            if (saveData == null)
                return;

            while(saveData.completeDatas.Count <= index)
            {
                saveData.completeDatas.Add(null);
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
    }
}