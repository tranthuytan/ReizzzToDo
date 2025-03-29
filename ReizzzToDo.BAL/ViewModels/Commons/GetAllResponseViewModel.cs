namespace ReizzzToDo.BAL.ViewModels.Commons
{
    public class GetAllResponseViewModel<T> where T : class
    {
        public string Error { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsPaginated { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int PageCount => (int)Math.Ceiling((double)(Data.Count / PageSize));
        public List<T> Data { get; set; } = new();
    }
}
