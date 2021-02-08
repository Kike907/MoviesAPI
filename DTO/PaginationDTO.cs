namespace MoviesAPI.DTO
{
    public class PaginationDTO
    {
        public int Page  {get; set; } = 1;

        private int recordsperPage = 10;

        private readonly int maxRecordsPerPage = 50;

        public int RecordPerPage  
        {
            get{
                return recordsperPage;
            }
            set{
                recordsperPage = (value > maxRecordsPerPage) ? maxRecordsPerPage : value;
            }
        }
    }
}