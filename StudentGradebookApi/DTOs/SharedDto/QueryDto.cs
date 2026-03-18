using System.Text.Json.Serialization;

namespace StudentGradebookApi.DTOs.SharedDto
{
    public class QueryDto
    {
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public string SortBy { get; set; } = string.Empty;
        public bool SortDescending { get; set; } = false;

        public const int DefaultMaxSize = 100;
        public const int DefaultPageSize = 20;
        public const int DefaultPageNumber = 1;

        internal int ValidPageSize
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

        internal int ValidPageNumber
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
