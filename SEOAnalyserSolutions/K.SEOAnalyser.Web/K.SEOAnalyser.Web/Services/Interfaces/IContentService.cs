using K.SEOAnalyser.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace K.SEOAnalyser.Web.Services.Interfaces
{
    public interface IContentService
    {
        Content Save(string url, string value);
        Content Update(Guid contentId, string url, string value);
        Content GetByUrl(string url);
        Content GetByValue(string value);
        Content Get(Guid contentId);
    }
}
