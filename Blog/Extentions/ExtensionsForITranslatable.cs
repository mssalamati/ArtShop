using Blog.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Extentions
{
    public static class ExtensionsForITranslatable
    {
        public static TTranslation Current<TTranslatable, TTranslation>(
                this ITranslatable<TTranslatable, TTranslation> translatable)
            where TTranslation : class, ITranslation<TTranslatable>
            where TTranslatable : class, ITranslatable<TTranslatable, TTranslation>
        {
            string currentCultureName = CultureInfo.CurrentCulture.Name.Substring(0, 2);
            if (translatable.Translations == null)
                return Activator.CreateInstance<TTranslation>();
            var result = translatable.Translations.SingleOrDefault(t => t.languageId == currentCultureName);
            if (result == null)
                return Activator.CreateInstance<TTranslation>();
            return result;
        }
    }
}
