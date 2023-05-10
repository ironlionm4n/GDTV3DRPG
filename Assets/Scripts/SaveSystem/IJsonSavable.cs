using Newtonsoft.Json.Linq;

namespace RPG.SaveSystem
{
    public interface IJsonSavable
    {
        /// <summary>
        /// Override to return JToken representing the state of the IJsonSaveable
        /// </summary>
        JToken CaptureAsJToken();

        /// <summary>
        /// Restore the state of the component using the information in JToken
        /// </summary>
        /// <param name="state">A JToken object representing the state of the module</param>
        void RestoreFromJToken(JToken state);
    }
}