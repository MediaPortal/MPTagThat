using System.Collections.Generic;
using System.Windows.Forms;

namespace LyricsEngine
{
    public class Setup
    {
        #region Singleton

        private Setup()
        {
            ActiveSites = new List<string>();
        }

        private static class SetupHolder
        {
            public static readonly Setup Instance = new Setup();
        }

        public static Setup GetInstance()
        {
            return SetupHolder.Instance;
        }

        #endregion Singleton

        public List<string> ActiveSites { get; set; }
        
        public bool IsMember(string site)
        {
            return ActiveSites.Contains(site);
        }

        public int NoOfSites()
        {
            return ActiveSites.Count;
        }

        public void UpdateActiveSitesInSetup(CheckedListBox sitesList)
        {
            ActiveSites.Clear();
            for (var index = 0; index < sitesList.Items.Count; index++)
            {
                var active = sitesList.GetItemChecked(index);
                if (active)
                {
                    ActiveSites.Add((string) (sitesList.Items[index]));
                }
            }
        }
    }
}