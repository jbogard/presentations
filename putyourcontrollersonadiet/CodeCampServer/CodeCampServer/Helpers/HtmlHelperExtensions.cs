using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace CodeCampServerLite.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static string SubmitButton(this HtmlHelper helper)
        {
            return "<input type=\"submit\"  value=\"Submit\"/>";
        }

        public static MvcHtmlString MyHiddenFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> member)
        {
            var id = UINameHelper.BuildIdFrom(member);
            var name = UINameHelper.BuildNameFrom(member);

            return helper.Hidden(name, ModelMetadata.FromLambdaExpression(member, helper.ViewData).Model, null);
        }

        public static MvcHtmlString MyTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> member)
        {
            var id = UINameHelper.BuildIdFrom(member);
            var name = UINameHelper.BuildNameFrom(member);

            return helper.TextBox(name, ModelMetadata.FromLambdaExpression(member, helper.ViewData).Model);
        }
    }
}