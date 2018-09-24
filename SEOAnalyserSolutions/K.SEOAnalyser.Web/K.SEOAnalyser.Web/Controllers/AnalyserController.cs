using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using K.SEOAnalyser.Web.Models;
using K.SEOAnalyser.Web.Services.Interfaces;
using K.SEOAnalyser.Web.Enums;
using K.SEOAnalyser.Web.Models.Views;
using K.SEOAnalyser.Web.Utils;
using K.SEOAnalyser.Web.Models.Entities;

namespace K.SEOAnalyser.Web.Controllers
{
    public class AnalyserController : BaseController
    {
        private readonly IWordService _wordService;
        private readonly IContentService _contentService;

        public AnalyserController(IOptions<AppSettings> appSettings, IWordService wordService, IContentService contentService) :base(appSettings)
        {
            _wordService = wordService;
            _contentService = contentService;
        }

        public IActionResult Index()
        {
            return View(new SearchViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> RegisterSearchValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return BadRequest("Value provided is not valid.");

            InputValueType valueType = CommonFunctions.ValidateValueType(value);
            string allWordsInPage = null;
            string url = null;
            Content content = null;
            
            switch (valueType)
            {
                case InputValueType.URL:
               
                    if (!CommonFunctions.IsUrlResponses(value))
                        return BadRequest("Unable to get response from URL given.");

                    url = value;
                    content = _contentService.GetByUrl(url);
                    if(content != null)
                    {
                        //if content is more than 1 days old, update latest content
                        if (content.UpdatedDate < DateTime.Now.AddDays(-1))
                        {
                            allWordsInPage = await CommonFunctions.GetPageWordsFromUrl(url);
                            content = _contentService.Update(content.Id, url, allWordsInPage);
                        }
                    }
                    else
                    {
                        allWordsInPage = await CommonFunctions.GetPageWordsFromUrl(url);
                        content = _contentService.Save(url, allWordsInPage);
                    }

                    break;
                case InputValueType.TEXT:
                    allWordsInPage = value;
                    content = _contentService.GetByValue(value);

                    if (content != null)
                    {
                        //if content is more than 1 days old, update latest content
                        if (content.UpdatedDate < DateTime.Now.AddDays(-1))
                        {
                            content = _contentService.Update(content.Id, url, allWordsInPage);
                        }
                    }
                    else
                    {
                        content = _contentService.Save(url, allWordsInPage);
                    }
                      
                    break;
                default:
                    break;
            }
            
            return Json(content.Id);
        }

        [HttpGet]
        public IActionResult GetNumberOfWordOccursOnValue(Guid contentId, bool isStopWordFilterOn)
        {
            if (contentId == null || contentId == Guid.Empty)
                return BadRequest("Value provided is not valid.");

            Content content = _contentService.Get(contentId);
            
            if(content == null)
                return BadRequest("Invalid data provided.");

            List<string> words = CommonFunctions.ExtractOnlyWords(content.Value);
        
            if (isStopWordFilterOn)
                words = _wordService.FilterWordsByStopWords(words);

            IEnumerable<SearchResultModel> results = GroupByWords(words);

            return Json(results);

        }

        [HttpGet]
        public IActionResult GetNumberOfWordOccursOnMetaTag(Guid contentId, bool isStopWordFilterOn)
        {
            if (contentId == null || contentId == Guid.Empty)
                return BadRequest("Value provided is not valid.");

            Content content = _contentService.Get(contentId);

            if (content == null)
                return BadRequest("Invalid data provided.");

            string allWords = CommonFunctions.GetStringInMetaTag(content.Value);
            List<string> words = CommonFunctions.ExtractOnlyWords(allWords);

            if (isStopWordFilterOn)
                words = _wordService.FilterWordsByStopWords(words);

            IEnumerable<SearchResultModel> results = GroupByWords(words);

            return Json(results);

        }

        [HttpGet]
        public IActionResult GetNumberOfExternalUrlOccursOnValue(Guid contentId)
        {
            if (contentId == null || contentId == Guid.Empty)
                return BadRequest("Value provided is not valid.");

            Content content = _contentService.Get(contentId);

            if (content == null)
                return BadRequest("Invalid data provided.");

            List<string> urls = CommonFunctions.ExtractUrlFromString(content.Value);
            IEnumerable<SearchResultModel> results = GroupByWords(urls);

            return Json(results);

        }

        private IEnumerable<SearchResultModel> GroupByWords(List<string> words)
        {
            if (words == null) return null;

           return words.GroupBy(w => w)
                .Select(g => new SearchResultModel
                {
                    Word = g.Key,
                    Frequency = g.Count()
                });
        }
    }
}
