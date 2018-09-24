using System.Collections.Generic;
using System.Linq;
using K.SEOAnalyser.Web.Enums;
using K.SEOAnalyser.Web.Models.Entities;
using K.SEOAnalyser.Web.Services.Interfaces;

namespace K.SEOAnalyser.Web.Services
{
    public class WordService : IWordService
    {
        private readonly SeoContext _seoContext;
        public WordService(SeoContext seoContext)
        {
            _seoContext = seoContext;
        }

        public IEnumerable<Word> GetsByWordType(WordType wordType)
        {
            return _seoContext.Words.Where(w => w.WordType == wordType && w.IsActive);
        }

        public List<string> FilterWordsByStopWords(List<string> contents)
        {
            if (contents == null && contents.Count <= 0) return null;

            IEnumerable<Word> stopWords = GetsByWordType(WordType.STOPWORD);
            return contents.Where(c => !stopWords.Any(sw => sw.Value.Equals(c.ToLower()))).ToList();
        }
    }
}
