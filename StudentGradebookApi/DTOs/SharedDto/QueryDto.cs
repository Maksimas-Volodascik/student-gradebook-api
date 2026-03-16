namespace StudentGradebookApi.DTOs.SharedDto
{
    public class QueryDto
    {
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }

        public const int DefaultMaxSize = 100;
        public const int DefaultPageSize = 20;
        public const int DefaultPageNumber = 1;

        public int ValidPageSize
        {
            get { 
                if(PageSize == null)
                {
                    return DefaultPageSize;
                }

                if (PageSize > DefaultMaxSize || PageSize < 1)
                {
                    return DefaultPageSize;
                }

                return PageSize.Value;
            }
        }

        public int ValidPageNumber
        {
            get
            {
                if (PageNumber == null)
                {
                    return DefaultPageNumber;
                }

                return PageNumber.Value;
            }
        }
    }
}
