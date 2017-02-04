using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DotCore.TagHelpers
{
    /// <summary>
    /// Allows to add active css class to the active route.
    /// </summary>
    [HtmlTargetElement("a", Attributes = CssClassAttribute)]
    [HtmlTargetElement("a", Attributes = ActionAttribute)]
    [HtmlTargetElement("a", Attributes = ControllerAttribute)] 
    public class RouteTagHelper : TagHelper
    {
        #region Static Fields and Constants

        private const string CssClassAttribute = "active-class";
        private const string ActionAttribute = "asp-action";
        private const string ControllerAttribute = "asp-controller";

        #endregion

        #region Fields

        private readonly IUrlHelperFactory urlHelperFactory;

        #endregion

        #region Properties, Indexers

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName(CssClassAttribute)]
        public string CssClass { get; set; } = "nav-link-current";

        [HtmlAttributeName(ActionAttribute)]
        public string Action { get; set; }

        [HtmlAttributeName(ControllerAttribute)]
        public string Controller { get; set; }

        #endregion

        #region Constructors

        /// <exception cref="ArgumentNullException"><paramref name="urlHelperFactory" /> is <see langword="null" />.</exception>
        public RouteTagHelper([NotNull] IUrlHelperFactory urlHelperFactory)
        {
            if (urlHelperFactory == null)
                throw new ArgumentNullException(nameof(urlHelperFactory));

            this.urlHelperFactory = urlHelperFactory;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);

            //Add active css class only when current request matches the generated href
            var currentRouteUrl = urlHelper.Action();
            var linkRouteUrl = urlHelper.Action(Action, Controller);

            if (string.Equals(linkRouteUrl, currentRouteUrl, StringComparison.Ordinal)
                ||
                !string.Equals(linkRouteUrl, "/", StringComparison.Ordinal) &&
                currentRouteUrl.IndexOf(linkRouteUrl, StringComparison.Ordinal) != -1)
            {
                var linkTag = new TagBuilder("a");
                linkTag.Attributes.Add("class", CssClass);
                output.MergeAttributes(linkTag);
            }
        }

        #endregion
    }
}