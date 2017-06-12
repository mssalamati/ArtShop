using DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Extentions
{
    public static class ExtensionsForITranslatable
    {
        public static TTranslation Current<TTranslatable, TTranslation>(
                this ITranslatable<TTranslatable, TTranslation> translatable)
            where TTranslation : class, ITranslation<TTranslatable>
            where TTranslatable : class, ITranslatable<TTranslatable, TTranslation>
        {
            string currentCultureName = CultureInfo.DefaultThreadCurrentUICulture.Name;
            return translatable.Translations.SingleOrDefault(t => t.language.Code == currentCultureName);
        }
    }
}
