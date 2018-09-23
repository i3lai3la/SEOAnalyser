using System;
using System.Linq;
using K.SEOAnalyser.Web.Models.Entities;
using K.SEOAnalyser.Web.Services.Interfaces;

namespace K.SEOAnalyser.Web.Services
{
    public class ContentService : IContentService
    {
        private readonly SeoContext _seoContext;
        public ContentService(SeoContext seoContext)
        {
            _seoContext = seoContext;
        }

        public Content Save(string url, string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            Content content = new Content {
                Url = url,
                Value = value,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            _seoContext.Add(content);
            _seoContext.SaveChanges();
         
            return content;
        }

        public Content Update(Guid contentId, string url, string value)
        {
            if(string.IsNullOrWhiteSpace(value) || contentId == null || contentId == Guid.Empty) return null;

            Content dbContent = Get(contentId);
            if (dbContent != null)
            {
                dbContent.Value = value;
                dbContent.UpdatedDate = DateTime.Now;
                _seoContext.SaveChanges();
            }
      
            return dbContent;
        }

        public Content GetByUrl(string url)
        {
            return _seoContext.Contents.SingleOrDefault(c => c.Url != null && c.Url.Equals(url));
        }

        public Content GetByValue(string value)
        {
            return _seoContext.Contents.SingleOrDefault(c => c.Value != null &&  c.Value.Equals(value));
        }

        public Content Get(Guid contentId)
        {
            return _seoContext.Contents.SingleOrDefault(c => c.Id.Equals(contentId));
        }
    }
}
