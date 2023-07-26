namespace Task3.Models
{
    public class DataTableParameters
    {
        public int StartIndex { get; set; }
        public int PageSize { get; set; }
        public string SearchValue { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }

        // Other employee-related properties
        public List<Employee> Employees { get; set; }
        public int TotalRecords { get; set; }
        public int CurrentPage { get; set; } // Current page number
        public int TotalPages { get; set; } // Total number of pages
        public bool HasPreviousPage => CurrentPage > 1; // Whether there is a previous page
        public bool HasNextPage => CurrentPage < TotalPages; // Whether there is a next page
    }
}
