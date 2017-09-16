using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Extentions;
using System.Resources;
using System.Globalization;
using System.Collections;
using DataLayer;
using DataLayer.LanguageUtil.WordResources;

namespace Resources
{
    public class ResBase
    {
        public static ResBase Instance { get { if (_Instance == null) _Instance = new ResBase(); return _Instance; } }

        private static ResBase _Instance;

        public Dictionary<string, List<Word>> Words;

        public void load()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //add new words
                var addorupdatedb = new Action<ResourceManager, string>((ResourceManager r, string s) =>
                {
                    if (!db.WordCategories.Any(x => x.Name == s))
                        db.WordCategories.Add(new WordCategory() { Name = s });
                    foreach (DictionaryEntry entry in r.GetResourceSet(CultureInfo.CurrentUICulture, true, true))
                    {
                        string resourceKey = entry.Key.ToString();
                        if (!db.Words.Any(x => x.Name == resourceKey && x.categoryId == s))
                            db.Words.Add(new Word() { Name = resourceKey, categoryId = s });
                    }
                });
                addorupdatedb(CardRes.ResourceManager, "Card");
                addorupdatedb(CardRes.ResourceManager, "Footer");
                addorupdatedb(CardRes.ResourceManager, "Profile");
                addorupdatedb(CardRes.ResourceManager, "Search");
                addorupdatedb(CardRes.ResourceManager, "Share");
                addorupdatedb(CardRes.ResourceManager, "Upload");
                db.SaveChanges();

                //fill cash
                Words = db.WordCategories.Select(x => new
                {
                    Name = x.Name,
                    Words = x.Words.Select(w => new
                    {
                        Id = w.Id,
                        Name = w.Name,
                        Translations = w.Translations.Select(t => new
                        {
                            t.languageId,
                            t.Name
                        }).ToList()
                    }).ToList()
                }).ToList().Select(x => new WordCategory()
                {
                    Name = x.Name,
                    Words = x.Words.Select(w => new Word()
                    {
                        Id = w.Id,
                        Name = w.Name,
                        Translations = w.Translations.Select(t => new WordTranslation()
                        {
                            languageId = t.languageId,
                            Name = t.Name
                        }).ToList()
                    }).ToList()
                }).ToDictionary(x => x.Name, x => x.Words.ToList());
            }
        }

        public string getWord(string category, string word)
        {
            if (word == null)
                load();
            List<Word> words;
            bool isCategoryExist = Words.TryGetValue(category, out words);
            if (isCategoryExist)
            {
                Word requestedWord = words.FirstOrDefault(x => x.Name == word);
                if (requestedWord != null)
                    return requestedWord.Current().Name;
            }
            return word;
        }
    }
}
