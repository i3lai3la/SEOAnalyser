using K.SEOAnalyser.Web.Enums;
using K.SEOAnalyser.Web.Models;
using K.SEOAnalyser.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace K.SEOAnalyser.Web.Controllers
{
    public class BaseController : Controller
    {
        internal readonly IOptions<AppSettings> _appSettings;
        public BaseController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }
    }
}
