using K.SEOAnalyser.Web.Enums;
using K.SEOAnalyser.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace K.SEOAnalyser.Web.Services.Interfaces
{
    public interface IWordService
    {
        IEnumerable<Word> GetsByWordType(WordType wordType);
        List<string> FilterWordsByStopWords(List<string> contents);
    }
}
