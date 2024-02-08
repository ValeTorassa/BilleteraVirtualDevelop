using Azure;

namespace AutheticationAPI2._0.Model.Dto
{
    public class Result
    {
        public bool IsSuccess { get; set; }

        public object? Response { get; set; }

        public string? Message { get; set; }
    }
}
