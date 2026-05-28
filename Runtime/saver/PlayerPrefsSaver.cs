using UnityEngine;

namespace PhylisiumStudio.CrimsonSaver.Extensions
{
    /// <summary>
    /// Player Prefs Saver implementation of the <see cref="ISaver<T>"/>. interface for saving and loading data to and from
    /// the PlayerPrefs using a key-value approach.
    /// </summary>
    public class PlayerPrefsSaver : ISaver
    {
        public void Save(string key, string data)
        {
            data.SaveToPlayerPrefs(KeyFormat(key));
        }

        public string Load(string key)
        {
            return key.LoadFromPlayerPrefs();
        }

        public bool Exists(string key)
        {
            return key.ExistsInPlayerPrefs();
        }

        public Awaitable SaveAsync(string key, string data)
        {
            return data.SaveToPlayerPrefsAsync(KeyFormat(key));
        }

        public Awaitable<string> LoadAsync(string key)
        {
            return key.LoadFromPlayerPrefsAsync();
        }

        public Awaitable<bool> ExistsAsync(string key)
        {
            return key.ExistsInPlayerPrefsAsync();
        }

        public string KeyFormat(string key)
        {
            return $"{key}";
        }
    }
}