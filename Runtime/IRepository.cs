using UnityEngine;

namespace PhylisiumStudio.CrimsonSaver
{
    /// <summary>
    /// Repository pattern interface for saving and loading data.
    /// This is a generic class that can be used for any type of data.
    /// The Save method is used to save the data, and the Load method is used to load the data.
    /// The default implementation of the Load method returns the default value of the type T, which is null for reference types and zero for value types.
    /// </summary>
    /// <typeparam name="T">The type of data to be saved and loaded (same one).</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Saves the data.
        /// The implementation of this method should handle the actual saving logic, such as writing to a file, database, or any other storage medium.
        /// </summary>
        /// <param name="data">The data to be saved.</param>
        public void Save(T data);

        /// <summary>
        /// Loads the data.
        /// The implementation of this method should handle the actual loading logic, such as reading from a file, database, or any other storage medium.
        /// </summary>
        /// <returns>The loaded data of type T. If the loading fails or if there is no data to load, it returns the default value of type T.</returns>
        public T Load();

        /// <summary>
        /// Verifies if the data exists.
        /// </summary>
        /// <param name="fileName">The name of the file to check for existence.
        /// This parameter is relevant for implementations that save data to files, and it should include the file extension (e.g., "saveData.json").
        /// For implementations that do not use files, this parameter can be ignored or set to a default value.</param>
        /// <returns>True if the data exists; otherwise, false. The implementation of this method should determine the existence of the data based on the storage medium being used (e.g., checking if a file exists, checking if a key exists in PlayerPrefs, etc.).</returns>
        public bool Exists();

        /// <summary>
        /// Asynchronous version of <see cref="Save(T)"/>.
        /// Default implementation offloads the synchronous Save call to the thread-pool using Task.Run.
        /// IMPORTANT: If your concrete Save implementation calls Unity APIs (PlayerPrefs, UnityEngine.Object, etc.) you MUST override this method and execute on the main thread because Unity APIs are not thread-safe.
        /// </summary>
        public Awaitable SaveAsync(T data);

        /// <summary>
        /// Asynchronous version of <see cref="Load()"/>.
        /// Default implementation offloads the synchronous Load call to the thread-pool using Task.Run.
        /// IMPORTANT: If your concrete Load implementation accesses Unity APIs or accesses UnityEngine.Object instances, override this to ensure main-thread execution.
        /// </summary>
        public Awaitable<T> LoadAsync();

        /// <summary>
        /// Asynchronous version of <see cref="Exists()"/>.
        /// Default implementation offloads the synchronous Exists call to the thread-pool using Task.Run.
        /// </summary>
        public Awaitable<bool> ExistsAsync();

        /// <summary>
        /// Name of the key to save the data in PlayerPrefs.
        /// By default, it returns the name of the type T, but it can be overridden to provide a custom key.
        /// </summary>
        /// <returns>The key to be used for saving the data in PlayerPrefs. This should be a unique identifier for the data being saved, especially if multiple types of data are being saved using PlayerPrefs.</returns>
        public string GetKey() => typeof(T).Name;
    }
}