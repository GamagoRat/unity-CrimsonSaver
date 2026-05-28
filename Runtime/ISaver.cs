using UnityEngine;

namespace PhylisiumStudio.CrimsonSaver
{
    /// <summary>
    /// Saver interface for saving and loading data using a key-value approach.
    /// This interface defines methods for saving, loading, and checking the existence of data based on a unique key.
    /// </summary>
    public interface ISaver
    {
        /// <summary>
        /// Saves the data associated with the specified key.
        /// </summary>
        /// <param name="key">The unique key associated with the data to be saved. This key should be used to identify and retrieve the data later. The implementation of this method should ensure that the key is properly formatted and stored in a way that allows for efficient retrieval.</param>
        /// <param name="data">The data to be saved as a string. The implementation of this method should handle the actual saving logic, such as writing to a file, database, or any other storage medium. The data should be stored in a way that allows for efficient retrieval based on the associated key.</param>
        public void Save(string key, string data);

        /// <summary>
        /// Loads the data associated with the specified key.
        /// </summary>
        /// <param name="key">The unique key associated with the data to be loaded. This key should be used to identify and retrieve the data that was previously saved. The implementation of this method should handle the actual loading logic, such as reading from a file, database, or any other storage medium. The method should return the data as a string if it exists, or an appropriate value (e.g., null or an empty string) if the data does not exist or if there is an error during loading.</param>
        /// <returns>The data associated with the specified key as a string. If the data does not exist or if there is an error during loading, the method should return an appropriate value (e.g., null or an empty string) to indicate that the data could not be loaded.</returns>
        public string Load(string key);

        /// <summary>
        /// Verifies if the data associated with the specified key exists.
        /// </summary>
        /// <param name="key">The unique key associated with the data to check for existence. This key should be used to identify the data that was previously saved. The implementation of this method should handle the logic to determine whether the data associated with the specified key exists in the storage medium (e.g., checking if a file exists, checking if a key exists in PlayerPrefs, etc.). The method should return true if the data exists and is accessible, or false if it does not exist or if there is an error during the existence check.</param>
        /// <returns>True if the data associated with the specified key exists and is accessible; otherwise, false. The implementation of this method should ensure that it accurately reflects the existence of the data based on the storage medium being used.</returns>
        public bool Exists(string key);

        /// <summary>
        /// Formats the key for saving and loading data. This method can be used to ensure that the key is in a consistent format, such as adding a file extension or prefixing it with a specific string. The implementation of this method should return the formatted key that will be used for saving and loading data based on the provided key. This allows for flexibility in how keys are structured and can help prevent naming conflicts or ensure that keys are easily identifiable in the storage medium.
        /// </summary>
        /// <param name="key">The unique key that needs to be formatted. This key should be the original key provided for saving or loading data, and the implementation of this method should apply any necessary formatting to it (e.g., adding a file extension, prefixing it with a specific string, etc.) to ensure that it is in a consistent format for use in the storage medium.</param>
        /// <returns>The formatted key that will be used for saving and loading data based on the provided key. The implementation of this method should ensure that the returned key is in a consistent format that is suitable for use in the storage medium being used (e.g., as a file name, as a key in PlayerPrefs, etc.).</returns>
        public string KeyFormat(string key);

        // -------------------- Asynchronous helpers --------------------

        /// <summary>
        /// Asynchronous version of <see cref="Save(string, string)"/>.
        /// Default implementation offloads the synchronous Save call to the thread-pool using Task.Run.
        /// IMPORTANT: If your concrete Save implementation uses Unity APIs (PlayerPrefs, UnityEngine.Object, etc.), override this method and execute on the main thread because Unity APIs are not thread-safe.
        /// </summary>
        public Awaitable SaveAsync(string key, string data);

        /// <summary>
        /// Asynchronous version of <see cref="Load(string)"/>.
        /// Default implementation offloads the synchronous Load call to the thread-pool using Task.Run.
        /// IMPORTANT: If your concrete Load implementation accesses Unity APIs or UnityEngine.Object instances, override this to ensure main-thread execution.
        /// </summary>
        public Awaitable<string> LoadAsync(string key);

        /// <summary>
        /// Asynchronous version of <see cref="Exists(string)"/>.
        /// Default implementation offloads the synchronous Exists call to the thread-pool using Task.Run.
        /// </summary>
        public Awaitable<bool> ExistsAsync(string key);
    }
}