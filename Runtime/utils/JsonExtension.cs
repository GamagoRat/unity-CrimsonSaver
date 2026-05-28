using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace PhylisiumStudio.CrimsonSaver.Utils
{
    public static class JsonExtension
    {
        /// <summary>
        /// Serializes an object to a JSON string with pretty printing enabled.
        /// </summary>
        /// <param name="obj">The object to be serialized to JSON, must be annotated with [Serializable] if it's a custom class.</param>
        /// <returns>A JSON string representation of the object, formatted with indentation for readability.</returns>
        public static string ToJson(this object obj) => JsonUtility.ToJson(obj, true);

        /// <summary>
        /// Deserializes a JSON string into an object of type T.
        /// </summary>
        /// <param name="json">The JSON string to be deserialized, must be in a format that matches the structure of type T.</param>
        /// <typeparam name="T">The type of object to be deserialized from the JSON string. This type must be annotated with [Serializable] if it's a custom class.</typeparam>
        /// <returns>An instance of type T that has been populated with data from the JSON string. If the JSON string is not in a valid format or does not match the structure of type T, this method may throw an exception or return a default instance of type T.</returns>
        public static T FromJson<T>(this string json) => JsonUtility.FromJson<T>(json);

        /// <summary>
        /// Asynchronously serializes an object to JSON.
        /// NOTE: Unity's JsonUtility is not guaranteed to be thread-safe for UnityEngine.Object-derived types
        /// (e.g. MonoBehaviour, ScriptableObject). Use this method only for plain data classes marked [Serializable].
        /// </summary>
        /// <param name="obj">The object to serialize. Cannot be null.</param>
        /// <param name="cancellationToken">Optional cancellation token to cancel the background operation.</param>
        /// <returns>A task that resolves to the pretty-printed JSON string.</returns>
        public static async Awaitable<string> ToJsonAsync(this object obj, CancellationToken cancellationToken = default)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            var json = await Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                return JsonUtility.ToJson(obj, true);
            }, cancellationToken);

            return json;
        }

        /// <summary>
        /// Asynchronously deserializes a JSON string into an object of type T.
        /// NOTE: Unity's JsonUtility is not guaranteed to be thread-safe for UnityEngine.Object-derived types
        /// (e.g. MonoBehaviour, ScriptableObject). Use this method only for plain data classes marked [Serializable].
        /// </summary>
        /// <typeparam name="T">The target type to deserialize.</typeparam>
        /// <param name="json">The JSON string to deserialize. May be null or empty depending on JsonUtility behavior.</param>
        /// <param name="cancellationToken">Optional cancellation token to cancel the background operation.</param>
        /// <returns>A task that resolves to an instance of T.</returns>
        public static async Awaitable<T> FromJsonAsync<T>(this string json, CancellationToken cancellationToken = default)
        {
            var obj = await Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                return JsonUtility.FromJson<T>(json);
            }, cancellationToken);

            return obj;
        }
    }
}