using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using SCUMMRevLib.Utils;

namespace SCUMMRevLib.Encryption
{
    public class TellTaleKeyStore
    {
        public Dictionary<string, string> ttarch { get; set; }
        public Dictionary<string, string> ttarch2 { get; set; }
    }

    public class TellTaleKeyInfo
    {
        public string Name { get; private set; }
        public byte[] Key { get; private set; }

        public TellTaleKeyInfo(string name, string key)
        {
            this.Name = name;
            this.Key = HexUtils.StringToByteArray(key);
        }
    }

    public class TellTaleKeyManager
    {
        private const string KEYS_PATH = @"data\telltalekeys.json";
        public static TellTaleKeyManager Instance { get { return instance; } }
        private static readonly TellTaleKeyManager instance = new TellTaleKeyManager();

        private List<TellTaleKeyInfo> keysTTArch;
        private List<TellTaleKeyInfo> keysTTArch2;

        public ReadOnlyCollection<TellTaleKeyInfo> KeysTTArch { get { return keysTTArch.AsReadOnly(); } }
        public ReadOnlyCollection<TellTaleKeyInfo> KeysTTArch2 { get { return keysTTArch2.AsReadOnly(); } }

        private TellTaleKeyManager()
        {
            keysTTArch = new List<TellTaleKeyInfo>();
            keysTTArch2 = new List<TellTaleKeyInfo>();

            if (!File.Exists(KEYS_PATH))
            {
                return;
            }
            string json = File.ReadAllText(KEYS_PATH);
            TellTaleKeyStore store = fastJSON.JSON.ToObject<TellTaleKeyStore>(json);

            foreach (var name in store.ttarch.Keys)
            {
                keysTTArch.Add(new TellTaleKeyInfo(name, store.ttarch[name]));
            }
            foreach (var name in store.ttarch2.Keys)
            {
                keysTTArch2.Add(new TellTaleKeyInfo(name, store.ttarch2[name]));
            }
        }
    }
}
