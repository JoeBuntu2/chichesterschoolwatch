using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Builder;

namespace ChichesterSchoolWatch.WebUI.Middleware.Angular
{
    public class InjectPageMetaTagsStaticFileOptions : StaticFileOptions
    {
        public static readonly Dictionary<string, PageMeta> PageMetaLookup = new Dictionary<string, PageMeta>(StringComparer.CurrentCultureIgnoreCase)
        {
            {
                "blog/2020/2/4/psers-narrative-part-1-actual-impact", new PageMeta
                {
                    Url = "https://www.chichesterschoolwatch.com/blog/2020/2/4/psers-narrative-part-1-actual-impact",
                    Title = "Title",
                    Description = "Subtitle",
                    Type = "article",
                    Image = "https://cdn.chichesterschoolwatch.com/articles/psers-narrative/chart-psers-state-vs-chihcester-share.png"
                }
            }
        };

        public static StaticFileOptions Generate()
        {
            var options = new InjectPageMetaTagsStaticFileOptions();
            options.OnPrepareResponse = ctx =>
            {

                if (PageMetaLookup.ContainsKey(ctx.Context.Request.Path))
                {
                    ctx.Context.Items.Add("ReplaceMeta", null);
                } 
            };

            return options;
        }


        public class PageMeta
        {
            public string Url { get; set; }
            public string Type { get; set; }
            public  string Title { get; set; }
            public string Description { get; set; }
            public string Image { get; set; }
        }
    }
}
