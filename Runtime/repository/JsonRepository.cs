using PhylisiumStudio.CrimsonSaver.Utils;
using UnityEngine;

namespace PhylisiumStudio.CrimsonSaver.Repository
{
    /// <summary>
    /// Json repository implementation of the <see cref="IRepository<T>"/>. interface for saving and loading data in JSON format.
    /// </summary>
    /// <typeparam name="T">The type of data to be saved and loaded (same one).</typeparam>
    public class JsonRepository<T> : IRepository<T>
    {
        private readonly ISaver _saver;

        public JsonRepository(ISaver saver)
        {
            _saver = saver;
        }

        public void Save(T data)
        {
            var json = data.ToJson();
            _saver.Save(GetKey(), json);
        }

        public T Load()
        {
            var json = _saver.Load(GetKey());
            return json.FromJson<T>();
        }

        public bool Exists()
        {
            return _saver.Exists(GetKey());
        }

        public async Awaitable SaveAsync(T data)
        {
            var json = await data.ToJsonAsync();
            await _saver.SaveAsync(GetKey(), json);
        }

        public async Awaitable<T> LoadAsync()
        {
            var json = await _saver.LoadAsync(GetKey());
            return await json.FromJsonAsync<T>();
        }

        public async Awaitable<bool> ExistsAsync()
        {
            return await _saver.ExistsAsync(GetKey());
        }


        public string GetKey() => typeof(T).Name;
    }
}