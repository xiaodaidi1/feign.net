﻿using Feign.Formatting;
using Feign.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Feign
{
    public class FeignOptions : IFeignOptions
    {
        public FeignOptions()
        {
            Assemblies = new List<Assembly>();
            Converters = new ConverterCollection();
            Converters.AddConverter(new ObjectStringConverter());
            MediaTypeFormatters = new MediaTypeFormatterCollection();
            MediaTypeFormatters.AddFormatter(new JsonMediaTypeFormatter());
            MediaTypeFormatters.AddFormatter(new JsonMediaTypeFormatter(Constants.MediaTypes.TEXT_JSON));
            MediaTypeFormatters.AddFormatter(new XmlMediaTypeFormatter());
            MediaTypeFormatters.AddFormatter(new XmlMediaTypeFormatter(Constants.MediaTypes.TEXT_XML));
            MediaTypeFormatters.AddFormatter(new FormUrlEncodedMediaTypeFormatter());
            MediaTypeFormatters.AddFormatter(new MultipartFormDataMediaTypeFormatter());
            FeignClientPipeline = new GlobalFeignClientPipeline();
            Lifetime = FeignClientLifetime.Transient;
            Types = new List<FeignClientTypeInfo>();
            DiscoverServiceCacheTime = TimeSpan.FromMinutes(10);
        }
        public IList<Assembly> Assemblies { get; }
        public ConverterCollection Converters { get; }
        public MediaTypeFormatterCollection MediaTypeFormatters { get; }
        public IGlobalFeignClientPipeline FeignClientPipeline { get; }
        public FeignClientLifetime Lifetime { get; set; }
        public bool IncludeMethodMetadata { get; set; }

        public DecompressionMethods? AutomaticDecompression { get; set; }

        public IList<FeignClientTypeInfo> Types { get; }
        public TimeSpan? DiscoverServiceCacheTime { get; set; }
    }
}
