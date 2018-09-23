using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace K.SEOAnalyser.Web.Models.Views
{
    public class SearchViewModel
    {
        public string ContentId { set; get; }
        public string Value { set; get; }
        public bool IsFilterStopWordsOn { set; get; } = true;
        public bool IsCalculateNumberOfWordOccurencesOn { set; get; } = true;
        public bool IsCalculateNumberOfOccurencesInMetaTagOn { set; get; } = true;
        public bool IsCalculateNumberOfExternalLinkOn { set; get; } = true;


    }
}
