namespace Camelonta.Boilerplate.TemplatePages
{
    public class SearchTemplatePage : CamelontaUmbracoTemplatePage
    {
        protected string SearchTerm
        {
            get
            {
                return Request.QueryString["q"] ?? "";
            }
        }

        protected int CurrentPageNumber
        {
            get
            {
                int pageNumber;
                int.TryParse(Request.QueryString["page"], out pageNumber);
                if (pageNumber == 0)
                {
                    pageNumber = 1;
                }
                return pageNumber;
            }
        }

        protected int NextPage
        {
            get
            {
                return CurrentPageNumber + 1;
            }
        }

        public override void Execute()
        {
            
        }
    }
}